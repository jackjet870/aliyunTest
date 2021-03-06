﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using aliyunTest.Framework.Config;
using aliyunTest.Framework;

namespace aliyunTest.Framework.Common
{
    /// <summary>
    /// 前台和后台用户登陆信息附助类
    /// </summary>
    public class AuthorHelper
    {
        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="source">要保存的对象</param>
        /// <param name="isManage">为True表示获取管理员否则为前台用户</param>
        public static void SetAuthorInfo(Object source, bool isManage)
        {
            FrameworkConfig config = FrameworkConfig.Instance<FrameworkConfig>();

            if (isManage)
                SetOwnData(config.ManageAuthorKey, source);
            else
                SetOwnData(config.MemberAuthorKey, source);

        }

        /// <summary>
        /// 清除用户登陆信息
        /// </summary>
        /// <param name="isManage">为True表示获取管理员否则为前台用户</param>
        public static void ClearAuthorInfo(bool isManage)
        {
            FrameworkConfig config = FrameworkConfig.Instance<FrameworkConfig>();

            if (isManage)
                ClearOwnData(config.ManageAuthorKey);
            else
                ClearOwnData(config.MemberAuthorKey);

        }

        /// <summary>
        /// 获取自定义数据
        /// </summary>
        /// <param name="key">自定义数据的名称</param>
        /// <returns></returns>
        public static object GetOwnData(string key)
        {
            object result = null;
            if (FrameworkConfig.Instance<FrameworkConfig>().IsSessionAuthor)

                result = HttpContext.Current.Session[key] ?? null;
            else
                result = HttpContext.Current.Request.Cookies[key] == null || string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[key].Value) ? null : HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[key].Value, Encoding.UTF8).Decrypt();


            return result;
        }

        /// <summary>
        /// 添加自定义数据
        /// </summary>
        /// <param name="key">自定义数据的名称</param>
        /// <param name="source">要添加的数据</param>
        public static void SetOwnData(string key, object source)
        {

            FrameworkConfig config = FrameworkConfig.Instance<FrameworkConfig>();

            if (config.IsSessionAuthor)
                HttpContext.Current.Session[key] = source;
            else
            {
                int cookiesTimer = config.CookiesTimer;

                HttpContext.Current.Response.Cookies[key].Value = HttpUtility.UrlEncode(source.ToString().Encrypt(), Encoding.UTF8);

                if (!string.IsNullOrEmpty(config.Domain))
                    HttpContext.Current.Response.Cookies[key].Domain = config.Domain;

                if (cookiesTimer > 0)
                    HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddMinutes(cookiesTimer);
            }
        }

        /// <summary>
        /// 删除自定义数据
        /// </summary>
        /// <param name="key"></param>
        public static void ClearOwnData(string key)
        {
            FrameworkConfig config = FrameworkConfig.Instance<FrameworkConfig>();
            if (config.IsSessionAuthor)

                HttpContext.Current.Session.Remove(key);

            else
            {
                HttpContext.Current.Response.Cookies[key].Value = null;
                HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.MiniDateTime();
                if (!string.IsNullOrEmpty(config.Domain))
                    HttpContext.Current.Response.Cookies[key].Domain = config.Domain;

            }
        }

        /// <summary>
        /// 是否己登陆了true为登陆flase为没登陆
        /// </summary>
        /// <param name="isManage">是否后台用户 ture:后台用户 false:前台用户</param>
        /// <returns>true为登陆flase为没登陆</returns>
        public static bool IsUsing(bool isManage)
        {
            object obj = GetAuthorInfo(isManage);

            if (obj == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 获取当前用户登陆名
        /// </summary>
        /// <param name="isManage">为True表示获取管理员否则为前台用户</param>
        /// <returns>当前用户登陆名，当没有用户时为空</returns>
        public static object GetAuthorInfo(bool isManage)
        {
            object result = null;
            FrameworkConfig config = FrameworkConfig.Instance<FrameworkConfig>();

            if (isManage)
                result = GetOwnData(config.ManageAuthorKey);
            else
                result = GetOwnData(config.MemberAuthorKey);

            return result;
        }

        /// <summary>
        /// 获取当前的生成的验证码
        /// </summary>
        /// <returns></returns>
        public static string GetVerifyCode()
        {
            object obj = GetOwnData(aliyunTest.Framework.StringExtensions.CookiesKey);
            if (obj == null)
                return null;
            else
                return obj.ToString().Trim();
        }
    }
}
