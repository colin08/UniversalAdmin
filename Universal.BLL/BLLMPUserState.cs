using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Universal.DataCore;
using Universal.Entity;
using Universal.Tools;

namespace Universal.BLL
{
    /// <summary>
    /// 微信用户操作
    /// </summary>
    public class BLLMPUserState
    {
        /// <summary>
        /// Session openid
        /// </summary>
        private static readonly string SESSION_OPENID = "SESSION-OPENID";

        /// <summary>
        /// Session 用户实体信息
        /// </summary>
        private static readonly string SESSION_USERINFO = "SESSION-USERINFO";

        /// <summary>
        /// Cookie Openid
        /// </summary>
        private static readonly string COOKIE_OPENID = "COOKIE-OPENID";



        /// <summary>
        /// 临时存储用户OPENID
        /// </summary>
        /// <param name="open_id"></param>
        public static void SetOpenID(string open_id)
        {
            if (string.IsNullOrWhiteSpace(open_id)) return;
            HttpContext.Current.Session[SESSION_OPENID] = open_id;
            WebHelper.SetCookie(COOKIE_OPENID, open_id, 14400);
        }

        /// <summary>
        /// 获取用户OPENID
        /// </summary>
        /// <returns></returns>
        public static string GetOpenID()
        {
            if (HttpContext.Current.Session[SESSION_OPENID] == null)
            {
                string cookie_open = WebHelper.GetCookie(COOKIE_OPENID);
                if (string.IsNullOrWhiteSpace(cookie_open)) return "";
                HttpContext.Current.Session[COOKIE_OPENID] = cookie_open;
                return cookie_open;
            }
            else
                return HttpContext.Current.Session[SESSION_OPENID].ToString();
        }

        /// <summary>
        /// 设置登录
        /// </summary>
        /// <returns></returns>
        public static bool SetLogin(string open_id)
        {
            if (string.IsNullOrWhiteSpace(open_id)) return false;
            SetOpenID(open_id);//Cookie中保存openid
            var entity = BLLMPUser.GetUserInfoOrAdd(open_id);
            if (entity != null)
            {
                HttpContext.Current.Session[SESSION_USERINFO] = entity;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断用户是否登陆
        /// 没登录就取Cookie做登录操作
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin()
        {
            if (HttpContext.Current.Session[SESSION_USERINFO] == null)
            {
                var open_id = GetOpenID();
                if (string.IsNullOrWhiteSpace(open_id)) return false;
                var entity = BLLMPUser.GetUserInfoOrAdd(open_id);
                if (entity != null)
                {
                    HttpContext.Current.Session[SESSION_USERINFO] = entity;
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }


        /// <summary>
        /// 获取登陆用户的信息
        /// </summary>
        /// <returns></returns>
        public static MPUser GetUserInfo()
        {
            if (IsLogin())
            {
                var model = HttpContext.Current.Session[SESSION_USERINFO] as MPUser;
                if (model != null)
                {
                    if (model.Status)
                        return model;
                    else
                        return null;
                }
                return null;
            }
            return null;
        }
    }
}
