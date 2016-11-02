using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;

namespace aliyunTest.Framework
{
    /// <summary>
    /// 数据格式化扩展
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// 格式化数字,去掉右边的0
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string FormartDecimal(this decimal src)
        {
            if (src.ToString().Contains('.'))
            {
                string temp = src.ToString().TrimEnd('0');
                if (temp[temp.Length - 1] == '.')
                    return temp.Replace(".", "");
                else
                    return temp;
            }
            else
                return src.ToString();
        }
    }
}
