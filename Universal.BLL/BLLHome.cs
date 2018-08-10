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
    /// 获取首页数据
    /// </summary>
    public class BLLHome
    {
        /// <summary>
        /// 获取首页展示的数据
        /// </summary>
        /// <returns></returns>
        public static Entity.ViewModel.HomeData GetData()
        {
            //string cache_key = "CACHE_HOME_DATA";
            //var cache_model = CacheHelper.Get<Entity.ViewModel.HomeData>(cache_key);
            //if (cache_model != null) return cache_model;

            var result_model = new Entity.ViewModel.HomeData();
            using (var db = new DataCore.EFDBContext())
            {
                //首页轮播图
                result_model.banner_list = db.Banners.SqlQuery("select * from Banner where Status=1 AND CategoryID =(select ID from Category where CallName = 'Home')  ORDER BY Weight DESC").ToList();
                //大事记
                result_model.time_line_list = db.TimeLines.Where(p => p.Status).OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                //合作企业
                result_model.team_work_list = db.TeamWorks.Where(p => p.Status).OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                //经典案例
                result_model.case_show_list = db.CaseShows.Where(p => p.Status && p.Type == Entity.CaseShowType.Classic && p.IsHome).OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                //数字展示
                var temp_shuzi = new Entity.ViewModel.HomeShuZiChuangYiData();
                var entity_category_shuzi = db.Categorys.Where(p => p.Status && p.CallName == "Digital-Display").AsNoTracking().FirstOrDefault();
                if (entity_category_shuzi != null)
                {
                    temp_shuzi.title = entity_category_shuzi.Title;
                    temp_shuzi.title_er = entity_category_shuzi.TitleEr;
                    temp_shuzi.summary = entity_category_shuzi.Summary;
                    temp_shuzi.image_url = entity_category_shuzi.ImgUrl;
                    //var temp_category_list = new List<Entity.ViewModel.HomeShuZiChuangYiCategoryData>();
                    //var category_shuzi = db.Categorys.Where(p => p.Status && p.PID == entity_category_shuzi.ID).OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                    //foreach (var item in category_shuzi)
                    //{
                    //    temp_category_list.Add(new Entity.ViewModel.HomeShuZiChuangYiCategoryData(item.ID, item.Title, item.CallName));
                    //}
                    //temp_shuzi.category_list = temp_category_list;
                }
                result_model.shuzi = temp_shuzi;
                //创意视觉
                var temp_chuangyi = new Entity.ViewModel.HomeShuZiChuangYiData();
                var entity_category_chuangyi = db.Categorys.Where(p => p.Status && p.CallName == "Creative-Vision").AsNoTracking().FirstOrDefault();
                if (entity_category_chuangyi != null)
                {
                    temp_chuangyi.title = entity_category_chuangyi.Title;
                    temp_chuangyi.title_er = entity_category_chuangyi.TitleEr;
                    temp_chuangyi.summary = entity_category_chuangyi.Summary;
                    temp_chuangyi.image_url = entity_category_chuangyi.ImgUrl;
                    //var temp_category_chuangyi_list = new List<Entity.ViewModel.HomeShuZiChuangYiCategoryData>();
                    //var category_chuangyi = db.Categorys.Where(p => p.Status && p.PID == entity_category_chuangyi.ID).OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                    //foreach (var item in category_chuangyi)
                    //{
                    //    temp_category_chuangyi_list.Add(new Entity.ViewModel.HomeShuZiChuangYiCategoryData(item.ID, item.Title, item.CallName));
                    //}
                    //temp_chuangyi.category_list = temp_category_chuangyi_list;
                }
                result_model.chuangyi = temp_chuangyi;
                //最新资讯
                result_model.news_list = db.News.Where(p => p.Status).OrderByDescending(p => p.Weight).Take(3).AsNoTracking().ToList();

                //CacheHelper.Insert(cache_key, result_model, 1200);
            }

            return result_model;

        }

        /// <summary>
        /// 根据顶级分类获取子分类
        /// </summary>
        /// <param name="call_name"></param>
        /// <returns></returns>
        public static List<Entity.Category> GetChildeCategory(string call_name)
        {
            using (var db=new DataCore.EFDBContext())
            {
                string strSql = "select * from Category where Status =1  AND PID =(select ID from Category where CallName = '" + call_name + "') ORDER BY Weight DESC";
                return db.Categorys.SqlQuery(strSql).AsNoTracking().ToList();
            }
        }

    }
}
