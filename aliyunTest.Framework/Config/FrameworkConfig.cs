using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace aliyunTest.Framework.Config
{
    /// <summary>
    /// Eilnt FrameWork
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class FrameworkConfig : ConfigBase
    {
        #region 框架属性

        /// <summary>
        /// 是否用Session来保存用户信息,True：用Session保存 False：用Cookies来保存
        /// </summary> 
        [JsonProperty]
        public bool IsSessionAuthor { get; set; }

        /// <summary>
        /// Cookies过期时间
        /// </summary>
        [JsonProperty]
        public int CookiesTimer { get; set; }

        /// <summary>
        /// 后台管理员登陆信息Key
        /// </summary>
        [JsonProperty]
        public string ManageAuthorKey { get; set; }

        /// <summary>
        /// 前台会员登陆信息Key
        /// </summary>
        [JsonProperty]
        public string MemberAuthorKey { get; set; }

        /// <summary>
        /// 上传文件的路径
        /// </summary>
        [JsonProperty]
        public string UploadFilePath { get; set; }

        /// <summary>
        /// IP数据库所在地址
        /// </summary>
        [JsonProperty]
        public string IPDatabasePath { get; set; }

        #endregion

        #region 代码属性
                
        /// <summary>
        /// 数据库的表前缀名
        /// </summary>
        [JsonProperty]
        public string TablePrefix { get; set; }

        /// <summary>
        /// 数据库的链接字符串
        /// </summary>
        [JsonProperty]
        public string ConnectionString { get; set; }


        /// <summary>
        /// 要进行权限反射的Dlls，多个Dll以","分隔
        /// </summary>
        [JsonProperty]
        public string ControllerRefs { get; set; }

        /// <summary>
        /// Cookies 域
        /// </summary>
        [JsonProperty]
        public string Domain { get; set; }

        #endregion

        #region 站点属性

        /// <summary>
        /// 站点名称
        /// </summary>
        [JsonProperty]
        public string SiteName { get; set; }

        /// <summary>
        /// ICP备案号
        /// </summary>
        [JsonProperty]
        public string ICP { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [JsonProperty]
        public string Address { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [JsonProperty]
        public string Tel { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        [JsonProperty]
        public string Email { get; set; }

        [JsonProperty]
        public string EmailUser { get; set; }

        [JsonProperty]
        public string EmailPassword { get; set; }

        [JsonProperty]
        public string EmailHost { get; set; }

        [JsonProperty]
        public int EmailPort { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [JsonProperty]
        public string ContactPerson { get; set; }

        /// <summary>
        /// 微信
        /// </summary>
        [JsonProperty]
        public string Weixin { get; set; }

        /// <summary>
        /// 微博
        /// </summary>
        [JsonProperty]
        public string Weibo { get; set; }

        /// <summary>
        /// 上传视频的路径
        /// </summary>
        [JsonProperty]
        public string UploadVideoPath { get; set; }

        [JsonProperty]
        public string Fax { get; set; }


        #endregion

        #region MVC视图属性

        /// <summary>
        /// 后台管理View 路径
        /// </summary>
        [JsonProperty]
        public string ManageViewPath { get; set; }

        [JsonProperty]
        public string AppViewPath { get; set; }

        /// <summary>
        /// 对话框View 路径
        /// </summary>
        [JsonProperty]
        public string DialogViewPath { get; set; }

        /// <summary>
        /// 普通View 路径
        /// </summary>
        [JsonProperty]
        public string NormalViewPath { get; set; }

        /// <summary>
        /// 前台会员View路径
        /// </summary>
        [JsonProperty]
        public string MemberViewPath { get; set; }

        /// <summary>
        /// 页面主题所在路径
        /// </summary>
        [JsonProperty]
        public string ThemesPath { get; set; }

        /// <summary>
        /// 当前主题
        /// </summary>
        [JsonProperty]
        public string CurrentTheme { get; set; }

        /// <summary>
        /// 通用控件所在路径
        /// </summary>
        [JsonProperty]
        public string ControlPath { get; set; }

        #endregion

       
        public override void InitConfig()
        {
        }
    }
}
