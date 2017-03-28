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

        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            WebAjaxEntity<bool> result = new WebAjaxEntity<bool>();
            IEnumerable<string> monsterApiKeyHeaderValues = null;

            //验证HTTP报文头
            if (request.Headers.TryGetValues("X-MonsterAccountToken", out monsterApiKeyHeaderValues))
            {
                //string[] apiKeyHeaderValue = monsterApiKeyHeaderValues.First().Split(':');
                string oauth = monsterApiKeyHeaderValues.First();
                if (!string.IsNullOrWhiteSpace(oauth))
                {
                    Tools.Crypto3DES des = new Tools.Crypto3DES(SiteKey.DES3KEY);
                    string[] vals = des.DESDeCode(oauth).Split('&');
                    if (vals.Length == 2)
                    {
                        string valstr = "lizd@fd9^orderfood!";
                        if (vals[0].Equals(valstr))
                        {

                            var userNameClaim = new Claim(ClaimTypes.Name, valstr);
                            var identity = new ClaimsIdentity(new[] { userNameClaim }, "MonsterAppApiKey");
                            var principal = new ClaimsPrincipal(identity);
                            Thread.CurrentPrincipal = principal;

                            if (System.Web.HttpContext.Current != null)
                            {
                                System.Web.HttpContext.Current.User = principal;
                            }

                            //DateTime dt_now = DateTime.Now;
                            //DateTime dt_old = WebHelper.GetTime(vals[1], dt_now);
                            //if (dt_now != dt_old)
                            //{
                            //    double diff = WebHelper.DateTimeDiff(dt_old, dt_now, "as");
                            //    int ss = 10;
                            //    if (request.Headers.UserAgent.ToString() == "Fiddler")
                            //        ss = 100000;
                            //    if (diff < ss) //10秒前的数据，则失败
                            //    {
                            //        var userNameClaim = new Claim(ClaimTypes.Name, valstr);
                            //        var identity = new ClaimsIdentity(new[] { userNameClaim }, "MonsterAppApiKey");
                            //        var principal = new ClaimsPrincipal(identity);
                            //        Thread.CurrentPrincipal = principal;

                            //        if (System.Web.HttpContext.Current != null)
                            //        {
                            //            System.Web.HttpContext.Current.User = principal;
                            //        }
                            //    }
                            //    else
                            //    {
                            //          result.msgbox = "超时";
                            //          return requestCancel(request, cancellationToken, result);
                            //    }
                            //}
                            //else
                            //{
                            //    result.msgbox = "授权数据错误2";
                            //    return requestCancel(request, cancellationToken, result);
                            //}
                        }
                        else
                        {
                            result.msgbox = "授权数据错误1";
                            return requestCancel(request, cancellationToken, result);
                        }
                    }
                    else
                    {
                        result.msgbox = "授权格式错误";
                        return requestCancel(request, cancellationToken, result);
                    }
                }
                else
                {
                    result.msgbox = "缺少授权参数";
                    return requestCancel(request, cancellationToken, result);
                }
            }
            else
            {
                result.msgbox = "Unauthorized";
                return requestCancel(request, cancellationToken, result);
            }

            return base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 取消请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private Task<HttpResponseMessage> requestCancel(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken, WebAjaxEntity<bool> result)
        {
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
