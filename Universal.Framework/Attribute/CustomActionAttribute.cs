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
    /// Action之前前后做处理
    /// </summary>
    public class CustomActionAttribute :ActionFilterAttribute,IActionFilter
    {
        private WebSiteModel SiteConfig =null;
        private Stopwatch timer;

        public CustomActionAttribute()
        {
            SiteConfig = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);
        }

        /// <summary>
        /// Action执行前，第一步
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //System.Diagnostics.Trace.WriteLine(filterContext.HttpContext.Request.Url.AbsoluteUri);
            //filterContext.HttpContext.Response.Write("1. OnActionExecuting  \r\n");
            if(SiteConfig.WebExecutionTime)
            {
                this.timer = new Stopwatch();
                this.timer.Start();
            }
        }

        /// <summary>
        /// Action执行后，第二步
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //filterContext.HttpContext.Response.Write("2. OnActionExecuted \r\n");
            if(SiteConfig.WebExecutionTime)
            {
                this.timer.Stop();
                long milli = this.timer.ElapsedMilliseconds;
                //Trace.WriteLine(string.Format("执行时间：{0}ms", milli.ToString()));
                if (filterContext.Result is ViewResult)
                {
                    ((ViewResult)filterContext.Result).ViewData["ExecutionTime"] = this.timer.ElapsedMilliseconds;
                }
                //string controllerName = filterContext.RouteData.Values["controller"].ToString();
                //string actionName = filterContext.RouteData.Values["action"].ToString();
                
            }
            
        }
    }
}
