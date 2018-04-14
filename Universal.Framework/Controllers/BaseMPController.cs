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
    /// <summary>
    /// 微信控制器基类
    /// </summary>
    public class BaseMPController : Controller
    {
        /// <summary>
        /// 工作上下文
        /// </summary>
        public MPWorkContext WorkContext = new MPWorkContext();

        /// <summary>
        /// 初始化调用构造函数后可能不可用的数据
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            WorkContext.WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);
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
            WorkContext.PageKeyCookie = ("mp" + WorkContext.Controller + WorkContext.Action).ToLower();
            //用户
            WorkContext.UserInfo = BLLMPUserState.GetUserInfo();
            WorkContext.open_id = BLLMPUserState.GetOpenID();            
        }


        /// <summary>
        /// 在进行授权时调用
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                
                string userAgent = filterContext.HttpContext.Request.UserAgent;

                //if (userAgent != null && (userAgent.Contains("MicroMessenger") || userAgent.Contains("Windows Phone")))
                //{ }
                //else
                //{
                //    filterContext.Result = PromptView("请在微信中访问");
                //    return;
                //}
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }

            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;

            //判断是否登陆
            if (!IsLogin())
            {
                List<string> un_auth = new List<string>();
                un_auth.Add("auth/index");
                un_auth.Add("auth/callback");
                if(!un_auth.Contains(WorkContext.PageKey))
                {
                    filterContext.Result = RedirectToAction("Index", "Auth", new { returnUrl = WorkContext.Url });
                }
            }
            
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
                filterContext.Result = PromptView(error_msg);
            }
        }

        /// <summary>
        /// 分页 计算总页数
        /// </summary>
        /// <param name="total"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        protected int CalculatePage(int total, int page_size)
        {
            if (page_size <= 0)
            {
                return 0;
            }
            return TypeHelper.ObjectToInt(Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(page_size)));
        }


        #region 提示信息视图

        /// <summary>
        /// 提示信息视图，只显示信息，不跳转
        /// </summary>
        /// <param name="message">简略消息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string details)
        {
            return View("Prompt", new PromptModel("", "", details));
        }
        
        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        protected ViewResult PromptView(PromptModel model)
        {
            return View("Prompt", model);
        }

        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="linkUrl">跳转地址</param>
        /// <param name="details">详细消息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string linkUrl, string details)
        {
            return View("Prompt", new PromptModel(linkUrl, "", "", details,3));
        }

        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="linkUrl">跳转地址</param>
        /// <param name="details">详细消息</param>
        /// <param name="time">倒计时</param>
        /// <returns></returns>
        protected ViewResult PromptView(string linkUrl,string details, int time)
        {
            return View("Prompt", new PromptModel(linkUrl, "", "", details, time));
        }
        #endregion

        #region 登陆的用户帮助类

        /// <summary>
        /// 判断用户是否登陆
        /// </summary>
        /// <returns></returns>
        protected bool IsLogin()
        {
            return BLLMPUserState.IsLogin();
        }


        /// <summary>
        /// 获取登陆用户的信息
        /// </summary>
        /// <returns></returns>
        protected Entity.MPUser GetUserInfo()
        {
            return BLLMPUserState.GetUserInfo();
        }


        #endregion
        
    }
}
