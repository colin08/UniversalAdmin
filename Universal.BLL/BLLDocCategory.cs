using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    public class BLLDocCategory
    {
        /// <summary>
        /// 添加部门数据
        /// </summary>
        /// <returns></returns>
        public static bool Add(int pid, string title)
        {
            if (pid <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(title))
                return false;

            var db = new DataCore.EFDBContext();
            var p_entity = db.DocCategorys.Find(pid);
            if (p_entity == null)
                return false;

            var entity = new Entity.DocCategory();
            entity.PID = pid;
            entity.Title = title;
            entity.Depth = p_entity.PID == null ? 1 : p_entity.Depth + 1;
            
            db.DocCategorys.Add(entity);
            db.SaveChanges();
            db.Dispose();
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
            string DataKey1 = "CreateDocCategoryTreeDataKEY";
            string DataKey2 = "CreateDocCategoryTreeDataDEFAULTID";
            object tree_data = Tools.CacheHelper.Get(DataKey1);
            if (tree_data == null)
            {
                tree_data = DocCategoryTreeData(out default_id);
                if (tree_data != null)
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

        private static string DocCategoryTreeData(out int default_id)
        {
            BLL.BaseBLL<Entity.DocCategory> bll = new BLL.BaseBLL<Entity.DocCategory>();
            List<Entity.DocCategory> list = bll.GetListBy(0, p => p.Status == true, "Priority Desc", false);
            System.Text.StringBuilder data = new System.Text.StringBuilder();
            default_id = 0;
            var db = new DataCore.EFDBContext();
            data.Append("<li>");
            foreach (var one in list.Where(p => p.Depth == 1))
            {
                if (default_id == 0)
                    default_id = one.ID;

                //构建第一层
                data.Append("<p class=\"ls_list_tt\" onclick='pageData(1," + one.ID + ",true)'><i></i>" + one.Title + "</p>");
                data.Append("<ul class=\"ls_menu_ul\">");
                foreach (var two in list.Where(p => p.Depth == 2 && p.PID == one.ID))
                {
                    data.Append("<li>");
                    data.Append("<p class=\"ls_list_tt\"  onclick='pageData(1," + two.ID + ",true)'>><i></i>" + two.Title + "</p>");
                    data.Append("<ul class=\"ls_menu_ul\">");
                    foreach (var three in list.Where(p => p.Depth == 3 && p.PID == two.ID))
                    {
                        data.Append("<li><a href=\"javascript:void(0)\" onclick='pageData(1," + three.ID + ",true)'>" + three.Title + "</a></li>");

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
