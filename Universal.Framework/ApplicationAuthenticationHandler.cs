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
                            //        return requestCancel(request, cancellationToken, "{\"_data\":null,\"_msgbox\":\"超时\",\"_msg\":0}");
                            //    }
                            //}
                            //else
                            //{
                            //    return requestCancel(request, cancellationToken, "{\"_data\":null,\"_msgbox\":\"授权数据错误2\",\"_msg\":0}");
                            //}
                        }
                        else
                        {
                            return requestCancel(request, cancellationToken, "{\"data\":\"\",\"msgbox\":\"授权数据错误1\",\"msg\":0}");
                        }
                    }
                    else
                    {
                        return requestCancel(request, cancellationToken, "{\"data\":\"\",\"msgbox\":\"授权格式错误\",\"msg\":0}");
                    }
                }
                else
                {
                    return requestCancel(request, cancellationToken, "{\"data\":\"\",\"msgbox\":\"缺少授权参数\",\"msg\":0}");
                }
                //if (apiKeyHeaderValue.Length == 2)
                //{
                //    var appID = apiKeyHeaderValue[0];
                //    var AppKey = apiKeyHeaderValue[1];
                //    if (appID.Equals("test") && AppKey.Equals("123"))
                //    {
                //        var userNameClaim = new Claim(ClaimTypes.Name, appID);
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
                //        //Web请求取消原因应用程序键是无效的
                //        return requestCancel(request, cancellationToken, "{\"msg\":0,\"msgbox\":\"AppID或AppKey有误\"}");
                //    }
                //}
                //else
                //{
                //    //Web请求取消原因丢失钥匙或应用程序ID
                //    return requestCancel(request, cancellationToken, "{\"msg\":0,\"msgbox\":\"丢失的用户信息参数\"}");
                //}
            }
            else
            {
                return requestCancel(request, cancellationToken, "{\"data\":\"\",\"msgbox\":\"未经授权\",\"msg\":0}");
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
        private System.Threading.Tasks.Task<HttpResponseMessage> requestCancel(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken, string message)
        {
            CancellationTokenSource _tokenSource = new CancellationTokenSource();
            cancellationToken = _tokenSource.Token;
            _tokenSource.Cancel();
            HttpResponseMessage response = new HttpResponseMessage();
            response = request.CreateResponse(HttpStatusCode.BadRequest);
            response.Content = new StringContent(message, Encoding.GetEncoding("UTF-8"), "application/json");
            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
                return response;
            });
        }
    }
}
