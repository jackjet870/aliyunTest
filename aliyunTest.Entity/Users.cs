using aliyunTest.Framework.Database.DBMapAttr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aliyunTest.Entity
{
    /// <summary>
    /// 用户类
    /// </summary>
    [DBTable("Users")]
    public class Users
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DBPrimaryKey("UserID", true)]
        public int UserID
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DBColumn("CreateTime")]
        public DateTime CreateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [DBColumn("UserName")]
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        [DBColumn("UserPwd")]
        public string UserPwd
        {
            get;
            set;
        }

        /// <summary>
        /// 手机号码
        /// </summary>
        [DBColumn("Phone")]
        public string Phone
        {
            get;
            set;
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DBColumn("Email")]
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 头像
        /// </summary>
        [DBColumn("UserImg")]
        public string UserImg
        {
            get;
            set;
        }


    }

}
