using System;

namespace aliyunTest.Framework.Database.DBMapAttr
{
    /// <summary>
    /// 标示列为主键字段,用在映射类的Field,Property上,不允许重复使用
    /// 并且不允许与DBColumnAttribute同时使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DBPrimaryKeyAttribute : DBColumnAttribute
    {
        /// <summary>
        /// 是否是自动增长
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// 主键名称
        /// </summary>
        public string KeyName { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        public DBPrimaryKeyAttribute(string fieldName)
            : this(fieldName, false)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="isIdentity">是否自动增长</param>
        /// <param name="size">字段长度</param>
        public DBPrimaryKeyAttribute(string fieldName, bool isIdentity)
            : base(fieldName)
        {
            IsIdentity = isIdentity;
        }
    }
}
