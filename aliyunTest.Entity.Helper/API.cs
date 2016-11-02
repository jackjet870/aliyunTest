using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xfrog.Net;


namespace aliyunTest.Entity.Helper
{
    public class API
    {
        /// <summary>
        /// 请求数据接口
        /// </summary>
        public static Entity.jilu IPtoArea(string ip)
        {
            Entity.jilu jl = new Entity.jilu();
            //数据URL
            string url = "http://apis.juhe.cn/ip/ip2addr";
            //需要查询的IP地址
            //string ip = "www.baidu.com";
            //申请的接口Appkey
            string appkey = "369f34676af10f1ad252aad3d86317c3";
            StringBuilder sbTemp = new StringBuilder();
            //get 传值
            sbTemp.Append("ip=" + ip + "&key=" + appkey);
            String postReturn = HttpGet(url, sbTemp.ToString());
            //请求结果显示到网页
            //this.Label1.Text = "响应内容: " + postReturn + "<br >";
            //引入json对象
            JsonObject newObj = new JsonObject(postReturn);
            //解析返回数据字段
            String errorCode = newObj["error_code"].Value;
            if (errorCode == "0")
            {
                //请求成功
                //this.Label1.Text += newObj["result"]["area"].ToString();
                jl.Area = newObj["result"]["area"].ToString();
                jl.Kd = newObj["result"]["location"].ToString();
                return jl;
            }
            else
            {
                //请求失败
                //this.Label1.Text += "字段reason：" + newObj["reason"].Value;
                jl.Area = newObj["reason"].ToString();
                return jl;
            }
        }

        #region GET、POST方式Http请求方法
        /// <summary>
        /// Http请求GET方式
        /// </summary>
        /// <param name="url">请求地址
        /// <param name="getdatastr">参数
        /// <returns></returns>
        public static string HttpGet(string Url, string getDataStr)
        {
            string retString = "";
            try
            {
                //创建URL链接
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (getDataStr == "" ? "" : "?") + getDataStr);
                //请求方式
                request.Method = "GET";
                //超时设置
                request.ReadWriteTimeout = 5000;
                //请求编码
                request.ContentType = "text/html;charset=UTF-8";
                //获取响应
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //读取响应流
                Stream myResponseStream = response.GetResponseStream();
                //页面编码
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                //末尾流读取
                retString = myStreamReader.ReadToEnd();
                //关闭流和请求
                myStreamReader.Close();
                myResponseStream.Close();
                response.Close();

            }
            catch (Exception err)
            {
                //异常消息
                Console.WriteLine(err);
                System.Diagnostics.Trace.WriteLine(err);
            }
            //返回响应数据
            return retString;
        }

        /// <summary>
        /// Http请求POST方式
        /// </summary>
        /// <param name="url">请求地址
        /// <param name="postdatastr">请求参数
        /// <returns></returns>
        public static string HttpPost(string Url, string postDataStr)
        {
            string retString = "";
            try
            {
                //创建URL链接
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                //请求方式
                request.Method = "POST";
                //超时设置
                request.ReadWriteTimeout = 5000;
                //设置HTTP标头的值
                request.ContentType = "application/x-www-form-urlencoded";
                //设置数据长度
                request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
                //获取用于写入请求数据的对象
                Stream myRequestStream = request.GetRequestStream();
                //页面编码
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
                //写入流
                myStreamWriter.Write(postDataStr);
                //关闭写入流
                myStreamWriter.Close();
                //获取响应
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //流获取内容
                Stream myResponseStream = response.GetResponseStream();
                //初始化流并编码
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                //读取流末尾
                retString = myStreamReader.ReadToEnd();
                //关闭流和响应
                myStreamReader.Close();
                myResponseStream.Close();
                response.Close();

            }
            catch (Exception err)
            {
                //异常消息
                Console.WriteLine(err);
                System.Diagnostics.Trace.WriteLine(err);
            }
            //返回响应数据
            return retString;
        }
        #endregion
    }
}
