using System;
using System.Text;
using System.Data;
using aliyunTest.Framework.Database.DBMap;
using aliyunTest.Framework.Database;

namespace aliyunTest.Framework
{
    public abstract partial class DBSession
    {
        #region insert

        /// <summary>
        /// 设置指定表的Insert语句
        /// </summary>
        /// <param name="table">属性表</param>
        protected virtual string GetInsertSql(DBTable table)
        {
            StringBuilder fields = new StringBuilder();
            StringBuilder values = new StringBuilder();
            if (!table.PrimaryKey.IsIdentity)//如果不是自增长
            {
                fields.Append(table.PrimaryKey.Name);
                values.Append(this.FormatParameterName(table.PrimaryKey.AliasName));
            }
            foreach (DBColumn col in table.ColumnList)
            {
                fields.AppendFormat(",{0}", col.Name);
                values.AppendFormat(",{0}", this.FormatParameterName(col.AliasName));
            }
            if (table.PrimaryKey.IsIdentity)//如果是自增长,则去掉前面的，号
            {
                fields = fields.Remove(0, 1);
                values = values.Remove(0, 1);
            }
            return string.Format("insert into {0}({1})values({2});", table.Name, fields, values);
        }


        /// <summary>
        /// 插入记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        protected virtual object Insert<T>(T instance, DBTable table, string insertSql)
        {
            Command.CommandText = insertSql;
            Command.CommandType = CommandType.Text;
            Command.Parameters.Clear();

            //添加主键参数
            IDataParameter pId = AddParameter(FormatParameterName(table.PrimaryKey.AliasName), ParameterDirection.InputOutput, this.GetValue(table.PrimaryKey, instance));
            //添加其他属性参数
            foreach (DBColumn col in table.ColumnList)
            {
                AddParameter(this.FormatParameterName(col.AliasName), ParameterDirection.Input, this.GetValue(col, instance));
            }
            //执行命令
            int result = Command.ExecuteNonQuery();

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public object Insert<T>(T instance)
        {
            Type type = typeof(T);
            DBSql dbsql = MapHelper.GetDBSql(type, _dbContext.DataType);
            if (string.IsNullOrEmpty(dbsql.InsertSql))//如果该表的新增语句为空，则生成该表的Insert语句
            {
                DBTable table = MapHelper.GetDBTable(type);
                dbsql.InsertSql = GetInsertSql(table);
            }
            return Insert<T>(instance, dbsql.Table, dbsql.InsertSql);
        }

        #endregion

        #region update

        /// <summary>
        /// 设置指定表的Update语句
        /// </summary>
        /// <param name="table">属性表</param>
        protected virtual string GetUpdateSql(DBTable table)
        {
            StringBuilder sets = new StringBuilder();
            foreach (DBColumn col in table.ColumnList)
            {
                sets.AppendFormat(",{0}={1}", col.Name, this.FormatParameterName(col.AliasName));
            }
            return string.Format("update {0} set {1} where {2}={3}",
                                    table.Name,
                                    sets.ToString().TrimStart(','),
                                    table.PrimaryKey.Name,
                                    this.FormatParameterName(table.PrimaryKey.AliasName)
                    );
        }

        /// <summary>
        /// 保存一个指定类型的对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="instance">对象</param>
        /// <returns>受影响的行数</returns>
        protected virtual int Update<T>(T instance, DBTable table, string updateSql)
        {
            Command.Parameters.Clear();
            //添加其他属性参数
            foreach (DBColumn col in table.ColumnList)
            {
                object value = this.GetValue(col, instance);
                if (col.ColumnType == DBColumnType.Guid)
                {
                    Guid vl = new Guid(value.ToString());
                    if (vl == Guid.Empty) value = null;
                }

                IDataParameter param = AddParameter(this.FormatParameterName(col.AliasName), ParameterDirection.Input, value);
                if (value == null && col.ColumnType == DBColumnType.ByteArray)
                {
                    param.DbType = DbType.Binary;
                }
            }
            //添加主键参数
            AddParameter(this.FormatParameterName(table.PrimaryKey.AliasName), ParameterDirection.Input, this.GetValue(table.PrimaryKey, instance));

            Command.CommandType = CommandType.Text;
            Command.CommandText = updateSql;
            int result = Command.ExecuteNonQuery();

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="selfOnly"></param>
        /// <returns>受影响的行数</returns>
        public int Update<T>(T instance)
        {
            Type type = typeof(T);
            DBSql dbsql = MapHelper.GetDBSql(type, _dbContext.DataType);
            if (string.IsNullOrEmpty(dbsql.UpdateSql))//如果该表的修改语句为空，则生成该表的update语句
            {
                DBTable table = MapHelper.GetDBTable(type);
                dbsql.UpdateSql = GetUpdateSql(table);
            }
            return Update<T>(instance, dbsql.Table, dbsql.UpdateSql);
        }

        #endregion

        #region delete

        /// <summary>
        /// 设置指定表的删除语句
        /// </summary>
        /// <param name="table">属性表</param>
        protected string GetDeleteSql(DBTable table)
        {
            return string.Format("delete from {0} where {1}={2}",
                table.Name,
                table.PrimaryKey.Name,
                this.FormatParameterName(table.PrimaryKey.AliasName));
        }

        /// <summary>
        /// 执行删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="IdT"></typeparam>
        /// <param name="id"></param>
        /// <param name="table"></param>
        /// <param name="deleteSql"></param>
        /// <returns></returns>
        protected int Delete<T, IdT>(IdT id, DBTable table, string deleteSql)
        {
            Command.Parameters.Clear();
            AddParameter(this.FormatParameterName(table.PrimaryKey.AliasName), ParameterDirection.Input, id);

            Command.CommandType = CommandType.Text;
            Command.CommandText = deleteSql;

            int result = Command.ExecuteNonQuery();

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <typeparam name="IdT">主键类型</typeparam>
        /// <param name="id">主键值</param>
        /// <returns>影响行数</returns>
        public int Delete<T, IdT>(IdT id)
        {
            Type type = typeof(T);
            DBSql dbsql = MapHelper.GetDBSql(type, _dbContext.DataType);
            if (string.IsNullOrEmpty(dbsql.DeleteSql))//如果该表的删除语句为空，则生成该表的delete语句
            {
                DBTable table = MapHelper.GetDBTable(type);
                dbsql.DeleteSql = GetDeleteSql(table);
            }
            return Delete<T, IdT>(id, dbsql.Table, dbsql.DeleteSql);
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <typeparam name="T">要删除的类型</typeparam>
        /// <param name="where">删除条件</param>
        /// <param name="paras">参数</param>
        /// <returns>影响行数</returns>
        public int Delete<T>(string where, params object[] paras)
        {
            Type type = typeof(T);
            DBTable table = MapHelper.GetDBTable(type);
            DBSql dbsql = MapHelper.GetDBSql(type, _dbContext.DataType);

            where = FormatWhereOrder(dbsql, where);
            Command.CommandText = string.Format("delete {0} {1}", table.Name, where.Length > 0 ? " where " + where : "");
            Command.CommandType = CommandType.Text;
            Command.Parameters.Clear();

            int i = 0;
            foreach (object obj in paras)
            {
                AddParameter(FormatParameterName("p" + (i++).ToString()), ParameterDirection.Input, obj);
            }

            int result = Command.ExecuteNonQuery();

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        #endregion
    }
}
