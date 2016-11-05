using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.Tools;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

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
                    Entity.CusUser model = bll.GetModel(filters, p => p.CusUserRoute.Select(s=>s.CusRoute));
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

        /// <summary>
        /// 添加/修改用户
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="route"></param>
        public static int Modify(Entity.CusUser entity, string route)
        {
            var db = new DataCore.EFDBContext();
            
            if(entity.ID > 0)
            {
                db.CusUserRoutes.Where(p => p.CusUserID == entity.ID).ToList().ForEach(p => db.CusUserRoutes.Remove(p));
                DbEntityEntry entry = db.Entry<Entity.CusUser>(entity);
                entry.State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                db.Set<Entity.CusUser>().Add(entity);
            }
            

            foreach (var item in route.Split(','))
            {
                int route_id = TypeHelper.ObjectToInt(item);
                var route_entity = new Entity.CusUserRoute();
                var rte = db.CusRoutes.Find(route_id);
                if (rte != null)
                {
                    route_entity.CusRoute = rte;
                    route_entity.CusUser = entity;
                    db.CusUserRoutes.Add(route_entity);
                }
            }
            db.SaveChanges();
            db.Dispose();
            return entity.ID;
        }

        /// <summary>
        /// 获取用户实体，包含incloud
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static Entity.CusUser GetModel(int user_id)
        {
            var db = new DataCore.EFDBContext();
            Entity.CusUser entity = db.CusUsers.Include(p=>p.CusDepartment).Include(p=>p.CusUserJob).Where(p=>p.ID == user_id).AsNoTracking().FirstOrDefault();
            db.Dispose();
            return entity;
        }
        
    }
}
