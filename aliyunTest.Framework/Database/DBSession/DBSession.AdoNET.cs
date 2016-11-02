using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using aliyunTest.Framework.Database;

namespace aliyunTest.Framework
{
    public abstract partial class DBSession
    {
        #region 数据库链接状态参数

        /// <summary>
        /// 当前对是否执行了Dispose,内部标识变量
        /// </summary>
        private bool __ISDISPOSED = false;

        /// <summary>
        /// 当前对是否执行了Dispose
        /// </summary>
        public bool IsDisposed
        {
            get { return __ISDISPOSED; }
            set { __ISDISPOSED = value; }
        }



        /// <summary>
        /// 是否执行完关闭数据库链接，内部标识变量
        /// </summary>
        private bool __ISCLOSEDCONNECTION = true;

        /// <summary>
        /// 是否执行完关闭数据库链接,默认为true
        /// </summary>
        public bool IsClosedConnection
        {
            get { return __ISCLOSEDCONNECTION; }
            set { __ISCLOSEDCONNECTION = value; }
        }

        /// <summary>
        /// 数据库命令执行完毕后的处理
        /// </summary>
        /// <returns></returns>
        protected void RunAfterFunc()
        {
            if (IsClosedConnection)
                Dispose();
        }

        #endregion

        /// <summary>
        /// 连接
        /// </summary>
        protected IDbConnection Connection { get; set; }

        /// <summary>
        /// Command
        /// </summary>
        public IDbCommand Command { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        private DBContext _dbContext;

        /// <summary>
        /// 获取连接驱动工厂
        /// </summary>
        /// <returns></returns>
        protected abstract DbProviderFactory CreateDbProviderFactory();

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        public void CreateConnection()
        {
            _dbContext.Factory = CreateDbProviderFactory();
            Connection = _dbContext.Factory.CreateConnection();
            Connection.ConnectionString = _dbContext.Connectstring;
            Command = Connection.CreateCommand();
        }

        /// <summary>
        /// 手动关闭数据库连接
        /// </summary>
        private void CloseConnection()
        {
            try
            {
                if (Command != null)
                {
                    Command.Dispose();
                }
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
                Connection.Dispose();
            }
            catch (Exception)
            { }
        }

        #region 事物控制

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction()
        {
            if (Command.Transaction != null)
            {
                throw new Exception("已经处于一个事务中,请提交或者回滚当前事务后再启动事务!");
            }
            Command.Transaction = Connection.BeginTransaction();

            IsClosedConnection = false;

            return Command.Transaction;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public void Commit()
        {
            if (Command.Transaction != null)
            {
                Command.Transaction.Commit();
                if (Command.Transaction != null)
                {
                    Command.Transaction.Dispose();
                    Command.Transaction = null;
                }
            }

            IsClosedConnection = true;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns></returns>
        public void Rollback()
        {
            if (Command.Transaction != null)
            {
                Command.Transaction.Rollback();
                if (Command.Transaction != null)
                {
                    Command.Transaction.Dispose();
                    Command.Transaction = null;
                }
            }

            IsClosedConnection = true;
        }

        #endregion

        #region 添加参数

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="dbtype">参数类型</param>
        /// <param name="direction">参数值</param>
        /// <returns></returns>
        public virtual IDataParameter AddParameter(string name, DbType dbtype, ParameterDirection direction, object value)
        {
            IDataParameter parameter = CreateParameter(name, direction, value);
            parameter.DbType = dbtype;
            Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="dbtype">参数类型</param>
        /// <param name="direction">参数值</param>
        /// <returns></returns>
        public virtual IDataParameter AddParameter(string name, ParameterDirection direction, object value)
        {
            IDataParameter parameter = CreateParameter(name, direction, value);
            Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="dbtype">参数类型</param>
        /// <param name="direction">参数值</param>
        /// <returns></returns>
        public virtual IDataParameter AddParameter(string name, ParameterDirection direction, object value, DBColumnType dbColumType)
        {
            IDataParameter parameter = CreateParameter(name, direction, value);
            switch (dbColumType)
            {
                case DBColumnType.Boolean:
                    parameter.DbType = DbType.Boolean;
                    break;
                case DBColumnType.Byte:
                    parameter.DbType = DbType.Byte;
                    break;
                case DBColumnType.SByte:
                    parameter.DbType = DbType.SByte;
                    break;
                case DBColumnType.Char:
                    parameter.DbType = DbType.String;
                    break;
                case DBColumnType.Decimal:
                    parameter.DbType = DbType.Decimal;
                    break;
                case DBColumnType.Double:
                    parameter.DbType = DbType.Double;
                    break;
                case DBColumnType.Single:
                    parameter.DbType = DbType.Single;
                    break;
                case DBColumnType.Int32:
                    parameter.DbType = DbType.Int32;
                    break;
                case DBColumnType.UInt32:
                    parameter.DbType = DbType.UInt32;
                    break;
                case DBColumnType.Int16:
                    parameter.DbType = DbType.Int16;
                    break;
                case DBColumnType.UInt16:
                    parameter.DbType = DbType.UInt16;
                    break;
                case DBColumnType.Int64:
                    parameter.DbType = DbType.Int64;
                    break;
                case DBColumnType.UInt64:
                    parameter.DbType = DbType.UInt64;
                    break;
                case DBColumnType.String:
                    parameter.DbType = DbType.String;
                    break;
                case DBColumnType.DateTime:
                    parameter.DbType = DbType.DateTime;
                    break;
                case DBColumnType.Guid:
                    parameter.DbType = DbType.Guid;
                    break;
                case DBColumnType.TimeSpan:
                    parameter.DbType = DbType.DateTime;
                    break;
                case DBColumnType.ByteArray:
                    parameter.DbType = DbType.Binary;
                    break;
                case DBColumnType.Xml:
                    parameter.DbType = DbType.Xml;
                    break;
                default:
                    break;
            }
            Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="dbtype">参数类型</param>
        /// <param name="direction">参数值</param>
        /// <returns></returns>
        public virtual IDataParameter AddParameter(string name, object value, DbType dbType)
        {
            IDataParameter parameter = CreateParameter(name, ParameterDirection.Input, value);
            parameter.DbType = dbType;
            Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// 指定存储过程的返回参数
        /// </summary>
        /// <param name="cmd">数据库命令</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="dbType">数据类型</param>
        public void AddReturnParameter(string parameterName, DbType dbType)
        {
            IDataParameter dbParameter = Command.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = ParameterDirection.ReturnValue;
            Command.Parameters.Add(dbParameter);
        }

        /// <summary>
        /// 指定存储过程的返回参数
        /// </summary>
        /// <param name="cmd">数据库命令</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="dbType">数据类型</param>
        public void AddOutParameter(string parameterName, DbType dbType, object value)
        {
            IDataParameter dbParameter = Command.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = ParameterDirection.Output;
            dbParameter.Value = value;
            Command.Parameters.Add(dbParameter);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="cmd">数据库命令</param>
        /// <param name="parameterName">参数名</param>
        /// <returns></returns>
        public DbParameter GetParameter(string parameterName)
        {
            return (DbParameter)Command.Parameters[parameterName];
        }

        /// <summary>
        /// 清除所有参数
        /// </summary>
        public virtual void ClearAllParameter()
        {
            Command.Parameters.Clear();
        }

        /// <summary>
        /// 创建参数
        /// 注意:所创建参数不自动增加到Command
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="direction">参数输入输出方向</param>
        /// <param name="value">参数值</param>
        /// <returns>创建好的参数</returns>
        protected virtual IDataParameter CreateParameter(string name, ParameterDirection direction, object value)
        {
            IDataParameter parameter = Command.CreateParameter();

            if (value is Guid) value = GuidToString((Guid)value);

            parameter.ParameterName = name;
            parameter.Direction = direction;
            parameter.Value = value == null ? DBNull.Value : value;
            return parameter;
        }

        #endregion

        #region  执行命令

        /// <summary>
        /// 执行增、删、改
        /// </summary>
        /// <param name="cmdstring">执行的sql语句</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params object[] paras)
        {
            sql = PrepareCustomSelect(sql, paras);
            Command.CommandText = sql;
            Command.CommandType = CommandType.Text;

            int result = Command.ExecuteNonQuery();

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;

        }

        /// <summary>
        /// 获取查询结果的第一行第一列
        /// </summary>
        /// <param name="sql">查询的sql语句</param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params object[] paras)
        {
            sql = PrepareCustomSelect(sql, paras);
            Command.CommandText = sql;
            Command.CommandType = CommandType.Text;

            object result = Command.ExecuteScalar();

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        /// <summary>
        /// 获取查询结果的第一行第一列
        /// </summary>
        /// <param name="sql">查询的sql语句</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, params object[] paras)
        {
            object obj = ExecuteScalar(sql, paras);
            if (obj is DBNull) return default(T);
            if (obj == null) return default(T);
            T result = (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        /// <summary>
        /// 获取一个DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetTable(string sql, params object[] paras)
        {
            Command.CommandText = FormatSqlForParameter(sql);
            Command.CommandType = CommandType.Text;
            Command.Parameters.Clear();
            int i = 0;
            foreach (object obj in paras)
            {
                AddParameter(FormatParameterName("p" + (i++).ToString()), ParameterDirection.Input, obj);
            }
            DataTable dt = new DataTable();
            dt.Load(Command.ExecuteReader());

            //执行数据库操作后的处理
            RunAfterFunc();


            return dt;
        }

        /// <summary>
        /// 读取一个Reader
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, params object[] paras)
        {
            Command.CommandText = sql;
            Command.CommandType = CommandType.Text;

            IDataReader result = Command.ExecuteReader();

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        /// <summary>
        /// 执行存储过程 并返回一张表
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="returnParamName">如果有返回值，返回值的名称例如："@costMoney"</param>
        /// <returns></returns>
        public DataTable ExecuteProcRtuTB(string procName)
        {
            Command.CommandText = procName;
            Command.CommandType = CommandType.StoredProcedure;

            DataTable dt = new DataTable();
            dt.Load(Command.ExecuteReader());
            //执行数据库操作后的处理
            RunAfterFunc();

            return dt;
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="returnParamName">如果有返回值，返回值的名称例如："@costMoney"</param>
        /// <returns></returns>
        public object ExecuteProc(string procName)
        {
            Command.CommandText = procName;
            Command.CommandType = CommandType.StoredProcedure;

            object result = Command.ExecuteNonQuery();

            //执行数据库操作后的处理
            RunAfterFunc();

            return result;
        }

        #endregion
    }
}
