using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Universal.Tools;

namespace Universal.BLL
{
    /// <summary>
    /// 联系我们栏目
    /// </summary>
    public class BLLContact
    {
        /// <summary>
        /// 获取职位分类列表数据
        /// </summary>
        /// <returns></returns>
        public static Entity.ViewModel.Job GetJobList(int top = 6)
        {
            //string cache_key = "CACHE_ABOUT_JobLIST";
            //var cache_model = CacheHelper.Get<Entity.ViewModel.Job>(cache_key);
            //if (cache_model != null) return cache_model;

            var result_model = new Entity.ViewModel.Job();
            using (var db = new DataCore.EFDBContext())
            {
                result_model.banner_list = db.Banners.SqlQuery("select * from Banner where Status=1 AND CategoryID =(select ID from Category where CallName = 'Join-US')  ORDER BY Weight DESC").ToList();
                result_model.job_list = LoadJobPageList(db, top, 1);//db.JoinUSs.Where(p => p.Status).OrderByDescending(p => p.Weight).Take(top).AsNoTracking().ToList();
                //if(result_model != null) CacheHelper.Insert(cache_key, result_model, 1200);
            }
            return result_model;
        }

        /// <summary>
        /// 分页加载职位
        /// </summary>
        /// <param name="db"></param>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="category_id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static List<Entity.JoinUS> LoadJobPageList(DataCore.EFDBContext db, int page_size, int page_index)
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

            var result_list = new List<Entity.JoinUS>();

            string strSql = "select * from (select ROW_NUMBER() OVER(ORDER BY Weight Desc) as row, * from (SELECT* FROM[dbo].[JoinUS] WHERE Status = 1) as Y) as T where row BETWEEN " + begin_index.ToString() + " and " + end_index.ToString() + "";
            result_list = db.JoinUSs.SqlQuery(strSql).ToList();
            if (clear) db.Dispose();
            return result_list;
        }

        /// <summary>
        /// 提供给前端的分页方法
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Entity.JoinUS> GetWebJobPageList(int page_size, int page_index)
        {
            return LoadJobPageList(null, page_size, page_index);
        }

        /// <summary>
        /// 获取前端最新资讯展示的数据
        /// </summary>
        /// <returns></returns>
        public static Entity.ViewModel.News GetNewsListData(int new_lx_top = 6, int new_hy_top = 6)
        {
            //string cache_key = "CACHE_JOIN_NEWQS";
            //var cache_model = CacheHelper.Get<Entity.ViewModel.News>(cache_key);
            //if (cache_model != null) return cache_model;

            var result_model = new Entity.ViewModel.News();
            using (var db = new DataCore.EFDBContext())
            {
                //轮播图
                result_model.banner_list = db.Banners.SqlQuery("select * from Banner where Status=1 AND CategoryID =(select ID from Category where CallName = 'News')  ORDER BY Weight DESC").ToList();
                //朗形动态
                result_model.new_list_lx = LoadNewsPageList(db, new_lx_top, 1, Entity.NewsType.LX);// db.News.Where(p => p.Status && p.Type == Entity.NewsType.LX).Take(new_lx_top).OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                //行业资讯
                result_model.new_list_hy = LoadNewsPageList(db, new_hy_top,1, Entity.NewsType.HY);//db.News.Where(p => p.Status && p.Type == Entity.NewsType.HY).Take(new_hy_top).OrderByDescending(p => p.Weight).AsNoTracking().ToList();

                //CacheHelper.Insert(cache_key, result_model, 1200);
            }

            return result_model;
        }

        /// <summary>
        /// 分页加载新闻
        /// </summary>
        /// <param name="db"></param>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="category_id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static List<Entity.News> LoadNewsPageList(DataCore.EFDBContext db, int page_size, int page_index, Entity.NewsType type)
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

            var result_list = new List<Entity.News>();

            string strSql = "select * from (select ROW_NUMBER() OVER(ORDER BY Weight Desc) as row, * from (SELECT* FROM[dbo].[News] WHERE Status = 1 and Type = " + (int)type + ") as Y) as T where row BETWEEN " + begin_index.ToString() + " and " + end_index.ToString() + "";
            result_list = db.News.SqlQuery(strSql).ToList();
            if (clear) db.Dispose();
            return result_list;
        }

        /// <summary>
        /// 提供给前端的分页方法
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Entity.News> GetWebNewsPageList(int page_size, int page_index, Entity.NewsType type)
        {
            return LoadNewsPageList(null, page_size, page_index, type);
        }

    }
}
