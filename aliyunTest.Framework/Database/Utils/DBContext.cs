using System.Data.Common;

namespace aliyunTest.Framework.Database
{
    /// <summary>
    /// 连接的数据库信息
    /// </summary>
    public class DBContext
    {
        /// <summary>
        /// 连接数据库自定义名称，唯一
        /// </summary>
        public string DbKey { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DataType { get; set; }

        /// <summary>
        /// 连接池大小
        /// </summary>
        public int ConnPoolNum { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connectstring { get; set; }

        /// <summary>
        /// 驱动工厂
        /// </summary>
        public DbProviderFactory Factory { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBContext() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbkey">连接数据库自定义唯一名称</param>
        /// <param name="dataType">数据库类型</param>
        /// <param name="conString">连接字符串</param>
        public DBContext(string dbkey, DataBaseType dataType, string conString)
        {
            this.DbKey = dbkey;
            this.DataType = dataType;
            this.Connectstring = conString;
            //默认连接池大小为10
            this.ConnPoolNum = 10;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbkey">连接数据库自定义唯一名称</param>
        /// <param name="dataType">数据库类型</param>
        /// <param name="conString">连接字符串</param>
        /// <param name="conPoolNum">连接池大小</param>
        public DBContext(string dbkey, DataBaseType dataType, string conString, int conPoolNum)
        {
           // if (conPoolNum <= 0) throw new MyDBException("连接池数量必须大于等于1");
            this.DbKey = dbkey;
            this.DataType = dataType;
            this.Connectstring = conString;
            this.ConnPoolNum = conPoolNum;
        }
    }
}
