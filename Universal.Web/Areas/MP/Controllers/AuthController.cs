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
        public ActionResult Index(string returnUrl, string t)
        {
            //默认需要用户授权
            var type = Senparc.Weixin.MP.OAuthScope.snsapi_userinfo;
            if (string.IsNullOrWhiteSpace(t))
            {
                if (t == "0") type = Senparc.Weixin.MP.OAuthScope.snsapi_base;

            }
            var state = "HouDe-" + DateTime.Now.Millisecond;//随机数，用于识别请求可靠性
            Session["Auth-State"] = state;
            string rediecturl = WorkContext.WebSite.SiteUrl + "/mp/auth/callback?returnUrl=" + returnUrl;
            string auth_url = OAuthApi.GetAuthorizeUrl(WorkContext.WebSite.WeChatAppID, rediecturl, state, type);
            return Redirect(auth_url);
        }

        /// <summary>
        /// 授权回调页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CallBack(string code, string state, string returnUrl)
        {
            string agen_auth_url = WorkContext.WebSite.SiteUrl + "/mp/auth/index?returnUrl=" + returnUrl + "&t=0";
            if (string.IsNullOrWhiteSpace(code))
            {
                //如果用户拒绝了授权，则重新跳到授权页面，使用静默方式
                return Redirect(agen_auth_url);
                //return PromptView(agen_auth_url, "授权失败，即将重试");
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
            //用token获取用户信息
            var url_getuserinfo = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", result.access_token, result.openid);
            var wx_user_info = Senparc.Weixin.CommonAPIs.CommonJsonSend.Send<OAuthUserInfo>(null, url_getuserinfo, null, CommonJsonSendType.GET);
            if(wx_user_info != null)
            {
                //判断用户，存在则不做操作，不存在则添加
                BLL.BLLMPUser.AddUserInfo(wx_user_info.openid, wx_user_info.nickname, wx_user_info.headimgurl, wx_user_info.sex);
            }
            var status = BLL.BLLMPUserState.SetLogin(result.openid);
            if (!status) return PromptView("登录失败");
            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);
            else
                return PromptView("没有跳转URL");//  Content("没有返回URL，OPENID：" + result.openid);
        }
    }
}