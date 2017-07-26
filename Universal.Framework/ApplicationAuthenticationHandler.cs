using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Universal.Web.Framework
{
    /// <summary>
    /// 更改返回的数据类型为Json
    /// </summary>
    public class JsonContentNegotiator : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter _jsonFormatter;

        public JsonContentNegotiator(JsonMediaTypeFormatter formatter)
        {
            _jsonFormatter = formatter;
        }

        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            var result = new ContentNegotiationResult(_jsonFormatter, new MediaTypeHeaderValue("application/json"));
            return result;
        }
    }

    /// <summary>
    /// APP接口验证
    /// </summary>
    public class ApplicationAuthenticationHandler : DelegatingHandler
    {

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var site_config = Tools.ConfigHelper.LoadConfig<Tools.WebSiteModel>(Tools.ConfigFileEnum.SiteConfig);
            IEnumerable<string> monsterApiKeyHeaderValues = null;

            //如果关闭验证
            if (!site_config.WebAPIAuthentication)
                return base.SendAsync(request, cancellationToken);

            //验证HTTP报文头
            if (request.Headers.TryGetValues(site_config.WebAPITokenKey, out monsterApiKeyHeaderValues))
            {
                string oauth = monsterApiKeyHeaderValues.First();
                if (string.IsNullOrWhiteSpace(oauth))
                    return requestCancel(request, cancellationToken, "缺少授权参数");

                Tools.Crypto3DES des = new Tools.Crypto3DES(SiteKey.DES3KEY);
                string[] vals = des.DESDeCode(oauth).Split('!');
                if (vals.Length != 2)
                    return requestCancel(request, cancellationToken, "授权格式错误");

                if (!vals[0].Equals(site_config.WebAPIMixer))
                    return requestCancel(request, cancellationToken, "授权数据错误1");


                DateTime dt_now = DateTime.Now;
                DateTime dt_old = Tools.WebHelper.GetTime(vals[1], dt_now);
                double diff = Tools.WebHelper.DateTimeDiff(dt_now, dt_old, "am"); //分钟
                if (dt_now == dt_old)
                    return requestCancel(request, cancellationToken, "授权时间有误");//如果时间一样，则客户端传上来的时间戳不正确
                if (site_config.WebAPITmeOut == 0) requestOK();
                else
                {
                    if (diff >= site_config.WebAPITmeOut) return requestCancel(request, cancellationToken, "请求超时");
                    else requestOK();
                }
            }
            else
            {
                return requestCancel(request, cancellationToken, "Unauthorized");
            }

            return base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 请求校验通过
        /// </summary>
        private void requestOK()
        {
            var thread_key = "Thread_KEY";
            var userNameClaim = new Claim(ClaimTypes.Name, thread_key);
            var identity = new ClaimsIdentity(new[] { userNameClaim }, "MonsterAppApiKey");
            var principal = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = principal;

            if (System.Web.HttpContext.Current != null)
                System.Web.HttpContext.Current.User = principal;
        }

        /// <summary>
        /// 取消请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private Task<HttpResponseMessage> requestCancel(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken, string message)
        {
            UnifiedResultEntity<bool> result = new UnifiedResultEntity<bool>();
            result.msgbox = message;

            CancellationTokenSource _tokenSource = new CancellationTokenSource();
            cancellationToken = _tokenSource.Token;
            _tokenSource.Cancel();
            HttpResponseMessage response = new HttpResponseMessage();
            response = request.CreateResponse(HttpStatusCode.Unauthorized);
            response.Content = new StringContent(Tools.JsonHelper.ToJson(result), Encoding.GetEncoding("UTF-8"), "application/json");
            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
                return response;
            });
        }
    }
}
