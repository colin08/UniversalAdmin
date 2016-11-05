using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Universal.Web.Framework
{
    /// <summary>
    /// 普通用户验证
    /// </summary>
    public class BasicUserAuthAttribute : ActionFilterAttribute, IAuthenticationFilter
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
            //else
            //{
            //    if (user.IsAdmin)
            //    {
            //        if (Tools.WebHelper.IsAjax())
            //        {
            //            WebAjaxEntity<string> res = new WebAjaxEntity<string>();
            //            res.msg = 0;
            //            res.msgbox = "只允许普通用户访问";
            //            JsonResult jr = new JsonResult();
            //            jr.ContentEncoding = Encoding.GetEncoding("UTF-8");
            //            jr.ContentType = "application/json";
            //            jr.Data = res;
            //            filterContext.Result = jr;
            //        }
            //        else
            //        {
            //            //var Url = new UrlHelper(filterContext.RequestContext);
            //            //var url = Url.Action("Error", "Home", new { area = "" });
            //            //filterContext.Result = new RedirectResult(url);

            //            ViewResult vr = new ViewResult();
            //            vr.ViewName = "permission";
            //            filterContext.Result = vr;
            //        }
            //    }
            //}
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //这个方法是在Action执行之后调用
        }
    }
}
