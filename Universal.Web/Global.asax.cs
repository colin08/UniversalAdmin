using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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

            //GlobalConfiguration.Configuration.MessageHandlers.Add(new Framework.ApplicationAuthenticationHandler());

            Senparc.Weixin.MP.Containers.AccessTokenContainer.Register("wxbc002fc3927e272d", "5a26dfba89b538fa60a31c97cacee3c7");
        }


        void Application_End()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected void Application_BeginRequest()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        protected void Application_EndRequest()
        {

        }
    }
}