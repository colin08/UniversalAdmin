using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.Tools;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using EntityFramework.Extensions;

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
        /// 设置默认的审核用户Cookie
        /// </summary>
        public static void SetDefaultApproveUser(int user_id)
        {
            var entity = GetSimpleModel(user_id);
            if (entity == null)
                return;

            WebHelper.SetCookie(CookieKey.Default_Approve_User, user_id.ToString());
        }

        /// <summary>
        /// 获取默认的审核用户Cookie
        /// </summary>
        /// <param name="nick_name"></param>
        /// <returns></returns>
        public static int GetDefaultApproveUser(out string nick_name)
        {
            nick_name = "";
            int user_id = TypeHelper.ObjectToInt(WebHelper.GetCookie(CookieKey.Default_Approve_User),-1);
            if(user_id == -1)
                return 0;
            var entity = GetSimpleModel(user_id);
            if (entity == null)
                return 0;
            if (!entity.Status)
                return 0;
            nick_name = entity.NickName;
            return user_id;
        }

        /// <summary>
        /// 添加/修改用户
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="route"></param>
        public static int Modify(Entity.CusUser entity, string route)
        {
            var db = new DataCore.EFDBContext();
            if (string.IsNullOrWhiteSpace(route))
                route = "";
            if (entity.ID > 0)
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
            Entity.CusUser entity = db.CusUsers.Include(p => p.CusUserRoute.Select(s => s.CusRoute)).Include(p => p.CusDepartment).Include(p => p.CusUserJob).Where(p => p.ID == user_id).AsNoTracking().FirstOrDefault();
            db.Dispose();
            return entity;
        }

        /// <summary>
        /// 获取用户简单的信息，不包含include
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static Entity.CusUser GetSimpleModel(int user_id)
        {
            using (var db =new DataCore.EFDBContext())
            {
                return db.CusUsers.Where(p => p.ID == user_id).AsNoTracking().FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取用户昵称
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static string GetNickName(int user_id)
        {
            var entity = GetSimpleModel(user_id);
            if (entity == null)
                return "";
            return entity.NickName;
        }

        /// <summary>
        /// 获取用户信息，不包含incloud
        /// </summary>
        /// <param name="telphone"></param>
        /// <returns></returns>
        public static Entity.CusUser GetModel(string telphone)
        {
            using (var db = new DataCore.EFDBContext())
            {
                return db.CusUsers.Where(p => p.Telphone == telphone).FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据邮箱和密码获取用户实体
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static Entity.CusUser GetModel(string email, string pwd)
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
            if (string.IsNullOrWhiteSpace(ids))
            {
                return new List<Entity.CusUser>();
            }
            //开头有逗号
            if (ids.StartsWith(","))
            {
                ids = ids.Substring(1, ids.Length - 1);
            }
            if (ids.EndsWith(","))
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

        /// <summary>
        /// 用户所属部门的主管信息，如果该用户已是主管，则向上层查找；如果当前部门没有主管，则向上层查找
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static List<Entity.CusUser> GetUserDepartmentAdmin(int user_id)
        {
            List<Entity.CusUser> response_entity = new List<Entity.CusUser>();
            var db = new DataCore.EFDBContext();
            var entity_user = db.CusUsers.Find(user_id);
            if (entity_user == null)
                return response_entity;
            int department_id = entity_user.CusDepartmentID;
            //如果用户是主管,则查找上级
            if (db.CusDepartmentAdmins.Any(p => p.CusUserID == user_id && p.CusDepartmentID == entity_user.CusDepartmentID))
            {
                var entity_department = db.CusDepartments.Where(p => p.ID == department_id).FirstOrDefault();
                if (entity_department != null)
                {
                    department_id = TypeHelper.ObjectToInt(entity_department.PID, department_id);
                }
            }
            response_entity = BLLDepartment.GetDepartmentAdminUp(db, department_id);
            db.Dispose();
            return response_entity;
        }

        /// <summary>
        /// 用户所属部门的主管信息【返回文本】，如果当前部门没有主管，则向上层查找，以此类推
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static string GetUserDepartmentAdminText(int user_id)
        {
            System.Text.StringBuilder name_text = new System.Text.StringBuilder();
            foreach (var User in BLL.BLLCusUser.GetUserDepartmentAdmin(user_id))
                name_text.Append(User.NickName + ",");
            if (name_text.Length > 0)
            {
                name_text = name_text.Remove(name_text.Length - 1, 1);
                return name_text.ToString();
            }
            else
                return "";
        }

        /// <summary>
        /// 校验用户是否存在
        /// </summary>
        /// <param name="telphone"></param>
        /// <returns></returns>
        public static bool Exists(string telphone)
        {
            using (var db = new DataCore.EFDBContext())
            {
                return db.CusUsers.Any(p => p.Telphone == telphone);
            }
        }

        /// <summary>
        /// 用户通讯录列表
        /// </summary>
        /// <returns></returns>
        public static List<Entity.CusUser> GetPageData(int page_index, int page_size, ref int rowCount, int department_id, string search_title)
        {
            rowCount = 0;
            List<Entity.CusUser> response_entity = new List<Entity.CusUser>();
            int begin_index = (page_index - 1) * page_size + 1;
            int end_index = page_index * page_size;

            var db = new DataCore.EFDBContext();
            string sql = "";
            string sql_total = "";

            string strWhere = " Where ID>0 ";
            if (department_id > 0)
            {
                strWhere += " and CusDepartmentID = " + department_id + " ";
            }

            if (!string.IsNullOrWhiteSpace(search_title))
                strWhere += " and (Charindex(N'" + search_title + "',NickName)>0 or Charindex(N'" + search_title + "',Telphone)>0)";

            if (department_id > 0 && !string.IsNullOrWhiteSpace(search_title))
                strWhere += " and CusDepartmentID = " + department_id + " and (Charindex('" + search_title + "',NickName)>0 or Charindex('" + search_title + "',Telphone)>0)";
            
            sql = "select * from (select ROW_NUMBER() OVER(ORDER BY RegTime ASC) as row,* from (select * from CusUser " + strWhere + ") as Z) as T  where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
            sql_total = "select count(1)  from CusUser " + strWhere;


            rowCount = db.Database.SqlQuery<int>(sql_total).ToList()[0];
            response_entity = db.Database.SqlQuery<Entity.CusUser>(sql).ToList();
            db.Dispose();
            return response_entity;
        }


        /// <summary>
        /// 添加用户下载记录
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool AddDownLog(int user_id, string title)
        {
            BLL.BaseBLL<Entity.DownloadLog> bll = new BaseBLL<Entity.DownloadLog>();
            var entity = new Entity.DownloadLog();
            entity.CusUserID = user_id;
            entity.Title = title;
            bll.Add(entity);
            return true;
        }

        /// <summary>
        /// 判断用户是否是部门主管，用于判断审批人是否必填
        /// </summary>
        /// <returns></returns>
        public static bool CheckUserIsAdmin(int user_id)
        {
            using (var db =new DataCore.EFDBContext())
            {
                return !(db.CusDepartmentAdmins.Any(p => p.CusUserID == user_id));
            }
        }

        /// <summary>
        /// 获取用户任务待办
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="user_id"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static List<Entity.CusUserMessage> GetJobTaskPageList(int page_size,int page_index,int user_id,out int total)
        {
            using (var db =new DataCore.EFDBContext())
            {
                List<Entity.CusUserMessageType> msg_type = new List<Entity.CusUserMessageType>();
                msg_type.Add(Entity.CusUserMessageType.approveproject);
                msg_type.Add(Entity.CusUserMessageType.appproveok);
                msg_type.Add(Entity.CusUserMessageType.appproveno);
                msg_type.Add(Entity.CusUserMessageType.confrimjoinmeeting);
                msg_type.Add(Entity.CusUserMessageType.meetingcancel);
                msg_type.Add(Entity.CusUserMessageType.meetingchangedate);
                msg_type.Add(Entity.CusUserMessageType.waitmeeting);
                msg_type.Add(Entity.CusUserMessageType.waitjobdone);
                msg_type.Add(Entity.CusUserMessageType.waitapproveplan);
                msg_type.Add(Entity.CusUserMessageType.planapproveok);

                //使用拓展框架
                var q = db.CusUserMessages.Where(p => p.CusUserID == user_id && p.IsDone == false
                            && msg_type.Contains(p.Type)
                            );
                var q1 = q.FutureCount();
                var q3 = q.OrderByDescending(p=>p.AddTime).Skip((page_index - 1) * page_size).Take(page_size).AsNoTracking().Future();
                total = q1.Value;
                return q3.ToList();
            }
        }

        /// <summary>
        /// 获取用户消息列表
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="user_id"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static List<Entity.CusUserMessage> GetUseMessagePageList(int page_size, int page_index, int user_id,int msg_type,string searh_word, out int total)
        {
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            filter.Add(new BLL.FilterSearch("CusUserID", user_id.ToString(), BLL.FilterSearchContract.等于));
            switch (msg_type)
            {
                case 1://未读
                    filter.Add(new BLL.FilterSearch("IsRead", "0", BLL.FilterSearchContract.等于));
                    break;
                case 2://已读
                    filter.Add(new BLL.FilterSearch("IsRead", "1", BLL.FilterSearchContract.等于));
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrWhiteSpace(searh_word))
                filter.Add(new BLL.FilterSearch("Content", searh_word, BLL.FilterSearchContract.like));

            filter.Add(new BLL.FilterSearch("Type", ((int)Entity.CusUserMessageType.approveproject).ToString(), FilterSearchContract.不等于));
            filter.Add(new BLL.FilterSearch("Type", ((int)Entity.CusUserMessageType.appproveok).ToString(), FilterSearchContract.不等于));
            filter.Add(new BLL.FilterSearch("Type", ((int)Entity.CusUserMessageType.appproveno).ToString(), FilterSearchContract.不等于));
            filter.Add(new BLL.FilterSearch("Type", ((int)Entity.CusUserMessageType.confrimjoinmeeting).ToString(), FilterSearchContract.不等于));
            filter.Add(new BLL.FilterSearch("Type", ((int)Entity.CusUserMessageType.meetingcancel).ToString(), FilterSearchContract.不等于));
            filter.Add(new BLL.FilterSearch("Type", ((int)Entity.CusUserMessageType.meetingchangedate).ToString(), FilterSearchContract.不等于));
            filter.Add(new BLL.FilterSearch("Type", ((int)Entity.CusUserMessageType.waitmeeting).ToString(), FilterSearchContract.不等于));
            filter.Add(new BLL.FilterSearch("Type", ((int)Entity.CusUserMessageType.waitjobdone).ToString(), FilterSearchContract.不等于));
            filter.Add(new BLL.FilterSearch("Type", ((int)Entity.CusUserMessageType.waitapproveplan).ToString(), FilterSearchContract.不等于));
            filter.Add(new BLL.FilterSearch("Type", ((int)Entity.CusUserMessageType.planapproveok).ToString(), FilterSearchContract.不等于));
            var db_list = new BLL.BaseBLL<Entity.CusUserMessage>().GetPagedList(page_index, page_size, ref rowCount, filter, "AddTime desc");

            total = rowCount;
            return db_list;
        }
    }
}
