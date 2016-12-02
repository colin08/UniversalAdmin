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
                    Entity.CusUser model = GetModelID(uid, upwd);
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
            Entity.CusUser entity = db.CusUsers.Include(p => p.CusUserRoute.Select(s => s.CusRoute)).Include(p=>p.CusDepartment).Include(p=>p.CusUserJob).Where(p=>p.ID == user_id).AsNoTracking().FirstOrDefault();
            db.Dispose();
            return entity;
        }
     
        /// <summary>
        /// 根据邮箱和密码获取用户实体
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static Entity.CusUser GetModel(string email,string pwd)
        {
            var db = new DataCore.EFDBContext();
            Entity.CusUser entity = db.CusUsers.Include(p => p.CusUserRoute.Select(s => s.CusRoute)).Include(p => p.CusDepartment).Include(p => p.CusUserJob).Where(p => p.Email == email && p.Password == pwd).AsNoTracking().FirstOrDefault();
            db.Dispose();
            return entity;
        }

        /// <summary>
        /// 根据手机号和密码获取用户实体
        /// </summary>
        /// <param name="telphone"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static Entity.CusUser GetModelTelphone(string telphone, string pwd)
        {
            var db = new DataCore.EFDBContext();
            Entity.CusUser entity = db.CusUsers.Include(p => p.CusUserRoute.Select(s => s.CusRoute)).Include(p => p.CusDepartment).Include(p => p.CusUserJob).Where(p => p.Telphone == telphone && p.Password == pwd).AsNoTracking().FirstOrDefault();
            db.Dispose();
            return entity;
        }

        /// <summary>
        /// 根据ID和密码获取用户实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static Entity.CusUser GetModelID(int id, string pwd)
        {
            var db = new DataCore.EFDBContext();
            Entity.CusUser entity = db.CusUsers.Include(p => p.CusUserRoute.Select(s => s.CusRoute)).Include(p => p.CusDepartment).Include(p => p.CusUserJob).Where(p => p.ID == id && p.Password == pwd).AsNoTracking().FirstOrDefault();
            db.Dispose();
            return entity;
        }


        /// <summary>
        /// 根据多个用户id获取集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<Entity.CusUser> GetListByIds(string ids)
        {
            if(string.IsNullOrWhiteSpace(ids))
            {
                return new List<Entity.CusUser>();
            }
            //开头有逗号
            if(ids.StartsWith(","))
            {
                ids = ids.Substring(1, ids.Length - 1);
            }
            if(ids.EndsWith(","))
            {
                ids = ids.Substring(0, ids.Length - 1);
            }

            List<Entity.CusUser> response_entity = new List<Entity.CusUser>();
            var db = new DataCore.EFDBContext();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse).ToList();
            response_entity = db.CusUsers.Where(p => id_list.Contains(p.ID)).ToList();
            db.Dispose();
            return response_entity;
        }

    }
}
