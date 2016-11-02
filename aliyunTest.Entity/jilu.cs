using aliyunTest.Framework.Database.DBMapAttr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aliyunTest.Entity
{
    /// <summary>
    /// fanwen类
    /// </summary>
    [DBTable("jilu")]
    public class jilu
    {
        /// <summary>
        /// ID
        /// </summary>
        [DBPrimaryKey("count", true)]
        public int count
        {
            get;
            set;
        }

        /// <summary>
        /// IP
        /// </summary>
        [DBColumn("ip")]
        public string ip
        {
            get;
            set;
        }

        /// <summary>
        /// 地址
        /// </summary>
        [DBColumn("Area")]
        public string Area
        {
            get;
            set;
        }

        /// <summary>
        /// 宽带供应商
        /// </summary>
        [DBColumn("Kd")]
        public string Kd
        {
            get;
            set;
        }

        /// <summary>
        /// 访问时间
        /// </summary>
        [DBColumn("AccessTime")]
        public DateTime AccessTime
        {
            get;
            set;
        }

        ///// <summary>
        ///// 访问用户
        ///// </summary>
        //[DBColumn("UserName")]
        //public string UserName
        //{
        //    get;
        //    set;
        //}
    }

}
