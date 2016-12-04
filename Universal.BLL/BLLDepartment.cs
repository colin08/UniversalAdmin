using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    public class BLLDepartment
    {

        static string CacheDataKey1 = "CreateDepartmentTreeDataKEY";
        static string CacheDataKey2 = "CreateDepartmentTreeDataDEFAULTID";

        /// <summary>
        /// 添加部门数据
        /// </summary>
        /// <returns></returns>
        public static bool Add(int pid, string title, string user_ids)
        {
            if (pid < 0)
                return false;

            if (string.IsNullOrWhiteSpace(title))
                return false;

            var db = new DataCore.EFDBContext();
            var department = new Entity.CusDepartment();
            if (pid == 0)
            {
                //顶级ID
                department.PID = null;
                department.Depth = 1;
            }
            else
            {
                var p_department = db.CusDepartments.Find(pid);
                if (p_department == null)
                    return false;

                department.PID = pid;
                department.Depth = p_department.Depth + 1;


                if (department.Depth > 3)
                    return false;
            }


            department.Title = title;
            foreach (var sid in user_ids.Split(','))
            {
                int id = Tools.TypeHelper.ObjectToInt(sid);
                var user = db.CusUsers.Find(id);
                if (user == null)
                    break;
                if (!user.IsAdmin)
                    break;

                var admin = new Entity.CusDepartmentAdmin();
                admin.CusDepartment = department;
                admin.CusUser = user;
                db.CusDepartmentAdmins.Add(admin);
            }

            db.CusDepartments.Add(department);
            db.SaveChanges();
            db.Dispose();
            Tools.CacheHelper.Remove(CacheDataKey1);
            Tools.CacheHelper.Remove(CacheDataKey2);
            return true;
        }

        /// <summary>
        /// 修改部门数据
        /// </summary>
        /// <returns></returns>
        public static bool Modify(int id, string title, string user_ids)
        {
            if (id <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(title))
                return false;

            var db = new DataCore.EFDBContext();
            var department = db.CusDepartments.Find(id);
            if (department == null)
                return false;

            department.Title = title;
            db.CusDepartmentAdmins.Where(p => p.CusDepartmentID == id).ToList().ForEach(p => db.CusDepartmentAdmins.Remove(p));

            foreach (var sid in user_ids.Split(','))
            {
                int user_id = Tools.TypeHelper.ObjectToInt(sid);
                var user = db.CusUsers.Find(user_id);
                if (user == null)
                    break;
                if (!user.IsAdmin)
                    break;

                var admin = new Entity.CusDepartmentAdmin();
                admin.CusDepartment = department;
                admin.CusUser = user;
                db.CusDepartmentAdmins.Add(admin);
            }

            db.SaveChanges();
            db.Dispose();
            Tools.CacheHelper.Remove(CacheDataKey1);
            Tools.CacheHelper.Remove(CacheDataKey2);
            return true;
        }

        /// <summary>
        /// 根据部门ID获取部门子或父数据
        /// </summary>
        /// <param name="up">查找父级，否则为查找子级</param>
        /// <param name="id">当前分类ID</param>
        /// <returns></returns>
        public static List<Entity.CusDepartment> GetList(bool up, int id)
        {
            List<Entity.CusDepartment> list = new List<Entity.CusDepartment>();
            var db = new DataCore.EFDBContext();
            SqlParameter[] param = { new SqlParameter("@Id", id) };
            string proc_name = "dbo.sp_GetParentDepartments @Id";
            if (!up)
                proc_name = "dbo.sp_GetChildDepartments @Id";
            list = db.CusDepartments.SqlQuery(proc_name, param).ToList();
            db.Dispose();
            return list;
        }

        /// <summary>
        /// 删除部门，同时删除其子数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Del(int id)
        {
            if (id <= 0)
                return false;
            var db = new DataCore.EFDBContext();
            List<Entity.CusDepartment> child_list = GetList(false, id);
            foreach (var item in child_list)
            {
                db.Set<Entity.CusDepartment>().Attach(item);
                db.Set<Entity.CusDepartment>().Remove(item);
            }
            var entity = db.CusDepartments.Find(id);
            db.CusDepartments.Remove(entity);
            db.SaveChanges();
            db.Dispose();
            Tools.CacheHelper.Remove(CacheDataKey1);
            Tools.CacheHelper.Remove(CacheDataKey2);
            return true;
        }

        /// <summary>
        /// 根据多个部门id获取集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<Entity.CusDepartment> GetListByIds(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return new List<Entity.CusDepartment>();
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

            List<Entity.CusDepartment> response_entity = new List<Entity.CusDepartment>();
            var db = new DataCore.EFDBContext();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse).ToList();
            response_entity = db.CusDepartments.Where(p => id_list.Contains(p.ID)).ToList();
            db.Dispose();
            return response_entity;
        }

        /// <summary>
        /// 获取所属某个分类的用户数量，包含子类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetDepartChildDataTotal(DataCore.EFDBContext db, int id)
        {
            string Sql = "select count(1) as total from CusUser where charindex(','+LTRIM(CusDepartmentID)+',',','+(select dbo.fn_GetChildDepartmentStr(" + id.ToString() + "))+',')>0";
            return db.Database.SqlQuery<int>(Sql).ToList()[0];
        }

        /// <summary>
        /// 获取部门主管，如果当前部门没有主管，则向上层查找，以此类推
        /// </summary>
        /// <param name="department_id">部门ID</param>
        /// <returns></returns>
        public static List<Entity.CusUser> GetDepartmentAdminUp(int department_id)
        {
            List<Entity.CusUser> response_entity = new List<Entity.CusUser>();
            var db = new DataCore.EFDBContext();
            string strSql = "select dbo.fn_GetWorkPlanApproveUserIds(" + department_id.ToString() + ")";
            var ids = db.Database.SqlQuery<string>(strSql).ToList()[0];
            if (string.IsNullOrWhiteSpace(ids))
                return response_entity;
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            response_entity = db.CusUsers.Where(p => id_list.Contains(p.ID)).ToList();
            db.Dispose();
            return response_entity;
        }

        /// <summary>
        /// 获取部门主管，如果当前部门没有主管，则向上层查找，以此类推
        /// </summary>
        /// <param name="department_id">部门ID</param>
        /// <returns></returns>
        public static List<Entity.CusUser> GetDepartmentAdminUp(DataCore.EFDBContext db,int department_id)
        {
            List<Entity.CusUser> response_entity = new List<Entity.CusUser>();
            string strSql = "select dbo.fn_GetWorkPlanApproveUserIds(" + department_id.ToString() + ")";
            var ids = db.Database.SqlQuery<string>(strSql).ToList()[0];
            if (string.IsNullOrWhiteSpace(ids))
                return response_entity;
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            response_entity = db.CusUsers.Where(p => id_list.Contains(p.ID)).ToList();
            return response_entity;
        }

        /// <summary>
        /// 构造部门树数据
        /// </summary>
        /// <param name="default_id">返回默认第一个供加载数据的分类ID</param>
        /// <returns></returns>
        public static string CreateDepartmentTreeData(out int default_id)
        {
            default_id = 0;

            object tree_data = Tools.CacheHelper.Get(CacheDataKey1);
            if (tree_data == null)
            {
                tree_data = DepartmentTreeData(out default_id);
                if (tree_data != null)
                {
                    Tools.CacheHelper.Insert(CacheDataKey1, tree_data, Tools.SiteKey.CACHE_TIME);
                    Tools.CacheHelper.Insert(CacheDataKey2, default_id, Tools.SiteKey.CACHE_TIME);
                }
            }
            else
            {
                default_id = Tools.TypeHelper.ObjectToInt(Tools.CacheHelper.Get(CacheDataKey2));
            }


            return tree_data.ToString();

        }

        private static string DepartmentTreeData(out int default_id)
        {
            BLL.BaseBLL<Entity.CusDepartment> bll = new BLL.BaseBLL<Entity.CusDepartment>();
            List<Entity.CusDepartment> list = bll.GetListBy(0, p => p.Status == true, "Priority Desc", false);
            System.Text.StringBuilder data = new System.Text.StringBuilder();
            default_id = 0;
            var db = new DataCore.EFDBContext();
            foreach (var one in list.Where(p => p.Depth == 1).OrderByDescending(p => p.Priority))
            {
                //构建第一层
                data.Append("<li><span class=\"tree2\" onclick='pageData(1," + one.ID + ",true)'><b class=\"Off\">" + one.Title + " (" + GetDepartChildDataTotal(db, one.ID).ToString() + "人)</b></span>");
                data.Append("<ul style=\"display: none; \">");
                foreach (var two in list.Where(p => p.Depth == 2 && p.PID == one.ID).OrderByDescending(p => p.Priority))
                {
                    data.Append("<li><span class=\"tree3\"  onclick='pageData(1," + two.ID + ",true)'><b class=\"Off\">" + two.Title + " (" + GetDepartChildDataTotal(db, two.ID).ToString() + "人)</b></span>");
                    data.Append("<ul style=\"display: none;\">");
                    foreach (var three in list.Where(p => p.Depth == 3 && p.PID == two.ID).OrderByDescending(p => p.Priority))
                    {
                        if (default_id == 0)
                            default_id = three.ID;

                        data.Append("<li><span class=\"tree4\" onclick='pageData(1," + three.ID + ",true)'><b>" + three.Title + "(" + GetDepartChildDataTotal(db, three.ID).ToString() + "人)</b></span></li>");

                    }
                    data.Append("</ul></li>");
                }

                data.Append("</ul></li>");                
            }
            db.Dispose();
            return data.ToString();
        }

    }
}
