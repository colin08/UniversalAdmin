using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Universal.Web.Framework
{
    /// <summary>
    /// Web API2 自定义异常过滤
    /// </summary>
    public class CustomAPIExceptionAttribute: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //异常信息
            string error_msg = actionExecutedContext.Exception.Message;

            //TODO:将异常信息保存到数据库

            WebAjaxEntity<string> model = new WebAjaxEntity<string>();
            model.msgbox = error_msg;
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, model);
            base.OnException(actionExecutedContext);
        }
    }
}
