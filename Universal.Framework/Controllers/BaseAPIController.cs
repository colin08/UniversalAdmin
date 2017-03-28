using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Threading;
using System.Net.Http.Headers;
using System.Web.Http.WebHost;
using System.Text;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace Universal.Web.Framework
{
    public class BaseAPIController : ApiController
    {
        public APIWorkContext WorkContext = new APIWorkContext();

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            WorkContext.AjaxStringEntity = new WebAjaxEntity<string>();
            WorkContext.AjaxStringEntity.msg = 0;
            WorkContext.AjaxStringEntity.data = "";
            base.Initialize(controllerContext);
        }
        

        /// <summary>
        /// 获取站点URL
        /// </summary>
        /// <returns></returns>
        protected string GetSiteUrl()
        {
            return "http://" + Request.RequestUri.Authority;
        }

        /// <summary>
        /// 用于通过WEB API上传文件同一返回请求
        /// </summary>
        /// <param name="result">返回对象</param>
        /// <returns></returns>
        protected HttpResponseMessage APIFileUploadResponse(HttpRequestMessage request, object result)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(Tools.JsonHelper.ToJson(result), Encoding.GetEncoding("UTF-8"), "application/json");
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }

    /// <summary>
    /// 拦截系统的异常信息，使用自定义格式进行输出
    /// </summary>
    public class Excenptss : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            WebAjaxEntity<string> model = new WebAjaxEntity<string>();
            model.msg = 0;
            model.msgbox = actionExecutedContext.Exception.Message;
            model.data = "";
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, model);
            base.OnException(actionExecutedContext);
        }
    }

    /// <summary>
    /// 取消内存缓存，以实现上传大文件
    /// </summary>
    public class CustomPolicy : WebHostBufferPolicySelector
    {
        #region Public Methods and Operators
        public override bool UseBufferedInputStream(object hostContext)
        {
            return false;
        }
        #endregion
    }
}
