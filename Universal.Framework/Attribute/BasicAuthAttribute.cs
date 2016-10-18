using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Universal.Web.Framework
{
    /// <summary>
    /// 用户验证
    /// </summary>
    public class BasicAuthAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            //这个方法是在Action执行之前调用
            var user = BLL.BLLCusUser.GetUserInfo();
            if (user == null)
            {
                var Url = new UrlHelper(filterContext.RequestContext);
                var url = Url.Action("Login", "Account", new { area = "" });
                filterContext.Result = new RedirectResult(url);
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //这个方法是在Action执行之后调用
        }
    }
}
