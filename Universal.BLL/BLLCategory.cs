using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 栏目分类
    /// </summary>
    public class BLLCategory
    {

        /// <summary>
        /// 获取案例展示的分类
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetCaseShowCategory()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            List<string> need_list = new List<string>();
            need_list.Add("Digital-Display");
            need_list.Add("Creative-Vision");
            using (var db = new DataCore.EFDBContext())
            {
                foreach (var ca in need_list)
                {
                    var shuzi = db.Categorys.Where(p => p.PID == null & p.CallName == ca).AsNoTracking().FirstOrDefault();
                    if (shuzi != null)
                    {
                        dic.Add(shuzi.ID, shuzi.Title);
                        //获取子分类
                        var shuzi_list = db.Categorys.Where(p => p.PID == shuzi.ID).AsNoTracking().ToList();
                        foreach (var item in shuzi_list)
                        {
                            dic.Add(item.ID, "|--" + item.Title);
                        }
                    }
                }

            }
            return dic;
        }

        /// <summary>
        /// 添加分类数据
        /// </summary>
        /// <returns></returns>
        public static int Add(Entity.Category entity)
        {
            if (entity == null)
                return -1;

            var db = new DataCore.EFDBContext();
            Entity.Category p_entity = null;
            if (entity.PID != null)
            {
                p_entity = db.Categorys.Find(entity.PID);
                if (p_entity == null)
                {
                    entity.PID = null;
                    entity.Depth = 1;
                }
                else
                {
                    entity.Depth = p_entity.Depth + 1;
                }
            }

            db.Categorys.Add(entity);
            db.SaveChanges();
            db.Dispose();
            return entity.ID;
        }

        /// <summary>
        /// 修改分类数据
        /// </summary>
        /// <returns></returns>
        public static bool Modify(Entity.Category entity)
        {
            if (entity == null)
                return false;

            var db = new DataCore.EFDBContext();
            Entity.Category p_entity = null;
            if (entity.PID != null)
            {
                p_entity = db.Categorys.Find(entity.PID);
                if (p_entity == null)
                {
                    entity.PID = null;
                    entity.Depth = 1;
                }
                else
                {
                    entity.Depth = p_entity.Depth + 1;
                }
            }

            db.Entry<Entity.Category>(entity).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// 根据ID获取子或父数据
        /// </summary>
        /// <param name="up">查找父级，否则为查找子级</param>
        /// <param name="id">当前分类ID</param>
        /// <returns></returns>
        public static List<Entity.Category> GetList(bool up, int id)
        {
            List<Entity.Category> list = new List<Entity.Category>();
            var db = new DataCore.EFDBContext();
            SqlParameter[] param = { new SqlParameter("@Id", id) };
            string proc_name = "dbo.sp_GetParentCategory @Id";
            if (!up)
                proc_name = "dbo.sp_GetChildCategory @Id";
            list = db.Categorys.SqlQuery(proc_name, param).ToList();
            db.Dispose();
            return list;
        }

        /// <summary>
        /// 获取某个分类下所有的子类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetChildIDStr(int id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                string Sql = "select dbo.fn_GetChildCategoryStr(" + id.ToString() + ") as idstr";
                return db.Database.SqlQuery<string>(Sql).ToList()[0];
            }
        }

        /// <summary>
        /// 获取所属某个分类的数据集数量，包含子类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public static int GetChildDataTotal(DataCore.EFDBContext db, int id)
        //{
        //    string Sql = "select count(1) as total from CusUser where charindex(rtrim(CusDepartmentID),(select dbo.fn_GetChildDepartmentStr(" + id.ToString() + ")))>0";
        //    return db.Database.SqlQuery<int>(Sql).ToList()[0];
        //}

    }
}
