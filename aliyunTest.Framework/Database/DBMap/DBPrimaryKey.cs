using System.Reflection;

namespace aliyunTest.Framework.Database.DBMap
{
    public class DBPrimaryKey : DBColumn
    {
        private string _equenceName;

        /// <summary>
        /// 是否是自增长
        /// </summary>
        public bool IsIdentity { get; internal set; }

        /// <summary>
        /// 如果是自动增长，对应的序列名称
        /// </summary>
        public string SequenceName { get { return _equenceName; } private set { _equenceName = value.ToUpper(); } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="fieldName"></param>
        /// <param name="identity"></param>
        public DBPrimaryKey(MemberInfo memberInfo, string fieldName, bool identity, string seqName)
            : base(memberInfo, fieldName)
        {
            SequenceName = seqName;
            IsIdentity = identity;
        }
    }
}
