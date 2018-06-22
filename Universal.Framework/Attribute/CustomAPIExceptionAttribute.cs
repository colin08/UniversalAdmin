//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Http.Filters;

//namespace Universal.Web.Framework
//{
//    /// <summary>
//    /// Web API2 自定义异常过滤
//    /// </summary>
//    public class CustomAPIExceptionAttribute : ExceptionFilterAttribute
//    {
//        public override void OnException(HttpActionExecutedContext actionExecutedContext)
//        {
//            //异常信息
//            string error_msg = actionExecutedContext.Exception.Message;

//            ExceptionInDB.ToInDB(actionExecutedContext.Exception);

//            UnifiedResultEntity<string> model = new UnifiedResultEntity<string>();
//            model.msgbox = error_msg;
//            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, model);
//            base.OnException(actionExecutedContext);
//        }
//    }
//}
