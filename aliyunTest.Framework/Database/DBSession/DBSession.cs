using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using aliyunTest.Framework.Database.Provider;
using aliyunTest.Framework.Database.DBMap;
using System.Data;
using aliyunTest.Framework.Database;
using aliyunTest.Framework.Database.FullData;

namespace aliyunTest.Framework
{
    public abstract partial class DBSession : IDisposable
    {
        /// <summary>
        /// 默认的连接数据库
        /// </summary>
        public static string DefaultDBKey { get; set; }

        /// <summary>
        /// 数据库连接信息
        /// </summary>
        private static IDictionary<string, DBContext> _idicDbContext = new Dictionary<string, DBContext>();

        /// <summary>
        /// 初始化数据库连接会话
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="assemblies"></param>
        public static void InitDBSession(DBContext dbContext, params Assembly[] assemblies)
        {
            //初始化程序集
            if (assemblies != null)
                MapHelper.InitDBMap(dbContext.DataType, assemblies);

            //初始化连接
            if (_idicDbContext.ContainsKey(dbContext.DbKey))
            {
                throw new Exception("该key的数据库已经被注册了！");
            }
            _idicDbContext.Add(dbContext.DbKey, dbContext);

            //初始化连接池
            InitConnPool(dbContext);
        }

        /// <summary>
        /// 返回表的查询名
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns></returns>
        public static string GetFullTableName(string table)
        {
            string tablePreFix = Config.FrameworkConfig.Instance<Config.FrameworkConfig>().TablePrefix;

            return tablePreFix + table;
        }

        #region 连接池管理

        /// <summary>
        /// 数据库连接池
        /// </summary>
        private static ConcurrentDictionary<string, ConcurrentQueue<DBSession>> _idiConPool = new ConcurrentDictionary<string, ConcurrentQueue<DBSession>>();

        /// <summary>
        /// 初始化连接池
        /// </summary>
        private static void InitConnPool(DBContext dbContext)
        {
            if (!_idiConPool.ContainsKey(dbContext.DbKey))
            {
                ConcurrentQueue<DBSession> queDS = new ConcurrentQueue<DBSession>();
                for (int i = 0; i < dbContext.ConnPoolNum; i++)
                {
                    queDS.Enqueue(Create(dbContext));
                }
                _idiConPool.TryAdd(dbContext.DbKey, queDS);
            }
        }

        /// <summary>
        /// 创建数据库连接会话
        /// </summary>
        /// <param name="dbContext"></param>
        private static DBSession Create(DBContext dbContext)
        {
            DBSession session = null;
            switch (dbContext.DataType)
            {
                case DataBaseType.SQLServer:
                    session = new SQLServer();
                    break;
                case DataBaseType.Oracle:
                    session = new Oracle();
                    break;
                case DataBaseType.MySQL:
                    session = new MySQL();
                    break;
                case DataBaseType.SQLite:
                    session = new SQLite();
                    break;
            }

            if (session == null) throw new MyDBException("未知的数据库连接！");

            //设置session的数据库连接对象
            session._dbContext = dbContext;
            //创建该Session的数据库连接
            session.CreateConnection();

            //初始化当前连接的 数据填充对象
            session._fullDataReader = new FullDataReader();

            return session;
        }

        /// <summary>
        /// 获取默认数据库的连接Session
        /// </summary>
        /// <returns></returns>
        public static DBSession TryGet()
        {
            return TryGet(DefaultDBKey);
        }

        /// <summary>
        /// 获取默认数据库的连接Session
        /// </summary>
        /// <param name="runAfterCloseConnection">执行完命令是否关闭数据库链接</param>
        /// <returns></returns>
        public static DBSession TryGet(bool runAfterCloseConnection)
        {
            return TryGet(DefaultDBKey, runAfterCloseConnection);
        }


        /// <summary>
        /// 获取指定数据库的连接Session
        /// </summary>
        /// <param name="dbKey">数据库Key</param>
        /// <returns></returns>
        /// 
        public static DBSession TryGet(string dbKey)
        {
            return TryGet(dbKey, true);
        }

        /// <summary>
        /// 获取指定数据库的连接Session
        /// </summary>
        /// <param name="dbKey">数据库Key</param>
        /// <param name="runAfterCloseConnection">执行完命令是否关闭数据库链接</param>
        /// <returns></returns>
        public static DBSession TryGet(string dbKey, bool runAfterCloseConnection)
        {
            if (!_idiConPool.ContainsKey(dbKey))
                throw new MyDBException(string.Format("不存在该[{0}]的数据库", dbKey));

            DBSession dbSession = null;
            if (!_idiConPool[dbKey].TryDequeue(out dbSession))//如果未取到数据库连接，则重新创建一个连接
            {
                DBContext context = null;
                if (_idicDbContext.TryGetValue(dbKey, out context))
                    dbSession = Create(context);
                else
                    throw new MyDBException(string.Format("不存在该[{0}]的数据库", dbKey));
            }

            //当前对是否执行了Dispose
            dbSession.IsDisposed = false;

            //设置数据库链接关闭属为True
            dbSession.IsClosedConnection = runAfterCloseConnection;


            if (dbSession.Connection.State != ConnectionState.Closed)
            {
                dbSession.Connection.Close();
            }
            else
                dbSession.Connection.Open();


            return dbSession;
        }

        /// <summary>
        /// 是事存在数据库链接
        /// </summary>
        /// <param name="dbKey">DBKeyy</param>
        /// <returns></returns>
        public static bool HasDbKey(string dbKey)
        {
            return _idiConPool.ContainsKey(dbKey);
        }

        /// <summary>
        /// 释放对象时，将使用完成的对象放入连接池中
        /// </summary>
        public void Dispose()
        {
            if (this.Connection.State == ConnectionState.Closed)
                return;

            //清除所有参数
            this.ClearAllParameter();

            //关闭连接
            this.Connection.Close();

            //如果连接池中连接数大于了 指定数量，则直接销毁该对象
            ConcurrentQueue<DBSession> conDBsession = null;
            _idiConPool.TryGetValue(this._dbContext.DbKey, out conDBsession);
            if (!_idiConPool.ContainsKey(this._dbContext.DbKey))
                throw new MyDBException("不存在该key的数据库");

            if (conDBsession.Count >= _dbContext.ConnPoolNum)
            {
                if (!this.IsDisposed)
                    this.CloseConnection();
            }
            else
            {
                this.IsDisposed = true;

                //将对象放入连接池
                conDBsession.Enqueue(this);
            }

        }


        #endregion
    }


}
