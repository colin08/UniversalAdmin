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

namespace Universal.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //清除系统监听器
            System.Diagnostics.Trace.Listeners.Clear();
            //添加自定义监听器
            System.Diagnostics.Trace.Listeners.Add(new Tools.CustomTraceListener());

            GlobalConfiguration.Configuration.MessageHandlers.Add(new Framework.ApplicationAuthenticationHandler());

        }

        protected void Application_BeginRequest()
        {

        }

        protected void Application_EndRequest()
        {

        }
    }
}