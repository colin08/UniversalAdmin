using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Universal.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            //异常过滤器
            config.Filters.Add(new Framework.CustomAPIExceptionAttribute());
            //接口日志过滤器
            config.Filters.Add(new Framework.CustomApiTrackerAttribute());
            
            //更改返回类型为json
            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd hh:mm:ss" });
            config.Services.Replace(typeof(IContentNegotiator), new Framework.JsonContentNegotiator(jsonFormatter));
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Error404",
                routeTemplate: "{*url}",
                defaults: new { controller = "Error", action = "Handle404" }
            );

            //处理Web API 404错误
            config.Services.Replace(typeof(System.Web.Http.Dispatcher.IHttpControllerSelector), new HttpNotFoundAwareDefaultHttpControllerSelector(config));
            config.Services.Replace(typeof(System.Web.Http.Controllers.IHttpActionSelector), new HttpNotFoundAwareControllerActionSelector());
        }
    }
}
