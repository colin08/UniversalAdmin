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
        /// <summary>
        /// 添加部门数据
        /// </summary>
        /// <returns></returns>
        public static bool Add(int pid, string title, string user_ids)
        {
            if (pid <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(title))
                return false;

            var db = new DataCore.EFDBContext();
            var p_department = db.CusDepartments.Find(pid);
            if (p_department == null)
                return false;

            var department = new Entity.CusDepartment();
            department.PID = pid;
            department.Title = title;
            department.Depth = p_department.PID == null ? 1 : p_department.Depth + 1;
            List<Entity.CusUser> admin_user = new List<Entity.CusUser>();
            foreach (var sid in user_ids.Split(','))
            {
                int id = Tools.TypeHelper.ObjectToInt(sid);
                var user = db.CusUsers.Find(id);
                if (user == null)
                    break;
                if (!user.IsAdmin)
                    break;

                admin_user.Add(user);
            }
            department.DepartmentAdminUsers = admin_user;
            db.CusDepartments.Add(department);
            db.SaveChanges();
            db.Dispose();
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
            List<Entity.CusUser> admin_user = new List<Entity.CusUser>();
            foreach (var sid in user_ids.Split(','))
            {
                int uid = Tools.TypeHelper.ObjectToInt(sid);
                var user = db.CusUsers.Find(uid);
                if (user == null)
                    break;
                if (!user.IsAdmin)
                    break;

                admin_user.Add(user);
            }
            department.DepartmentAdminUsers = admin_user;

            db.SaveChanges();
            db.Dispose();
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
            string proc_name = "dbo.sp_GetParentCategories @Id";
            if (!up)
                proc_name = "dbo.sp_GetChildCategories @Id";
            list = db.CusDepartments.SqlQuery(proc_name, param).ToList();
            db.Dispose();
            return list;
        }

        /// <summary>
        /// 获取所属某个分类的用户数量，包含子类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetDepartChildDataTotal(DataCore.EFDBContext db,int id)
        {
            string Sql = "select count(1) as total from CusUser where charindex(rtrim(CusDepartmentID),(select dbo.fn_GetChildDepartmentStr(" + id.ToString() + ")))>0";
            return db.Database.SqlQuery<int>(Sql).ToList()[0];
        }

        /// <summary>
        /// 构造部门树数据
        /// </summary>
        /// <param name="default_id">返回默认第一个供加载数据的分类ID</param>
        /// <returns></returns>
        public static string CreateDepartmentTreeData(out int default_id)
        {
            default_id = 0;
            string DataKey1 = "CreateDepartmentTreeDataKEY";
            string DataKey2 = "CreateDepartmentTreeDataDEFAULTID";
            object tree_data = Tools.CacheHelper.Get(DataKey1);
            if (tree_data == null)
            {
                tree_data = DepartmentTreeData(out default_id);
                if(tree_data!= null)
                {
                    Tools.CacheHelper.Insert(DataKey1, tree_data, Tools.SiteKey.CACHE_TIME);
                    Tools.CacheHelper.Insert(DataKey2, default_id, Tools.SiteKey.CACHE_TIME);
                }
            }
            else
            {
                default_id = Tools.TypeHelper.ObjectToInt(Tools.CacheHelper.Get(DataKey2));
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
            data.Append("<li>");
            foreach (var one in list.Where(p => p.Depth == 1))
            {
                //构建第一层
                data.Append("<p class=\"ls_list_tt\"  onclick='pageData(1," + one.ID + ",true)'>><i></i>" + one.Title + " ("+ GetDepartChildDataTotal(db,one.ID).ToString() + "人)</p>");
                data.Append("<ul class=\"ls_menu_ul\">");
                foreach (var two in list.Where(p => p.Depth == 2 && p.PID == one.ID))
                {
                    data.Append("<li>");
                    data.Append("<p class=\"ls_list_tt\"  onclick='pageData(1," + two.ID + ",true)'>><i></i>" + two.Title + " ("+ GetDepartChildDataTotal(db, two.ID).ToString() + "人)</p>");
                    data.Append("<ul class=\"ls_menu_ul\">");
                    foreach (var three in list.Where(p => p.Depth == 3 && p.PID == two.ID))
                    {
                        if (default_id == 0)
                            default_id = three.ID;

                        data.Append("<li><a href=\"javascript:void(0)\" onclick='pageData(1,"+three.ID+",true)'>" + three.Title + "("+ GetDepartChildDataTotal(db, three.ID).ToString() + "人)</a></li>");

                    }
                    data.Append("</ul>");
                    data.Append("</li>");

                }
                data.Append("</ul>");

            }
            data.Append("</li>");
            db.Dispose();
            return data.ToString();
        }

    }
}
