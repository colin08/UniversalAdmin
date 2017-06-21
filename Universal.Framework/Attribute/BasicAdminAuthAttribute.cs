using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Universal.Web.Framework
{
    /// <summary>
    /// 管理员用户验证
    /// </summary>
    public class BasicAdminAuthAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            //这个方法是在Action执行之前调用
            var user = BLL.BLLCusUser.GetUserInfo();
            if (user == null)
            {
                if (Tools.WebHelper.IsAjax())
                {
                    WebAjaxEntity<string> res = new WebAjaxEntity<string>();
                    res.msg = 0;
                    res.msgbox = "用户未登录或登录超时";
                    JsonResult jr = new JsonResult();
                    jr.Data = res;
                    filterContext.Result = jr;
                }
                else
                {
                    var Url = new UrlHelper(filterContext.RequestContext);
                    var url = Url.Action("Login", "Account", new { area = "" });
                    filterContext.Result = new RedirectResult(url);

                }
            }
            else
            {
                //if (!user.IsAdmin)
                //{
                //    if (Tools.WebHelper.IsAjax())
                //    {
                //        WebAjaxEntity<string> res = new WebAjaxEntity<string>();
                //        res.msg = 0;
                //        res.msgbox = "无操作权限，请联系管理员";
                //        JsonResult jr = new JsonResult();
                //        jr.Data = res;
                //        filterContext.Result = jr;
                //    }
                //    else
                //    {
                //        //var Url = new UrlHelper(filterContext.RequestContext);
                //        //var url = Url.Action("Error", "Home", new { area = "" });
                //        //filterContext.Result = new RedirectResult(url);
                //        ViewResult vr = new ViewResult();
                //        vr.ViewName = "permission";
                //        filterContext.Result = vr;
                //    }
                //}else
                //{
                string controllerName = filterContext.RouteData.Values["controller"].ToString().ToLower();
                if (user.CusUserRoute != null)
                {
                    bool is_see = false;
                    foreach (var item in user.CusUserRoute)
                    {
                        if (is_see)
                            break;
                        is_see = item.CusRoute.ControllerName.Equals(controllerName);
                    }

                    if (!is_see)
                    {
                        if (Tools.WebHelper.IsAjax())
                        {
                            WebAjaxEntity<string> res = new WebAjaxEntity<string>();
                            res.msg = 0;
                            res.msgbox = "无操作权限，请联系管理员";
                            JsonResult jr = new JsonResult();
                            jr.Data = res;
                            filterContext.Result = jr;
                        }
                        else
                        {
                            //var Url = new UrlHelper(filterContext.RequestContext);
                            //var url = Url.Action("Error", "Home", new { area = "" });
                            //filterContext.Result = new RedirectResult(url);
                            ViewResult vr = new ViewResult();
                            vr.ViewName = "permission";
                            filterContext.Result = vr;
                        }
                    }
                }
                else
                {
                    if (Tools.WebHelper.IsAjax())
                    {
                        WebAjaxEntity<string> res = new WebAjaxEntity<string>();
                        res.msg = 0;
                        res.msgbox = "无操作权限，请联系管理员";
                        JsonResult jr = new JsonResult();
                        jr.Data = res;
                        filterContext.Result = jr;
                    }
                    else
                    {
                        //var Url = new UrlHelper(filterContext.RequestContext);
                        //var url = Url.Action("Error", "Home", new { area = "" });
                        //filterContext.Result = new RedirectResult(url);
                        ViewResult vr = new ViewResult();
                        vr.ViewName = "permission";
                        filterContext.Result = vr;
                    }
                    //}
                }
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //这个方法是在Action执行之后调用
        }
    }
}
