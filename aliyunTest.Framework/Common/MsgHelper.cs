using System.Collections;

namespace aliyunTest.Framework
{
    /// <summary>
    /// 信息附助类,可以在各个View,Controller里调用一次
    /// </summary>
    public class MsgHelper
    {
        private static string _key = "86F500E0-5571-472A-842A-44B393318DB0";

        /// <summary>
        /// 插入指定键（Result）的信息
        /// </summary>
        /// <param name="value"></param>
        public static void InsertResult(string value)
        {
            Insert("result", value);
        }

        /// <summary>
        /// 插入信息
        /// </summary>
        /// <param name="key">信息键</param>
        /// <param name="value">信息值</param>
        public static void Insert(string key, string value)
        {
            Hashtable msg = System.Web.HttpContext.Current.Session[_key] as Hashtable;

            if (msg == null)
                msg = new Hashtable();

            if (msg.Contains(key)) msg[key] = value;
            else msg.Add(key, value);

            Save(msg);
        }

        /// <summary>
        /// 获取以Result为键的信息不带格式化的
        /// </summary>
        /// <returns></returns>
        public static string ShowResult()
        {
            return ShowResult(false);
        }

        /// <summary>
        /// 获取以Result为键的信息
        /// </summary>
        /// <param name="isFormart">是否要格式化</param>
        /// <returns></returns>
        public static string ShowResult(bool isFormart)
        {
            Hashtable msg = System.Web.HttpContext.Current.Session[_key] as Hashtable;

            string key = "result";

            if (IsVaild || !msg.Contains(key))
                return "";

            Save(msg);

            string values = msg[key].ToString();

            if (isFormart)
                values = "<div class=\"MsgHelper_Tips\">" + msg[key].ToString() + "</div>";


            msg.Remove(key);

            return values;
        }

        /// <summary>
        /// 获取信息不带格式化的
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Show(string key)
        {
            return Show(key, false);
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="key">信息键</param>
        /// <param name="isFormart">是否格式化</param>
        /// <returns></returns>
        public static string Show(string key, bool isFormart)
        {
            Hashtable msg = System.Web.HttpContext.Current.Session[_key] as Hashtable;

            if (IsVaild || !msg.Contains(key))
                return "";

            Save(msg);

            string values = msg[key].ToString();

            if (isFormart)
                values = "<span class=\"field-validation-error\">" + msg[key].ToString() + "</span>";

            msg.Remove(key);

            return values;
        }

        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <returns>获取所有信息</returns>
        public static string ShowAll()
        {
            Hashtable msg = System.Web.HttpContext.Current.Session[_key] as Hashtable;

            if (IsVaild)
                return "";

            string values = "<div class=\"MsgHelper_Tips\">";
            foreach (DictionaryEntry de in msg)
            {
                values += string.Format("<p>{0}</p>", de.Value);
            }
            values += "</div>";

            msg.Clear();

            Save(msg);

            return values;
        }

        /// <summary>
        /// 获取所有信息,并带指定分隔符
        /// </summary>
        /// <param name="splitTag">分隔符</param>
        /// <returns></returns>
        public static string ShowAll(string splitTag)
        {
            Hashtable msg = System.Web.HttpContext.Current.Session[_key] as Hashtable;

            if (IsVaild)
                return "";

            string values = "";
            foreach (DictionaryEntry de in msg)
            {
                values += string.Format("{0}" + splitTag, de.Value);
            }
            values = values.Remove(values.LastIndexOf(splitTag));

            msg.Clear();

            Save(msg);

            return values;
        }

        /// <summary>
        /// 信息集是否为空 ，True为没有信息,flase表示有信息
        /// </summary>
        public static bool IsVaild
        {
            get
            {
                Hashtable msg = System.Web.HttpContext.Current.Session[_key] as Hashtable;
                return msg == null || msg.Count == 0;
            }
        }

        /// <summary>
        /// 保存信息到Session里
        /// </summary>
        /// <param name="msg"></param>
        private static void Save(Hashtable msg)
        {
            System.Web.HttpContext.Current.Session[_key] = msg;
        }
    }
}
