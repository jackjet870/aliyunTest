
namespace aliyunTest.Framework.Database
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DataBaseType
    {
        SQLServer = 1,
        Oracle = 2,
        MySQL = 3,
        SQLite = 4
    }

    /// <summary>
    /// 数据库字段类型
    /// </summary>
    public enum DBColumnType
    {
        Boolean,
        Byte,
        SByte,
        Char,
        Decimal,
        Double,
        Single,
        Int32,
        UInt32,
        Int16,
        UInt16,
        Int64,
        UInt64,
        String,
        DateTime,
        Guid,
        TimeSpan,
        ByteArray,
        Xml
    }
}
