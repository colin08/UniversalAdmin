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
    /// 后台控制器基类
    /// </summary>
    public class BaseAdminController : Controller
    {
        /// <summary>
        /// 工作上下文
        /// </summary>
        public AdminWorkContext WorkContext = new AdminWorkContext();
        /// <summary>
        /// 站点配置文件
        /// </summary>
        public WebSiteModel WebSite = null;

        /// <summary>
        /// 初始化调用构造函数后可能不可用的数据
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);
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
            //用户
            WorkContext.UserInfo = GetUserInfo();
        }


        /// <summary>
        /// 在进行授权时调用
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;

            //判断是否登陆
            if (!IsLogin())
            {
                if (WorkContext.PageKey.ToLower() != "home/login")
                {
                    if (WebHelper.IsAjax())
                    {
                        WorkContext.AjaxStringEntity.msg = 0;
                        WorkContext.AjaxStringEntity.msgbox = "请重新登陆";
                        filterContext.Result = Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        filterContext.Result = View("OutFram");
                    }
                }
            }

            //验证是否有权限
            if (WorkContext.UserInfo != null)
            {
                if (!CheckAdminPower("",false))
                {
                    if (WebHelper.IsAjax())
                    {
                        WorkContext.AjaxStringEntity.msg = 0;
                        WorkContext.AjaxStringEntity.msgbox = "您没有权限进行此操作";
                        filterContext.Result = Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        filterContext.Result = PromptView("Error","无权限","您没有权限进行此操作");
                    }
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
            if(!filterContext.ExceptionHandled)
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
                    filterContext.Result = PromptView("500", "系统发生错误", error_msg);
                //if (string.IsNullOrWhiteSpace(WorkContext.UrlReferrer))
                //{
                //}
                //else
                //{
                //    PromptModel model = new PromptModel();
                //    model.IsAutoLink = true;
                //    model.IsShowLink = true;
                //    model.Status = "500";
                //    model.Details = error_msg;
                //    model.Message = "系统发生错误";
                //    model.CountdownTime = 10;
                //    model.CountdownModel = 10;
                //    model.LinkUrl = WorkContext.UrlReferrer;
                //    filterContext.Result = PromptView(model);
                //}
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



        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="LogType">操作类别</param>
        /// <param name="obj">操作对象</param>
        /// <param name="detail">介绍内容</param>
        protected void AddAdminLogs(Entity.SysLogMethodType LogType, string detail, int user_id = 0)
        {
            if (!WebSite.LogMethodInDB)
                return;


            if (WorkContext.UserInfo != null)
            {
                user_id = WorkContext.UserInfo.ID;
            }

            var entity = new Entity.SysLogMethod()
            {
                AddTime = DateTime.Now,
                Detail = detail,
                SysUserID = user_id,
                Type = LogType
            };
            BaseBLL<Entity.SysLogMethod> bll = new BaseBLL<Entity.SysLogMethod>();
            bll.Add(entity);            
        }


        #region 提示信息视图
        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="status">状态码</param>
        /// <param name="details">详细消息</param>
        /// <param name="message">简略消息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string status,string message,string details)
        {
            return View("Prompt", new PromptModel(status,message, details));
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
        /// <param name="status">状态码</param>
        /// <param name="message">简略消息</param>
        /// <param name="details">详细消息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string linkUrl,string status,string message,string details)
        {
            return View("Prompt", new PromptModel(linkUrl, status, message, details));
        }

        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="linkUrl">跳转地址</param>
        /// <param name="status">状态码</param>
        /// <param name="message">简略消息</param>
        /// <param name="details">详细消息</param>
        /// <param name="time">倒计时</param>
        /// <returns></returns>
        protected ViewResult PromptView(string linkUrl, string status, string message, string details,int time)
        {
            return View("Prompt", new PromptModel(linkUrl, status, message, details,time));
        }
        #endregion

        #region 登陆的用户帮助类

        /// <summary>
        /// 判断用户是否登陆
        /// </summary>
        /// <returns></returns>
        protected bool IsLogin()
        {
            if (Session[SessionKey.Admin_User_Info] != null)
                return true;
            else
            {
                //检查COOKIE
                int uid = TypeHelper.ObjectToInt(WebHelper.GetCookie(CookieKey.Login_UserID));
                string upwd = WebHelper.GetCookie(CookieKey.Login_UserPassword);
                if (uid != 0 && !string.IsNullOrWhiteSpace(upwd))
                {
                    BaseBLL<Entity.SysUser> bll = new BaseBLL<Entity.SysUser>();
                    List<FilterSearch> filters = new List<FilterSearch>();
                    filters.Add(new FilterSearch("ID", uid.ToString(), FilterSearchContract.等于));
                    filters.Add(new FilterSearch("Password", upwd, FilterSearchContract.等于));
                    Entity.SysUser model = bll.GetModel(filters, null, "SysRole.SysRoleRoutes.SysRoute");
                    if(model != null)
                    {
                        if (model.Status)
                        {
                            Session[SessionKey.Admin_User_Info] = model;
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
            }
        }


        /// <summary>
        /// 获取登陆用户的信息
        /// </summary>
        /// <returns></returns>
        protected Entity.SysUser GetUserInfo()
        {
            if (IsLogin())
            {
                Entity.SysUser model = Session[SessionKey.Admin_User_Info] as Entity.SysUser;
                if (model != null)
                {
                    if (model.Status)
                        return model;
                    else
                        return null;
                }
                return null;
            }
            return null;
        }


        #endregion

        #region 验证权限

        /// <summary>
        /// 校验用户权限
        /// </summary>
        /// <param name="PageKey"></param>
        /// <returns></returns>
        protected bool CheckAdminPower(string PageKey,bool isPost)
        {
            if (string.IsNullOrWhiteSpace(PageKey))
            {
                isPost = WorkContext.IsHttpPost;
                PageKey = WorkContext.PageKey;
            }
            PageKey = PageKey.ToLower();
            if (WorkContext.UserInfo.SysRole.IsAdmin)
                return true;
            var result = true;
            BaseBLL<Entity.SysRoute> bll = new BaseBLL<Entity.SysRoute>();
            List<FilterSearch> filters = new List<FilterSearch>();
            filters.Add(new FilterSearch("IsPost", isPost.ToString(), FilterSearchContract.等于));
            filters.Add(new FilterSearch("Route", PageKey, FilterSearchContract.等于));
            int total =bll.GetCount(filters);
            if(total>0)
            {
                var entity = WorkContext.UserInfo.SysRole.SysRoleRoutes.Where(p => p.SysRoute.Route == PageKey && p.SysRoute.IsPost == isPost).FirstOrDefault();
                result = entity == null ? false : true;
            }

            return result;
        }

        #endregion

    }
}
