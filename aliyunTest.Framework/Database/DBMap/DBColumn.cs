using System;
using System.Reflection;
using System.Xml;
using aliyunTest.Framework.Database.FullData;

namespace aliyunTest.Framework.Database.DBMap
{
    /// <summary>
    /// 列映射实体
    /// </summary>
    public class DBColumn
    {
        private string _name;
        private string _aliasName;

        /// <summary>
        /// 数据列类型
        /// </summary>
        public DBColumnType ColumnType { get; protected set; }

        /// <summary>
        /// 数据库字段名称
        /// </summary>
        public string Name { get { return _name; } private set { _name = value.ToUpper(); } }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string AliasName { get { return _aliasName; } private set { _aliasName = value.ToUpper(); } }

        /// <summary>
        /// 映射属性的数据类型
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// 获取属性值委托
        /// </summary>
        public DegGetValue GetHandler { get; private set; }
        /// <summary>
        /// 设置属性值委托
        /// </summary>
        public DegSetValue SetHandler { get; private set; }

        /// <summary>
        /// 属性信息
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }
        /// <summary>
        /// 字段信息
        /// </summary>
        public FieldInfo FieldInfo { get; private set; }

        private ObjectHelper _objHelper = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="memberInfo">映射成员信息</param>
        /// <param name="fieldName">数据库字段名</param>
        public DBColumn(MemberInfo memberInfo, string fieldName)
        {
            _objHelper = new ObjectHelper();

            if (memberInfo is PropertyInfo)
            {
                PropertyInfo = (PropertyInfo)memberInfo;
                Type = PropertyInfo.PropertyType;
            }
            else if (memberInfo is FieldInfo)
            {
                FieldInfo = (FieldInfo)memberInfo;
                Type = FieldInfo.FieldType;
            }
            else
            {
                throw new MyDBException("在构造DBColumn[" + memberInfo.Name + "]时,传入了无效的类型!");
            }

            if (Type.IsGenericType && Type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type = Nullable.GetUnderlyingType(Type);
            }

            GetHandler = _objHelper.CreateDegGetValue(memberInfo.DeclaringType, memberInfo.Name);
            SetHandler = _objHelper.CreateDegSetValue(memberInfo.DeclaringType, memberInfo.Name);

            ColumnType = GetColumnType(Type);
            Name = fieldName;
            AliasName = memberInfo.Name;
        }

        private static DBColumnType GetColumnType(Type type)
        {
            if (type == typeof(string))
                return DBColumnType.String;
            else if (type == typeof(bool))
                return DBColumnType.Boolean;
            else if (type == typeof(byte))
                return DBColumnType.Byte;
            else if (type == typeof(sbyte))
                return DBColumnType.SByte;
            else if (type == typeof(char))
                return DBColumnType.Char;
            else if (type == typeof(decimal))
                return DBColumnType.Decimal;
            else if (type == typeof(double))
                return DBColumnType.Double;
            else if (type == typeof(float))
                return DBColumnType.Single;
            else if (type == typeof(int) || type.IsEnum)
                return DBColumnType.Int32;
            else if (type == typeof(uint))
                return DBColumnType.UInt32;
            else if (type == typeof(short))
                return DBColumnType.Int16;
            else if (type == typeof(ushort))
                return DBColumnType.UInt16;
            else if (type == typeof(long))
                return DBColumnType.Int64;
            else if (type == typeof(ulong))
                return DBColumnType.UInt64;
            else if (type == typeof(DateTime))
                return DBColumnType.DateTime;
            else if (type == typeof(Guid))
                return DBColumnType.Guid;
            else if (type == typeof(TimeSpan))
                return DBColumnType.TimeSpan;
            else if (type == typeof(byte[]))
                return DBColumnType.ByteArray;
            else if (type == typeof(XmlDocument))
                return DBColumnType.Xml;
            else
                throw new Exception("不支持数据类型映射[" + type.ToString() + "]!");
        }

    }
}
