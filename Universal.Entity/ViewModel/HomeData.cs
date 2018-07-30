using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 首页数据
    /// </summary>
    public class HomeData
    {
        /// <summary>
        /// 轮播图列表
        /// </summary>
        public List<Entity.Banner> banner_list { get; set; }

        /// <summary>
        /// 大事记列表
        /// </summary>
        public List<Entity.TimeLine> time_line_list { get; set; }

        /// <summary>
        /// 经典案例列表
        /// </summary>
        public List<CaseShow> case_show_list { get; set; }

        /// <summary>
        /// 合作企业列表
        /// </summary>
        public List<TeamWork> team_work_list { get; set; }

        /// <summary>
        /// 数字展示
        /// </summary>
        public HomeShuZiChuangYiData shuzi { get; set; }

        /// <summary>
        /// 创意视觉
        /// </summary>
        public HomeShuZiChuangYiData chuangyi { get; set; }

        /// <summary>
        /// 最新资讯
        /// </summary>
        public List<News> news_list { get; set; }

    }

    /// <summary>
    /// 首页数字展示和创意视觉
    /// </summary>
    public class HomeShuZiChuangYiData
    {
        public string title { get; set; }

        public string title_er { get; set; }

        public string summary { get; set; }

        public string image_url { get; set; }

        /// <summary>
        /// 下属分类
        /// </summary>
        public List<HomeShuZiChuangYiCategoryData> category_list { get; set; }
    }

    public class HomeShuZiChuangYiCategoryData
    {
        public HomeShuZiChuangYiCategoryData()
        {
            
        }
        public HomeShuZiChuangYiCategoryData(int id, string title,string call_name)
        {
            this.id = id;
            this.title = title;
            this.call_name = call_name;
        }

        public int id { get; set; }
        public string title { get; set; }

        public string call_name { get; set; }

    }


}
