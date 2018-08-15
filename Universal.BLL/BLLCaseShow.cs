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
    /// 案例展示
    /// </summary>
    public class BLLCaseShow
    {

        /// <summary>
        /// 获取前端页面展示的数据
        /// </summary>
        /// <param name="category_er_call_name">二级导航的标识</param>
        /// <returns></returns>
        public static Entity.ViewModel.CaseShow GetWebData(string category_er_call_name, int case_new_top = 3, int case_classic_top = 6)
        {
            //string cache_key = "CACHE_CASE_SHOW_WEB";
            //var cache_model = CacheHelper.Get<Entity.ViewModel.CaseShow>(cache_key);
            //if (cache_model != null) return cache_model;

            var result_model = new Entity.ViewModel.CaseShow();
            using (var db = new DataCore.EFDBContext())
            {
                //查找分类是否存在
                var entity_category = db.Categorys.Where(p => p.CallName == category_er_call_name).AsNoTracking().FirstOrDefault();
                if (entity_category == null) return null;

                result_model.category_id = entity_category.ID;
                result_model.category_title = entity_category.Title;
                result_model.category_call_name = category_er_call_name;

                //轮播图
                result_model.banner_list = db.Banners.SqlQuery("select * from Banner where Status=1 AND CategoryID =" + entity_category.ID.ToString() + "  ORDER BY Weight DESC").ToList();
                //最新案例
                result_model.case_list_new = LoadPageList(db, 3, 1, entity_category.ID, Entity.CaseShowType.New);//db.CaseShows.Where(p => p.Status && p.CategoryID == entity_category.ID && p.Type == Entity.CaseShowType.New).OrderByDescending(p => p.Weight).Skip(0).Take(case_new_top).AsNoTracking().ToList();
                //经典案例
                result_model.case_list_classic = LoadPageList(db, 6, 1, entity_category.ID, Entity.CaseShowType.Classic); //db.CaseShows.Where(p => p.Status && p.CategoryID == entity_category.ID && p.Type == Entity.CaseShowType.Classic).OrderByDescending(p => p.Weight).Skip(0).Take(case_classic_top).AsNoTracking().ToList();

                //CacheHelper.Insert(cache_key, result_model, 1200);
            }

            return result_model;

        }
        
        /// <summary>
        /// 分页加载案例
        /// </summary>
        /// <param name="db"></param>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="category_id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static List<Entity.CaseShow> LoadPageList(DataCore.EFDBContext db,int page_size,int page_index,int category_id,Entity.CaseShowType type)
        {
            if (page_size <= 0) page_size = 6;
            if (page_index <= 0) page_index = 1;
            int begin_index = (page_index - 1) * page_size + 1;
            int end_index = page_size * page_index;

            bool clear = false;
            if (db == null)
            {
                db = new DataCore.EFDBContext();
                clear = true;
            }

            var result_list = new List<Entity.CaseShow>();

            string strSql = "select * from (select ROW_NUMBER() OVER(ORDER BY Weight Desc) as row, * from (SELECT* FROM[dbo].[CaseShow] WHERE Status = 1 AND CategoryID = " + category_id + " and Type = " + (int)type + ") as Y) as T where row BETWEEN " + begin_index.ToString() + " and " + end_index.ToString() + "";
            result_list = db.CaseShows.SqlQuery(strSql).ToList();
            if (clear) db.Dispose();
            return result_list;
        }

        /// <summary>
        /// 提供给前端的分页方法
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="category_id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Entity.CaseShow> GetWebPageList(int page_size, int page_index, int category_id, Entity.CaseShowType type)
        {
            return LoadPageList(null, page_size, page_index, category_id, type);
        }

        /// <summary>
        /// 获取合作企业的案例列表
        /// </summary>
        /// <param name="id">企业ID</param>
        /// <returns></returns>
        public static Entity.ViewModel.CaseShowTeamWork GetTeamWorkCaseList(int id)
        {
            //string cache_key = "CACHE_CASE_SHOW_TEAM_WORK";
            //var cache_model = CacheHelper.Get<Entity.ViewModel.CaseShowTeamWork>(cache_key);
            //if (cache_model != null) return cache_model;

            var result_model = new Entity.ViewModel.CaseShowTeamWork();
            using (var db = new DataCore.EFDBContext())
            {
                var team_work_entity = db.TeamWorks.Where(p => p.Status && p.ID == id).AsNoTracking().FirstOrDefault();
                if (team_work_entity == null) return null;
                result_model.team_work_info = team_work_entity;
                string strSql = "select * from CaseShow where Status =1 AND ID in(select CaseShow_ID FROM TeamWorkCaseShow where TeamWork_ID = 1) ORDER BY Weight DESC";
                result_model.case_show_list = db.CaseShows.SqlQuery(strSql).ToList();

                //CacheHelper.Insert(cache_key, result_model, 1200);
            }
            return result_model;
        }

        /// <summary>
        /// 获取一个实体，返回所拥有的企业合作，id逗号分隔
        /// </summary>
        /// <param name="id"></param>
        /// <param name="team_work_ids"></param>
        /// <returns></returns>
        public static Entity.CaseShow GetModel(int id, out string team_work_ids)
        {
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.CaseShows.Where(p => p.ID == id).AsNoTracking().FirstOrDefault();
                string strSql = "select Cast(TeamWork_ID as nvarchar(20))+',' from(SELECT TeamWork_ID FROM[dbo].[TeamWorkCaseShow] where CaseShow_ID = " + id.ToString() + ") as T for xml path('')";
                team_work_ids = db.Database.SqlQuery<string>(strSql).FirstOrDefault();
                return entity;
            }
        }

        /// <summary>
        /// 添加案例
        /// </summary>
        /// <returns></returns>
        public static bool Add(Entity.CaseShow model, string team_work_ids)
        {
            if (model == null) return false;
            using (var db = new DataCore.EFDBContext())
            {
                var entity = new Entity.CaseShow();
                entity.Address = model.Address;
                entity.AddUserID = model.AddUserID;
                entity.CategoryID = model.CategoryID;
                entity.Content = model.Content;
                entity.ImgUrl = model.ImgUrl;
                entity.ImgUrlBig = model.ImgUrlBig;
                entity.LastUpdateUserID = model.LastUpdateUserID;
                entity.Status = model.Status;
                entity.Summary = model.Summary;
                entity.Time = model.Time;
                entity.ErTitle = model.ErTitle;
                entity.FuWu = model.FuWu;
                entity.Title = model.Title;
                entity.Type = model.Type;
                entity.ImgType = model.ImgType;
                entity.Weight = model.Weight;
                entity.IsHome = model.IsHome;
                db.CaseShows.Add(entity);
                db.SaveChanges();
                StringBuilder sb_sql = new StringBuilder();
                foreach (var item in team_work_ids.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                    sb_sql.Append(string.Format("INSERT INTO dbo.TeamWorkCaseShow values({0},{1});", item, entity.ID));
                }
                if (sb_sql.Length > 0)
                {
                    db.Database.ExecuteSqlCommand(sb_sql.ToString());
                }
            }
            return true;

        }

        /// <summary>
        /// 修改案例
        /// </summary>
        /// <param name="model"></param>
        /// <param name="team_work_ids"></param>
        /// <returns></returns>
        public static bool Modify(Entity.CaseShow model, string team_work_ids)
        {
            if (model == null) return false;
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.CaseShows.Find(model.ID);
                if (entity == null) return false;

                entity.Address = model.Address;
                entity.AddUserID = model.AddUserID;
                entity.CategoryID = model.CategoryID;
                entity.Content = model.Content;
                entity.ImgUrl = model.ImgUrl;
                entity.ImgUrlBig = model.ImgUrlBig;
                entity.LastUpdateUserID = model.LastUpdateUserID;
                entity.Status = model.Status;
                entity.Summary = model.Summary;
                entity.Time = model.Time;
                entity.Title = model.Title;
                entity.ErTitle = model.ErTitle;
                entity.FuWu = model.FuWu;
                entity.Type = model.Type;
                entity.ImgType = model.ImgType;
                entity.Weight = model.Weight;
                entity.IsHome = model.IsHome;
                db.SaveChanges();
                StringBuilder sb_sql = new StringBuilder();
                //先清空
                sb_sql.Append(string.Format("DELETE dbo.TeamWorkCaseShow where CaseShow_ID = {0};", entity.ID));
                foreach (var item in team_work_ids.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                    sb_sql.Append(string.Format("INSERT INTO dbo.TeamWorkCaseShow values({0},{1});", item, entity.ID));
                }
                db.Database.ExecuteSqlCommand(sb_sql.ToString());
            }
            return true;
        }

    }
}
