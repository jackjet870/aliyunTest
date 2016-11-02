using System;

namespace aliyunTest.Framework.Database.DBMapAttr
{
    /// <summary>
    /// 数据库列属性映射,用在映射类的Field,Property上,不允许重复使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DBColumnAttribute : Attribute
    {
        /// <summary>
        /// 对应数据库的字段名称
        /// </summary>
        public string FieldName { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">数据库字段名称</param>
        public DBColumnAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}
