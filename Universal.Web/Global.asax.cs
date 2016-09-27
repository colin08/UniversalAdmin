using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Optimization;
using StackExchange.Profiling;

namespace Universal.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            MiniProfiler.Settings.Results_Authorize = Request =>
            {
                string oid = Request.Cookies["oid"] == null ? "0" : Request.Cookies["oid"].Value;
                if (oid.Equals("1"))
                    return true;
                else
                    return false;
            };

            //EF监控
            StackExchange.Profiling.EntityFramework6.MiniProfilerEF6.Initialize();

            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //TODO 接口校验处于关闭状态
            //GlobalConfiguration.Configuration.MessageHandlers.Add(new Universal.Web.Framework.ApplicationAuthenticationHandler());

        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)//这里是允许本地访问启动监控,可不写
            {
                MiniProfiler.Start();

            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }
    }
}