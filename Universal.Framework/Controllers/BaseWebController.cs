using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using Universal.Tools;
using System.Data.Entity;
using Universal.BLL;
using System.Collections.Generic;

namespace Universal.Web.Framework
{
    public class BaseWebController:Controller
    {
        public WebWorkContext WorkContext = new WebWorkContext();

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            WorkContext.WebSiteConfig = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);
            WorkContext.SessionId = Session.SessionID;
            WorkContext.IsHttpAjax = WebHelper.IsAjax();
            WorkContext.IsHttpPost = WebHelper.IsPost();
            WorkContext.IP = WebHelper.GetIP();
            WorkContext.Url = WebHelper.GetUrl();
            WorkContext.UrlReferrer = WebHelper.GetUrlReferrer();
            WorkContext.AjaxStringEntity = new UnifiedResultEntity<string>();
            WorkContext.AjaxStringEntity.msg = 0; //默认错误
            WorkContext.AjaxStringEntity.total = 0;
            //设置当前控制器类名
            WorkContext.Controller = RouteData.Values["controller"].ToString().ToLower();
            //设置当前动作方法名
            WorkContext.Action = RouteData.Values["action"].ToString().ToLower();
            WorkContext.PageKey = string.Format("{0}/{1}", WorkContext.Controller, WorkContext.Action).ToLower();
        }

        /// <summary>
        /// 当操作中发生未经处理的异常时调用
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            string error_msg = filterContext.Exception.Message;
            if (!filterContext.ExceptionHandled)
            {
                filterContext.ExceptionHandled = true;
                ExceptionInDB.ToInDB(filterContext.Exception);
            }

            if (WorkContext.IsHttpAjax)
            {
                WorkContext.AjaxStringEntity.msg = 0;
                WorkContext.AjaxStringEntity.msgbox = error_msg;
                filterContext.Result = Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                filterContext.Result = ErrorView("System Error:" + error_msg);
            }
        }

        protected ViewResult ErrorView(string msg)
        {
            return View("Error", new WebErrorModel(WorkContext.UrlReferrer, msg));
        }

        protected ViewResult ErrorView(string back_url,string msg)
        {
            return View("Error", new WebErrorModel(back_url, msg));
        }        
    }
}
