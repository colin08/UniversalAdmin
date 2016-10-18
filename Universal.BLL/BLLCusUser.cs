using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.Tools;
using System.Web;

namespace Universal.BLL
{
    /// <summary>
    /// 用户帮助类
    /// </summary>
    public class BLLCusUser
    {
        /// <summary>
        /// 判断用户是否登陆
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin()
        {
            if (HttpContext.Current.Session[SessionKey.Web_User_Info] != null)
                return true;
            else
            {
                //检查COOKIE
                int uid = TypeHelper.ObjectToInt(WebHelper.GetCookie(CookieKey.Web_Login_UserID));
                string upwd = WebHelper.GetCookie(CookieKey.Web_Login_UserPassword);
                if (uid != 0 && !string.IsNullOrWhiteSpace(upwd))
                {
                    BaseBLL<Entity.CusUser> bll = new BaseBLL<Entity.CusUser>();
                    List<FilterSearch> filters = new List<FilterSearch>();
                    filters.Add(new FilterSearch("ID", uid.ToString(), FilterSearchContract.等于));
                    filters.Add(new FilterSearch("Password", upwd, FilterSearchContract.等于));
                    Entity.CusUser model = bll.GetModel(filters);
                    if (model != null)
                    {
                        if (model.Status)
                        {
                            HttpContext.Current.Session[SessionKey.Web_User_Info] = model;
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
            }
        }


        /// <summary>
        /// 获取登陆用户的信息
        /// </summary>
        /// <returns></returns>
        public static Entity.CusUser GetUserInfo()
        {
            if (IsLogin())
            {
                Entity.CusUser model = HttpContext.Current.Session[SessionKey.Web_User_Info] as Entity.CusUser;
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

        /// <summary>
        /// 更新Session信息
        /// </summary>
        public static void ModifySession(Entity.CusUser model)
        {
            if (model == null)
                return;

            HttpContext.Current.Session[SessionKey.Web_User_Info] = null;
            
            HttpContext.Current.Session[SessionKey.Web_User_Info] = model;

        }
        

        /// <summary>
        /// 注销登录
        /// </summary>
        public static void LoginOut()
        {
            WebHelper.SetCookie(CookieKey.Web_Is_Remeber, "", -1);
            WebHelper.SetCookie(CookieKey.Web_Login_UserID, "", -1);
            WebHelper.SetCookie(CookieKey.Web_Login_UserPassword, "", -1);
            HttpContext.Current.Session[SessionKey.Web_User_Info] = null;
        }

    }
}
