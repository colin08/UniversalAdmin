using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Universal.Web.Framework
{
    /// <summary>
    /// Web Api接口监控
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CustomApiTrackerAttribute : ActionFilterAttribute
    {
        private readonly string Key = "_thisWebApiOnActionMonitorLog_";
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            WebApiMonitorLog MonLog = new WebApiMonitorLog();
            MonLog.ExecuteStartTime = DateTime.Now;
            //获取Action 参数
            MonLog.ActionParams = actionContext.ActionArguments;
            MonLog.HttpRequestHeaders = actionContext.Request.Headers.ToString();
            MonLog.HttpMethod = actionContext.Request.Method.Method;

            actionContext.Request.Properties[Key] = MonLog;
            var form = System.Web.HttpContext.Current.Request.Form;
            #region 如果参数是实体对象，获取序列化后的数据
            Stream stream = actionContext.Request.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            stream.Position = 0;
            string responseData = "";
            //启用下面的代码接口上传文件将会报错
            //using (StreamReader reader = new StreamReader(stream, encoding))
            //{
            //    responseData = reader.ReadToEnd().ToString();
            //}
            if (!string.IsNullOrWhiteSpace(responseData) && !MonLog.ActionParams.ContainsKey("__EntityParamsList__"))
            {
                MonLog.ActionParams["__EntityParamsList__"] = responseData;
            }
            #endregion
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            WebApiMonitorLog MonLog = actionExecutedContext.Request.Properties[Key] as WebApiMonitorLog;
            MonLog.ExecuteEndTime = DateTime.Now;
            MonLog.ActionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            MonLog.ControllerName = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            MonLog.Uri = actionExecutedContext.Request.RequestUri.OriginalString;
            //Trace.WriteLine(MonLog.GetLoginfo());
            Tools.WebSiteModel site = Tools.ConfigHelper.LoadConfig<Tools.WebSiteModel>(Tools.ConfigFileEnum.SiteConfig);
            if (site.EnableAPILog)
            {
                BLL.BaseBLL<Entity.SysLogApiAction> bll = new BLL.BaseBLL<Entity.SysLogApiAction>();
                DateTime now_date = DateTime.Now.Date;
                bll.DelBy(p => p.ExecuteStartTime < now_date);
                bll.Add(MonLog.GetLogEntity());
            }
        }
    }
}
