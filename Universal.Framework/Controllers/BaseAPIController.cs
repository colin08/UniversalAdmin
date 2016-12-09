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
using Universal.Tools;

namespace Universal.Web.Framework
{
    public class BaseAPIController : ApiController
    {
        public APIWorkContext WorkContext = new APIWorkContext();
        /// <summary>
        /// 站点配置文件
        /// </summary>
        public WebSiteModel WebSite = null;
        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);
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
        /// <param name="request"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected HttpResponseMessage APIUploadReturnMsg(HttpRequestMessage request, string msgbox, string data)
        {
            string json = string.Empty;
            if (data == null)
            {
                json = "{\"data\":null,\"msgbox\":\"" + msgbox + "\",\"msg\":0}";
            }
            else
            {
                if (data.StartsWith("{")) //如果是对象类型
                    json = "{\"data\":" + data + ",\"msgbox\":\"" + msgbox + "\",\"msg\":1}";
                else
                    json = "{\"data\":\"" + data + "\",\"msgbox\":\"" + msgbox + "\",\"msg\":1}";
            }

            HttpResponseMessage response = new HttpResponseMessage();
            response = request.CreateResponse(HttpStatusCode.BadRequest);
            response.Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json");
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
