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
                result_model.banner_list = db.Banners.SqlQuery("select * from Banner where Status=1 AND CategoryID =(select ID from Category where CallName = 'Company-Honor')  ORDER BY Weight DESC").ToList();
                result_model.job_list = db.JoinUSs.Where(p => p.Status).OrderByDescending(p => p.Weight).Take(top).AsNoTracking().ToList();
                //if(result_model != null) CacheHelper.Insert(cache_key, result_model, 1200);
            }
            return result_model;
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
                result_model.new_list_lx = db.News.Where(p => p.Status && p.Type == Entity.NewsType.LX).Take(new_lx_top).OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                //行业资讯
                result_model.new_list_hy = db.News.Where(p => p.Status && p.Type == Entity.NewsType.HY).Take(new_hy_top).OrderByDescending(p => p.Weight).AsNoTracking().ToList();

                //CacheHelper.Insert(cache_key, result_model, 1200);
            }

            return result_model;

        }

    }
}
