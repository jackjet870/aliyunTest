using System;
using System.Data.Common;

namespace aliyunTest.Framework.Database.Provider
{
    public class MySQL : DBSession
    {
        public const string ProviderTypeString = "MySql.Data";

        protected override DbProviderFactory CreateDbProviderFactory()
        {
            Type type = GetType(ProviderTypeString, "MySql.Data.MySqlClient.MySqlClientFactory");
            return (DbProviderFactory)Activator.CreateInstance(type, true);
        }

        protected override string FormatParameterName(string p)
        {
            return "@" + p;
        }

        protected override string GuidToString(Guid guid)
        {
            return guid.ToString("N").ToUpper();
        }

        protected override string PrepareCustomSelectPaging(int pageIndex, int pageSize, string fields, string from, string where, string group, string order, object[] paras)
        {
            throw new NotImplementedException();
        }

        protected override string PrepareCustomSelectTopN(int topN, string fields, string from, string where, string group, string order, object[] paras)
        {
            throw new NotImplementedException();
        }

        
    }
}
