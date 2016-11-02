using System;
using System.Text;
using System.Data.Common;
using aliyunTest.Framework.Database.DBMap;
using System.Data;

namespace aliyunTest.Framework.Database.Provider
{
    public class Oracle : DBSession
    {
        public const string ProviderTypeString = "Oracle.ManagedDataAccess";

        protected override DbProviderFactory CreateDbProviderFactory()
        {
            Type type = GetType(ProviderTypeString, "Oracle.ManagedDataAccess.Client.OracleClientFactory");
            return (DbProviderFactory)Activator.CreateInstance(type, true);
        }

        #region override

        protected override string FormatParameterName(string p)
        {
            return ":" + p;
        }

        protected override string GuidToString(Guid guid)
        {
            return guid.ToString("N").ToUpper();
        }

        protected override string GetInsertSql(DBTable table)
        {
            StringBuilder fields = new StringBuilder();
            StringBuilder values = new StringBuilder();

            //添加主键
            fields.Append(table.PrimaryKey.Name);
            values.Append(this.FormatParameterName(table.PrimaryKey.AliasName));
            //添加其他属性
            foreach (DBColumn col in table.ColumnList)
            {
                fields.AppendFormat(",{0}", col.Name);
                values.AppendFormat(",{0}", this.FormatParameterName(col.AliasName));
            }

            return string.Format("insert into {0}({1})values({2})", table.Name, fields, values);
        }

        /// <summary>
        /// 重写父类的新增--在父类的基础上增加了 返回新增的ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="table"></param>
        /// <param name="insertSql"></param>
        protected override object Insert<T>(T instance, DBTable table, string insertSql)
        {
            Command.CommandText = insertSql;
            Command.CommandType = CommandType.Text;
            Command.Parameters.Clear();
            //获取seq 主键值
            object primaryValue = null;
            if (table.PrimaryKey.IsIdentity)
            {
                primaryValue = ExecuteScalar(string.Format("select {0}.nextval from dual", table.PrimaryKey.SequenceName));
                //添加主键参数
                AddParameter(FormatParameterName(table.PrimaryKey.AliasName), ParameterDirection.Input, primaryValue);
                Command.CommandText = insertSql;
            }
            else
            {
                object value = this.GetValue(table.PrimaryKey, instance);
                if (table.PrimaryKey.ColumnType == DBColumnType.Guid)
                {
                    Guid guid = new Guid(value.ToString());
                    if (guid == Guid.Empty)
                    {
                        guid = Guid.NewGuid();
                        this.SetValue(table.PrimaryKey, instance, guid);
                    }
                    value = GuidToString(guid);
                }
                //添加主键参数
                AddParameter(FormatParameterName(table.PrimaryKey.AliasName), ParameterDirection.Input, value);
            }
            //添加其他属性参数
            foreach (DBColumn col in table.ColumnList)
            {
                object value = this.GetValue(col, instance);
                if (col.ColumnType == DBColumnType.Guid)
                {
                    Guid vl = new Guid(value.ToString());
                    if (vl == Guid.Empty) value = null;
                }
                AddParameter(this.FormatParameterName(col.AliasName), ParameterDirection.Input, value);
            }
            //执行命令
            Command.ExecuteNonQuery();
            if (table.PrimaryKey.IsIdentity)
                this.SetValue(table.PrimaryKey, instance, Convert.ChangeType(primaryValue, table.PrimaryKey.Type));

            //执行数据库操作后的处理
            RunAfterFunc();

            return primaryValue;
        }

        protected override string PrepareCustomSelectPaging(int pageIndex, int pageSize, string fields, string from, string where, string group, string order, object[] paras)
        {
            PrepareSelectParameter(paras);

            if (pageSize == 0)
            {
                return FormatSqlForParameter(string.Format(@"
select	{0}
from	{1}
		{2}
		{3}
		{4}",
                        fields,
                        from,
                        string.IsNullOrEmpty(where) ? "" : "where " + where,
                        string.IsNullOrEmpty(group) ? "" : "group by " + group,
                        string.IsNullOrEmpty(order) ? "" : "order by " + order
                    )
                );
            }

            else if (string.IsNullOrEmpty(order))
            {
                return FormatSqlForParameter(string.Format(@"
select * from(
		select	a.*, rownum r__n from(
			select	{0}
			from	{1}
					{2}
					{3}
			order by {4}
		) a
	) b
where r__n<={5} and r__n > {6}",
                        fields,
                        from,
                        string.IsNullOrEmpty(where) ? "" : where + " and ",
                        pageIndex * pageSize,
                        string.IsNullOrEmpty(group) ? "" : "group by " + group,
                        (pageIndex - 1) * pageSize
                    )
                );
            }
            else
            {
                return FormatSqlForParameter(string.Format(@"
select * from(
		select	a.*, rownum r__n from(
			select	{0}
			from	{1}
					{2}
					{3}
			order by {4}
		) a
		where rownum <= {5}
	) b
where r__n > {6}",
                        fields,
                        from,
                        string.IsNullOrEmpty(where) ? "" : "where " + where,
                        string.IsNullOrEmpty(group) ? "" : "group by " + group,
                        order,
                        pageIndex * pageSize,
                        (pageIndex - 1) * pageSize
                    )
                );
            }
        }

        protected override string PrepareCustomSelectTopN(int topN, string fields, string from, string where, string group, string order, object[] paras)
        {
            return PrepareCustomSelectPaging(1, topN, fields, from, where, group, order, paras);
        }

        #endregion
    }
}
