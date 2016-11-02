using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
namespace aliyunTest.Framework
{
    /// <summary>
    /// 上传文件附助类
    /// </summary>
    public static class FileUploadExtensions
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        public static string Attachment(this HttpFileCollectionBase hfc)
        {
            try
            {
                return Attachment(hfc, "");
            }
            catch (Exception ex)
            {
                "上传文件出错".TxtLogger(ex);
                return null;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="input_name">控件名</param>
        /// <returns></returns>
        public static string Attachment(this HttpFileCollectionBase hfc, string input_name)
        {
            try
            {
                HttpPostedFile file = null;
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    if (string.IsNullOrEmpty(input_name))
                        file = HttpContext.Current.Request.Files[0];
                    else
                        file = HttpContext.Current.Request.Files[input_name];

                    string msg = null;
                    string attachment_name = "";

                    if (file.ContentLength == 0)
                        msg = null;
                    else
                    {
                        attachment_name = System.IO.Path.GetFileName(file.FileName);

                        string filename = DateTime.Now.ToString("ddHHmmssffff") + System.IO.Path.GetExtension(file.FileName);

                        string virPath = Config.FrameworkConfig.Instance<Config.FrameworkConfig>().UploadFilePath + DateTime.Now.ToString("yyyy/MM") + "/";

                        string path = HttpContext.Current.Server.MapPath(virPath);

                        if (!Directory.Exists(@path))
                            Directory.CreateDirectory(@path);

                        file.SaveAs(path + filename);


                        msg = virPath.Replace("~", "") + filename;

                    }

                    return msg;
                }
            }
            catch (Exception ex)
            {
                "上传文件出错".TxtLogger(ex);
            }
            return null;
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="input_name">控件名</param>
        /// <returns></returns>
        public static string Attachment(this HttpFileCollectionBase hfc, int index)
        {
            try
            {
                HttpPostedFile file = null;
                if (HttpContext.Current.Request.Files.Count > 0)
                {

                    file = HttpContext.Current.Request.Files[index];

                    string msg = null;
                    string attachment_name = "";

                    if (file.ContentLength == 0)
                        msg = null;
                    else
                    {
                        attachment_name = System.IO.Path.GetFileName(file.FileName);

                        string filename = DateTime.Now.ToString("ddHHmmssffff") + System.IO.Path.GetExtension(file.FileName);

                        string virPath = Config.FrameworkConfig.Instance<Config.FrameworkConfig>().UploadFilePath + DateTime.Now.ToString("yyyy/MM") + "/";

                        string path = HttpContext.Current.Server.MapPath(virPath);

                        if (!Directory.Exists(@path))
                            Directory.CreateDirectory(@path);

                        file.SaveAs(path + filename);


                        msg = virPath.Replace("~", "") + filename;

                    }

                    return msg;
                }
            }
            catch (Exception ex)
            {
                "上传文件出错".TxtLogger(ex);
            }
            return null;
        }


    }
}
