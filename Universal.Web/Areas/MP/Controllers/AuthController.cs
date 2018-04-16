using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;
using Universal.Tools;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin;

namespace Universal.Web.Areas.MP.Controllers
{
    /// <summary>
    /// 授权页面
    /// </summary>
    public class AuthController : BaseMPController
    {
        /// <summary>
        /// 授权页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string returnUrl)
        {
            var state = "HouDe-" + DateTime.Now.Millisecond;//随机数，用于识别请求可靠性
            Session["Auth-State"] = state;
            string rediecturl = WorkContext.WebSite.SiteUrl + "/mp/auth/callback?returnUrl=" + returnUrl;
            string auth_url = OAuthApi.GetAuthorizeUrl(WorkContext.WebSite.WeChatAppID, rediecturl, state, Senparc.Weixin.MP.OAuthScope.snsapi_base);
            return Redirect(auth_url);
        }

        /// <summary>
        /// 授权回调页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CallBack(string code, string state, string returnUrl)
        {
            string agen_auth_url = WorkContext.WebSite.SiteUrl + "/mp/auth/index?returnUrl=" + returnUrl;
            if (string.IsNullOrWhiteSpace(code))
            {
                return PromptView(agen_auth_url, "授权失败，即将重试");
            }
            if (state != Session["Auth-State"] as string)
            {
                return PromptView("验证失败！请从正规途径进入！");
            }
            //通过，用code换取access_token
            var result = OAuthApi.GetAccessToken(WorkContext.WebSite.WeChatAppID, WorkContext.WebSite.WeChatAppSecret, code);
            if (result.errcode != ReturnCode.请求成功)
            {
                return PromptView(result.errmsg);
            }
            var status = BLL.BLLMPUserState.SetLogin(result.openid);
            if (!status) return Content("登录失败");
            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);
            else
                return Content("没有返回URL，OPENID：" + result.openid);
        }
    }
}