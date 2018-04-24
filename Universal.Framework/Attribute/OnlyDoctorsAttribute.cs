using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Universal.Tools;

namespace Universal.Web.Framework
{
    /// <summary>
    /// 只允许医生访问的Action
    /// </summary>
    public class OnlyDoctorsAttribute : ActionFilterAttribute,IActionFilter
    {
        /// <summary>
        /// Action执行前，第一步
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var entity_user = BLL.BLLMPUserState.GetUserInfo();
            if(entity_user != null)
            {
                if(entity_user.Identity != Entity.MPUserIdentity.Doctors)
                {
                    if (WebHelper.IsAjax())
                    {
                        UnifiedResultEntity<string> res = new UnifiedResultEntity<string>();
                        res.msg = 0;
                        res.msgbox = "此功能只允许医生使用";
                        JsonResult jr = new JsonResult();
                        jr.Data = res;
                        filterContext.Result = jr;
                    }
                    else
                    {
                        //当前访问的URL
                        string request_url = filterContext.RequestContext.HttpContext.Request.Url.AbsolutePath.ToLower();
                        UserDocRouteMap map = new UserDocRouteMap();
                        //跳到对应用户页面
                        filterContext.Result = new RedirectResult(map.GetMapUrl(false, request_url));
                    }
                }
            }

            //base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// Action执行后，第二步
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Action执行之后
            //base.OnActionExecuted(filterContext);            
        }
    }
}
