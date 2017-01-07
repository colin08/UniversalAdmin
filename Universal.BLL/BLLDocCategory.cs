using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    public class BLLDocCategory
    {
        static string CacheDataKey1 = "CreateDocCategoryTreeDataKEY";
        static string CacheDataKey2 = "CreateDocCategoryTreeDataDEFAULTID";
        static string CacheDataKeyTreeCategory = "CreateDocCategoryTreeDataOrderKEY";

        /// <summary>
        /// 添加分类数据
        /// </summary>
        /// <returns></returns>
        public static int Add(int pid, string title)
        {
            if (pid < 0)
                return 0;

            if (string.IsNullOrWhiteSpace(title))
                return 0;

            var db = new DataCore.EFDBContext();
            var entity = new Entity.DocCategory();
            if (pid == 0)
            {
                //顶级
                entity.PID = null;
                entity.Depth = 1;
            }
            else
            {
                var p_entity = db.DocCategorys.Find(pid);
                if (p_entity == null)
                    return 0;
                entity.PID = pid;
                entity.Depth = p_entity.Depth + 1;

                if (entity.Depth > 3)
                    return 0;
            }
            entity.Title = title;

            db.DocCategorys.Add(entity);
            db.SaveChanges();
            db.Dispose();
            Tools.CacheHelper.Remove(CacheDataKey1);
            Tools.CacheHelper.Remove(CacheDataKey2);
            return entity.ID;
        }



        /// <summary>
        /// 修改分类数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool Modify(int id, string title)
        {
            if (id <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(title))
                return false;

            var db = new DataCore.EFDBContext();
            var p_entity = db.DocCategorys.Find(id);
            if (p_entity == null)
                return false;
            p_entity.Title = title;
            db.SaveChanges();
            Tools.CacheHelper.Remove(CacheDataKey1);
            Tools.CacheHelper.Remove(CacheDataKey2);
            return true;
        }

        /// <summary>
        /// 根据分类ID获取分类子或父数据
        /// </summary>
        /// <param name="up">查找父级，否则为查找子级</param>
        /// <param name="id">当前分类ID</param>
        /// <returns></returns>
        public static List<Entity.DocCategory> GetList(bool up, int id)
        {
            List<Entity.DocCategory> list = new List<Entity.DocCategory>();
            var db = new DataCore.EFDBContext();
            SqlParameter[] param = { new SqlParameter("@Id", id) };
            string proc_name = "dbo.sp_GetParentCategorys @Id";
            if (!up)
                proc_name = "dbo.sp_GetChildCategorys @Id";
            list = db.DocCategorys.SqlQuery(proc_name, param).ToList();
            db.Dispose();
            return list;
        }

        /// <summary>
        /// 获取一、二级分类名称
        /// </summary>
        /// <param name="doc_id"></param>
        /// <param name="er_txt"></param>
        /// <returns></returns>
        public static string GetYiErTxt(int doc_id,out string er_txt)
        {
            er_txt = "";
            string yi_txt = "";
            var db = new DataCore.EFDBContext();
            var doc_entity = db.DocPosts.AsNoTracking().Include(p => p.DocCategory).Where(p => p.ID == doc_id).FirstOrDefault();
            if (doc_entity == null)
                return yi_txt;
            if(doc_entity.DocCategory.PID != null)
            {
                var yi_cate = db.DocCategorys.Find(doc_entity.DocCategory.PID);
                if(yi_cate != null)
                {
                    yi_txt = yi_cate.Title;
                }
            }
            er_txt = doc_entity.DocCategory.Title;
            db.Dispose();
            return yi_txt;
        }

        /// <summary>
        /// 获取顶级父类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetTopParent(int id)
        {
            string sql = "select dbo.fn_DocCateTopPTitle(" + id + ")";
            using (var db =new DataCore.EFDBContext())
            {
                return db.Database.SqlQuery<string>(sql).ToList()[0];
            }
        }

        /// <summary>
        /// 删除分类，同时删除其子数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Del(int id)
        {
            if (id <= 0)
                return false;
            var db = new DataCore.EFDBContext();
            var entity = db.DocCategorys.Find(id);
            List<Entity.DocCategory> child_list = GetList(false, id);
            foreach (var item in child_list)
                db.DocCategorys.Remove(db.DocCategorys.Find(item.ID));
            db.SaveChanges();
            db.Dispose();
            Tools.CacheHelper.Remove(CacheDataKey1);
            Tools.CacheHelper.Remove(CacheDataKey2);
            return true;
        }

        /// <summary>
        /// 同级排序
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool Sort(string ids,out string msg)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                msg = "非法参数";
                return false;
            }
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse).ToList();
            var temp_id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse).OrderByDescending(p=>p).ToList();
            if (temp_id_list.Count == 0)
            {
                msg = "非法参数2";
                return false;
            }
            var db = new DataCore.EFDBContext();
            var entity_frist = db.DocCategorys.Find(id_list[0]);
            if (entity_frist == null)
            {
                msg = "第一个数据不存在";
                return false;
            }

            //查询当前同级所有的数据
            string sql = "";
            if (entity_frist.PID == null)
                sql = "select ID from DocCategory where PID is null and Status = 1";
            else
                sql = "select ID from DocCategory where PID = " + entity_frist.PID.ToString() + " and Status = 1";
            var db_id_list = db.Database.SqlQuery<int>(sql).OrderByDescending(p => p).ToList();
            if(!(temp_id_list.All(db_id_list.Contains) && temp_id_list.Count == db_id_list.Count))
            {
                msg = "数据与db里不一致";
                return false;
            }
            //排序
            StringBuilder str_sql = new StringBuilder();
            int total = id_list.Count;
            for (int i = 0; i < total; i++)
            {
                str_sql.Append("update DocCategory set Priority = " + (total - i).ToString() + " where ID = " + id_list[i].ToString() + ";") ;
            }
            if (str_sql.Length > 0)
            {
                int ssss = db.Database.ExecuteSqlCommand(str_sql.ToString());
            }
            db.Dispose();
            Tools.CacheHelper.Remove(CacheDataKey1);
            Tools.CacheHelper.Remove(CacheDataKey2);
            msg = "成功";
            return true;
        }

        /// <summary>
        /// 构造秘籍树数据
        /// </summary>
        /// <param name="default_id">返回默认第一个供加载数据的分类ID</param>
        /// <returns></returns>
        public static string CreateDocCategoryTreeData(out int default_id)
        {
            default_id = 0;

            object tree_data = Tools.CacheHelper.Get(CacheDataKey1);
            if (tree_data == null)
            {
                tree_data = DocCategoryTreeData(out default_id);
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

        private static string DocCategoryTreeData(out int default_id)
        {
            BLL.BaseBLL<Entity.DocCategory> bll = new BLL.BaseBLL<Entity.DocCategory>();
            List<Entity.DocCategory> list = bll.GetListBy(0, p => p.Status == true, "Priority Desc", false);
            System.Text.StringBuilder data = new System.Text.StringBuilder();
            default_id = 0;
            var db = new DataCore.EFDBContext();
            foreach (var one in list.Where(p => p.Depth == 1).OrderByDescending(p=>p.Priority))
            {
                if (default_id == 0)
                    default_id = one.ID;

                //构建第一层
                data.Append("<li><span class=\"tree2\" onclick='pageData(1," + one.ID + ",true)'><b class=\"Off\">" + one.Title + "</b></span>");
                data.Append("<ul style=\"display: none; \">");
                foreach (var two in list.Where(p => p.Depth == 2 && p.PID == one.ID).OrderByDescending(p => p.Priority))
                {
                    data.Append("<li><span class=\"tree3\"  onclick='pageData(1," + two.ID + ",true)'><b class=\"Off\">" + two.Title + "</b></span>");
                    data.Append("<ul style=\"display: none;\">");
                    foreach (var three in list.Where(p => p.Depth == 3 && p.PID == two.ID).OrderByDescending(p => p.Priority))
                    {
                        data.Append("<li><span class=\"tree4\" onclick='pageData(1," + three.ID + ",true)'><b>" + three.Title + "</b></span></li>");
                    }
                    data.Append("</ul></li>");
                }
                data.Append("</ul></li>");

            }
            db.Dispose();
            return data.ToString();
        }

        /// <summary>
        /// 获取所有分类，经过父子排序
        /// </summary>
        /// <returns></returns>
        public static List<Entity.DocCategory> GetTreeCategory()
        {
            List<Entity.DocCategory> new_list = new List<Entity.DocCategory>();
            object tree_data = Tools.CacheHelper.Get(CacheDataKeyTreeCategory);
            if (tree_data == null)
            {
                BLL.BaseBLL<Entity.DocCategory> bll = new BLL.BaseBLL<Entity.DocCategory>();
                List<Entity.DocCategory> list = bll.GetListBy(0, p => p.Status == true, "Priority Desc");
                GetCategoryChilds(list, new_list, null);
                if (new_list.Count > 0)
                {
                    Tools.CacheHelper.Insert(CacheDataKeyTreeCategory, new_list, Tools.SiteKey.CACHE_TIME);
                }
            }
            else
                new_list = (List<Entity.DocCategory>)tree_data;
            return new_list;
        }

        /// <summary>
        /// 迭代处理父子数据
        /// </summary>
        /// <param name="oldData"></param>
        /// <param name="newData"></param>
        /// <param name="pid"></param>
        private static void GetCategoryChilds(List<Entity.DocCategory> oldData, List<Entity.DocCategory> newData, int? pid)
        {

            List<Entity.DocCategory> list = new List<Entity.DocCategory>();
            if (pid == null)
                list = oldData.Where(p => p.PID == null).ToList();
            else
                list = oldData.Where(p => p.PID == pid).ToList();
            foreach (var item in list)
            {
                Entity.DocCategory entity = new Entity.DocCategory();
                entity.AddTime = item.AddTime;
                entity.Depth = item.Depth;
                entity.ID = item.ID;
                entity.PID = item.PID;
                entity.Priority = item.Priority;
                entity.Status = item.Status;
                entity.Title = item.Title;
                newData.Add(entity);
                GetCategoryChilds(oldData, newData, item.ID);
            }
        }

    }
}
