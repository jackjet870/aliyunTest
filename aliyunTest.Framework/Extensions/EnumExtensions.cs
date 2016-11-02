using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace aliyunTest.Framework
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 将枚举转换成Dictionary类型,供前台用
        /// </summary>
        /// <param name="type">枚举</param>
        /// <returns></returns>
        public static Dictionary<int, string> EnumParseDictionary(this Enum type)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string[] names = Enum.GetNames(type.GetType());
            int[] values = (int[])Enum.GetValues(type.GetType());
            for (int i = 0; i < names.Length; i++)
            {
                dic.Add(values[i], names[i]);
            }

            return dic;
        }


        /// <summary>
        /// 将枚举转换为checkbox控件
        /// </summary>
        /// <param name="e">要转换的枚举</param>
        /// <param name="name">生成的控件名</param>
        /// <returns></returns>
        public static string ToCheckbox(this Enum e, string name)
        {
            string html = "";

            foreach (string s in Enum.GetNames(e.GetType()))
            {
                html += string.Format("<span><input type='checkbox' value='{1}' name='{0}' id='dynchk{1}'/><label for='dynchk{1}'>{1}</label></span>", name, s);
            }

            return html;
        }

        /// <summary>
        /// 将枚举转换为checkbox控件
        /// </summary>
        /// <param name="e">要转换的枚举</param>
        /// <param name="name">生成的控件名</param>
        /// <param name="value">默认选中的值</param>
        /// <returns></returns>
        public static string ToCheckbox(this Enum e, string name, string[] value)
        {
            string html = "";

            foreach (string s in Enum.GetNames(e.GetType()))
            {
                html += string.Format("<span><input type='checkbox' value='{1}' id='dynchk{1}' name='{0}' " + (value.Contains(s) ? "checked='checked'" : "") + "/><label for='dynchk{1}'>{1}</label></span>", name, System.Web.HttpUtility.UrlDecode(s));
            }

            return html;
        }


        /// <summary>
        /// 将枚举转换为checkbox控件
        /// </summary>
        /// <param name="e">要转换的枚举</param>
        /// <param name="name">生成的控件名</param>
        /// <param name="value">默认选中的值</param>
        /// <param name="attr">其它属性</param>
        /// <returns></returns>
        public static string ToCheckbox(this Enum e, string name, string[] value, string attr)
        {
            string html = "";

            foreach (string s in Enum.GetNames(e.GetType()))
            {
                html += string.Format("<span><input type='checkbox' value='{1}' id='dynchk{1}' name='{0}' {2} " + (value.Contains(s) ? "checked='checked'" : "") + "/><label for='dynchk{1}'>{1}</label></span>", name, System.Web.HttpUtility.UrlDecode(s), attr);
            }

            return html;
        }





        /// <summary>
        /// 将枚举转换为Radio控件
        /// </summary>
        /// <param name="e">要转换的枚举</param>
        /// <param name="name">生成的控件名</param>
        /// <returns></returns>
        public static string ToRadio(this Enum e, string name)
        {
            string html = "";

            foreach (string s in Enum.GetNames(e.GetType()))
            {
                html += string.Format("<span><input type='radio' value='{1}' name='{0}' id='dynrad{1}'/><label for='dynrad{1}'>{1}</label></span>", name, s);
            }

            return html;
        }

        /// <summary>
        /// 将枚举转换为Radio控件
        /// </summary>
        /// <param name="e">要转换的枚举</param>
        /// <param name="name">生成的控件名</param>
        /// <param name="value">默认选中的值</param>
        /// <returns></returns>
        public static string ToRadio(this Enum e, string name, string value)
        {
            string html = "";

            foreach (string s in Enum.GetNames(e.GetType()))
            {
                html += string.Format("<span><input type='radio' value='{1}' id='dynrad{1}' name='{0}' " + (s == value ? "checked='checked'" : "") + "/><label for='dynrad{1}'>{1}</label></span>", name, System.Web.HttpUtility.UrlDecode(s));
            }

            return html;
        }


        /// <summary>
        /// 将枚举转换为Radio控件
        /// </summary>
        /// <param name="e">要转换的枚举</param>
        /// <param name="name">生成的控件名</param>
        /// <param name="value">默认选中的值</param>
        /// <param name="attr">其它属性</param>
        /// <returns></returns>
        public static string ToRadio(this Enum e, string name, string value, string attr)
        {
            string html = "";

            foreach (string s in Enum.GetNames(e.GetType()))
            {
                html += string.Format("<span><input type='radio' value='{1}' id='dynrad{1}' name='{0}' {2} " + (s == value ? "checked='checked'" : "") + "/><label for='dynrad{1}'>{1}</label></span>", name, System.Web.HttpUtility.UrlDecode(s), attr);
            }

            return html;
        }


        /// <summary>
        /// 将枚举转换为select控件
        /// </summary>
        /// <param name="e">要转换的枚举</param>
        /// <param name="name">生成的控件名</param>
        /// <returns></returns>
        public static string ToSelect(this Enum e, string name)
        {
            string html = "";
            
            foreach (string s in Enum.GetNames(e.GetType()))
            {
                html += string.Format("<option value='{0}'>{0}</option>", s);
            }

            return string.Format("<select name='{0}'>{1}</select>", name, html);
        }


        /// <summary>
        /// 将枚举转换为select控件
        /// </summary>
        /// <param name="e">要转换的枚举</param>
        /// <param name="name">生成的控件名</param>
        /// <param name="value">默认选中的值</param>
        /// <returns></returns>
        public static string ToSelect(this Enum e, string name, string value)
        {
            string html = "";

            value = string.IsNullOrEmpty(value) ? "" : value;

            foreach (string s in Enum.GetNames(e.GetType()))
            {
                html += string.Format("<option value='{0}' {1}>{0}</option>", s, (value.Contains(System.Web.HttpUtility.UrlDecode(s)) ? " selected='selected'" : ""));
            }

            return string.Format("<select name='{0}'>{1}</select>", name, html);
        }


        /// <summary>
        /// 将枚举转换为select控件
        /// </summary>
        /// <param name="e">要转换的枚举</param>
        /// <param name="name">生成的控件名</param>
        /// <param name="value">默认选中的值</param>
        /// <param name="noSelect">默认提示</param>
        /// <returns></returns>
        public static string ToSelect(this Enum e, string name, string value, string noSelect)
        {

            string html = "";
            value = string.IsNullOrEmpty(value) ? "" : value;
            if (noSelect != null)
                html = string.Format("<option value=''>{0}</option>", noSelect);

            foreach (string s in Enum.GetNames(e.GetType()))
            {
                html += string.Format("<option value='{0}' {1}>{0}</option>", s, (value.Contains(System.Web.HttpUtility.UrlDecode(s)) ? " selected='selected'" : ""));
            }

            return string.Format("<select name='{0}'>{1}</select>", name, html);
        }

        /// <summary>
        /// 将枚举转换为select控件
        /// </summary>
        /// <param name="e">要转换的枚举</param>
        /// <param name="name">生成的控件名</param>
        /// <param name="value">默认选中的值</param>
        /// <param name="noSelect">默认提示</param>
        /// <returns></returns>
        public static string ToSelect(this Enum e, string name, string value, string noSelect, string repeatStr, string repeatedStr)
        {

            string html = "";
            value = string.IsNullOrEmpty(value) ? "" : value;
            if (noSelect != null)
                html = string.Format("<option value=''>{0}</option>", noSelect);

            foreach (string s in Enum.GetNames(e.GetType()))
            {
                html += string.Format("<option value='{0}' {1}>{0}</option>", s.Replace(repeatStr, repeatedStr), (value.Contains(System.Web.HttpUtility.UrlDecode(s)) ? " selected='selected'" : ""));
            }

            return string.Format("<select name='{0}'>{1}</select>", name, html);
        }



        /// <summary>
        /// 将枚举转换为select控件
        /// </summary>
        /// <param name="e">要转换的枚举</param>
        /// <param name="name">生成的控件名</param>
        /// <param name="value">默认选中的值</param>
        /// <param name="noSelect">默认提示</param>
        /// <param name="attr">其它HTML属性</param>
        /// <returns></returns>
        public static string ToSelect(this Enum e, string name, string value, string noSelect, string attr)
        {

            string html = "";
            value = string.IsNullOrEmpty(value) ? "" : value;
            if (noSelect != null)
                html = string.Format("<option value=''>{0}</option>", noSelect);

            foreach (string s in Enum.GetNames(e.GetType()))
            {
                html += string.Format("<option value='{0}' {1}>{0}</option>", s, (value.Contains(System.Web.HttpUtility.UrlDecode(s)) ? " selected='selected'" : ""));
            }

            return string.Format("<select name='{0}' {2}>{1}</select>", name, html, attr);
        }

        /// <summary>
        /// 将枚举转换为select控件
        /// </summary>
        /// <param name="e">要转换的枚举</param>
        /// <param name="name">生成的控件名</param>
        /// <param name="value">默认选中的值</param>
        /// <param name="noSelect">默认提示</param>
        /// <param name="removeItem">队外的选项</param>
        /// <returns></returns>
        public static string ToSelect(this Enum e, string name, string value, string noSelect, List<string> removeItem)
        {

            string html = "";
            value = string.IsNullOrEmpty(value) ? "" : value;
            if (noSelect != null)
                html = string.Format("<option value=''>{0}</option>", noSelect);

            removeItem = removeItem ?? new List<string>();
            foreach (string s in Enum.GetNames(e.GetType()))
            {
                if (!removeItem.Contains(s))
                    html += string.Format("<option value='{0}' {1}>{0}</option>", s, (value.Contains(System.Web.HttpUtility.UrlDecode(s)) ? " selected='selected'" : ""));
            }

            return string.Format("<select name='{0}'>{1}</select>", name, html);
        }
        /// <summary>
        /// 将枚举转换为select控件
        /// joe 2015-3-24
        /// </summary>
        /// <returns></returns>
        public static string ToSelect(this Enum e, string name, string value, string noSelect,int test)
        {
            string html = "";
            value = string.IsNullOrEmpty(value) ? "" : value;
            if (noSelect != null)
            html = string.Format("<option value=''>{0}</option>", noSelect);
            string strKey, strValue;
            foreach (int intError in Enum.GetValues(e.GetType()))
            {
                strKey = intError.ToString();
                strValue = Enum.GetName(e.GetType(), intError);
                html += string.Format("<option value='{2}' {1}>{0}</option>", strValue, (value.Contains(System.Web.HttpUtility.UrlDecode(strValue)) ? " selected='selected'" : ""), strKey);
            }
            return string.Format("<select name='{0}'>{1}</select>", name, html);
        }
    }
}
