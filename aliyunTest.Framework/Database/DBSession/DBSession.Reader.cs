using System.Collections.Generic;
using System.Text;
using System.Data;
using aliyunTest.Framework.Database.DBMap;
using aliyunTest.Framework.Database.FullData;
using aliyunTest.Framework.Database;

namespace aliyunTest.Framework
{
    public abstract partial class DBSession
    {
        /// <summary>
        /// 当前会话的数据填充对象
        /// </summary>
        private FullDataReader _fullDataReader;

        #region 映射类Select方法

        /// <summary>
        /// 通过ID获取指定类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(object id)
        {
            DBSql dbSql = MapHelper.GetDBSql(typeof(T), _dbContext.DataType);

            DBTable table = dbSql.Table;

            string pName = this.FormatParameterName(table.PrimaryKey.AliasName);
            string sql = string.Format("select * from {0} where {1}={2}", table.Name, table.PrimaryKey.Name, pName);

            Command.Parameters.Clear();
            //添加查询条件参数
            AddParameter(this.FormatParameterName(table.PrimaryKey.AliasName), ParameterDirection.Input, id);
            Command.CommandText = sql;
            Command.CommandType = CommandType.Text;

            T result = default(T);

            using (IDataReader reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = _fullDataReader.CreateDegFullMapObj<T>(reader)(reader);
                }
            }


            //执行数据库操作后的处理
            RunAfterFunc();

            return result;

        }

        /// <summary>
        /// 根据条件查询对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public T GetObject<T>(string where, params object[] paras)
        {
            DBSql dbSql = MapHelper.GetDBSql(typeof(T), _dbContext.DataType);

            DBTable table = dbSql.Table;

            string pName = this.FormatParameterName(table.PrimaryKey.AliasName);
            string sql = string.Format("select * from {0} {1}",
                table.Name,
                string.IsNullOrEmpty(where) ? "" : "where " + FormatWhereOrder(dbSql, where));

            Command.CommandText = sql;
            Command.CommandType = CommandType.Text;
            Command.Parameters.Clear();

            int i = 0;
            foreach (object obj in paras)
            {
                AddParameter(FormatParameterName("p" + (i++).ToString()), ParameterDirection.Input, obj);
            }

            T result = default(T);

            using (IDataReader reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = _fullDataReader.CreateDegFullMapObj<T>(reader)(reader);
                }
            }
            //执行数据库操作后的处理
            RunAfterFunc();
            return result;
        }

        /// <summary>
        /// 根据SQL语句查询对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public T GetObjectBySQL<T>(string sql, params object[] paras)
        {
            DBSql dbSql = MapHelper.GetDBSql(typeof(T), _dbContext.DataType);
            Command.CommandText = FormatWhereOrder(dbSql, sql);
            Command.CommandType = CommandType.Text;
            Command.Parameters.Clear();
            int i = 0;
            foreach (object obj in paras)
            {
                AddParameter(FormatParameterName("p" + (i++).ToString()), ParameterDirection.Input, obj);
            }

            T result = default(T);

            using (IDataReader reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = _fullDataReader.CreateDegFullMapObj<T>(reader)(reader);
                }
            }
            //执行数据库操作后的处理
            RunAfterFunc();
            return result;
        }

        /// <summary>
        /// 根据SQL语句查询列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> GetListBySQL<T>(string sql, params object[] paras)
        {
            DBSql dbSql = MapHelper.GetDBSql(typeof(T), _dbContext.DataType);
            Command.CommandText = FormatWhereOrder(dbSql, sql);
            Command.CommandType = CommandType.Text;
            Command.Parameters.Clear();
            int i = 0;
            foreach (object obj in paras)
            {
                AddParameter(FormatParameterName("p" + (i++).ToString()), ParameterDirection.Input, obj);
            }

            List<T> result = null;

            using (IDataReader reader = Command.ExecuteReader())
            {
                result = _fullDataReader.CreateDegFullMapList<T>(reader)(reader);
            }


            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        /// <summary>
        /// 根据条件获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string where, string order, params object[] paras)
        {
            return GetTopList<T>(where, order, 0, paras);
        }

        /// <summary>
        /// 根据条件获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="top">返回多少条</param>
        /// <param name="paras"></param>
        /// <returns></returns>

        public List<T> GetTopList<T>(string where, string order, int top, params object[] paras)
        {
            DBSql dbSql = MapHelper.GetDBSql(typeof(T), _dbContext.DataType);

            DBTable table = dbSql.Table;

            string pName = this.FormatParameterName(table.PrimaryKey.AliasName);
            string sql = string.Format("select " + (top == 0 ? "" : "top " + top.ToString()) + " * from {0} {1} {2}",
                table.Name,
                string.IsNullOrEmpty(where) ? "" : "where " + FormatWhereOrder(dbSql, where),
                string.IsNullOrEmpty(order) ? "" : string.Format(" order by {0}", order));

            Command.CommandText = sql;
            Command.CommandType = CommandType.Text;
            Command.Parameters.Clear();

            int i = 0;
            foreach (object obj in paras)
            {
                AddParameter(FormatParameterName("p" + (i++).ToString()), ParameterDirection.Input, obj);
            }
            List<T> result = null;

            using (IDataReader reader = Command.ExecuteReader())
            {
                result = _fullDataReader.CreateDegFullMapList<T>(reader)(reader);
            }

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        #endregion

        #region 自定义类型Select方法

        /// <summary>
        /// 获取自定义的对象列表列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public List<T> GetCustomerList<T>(string sql, params object[] paras)
        {
            Command.CommandText = FormatSqlForParameter(sql);
            Command.CommandType = CommandType.Text;
            Command.Parameters.Clear();
            int i = 0;
            foreach (object obj in paras)
            {
                AddParameter(FormatParameterName("p" + (i++).ToString()), ParameterDirection.Input, obj);
            }

            List<T> result = null;

            using (IDataReader reader = Command.ExecuteReader())
            {
                result = _fullDataReader.CreateDegFullCustomList<T>(reader)(reader);
            }

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        /// <summary>
        /// 根据SQL语句查询对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public T GetCustomerObject<T>(string sql, params object[] paras)
        {
            Command.CommandText = FormatSqlForParameter(sql);
            Command.CommandType = CommandType.Text;
            Command.Parameters.Clear();
            int i = 0;
            foreach (object obj in paras)
            {
                AddParameter(FormatParameterName("p" + (i++).ToString()), ParameterDirection.Input, obj);
            }

            T result = default(T);

            using (IDataReader reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = _fullDataReader.CreateDegFullCustomObj<T>(reader)(reader);
                }
            }

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }


        /// <summary>
        /// 获取自定义对象列表,自己给定SQL语句
        /// 通过字段名称与属性名称匹配来进行填充(不区分大小写)
        /// </summary>
        /// <typeparam name="T">返回的对象类型</typeparam>
        /// <param name="pageIndex">分页索引，以1开始</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="before">加在SQL语句最前面</param>
        /// <param name="fields">字段列表，以“,”分隔</param>
        /// <param name="from">表名称,比如t_a left join t_b on t_a.id=t_b.id</param>
        /// <param name="where">Where 条件，参数用？代替</param>
        /// <param name="group">Group by 子句</param>
        /// <param name="order">排序方式,不包含"order by"</param>
        /// <param name="paras">条件参数</param>
        /// <returns>List</returns>
        public List<T> GetCustomPagingList<T>(
            int pageIndex,
            int pageSize,
            string before,
            string fields,
            string from,
            string where,
            string group,
            string order,
            params object[] paras)
        {
            Command.CommandType = CommandType.Text;
            string sql = PrepareCustomSelectPaging(pageIndex, pageSize, fields, from, where, group, order, paras);
            Command.CommandText = string.Format("{0} {1}", before, sql);
            List<T> result = new List<T>();

            using (IDataReader reader = Command.ExecuteReader())
            {
                result = _fullDataReader.CreateDegFullCustomList<T>(reader)(reader);
            }

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        #endregion

        #region 动态类Select方法

        /// <summary>
        /// 获取自定义对象列表,自己给定SQL语句
        /// 通过字段名称与属性名称匹配来进行填充(不区分大小写)
        /// </summary>
        /// <param name="sql">sql语句,其中参数以?代替</param>
        /// <param name="paras">传入的参数</param>
        /// <returns>List</returns>
        public object GetDynamicList(string sql, params object[] paras)
        {
            Command.CommandType = CommandType.Text;
            Command.CommandText = PrepareCustomSelect(sql, paras);

            object result = null;
            using (IDataReader reader = Command.ExecuteReader())
            {
                result = _fullDataReader.CreateDegFullDynamicList(reader)(reader);
            }

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        /// <summary>
        /// 获取符合条件的第一条记录
        /// 给定SQL语句，用于获取自定义对象
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="paras">参数列表</param>
        /// <returns>T</returns>
        public object GetDynamicObject(string sql, params object[] paras)
        {
            Command.CommandType = CommandType.Text;
            Command.CommandText = PrepareCustomSelect(sql, paras);

            object result = null;

            using (IDataReader reader = Command.ExecuteReader(CommandBehavior.SingleRow))
            {
                while (reader.Read())
                {
                    result = _fullDataReader.CreateDegDynamicFullObj(reader)(reader);
                }
            }

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        /// <summary>
        /// 获取自定义对象列表,自己给定SQL语句
        /// 通过字段名称与属性名称匹配来进行填充(不区分大小写)
        /// </summary>
        /// <param name="pageIndex">分页索引，以1开始</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="fields">字段列表，以“,”分隔</param>
        /// <param name="from">表名称,比如t_a left join t_b on t_a.id=t_b.id</param>
        /// <param name="where">Where 条件，参数用？代替</param>
        /// <param name="group">Group by 子句</param>
        /// <param name="order">排序方式,不包含"order by"</param>
        /// <param name="paras">条件参数</param>
        /// <returns>List</returns>
        public object GetDynamicPagingList(
            int pageIndex,
            int pageSize,
            string fields,
            string from,
            string where,
            string group,
            string order,
            params object[] paras)
        {
            Command.CommandType = CommandType.Text;
            Command.CommandText = PrepareCustomSelectPaging(pageIndex, pageSize, fields, from, where, group, order, paras);
            object result = null;
            using (IDataReader reader = Command.ExecuteReader())
            {
                result = _fullDataReader.CreateDegFullDynamicList(reader)(reader);
            }

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        #endregion

        /// <summary>
        /// 获取自定义对象列表,自己给定SQL语句
        /// 通过字段名称与属性名称匹配来进行填充(不区分大小写)
        /// </summary>
        /// <param name="pageIndex">分页索引，以1开始</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="fields">字段列表，以“,”分隔</param>
        /// <param name="from">表名称,比如t_a left join t_b on t_a.id=t_b.id</param>
        /// <param name="where">Where 条件，参数用？代替</param>
        /// <param name="group">Group by 子句</param>
        /// <param name="order">排序方式,不包含"order by"</param>
        /// <param name="paras">条件参数</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTablePaging(
            int pageIndex,
            int pageSize,
            string fields,
            string from,
            string where,
            string group,
            string order,
            params object[] paras)
        {
            Command.CommandType = CommandType.Text;
            Command.CommandText = PrepareCustomSelectPaging(pageIndex, pageSize, fields, from, where, group, order, paras);
            DataTable dt = new DataTable();
            dt.Load(Command.ExecuteReader());


            //执行数据库操作后的处理
            RunAfterFunc();

            return dt;
        }

        #region 子类重写分页SQL语句

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
        protected abstract string PrepareCustomSelectPaging(int pageIndex, int pageSize, string fields, string from, string where, string group, string order, object[] paras);

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
        protected abstract string PrepareCustomSelectTopN(int topN, string fields, string from, string where, string group, string order, object[] paras);

        #endregion
    }
}
