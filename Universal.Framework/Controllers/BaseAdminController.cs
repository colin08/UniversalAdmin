using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using Universal.Tools;
using System.Data.Entity;

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
        /// 初始化调用构造函数后可能不可用的数据
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            WorkContext.SessionId = Session.SessionID;
            WorkContext.IsHttpAjax = WebHelper.IsAjax();
            WorkContext.IP = WebHelper.GetIP();
            WorkContext.Url = WebHelper.GetUrl();
            WorkContext.UrlReferrer = WebHelper.GetUrlReferrer();
            WorkContext.AjaxStringEntity = new WebAjaxEntity<string>();
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
                        filterContext.Result = View("Login");
                    }
                }
            }

            //验证是否有权限
            if (WorkContext.UserInfo != null)
            {
                if (!CheckAdminPower(""))
                {
                    if (WebHelper.IsAjax())
                    {
                        WorkContext.AjaxStringEntity.msg = 0;
                        WorkContext.AjaxStringEntity.msgbox = "您没有权限进行此操作";
                        filterContext.Result = Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        filterContext.Result = PromptView("您没有权限访问此页面");
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
            ExceptionInDB.ToInDB(filterContext.Exception);
            if (WorkContext.IsHttpAjax)
            {
                WorkContext.AjaxStringEntity.msg = 0;
                WorkContext.AjaxStringEntity.msgbox = error_msg;
                filterContext.Result = Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(WorkContext.UrlReferrer))
                {
                    filterContext.Result = PromptView(error_msg);
                }
                else
                {
                    PromptModel model = new PromptModel();
                    model.IsAutoBack = true;
                    model.IsShowBackLink = true;
                    model.Message = error_msg;
                    model.CountdownTime = 10;
                    model.CountdownModel = 10;
                    model.BackUrl = WorkContext.UrlReferrer;
                    filterContext.Result = PromptView(model);
                }
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
        protected void AddAdminLogs(DataCore.Entity.SysLogMethodType LogType, string detail, int user_id = 0)
        {
            using (DataCore.EFDBContext db = new DataCore.EFDBContext())
            {
                if (WorkContext.UserInfo != null)
                {
                    user_id = WorkContext.UserInfo.ID;
                }
                var entity = new DataCore.Entity.SysLogMethod()
                {
                    AddTime = DateTime.Now,
                    Detail = detail,
                    SysUserID = user_id,
                    Type = LogType
                };
                db.SysLogMethods.Add(entity);
                db.SaveChanges();
            }
        }


        #region 提示信息视图
        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string message)
        {
            return View("Prompt", new PromptModel(message));
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
        /// <param name="backUrl">返回地址</param>
        /// <param name="message">提示消息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string backUrl, string message)
        {
            return View("Prompt", new PromptModel(backUrl, message));
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
                    using (DataCore.EFDBContext db = new DataCore.EFDBContext())
                    {
                        DataCore.Entity.SysUser model = db.SysUsers.Where(s => s.ID == uid & s.Password == upwd).Include(s => s.SysRole.SysRoleRoutes.Select(y => y.SysRoute)).FirstOrDefault();
                        if (model != null)
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
                }
                return false;
            }
        }


        /// <summary>
        /// 获取登陆用户的信息
        /// </summary>
        /// <returns></returns>
        protected DataCore.Entity.SysUser GetUserInfo()
        {
            if (IsLogin())
            {
                DataCore.Entity.SysUser model = Session[SessionKey.Admin_User_Info] as DataCore.Entity.SysUser;
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
        protected bool CheckAdminPower(string PageKey)
        {
            if (string.IsNullOrWhiteSpace(PageKey))
                PageKey = WorkContext.PageKey;

            if (WorkContext.UserInfo.IsAdmin)
                return true;

            var result = WorkContext.UserInfo.SysRole.SysRoleRoutes.Where(p => p.SysRoute.Route == PageKey).FirstOrDefault();

            return result == null ? false : true;
        }

        #endregion

    }
}
