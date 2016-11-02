using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using aliyunTest.Framework.Config;

namespace aliyunTest.Framework
{
    /// <summary>
    /// Eilnt 控制器基类
    /// </summary>
    public class ControllerBase : System.Web.Mvc.Controller
    {

        #region 控制器与视力重定义

        #region 私有属性

        /// <summary>
        /// 配置缓存对像
        /// </summary>
        private FrameworkConfig CACHEOBJECT = null;

        /// <summary>
        /// 管理View路径
        /// </summary>
        private string MANAGEVIEWPATH = "";

        private string APPVIEWPATH = "";

        /// <summary>
        /// 对话框View路径
        /// </summary>
        private string DIALOGVIEWPATH = "";

        /// <summary>
        /// 普通View路径
        /// </summary>
        private string NORMALVIEWPATH = "";

        /// <summary>
        /// 前台会员View
        /// </summary>
        private string MEMBERVIEWPATH = "";

        /// <summary>
        /// 控件View路径
        /// </summary>
        private string CONTROLPATH = "";

        /// <summary>
        /// 主题路径
        /// </summary>
        private string CURRENTABSOLUTTHEMPATH = "";


        /// <summary>
        /// 构造函数
        /// </summary>
        public ControllerBase()
        {
            //base.ValidateRequest = false;

            CACHEOBJECT = FrameworkConfig.Instance<FrameworkConfig>();

            //主题文件夹\主题\控件器\相应类型文件夹\action.cshtml
            CURRENTABSOLUTTHEMPATH = CACHEOBJECT.ThemesPath + "/" + CACHEOBJECT.CurrentTheme + "/{0}" + "/";



            MANAGEVIEWPATH = CURRENTABSOLUTTHEMPATH + CACHEOBJECT.ManageViewPath;
            APPVIEWPATH = CURRENTABSOLUTTHEMPATH + CACHEOBJECT.AppViewPath;
            DIALOGVIEWPATH = CURRENTABSOLUTTHEMPATH + CACHEOBJECT.DialogViewPath;
            NORMALVIEWPATH = CURRENTABSOLUTTHEMPATH + CACHEOBJECT.NormalViewPath;
            MEMBERVIEWPATH = CURRENTABSOLUTTHEMPATH + CACHEOBJECT.MemberViewPath;
            CONTROLPATH = string.Format(CURRENTABSOLUTTHEMPATH, CACHEOBJECT.ControlPath);
        }


        #endregion

        #region 手机View

        /// <summary>
        /// 应用管理View
        /// </summary>
        /// <returns></returns>
        public ViewResult AppView()
        {
            return View(_getCurrentViewPath(APPVIEWPATH));
        }

        /// <summary>
        /// 应用管理View
        /// </summary>
        /// <param name="model">传参数到View</param>
        /// <returns></returns>
        public ViewResult AppView(object model)
        {
            return View(_getCurrentViewPath(APPVIEWPATH), model);
        }

        /// <summary>
        /// 应用管理View    Title:提示信息 Time：跳转时间   ToUrl：返回地址  PostUrl:Post提交地址
        /// </summary>
        /// <param name="viewName">指定View名</param>
        /// <returns></returns>
        public ViewResult AppView(string viewName)
        {
            return View(_getCurrentViewPath(APPVIEWPATH, viewName));
        }


        /// <summary>
        /// 应用管理View  Title:提示信息 Time：跳转时间   ToUrl：返回地址  PostUrl:Post提交地址
        /// </summary>
        /// <param name="model">传参数到View</param>
        /// <param name="viewName">指定View名</param>
        /// <returns></returns>
        public ViewResult AppView(string viewName, object model)
        {
            return View(_getCurrentViewPath(APPVIEWPATH, viewName), model);
        }

        /// <summary>
        /// 应用管理路径
        /// </summary>
        /// <returns></returns>
        public string AppViewPath()
        {
            return string.Format(APPVIEWPATH, ControllerContext.RouteData.Values["controller"]);
        }

        #endregion

        #region 后台管理View

        /// <summary>
        /// 应用管理View
        /// </summary>
        /// <returns></returns>
        public ViewResult ManageView()
        {
            return View(_getCurrentViewPath(MANAGEVIEWPATH));
        }

        /// <summary>
        /// 应用管理View
        /// </summary>
        /// <param name="model">传参数到View</param>
        /// <returns></returns>
        public ViewResult ManageView(object model)
        {
            return View(_getCurrentViewPath(MANAGEVIEWPATH), model);
        }

        /// <summary>
        /// 应用管理View    Title:提示信息 Time：跳转时间   ToUrl：返回地址  PostUrl:Post提交地址
        /// </summary>
        /// <param name="viewName">指定View名</param>
        /// <returns></returns>
        public ViewResult ManageView(string viewName)
        {
            return View(_getCurrentViewPath(MANAGEVIEWPATH, viewName));
        }


        /// <summary>
        /// 应用管理View  Title:提示信息 Time：跳转时间   ToUrl：返回地址  PostUrl:Post提交地址
        /// </summary>
        /// <param name="model">传参数到View</param>
        /// <param name="viewName">指定View名</param>
        /// <returns></returns>
        public ViewResult ManageView(string viewName, object model)
        {
            return View(_getCurrentViewPath(MANAGEVIEWPATH, viewName), model);
        }

        /// <summary>
        /// 应用管理路径
        /// </summary>
        /// <returns></returns>
        public string ManagePath()
        {
            return string.Format(MANAGEVIEWPATH, ControllerContext.RouteData.Values["controller"]);
        }

        #endregion

        #region 前台普通View

        /// <summary>
        /// 普通View
        /// </summary>
        /// <returns></returns>
        public ViewResult NormalView()
        {
            return View(_getCurrentViewPath(NORMALVIEWPATH));
        }

        /// <summary>
        /// 普通View
        /// </summary>
        /// <param name="model">传参数到View</param>
        /// <returns></returns>
        public ViewResult NormalView(object model)
        {
            return View(_getCurrentViewPath(NORMALVIEWPATH), model);
        }


        /// <summary>
        /// 普通View  Title:提示信息 Time：跳转时间   ToUrl：返回地址  PostUrl:Post提交地址
        /// </summary> 
        /// <param name="viewName">指定View名</param>
        /// <returns></returns>
        public ViewResult NormalView(string viewName)
        {
            return View(_getCurrentViewPath(NORMALVIEWPATH, viewName));
        }

        /// <summary>
        /// 普通View Title:提示信息 Time：跳转时间   ToUrl：返回地址  PostUrl:Post提交地址
        /// </summary>  
        /// <param name="model">传参数到View</param>
        /// <param name="viewName">指定View名</param>
        /// <returns></returns>
        public ViewResult NormalView(string viewName, object model)
        {
            return View(_getCurrentViewPath(NORMALVIEWPATH, viewName), model);
        }


        /// <summary>
        /// 应用普通路径
        /// </summary>
        /// <returns></returns>
        public string NormalPath()
        {
            return string.Format(NORMALVIEWPATH, ControllerContext.RouteData.Values["controller"]);
        }

        #endregion

        #region 前台会员View

        /// <summary>
        /// 前台会员View
        /// </summary>
        /// <returns></returns>
        public ViewResult MemberView()
        {
            return View(_getCurrentViewPath(MEMBERVIEWPATH));
        }

        /// <summary>
        /// 前台会员View
        /// </summary>
        /// <param name="model">传参数到View</param>
        /// <returns></returns>
        public ViewResult MemberView(object model)
        {
            return View(_getCurrentViewPath(MEMBERVIEWPATH), model);
        }

        /// <summary>
        /// 前台会员View Title:提示信息 Time：跳转时间   ToUrl：返回地址  PostUrl:Post提交地址
        /// </summary> 
        /// <param name="viewName">指定View名</param>
        /// <returns></returns>
        public ViewResult MemberView(string viewName)
        {
            return View(_getCurrentViewPath(MEMBERVIEWPATH, viewName));
        }

        /// <summary>
        /// 前台会员View  Title:提示信息 Time：跳转时间   ToUrl：返回地址  PostUrl:Post提交地址
        /// </summary> 
        /// <param name="model">传参数到View</param>
        /// <param name="viewName">指定View名</param>
        /// <returns></returns>
        public ViewResult MemberView(string viewName, object model)
        {
            return View(_getCurrentViewPath(MEMBERVIEWPATH, viewName), model);
        }


        /// <summary>
        /// 前台会员路径
        /// </summary>
        /// <returns></returns>
        public string MemberPath()
        {
            return string.Format(MEMBERVIEWPATH, ControllerContext.RouteData.Values["controller"]);
        }

        #endregion

        #region 对话框View

        /// <summary>
        /// 对话框View
        /// </summary>
        /// <returns></returns>
        public ViewResult DialogView()
        {
            return View(_getCurrentViewPath(DIALOGVIEWPATH));
        }

        /// <summary>
        /// 对话框View
        /// </summary>
        /// <param name="model">传参数到View</param>
        /// <returns></returns>
        public ViewResult DialogView(object model)
        {
            return View(_getCurrentViewPath(DIALOGVIEWPATH), model);
        }

        /// <summary>
        /// 对话框View  Title:提示信息 Time：跳转时间   ToUrl：返回地址  PostUrl:Post提交地址
        /// </summary> 
        /// <param name="viewName">指定View名</param>
        /// <returns></returns>
        public ViewResult DialogView(string viewName)
        {
            return View(_getCurrentViewPath(DIALOGVIEWPATH, viewName));
        }

        /// <summary>
        /// 对话框View  Title:提示信息 Time：跳转时间   ToUrl：返回地址  PostUrl:Post提交地址
        /// </summary>
        /// <param name="model">传参数到View</param>
        /// <param name="viewName">指定View名</param>
        /// <returns></returns>
        public ViewResult DialogView(string viewName, object model)
        {
            return View(_getCurrentViewPath(DIALOGVIEWPATH, viewName), model);
        }

        /// <summary>
        /// 对话框路径
        /// </summary>
        /// <returns></returns>
        public string DialogPath()
        {
            return string.Format(DIALOGVIEWPATH, ControllerContext.RouteData.Values["controller"]);
        }


        #endregion

        #region 附助方法

        /// <summary>
        /// 获取当前Action的路径如 /controler/action.aspx
        /// </summary>
        /// <returns></returns>
        private string _getCurrentViewPath(string path)
        {
            return string.Format(path + "/{1}.cshtml", ControllerContext.RouteData.Values["controller"], ControllerContext.RouteData.Values["action"]);
        }

        /// <summary>
        /// 获取当前Action的路径如 /controler/viewName.aspx
        /// </summary>
        /// <param name="path"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        private string _getCurrentViewPath(string path, string viewName)
        {
            return string.Format(path + "/{1}.cshtml", ControllerContext.RouteData.Values["controller"], viewName);
        }


        #endregion

        #region 重定向

        /// <summary>
        /// 重定向到来源页
        /// </summary>
        /// <param name="noFoundUrl">当没有来源页里要定向的页</param>
        /// <returns></returns>
        public ActionResult ToReffer(object noFoundUrl)
        {
            if (Request.UrlReferrer == null)
                return Redirect(noFoundUrl.ToString());
            else
                return Redirect(Request.UrlReferrer.ToString());
        }

        /// <summary>
        /// 重定向到来源页
        /// </summary>
        /// <param name="action">当没有来源页里要定向的Action名</param> 
        /// <returns></returns>
        public ActionResult ToReffer(string action)
        {
            if (Request.UrlReferrer == null)
                return RedirectToAction(action);
            else
                return Redirect(Request.UrlReferrer.ToString());
        }

        /// <summary>
        ///  重定向到来源页
        /// </summary>
        /// <param name="action">当没有来源页里要定向的Action名</param> 
        /// <param name="routeValue">路由值</param> 
        /// <returns></returns>
        public ActionResult ToReffer(string action, object routeValue)
        {
            if (Request.UrlReferrer == null)
                return RedirectToAction(action, routeValue);
            else
                return Redirect(Request.UrlReferrer.ToString());
        }

        /// <summary>
        /// 重定向到来源页
        /// </summary>
        /// <param name="action">当没有来源页里要定向的Action名</param>
        /// <param name="controller">当没有来源页里要定向的Controller名</param>
        /// <returns></returns>
        public ActionResult ToReffer(string action, string controller)
        {
            if (Request.UrlReferrer == null)
                return RedirectToAction(action, controller);
            else
                return Redirect(Request.UrlReferrer.ToString());
        }

        /// <summary>
        /// 重定向到来源页
        /// </summary>
        /// <param name="action">当没有来源页里要定向的Action名</param>
        /// <param name="controller">当没有来源页里要定向的Controller名</param>
        /// <param name="routeValue">路由值</param> 
        /// <returns></returns>
        public ActionResult ToReffer(string action, string controller, object routeValue)
        {
            if (Request.UrlReferrer == null)
                return RedirectToAction(action, controller, routeValue);
            else
                return Redirect(Request.UrlReferrer.ToString());
        }

        #endregion

        #region 重定向ToUrl

        /// <summary>
        /// 重定向到Url里ToUrl
        /// </summary>
        /// <param name="noFoundUrl">当没有来源页里要定向的页</param>
        /// <returns></returns>
        public ActionResult ToUrl(object noFoundUrl)
        {
            if (string.IsNullOrEmpty(Request.QueryString["ToUrl"]))
                return Redirect(noFoundUrl.ToString());
            else
                return Redirect(Request.QueryString["ToUrl"].ToString());
        }

        /// <summary>
        /// 重定向到Url里ToUrl
        /// </summary>
        /// <param name="action">当没有来源页里要定向的Action名</param> 
        /// <returns></returns>
        public ActionResult ToUrl(string action)
        {
            if (string.IsNullOrEmpty(Request.QueryString["ToUrl"]))
                return RedirectToAction(action);
            else
                return Redirect(Request.QueryString["ToUrl"].ToString());
        }

        /// <summary>
        ///  重定向到Url里ToUrl
        /// </summary>
        /// <param name="action">当没有来源页里要定向的Action名</param> 
        /// <param name="routeValue">路由值</param> 
        /// <returns></returns>
        public ActionResult ToUrl(string action, object routeValue)
        {
            if (string.IsNullOrEmpty(Request.QueryString["ToUrl"]))
                return RedirectToAction(action, routeValue);
            else
                return Redirect(Request.QueryString["ToUrl"].ToString());
        }

        /// <summary>
        /// 重定向到Url里ToUrl
        /// </summary>
        /// <param name="action">当没有来源页里要定向的Action名</param>
        /// <param name="controller">当没有来源页里要定向的Controller名</param>
        /// <returns></returns>
        public ActionResult ToUrl(string action, string controller)
        {
            if (string.IsNullOrEmpty(Request.QueryString["ToUrl"]))
                return RedirectToAction(action, controller);
            else
                return Redirect(Request.QueryString["ToUrl"].ToString());
        }

        /// <summary>
        /// 重定向到Url里ToUrl
        /// </summary>
        /// <param name="action">当没有来源页里要定向的Action名</param>
        /// <param name="controller">当没有来源页里要定向的Controller名</param>
        /// <param name="routeValue">路由值</param> 
        /// <returns></returns>
        public ActionResult ToUrl(string action, string controller, object routeValue)
        {
            if (string.IsNullOrEmpty(Request.QueryString["ToUrl"]))
                return RedirectToAction(action, controller, routeValue);
            else
                return Redirect(Request.QueryString["ToUrl"].ToString());
        }

        #endregion

        #region 文件下载

        /// <summary>
        /// 文件下载 ,以指定编码下载
        /// </summary>
        /// <param name="encoder">文件编码</param>
        /// <param name="content">文件内容</param>
        /// <param name="contentType">文件MiniType</param>
        /// <param name="downloadFileName">下载名</param>
        /// <returns></returns>
        public FileResult File(System.Text.Encoding encoder, string content, string contentType, string downloadFileName)
        {
            return File(encoder.GetBytes(content), contentType, downloadFileName);
        }


        #endregion

        #endregion


        #region 其它属性定义

        /// <summary>
        /// 是否以POST方式请求
        /// </summary>
        public bool IsPost { get { return Request.HttpMethod.ToUpper().Trim() == "POST" ? true : false; } }

        /// <summary>
        /// 控件所在路径,最后带“/”
        /// </summary>
        public string ControlPath
        {
            get
            {
                return CONTROLPATH;
            }
        }

        #endregion
    }

}
