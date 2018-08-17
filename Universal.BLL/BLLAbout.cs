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
    /// 关于我们页面所需数据
    /// </summary>
    public class BLLAbout
    {

        /// <summary>
        /// 获取公司简介Banner
        /// </summary>
        /// <returns></returns>
        public static List<Entity.Banner> GetCompanyProfileBannerList()
        {
            //string cache_key = "CACHE_ABOUT_CompanyProfile";
            //var cache_model = CacheHelper.Get<Entity.Banner>(cache_key);
            //if (cache_model != null) return cache_model;

            var result_model = new List<Entity.Banner>();
            using (var db = new DataCore.EFDBContext())
            {
                result_model = db.Banners.SqlQuery("select * from Banner where Status=1 AND CategoryID =(select ID from Category where CallName = 'Company-Profile')  ORDER BY Weight DESC").ToList();
                //if(result_model != null) CacheHelper.Insert(cache_key, result_model, 1200);
            }
            return result_model;
        }


        /// <summary>
        /// 获取荣誉列表
        /// </summary>
        /// <returns></returns>
        public static Entity.ViewModel.Honour GetHonourList()
        {
            //string cache_key = "CACHE_ABOUT_HONOUR";
            //var cache_model = CacheHelper.Get<Entity.ViewModel.Honour>(cache_key);
            //if (cache_model != null) return cache_model;

            var result_model = new Entity.ViewModel.Honour();
            using (var db=new DataCore.EFDBContext())
            {
                result_model.banner_list = db.Banners.SqlQuery("select * from Banner where Status=1 AND CategoryID =(select ID from Category where CallName = 'Company-Honor')  ORDER BY Weight DESC").ToList();
                result_model.honor_list = db.Honours.Where(p => p.Status).OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                //if(result_model != null) CacheHelper.Insert(cache_key, result_model, 1200);
            }
            return result_model;
        }

        /// <summary>
        /// 获取大事记列表
        /// </summary>
        /// <returns></returns>
        public static Entity.ViewModel.TimeLine GetTimeLineList()
        {
            //string cache_key = "CACHE_ABOUT_HONOUR";
            //var cache_model = CacheHelper.Get<Entity.ViewModel.TimeLine>(cache_key);
            //if (cache_model != null) return cache_model;

            var result_model = new Entity.ViewModel.TimeLine();
            using (var db = new DataCore.EFDBContext())
            {
                result_model.banner_list = db.Banners.SqlQuery("select * from Banner where Status=1 AND CategoryID =(select ID from Category where CallName = 'Memorabilia')  ORDER BY Weight DESC").ToList();
                result_model.time_line_list = db.TimeLines.Where(p => p.Status).OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                //if(result_model != null) CacheHelper.Insert(cache_key, result_model, 1200);
            }
            return result_model;
        }

        /// <summary>
        /// 获取未来愿景列表
        /// </summary>
        /// <returns></returns>
        public static Entity.ViewModel.FutureVision GetFutureVisionList()
        {
            //string cache_key = "CACHE_ABOUT_HONOUR";
            //var cache_model = CacheHelper.Get<Entity.ViewModel.FutureVision>(cache_key);
            //if (cache_model != null) return cache_model;

            var result_model = new Entity.ViewModel.FutureVision();
            using (var db = new DataCore.EFDBContext())
            {
                result_model.banner_list = db.Banners.SqlQuery("select * from Banner where Status=1 AND CategoryID =(select ID from Category where CallName = 'Future-Vision')  ORDER BY Weight DESC").ToList();
                result_model.future_vision_list = db.FutureVisions.Where(p => p.Status).OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                //if(result_model != null) CacheHelper.Insert(cache_key, result_model, 1200);
            }
            return result_model;
        }
    }
}
