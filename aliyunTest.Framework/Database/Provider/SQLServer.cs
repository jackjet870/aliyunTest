using System;
using aliyunTest.Framework.Database.DBMap;
using System.Data;
using System.Data.Common;

namespace aliyunTest.Framework.Database.Provider
{
    public class SQLServer : DBSession
    {
        /// <summary>
        /// 设置父类 连接工厂
        /// </summary>
        /// <returns></returns>
        protected override DbProviderFactory CreateDbProviderFactory()
        {
            return System.Data.SqlClient.SqlClientFactory.Instance;
        }

        #region override

        protected override string GuidToString(Guid guid)
        {
            return guid.ToString();
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
            insertSql = string.Format("{0}select @@identity;", insertSql);
            Command.CommandText = insertSql;
            Command.CommandType = CommandType.Text;
            Command.Parameters.Clear();

            //添加主键参数
            if (!table.PrimaryKey.IsIdentity)
            {
                object value = this.GetValue(table.PrimaryKey, instance);
                if (table.PrimaryKey.ColumnType == DBColumnType.Guid)
                {
                    Guid guid = new Guid(value.ToString());
                    if (guid == Guid.Empty)
                        value = Guid.NewGuid();
                    this.SetValue(table.PrimaryKey, instance, value);
                }
                AddParameter(FormatParameterName(table.PrimaryKey.AliasName), ParameterDirection.InputOutput, value);
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

                if (value == null)
                    AddParameter(this.FormatParameterName(col.AliasName), ParameterDirection.Input, value, col.ColumnType);
                else
                    AddParameter(this.FormatParameterName(col.AliasName), ParameterDirection.Input, value);

            }
            //执行命令
            object obj = Command.ExecuteScalar();
            if (table.PrimaryKey.IsIdentity)
            {
                this.SetValue(table.PrimaryKey, instance, Convert.ChangeType(obj, table.PrimaryKey.Type));
            }

            //执行数据库操作后的处理
            RunAfterFunc();

            return obj;
        }

        /// <summary>
        /// 分页Sql,页码从1开始
        /// </summary>
        /// <param name="pageIndex">分页索引，以1开始</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="fields">字段列表，以“,”分隔</param>
        /// <param name="from">表名称,比如t_a left join t_b on t_a.id=t_b.id</param>
        /// <param name="where">Where 条件，参数用？代替</param>
        /// <param name="group">Group by 子句</param>
        /// <param name="order">排序方式,不包含"order by"</param>
        /// <param name="paras">条件参数</param>
        /// <returns>分页的SQL语句</returns>
        protected override string PrepareCustomSelectPaging(int pageIndex, int pageSize, string fields, string from, string where, string group, string order, object[] paras)
        {
            //if (pageIndex == 1)
            //{
            //    return PrepareCustomSelectTopN(pageSize, fields, from, where, group, order, paras);
            //}

            PrepareSelectParameter(paras);
            if (string.IsNullOrEmpty(order))
            {
                throw new Exception("Sql Server无法对没有排序的SQL语句进行分页查询!");
            }

            return FormatSqlForParameter(string.Format(@"
select top {0} * from (
	select	row_number() over (order by {1}
			) as r__n,
			{2}
	from	{3}
			{4}
			{5}
	) a
where r__n > {6}",
                 pageSize,
                 order,
                 fields,
                 from,
                 string.IsNullOrEmpty(where) ? "" : "where " + where,
                 string.IsNullOrEmpty(group) ? "" : "group by " + group,
                 (pageIndex - 1) * pageSize)
            );
        }

        /// <summary>
        /// select topN dbSql
        /// </summary>
        /// <param name="topN">指定获取记录条数</param>
        /// <param name="fields">字段列表，以“,”分隔</param>
        /// <param name="from">表名称,比如t_a left join t_b on t_a.id=t_b.id</param>
        /// <param name="where">Where 条件，参数用？代替</param>
        /// <param name="group">Group by 子句</param>
        /// <param name="order">排序方式,不包含"order by"</param>
        /// <param name="paras">条件参数</param>
        /// <returns>格式后的sql语句</returns>
        protected override string PrepareCustomSelectTopN(int topN, string fields, string from, string where, string group, string order, object[] paras)
        {
            PrepareSelectParameter(paras);

            return FormatSqlForParameter(string.Format(@"
select 	{0}
		{1}
from	{2}
		{3}
		{4}
		{5}",
                topN > 0 ? string.Format("top {0}", topN) : "",
                fields,
                from,
                string.IsNullOrEmpty(where) ? "" : "where " + where,
                string.IsNullOrEmpty(group) ? "" : "group by " + group,
                string.IsNullOrEmpty(order) ? "" : "order by " + order)
            );
        }

        #endregion
    }
}
