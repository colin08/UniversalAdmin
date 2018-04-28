using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;


namespace Universal.BLL
{
    public class BLLNews
    {
        /// <summary>
        /// 获取新闻分页列表
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="cid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static List<Entity.ViewModel.News> GetPageList(int page_size, int page_index, int cid, int tid, out int total)
        {
            List<Entity.ViewModel.News> result_list = new List<Entity.ViewModel.News>();
            total = 0;
            if (cid <= 0) return result_list;
            if (page_size <= 0) page_size = 5;
            if (page_index <= 0) page_index = 1;
            int begin_index = (page_index - 1) * page_size + 1;
            int end_index = page_size * page_index;
            string select_cloumn = "";
            string select_where = "";
            if (tid > 0)
            {
                select_cloumn = ",dbo.fn_GetNewsTagIds(ID) as tag_ids";
                select_where = " where CHARINDEX('," + tid.ToString() + ",',tag_ids) > 0";
            }
            string sql_total = "select count(1) from(select ID, NewsCategoryID, ImgUrl, Summary, Weight, AddTime, LinkUrl" + select_cloumn + " from News where NewsCategoryID = " + cid.ToString() + ") as S" + select_where;
            string sql = "select row,id,title,img_url,summer from(select ROW_NUMBER() OVER(ORDER BY Weight Desc, AddTime DESC) as row,*from(select * from(select ID as id, Title as title, NewsCategoryID, ImgUrl as img_url, Summary as summer, Weight, AddTime, LinkUrl" + select_cloumn + " from News where NewsCategoryID = " + cid.ToString() + ") as S " + select_where + ") as SS) as T where row BETWEEN " + begin_index.ToString() + " and " + end_index.ToString() + "";
            using (var db = new DataCore.EFDBContext())
            {
                total = db.Database.SqlQuery<int>(sql_total).ToList()[0];
                result_list = db.Database.SqlQuery<Entity.ViewModel.News>(sql).ToList();
            }
            return result_list;
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public static int Add(Entity.News model, string tag_ids)
        {
            if (model == null) return 0;
            using (var db = new DataCore.EFDBContext())
            {
                var entity = new Entity.News();
                entity.Content = model.Content;
                entity.ImgUrl = model.ImgUrl;
                entity.LinkUrl = model.LinkUrl;
                entity.NewsCategoryID = model.NewsCategoryID;
                entity.Status = model.Status;
                entity.Summary = model.Summary;
                entity.Title = model.Title;
                entity.Weight = model.Weight;
                entity.TResource = model.TResource;
                if (!string.IsNullOrWhiteSpace(tag_ids))
                {
                    var tag_id_list = Array.ConvertAll<string, int>(tag_ids.Split(','), int.Parse);
                    entity.NewsTags = db.NewsTags.Where(p => tag_id_list.Contains(p.ID)).ToList();
                }
                db.News.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public static bool Edit(Entity.News model, string tag_ids)
        {
            if (model == null) return false;
            if (model.ID <= 0) return false;
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.News.Where(p => p.ID == model.ID).Include(p => p.NewsTags).FirstOrDefault();
                if (entity == null) return false;
                entity.Content = model.Content;
                entity.ImgUrl = model.ImgUrl;
                entity.LinkUrl = model.LinkUrl;
                entity.NewsCategoryID = model.NewsCategoryID;
                entity.Status = model.Status;
                entity.Summary = model.Summary;
                entity.Title = model.Title;
                entity.TResource = model.TResource;
                entity.Weight = model.Weight;
                entity.NewsTags = null;
                if (!string.IsNullOrWhiteSpace(tag_ids))
                {
                    var tag_id_list = Array.ConvertAll<string, int>(tag_ids.Split(','), int.Parse);
                    entity.NewsTags = db.NewsTags.Where(p => tag_id_list.Contains(p.ID)).ToList();
                }
                db.SaveChanges();
                return true;
            }
        }


        /// <summary>
        /// 批量禁用
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool DisEnble(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids)) return false;
            using (var db = new DataCore.EFDBContext())
            {
                string strSql = "update News set Status=0 where id in(" + ids + ")";
                db.Database.ExecuteSqlCommand(strSql);
                return true;
            }
        }
    }
}
