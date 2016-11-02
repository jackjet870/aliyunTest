using System;
using System.Xml;
using System.Web;
using System.IO;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace aliyunTest.Framework.Config
{
    /// <summary>
    /// 配置文件基类 子类必需重写InitConfig方法,并指定“配置文件的路径”
    /// </summary> 
    public abstract class ConfigBase
    {

        public ConfigBase()
        { InitConfig(); }

        /// <summary>
        /// 初始化配置文件的路径
        /// </summary>
        public abstract void InitConfig();

        #region 局部变量

        public readonly static string RootPath = HttpContext.Current == null ? (AppDomain.CurrentDomain.BaseDirectory) + @"Config\" : HttpContext.Current.Server.MapPath("~/Config/");

        /// <summary>
        /// 配置文件所在路径(相对，会应用程序的设目录上加上指定的路径)
        /// </summary>
        protected string SaveJsonPath = "Framework.config";

        /// <summary>
        /// 缓存变量
        /// </summary>
        private static Dictionary<string, ConfigBase> JsonCache = new Dictionary<string, ConfigBase>();

        #endregion

        #region 实例方法

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void ClearCache()
        {
            JsonCache.Clear();
        }

        /// <summary>
        /// 保存到配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Save()
        {
            System.IO.File.WriteAllText(RootPath + SaveJsonPath, JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented), System.Text.Encoding.UTF8);

            //加入缓存
            if (!JsonCache.ContainsKey(this.GetType().GUID.ToString()))
                JsonCache.Add(this.GetType().GUID.ToString(), this);
            else
                JsonCache[this.GetType().GUID.ToString()] = this;


        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T RevertJson<T>() where T : ConfigBase, new()
        {
            try
            {

                T temp = JsonConvert.DeserializeObject<T>(System.IO.File.ReadAllText(RootPath + SaveJsonPath, System.Text.Encoding.UTF8));
                //加入缓存
                if (!JsonCache.ContainsKey(typeof(T).GUID.ToString()))
                    JsonCache.Add(typeof(T).GUID.ToString(), temp);

                return temp;
            }
            catch
            {
                return new T();
            }

        }

        #endregion

        #region 实例函数

        /// <summary>
        /// 静态方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Instance<T>() where T : ConfigBase, new()
        {
            ///加入缓存
            if (JsonCache.ContainsKey(typeof(T).GUID.ToString()))
                return (T)JsonCache[typeof(T).GUID.ToString()];

            return new T().RevertJson<T>();
        }

        /// <summary>
        /// 移除指定类型的缓存
        /// </summary>
        /// <typeparam name="T">要移除对象的类型</typeparam>
        /// <returns></returns>
        public static bool RemoveInstance<T>() where T : ConfigBase, new()
        {
            string typeGuid = typeof(T).GUID.ToString();
            if (JsonCache.ContainsKey(typeGuid))
            {
                return JsonCache.Remove(typeGuid);
            }
            else
            {
                return false;
            }
        }


        #endregion


    }

}