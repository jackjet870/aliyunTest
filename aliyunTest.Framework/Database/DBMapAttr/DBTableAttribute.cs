using System;

namespace aliyunTest.Framework.Database.DBMapAttr
{
    /// <summary>
    /// 数据库表映射属性,使用映射类上
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DBTableAttribute : Attribute
    {
        /// <summary>
        /// 数据库表名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">数据库表名称</param>
        public DBTableAttribute(string name)
        {
            Name = name;
        }
    }
}
