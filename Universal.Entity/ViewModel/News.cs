using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 最新资讯
    /// </summary>
    public class News
    {
        public News()
        {
            this.banner_list = new List<Banner>();
            this.new_list_lx = new List<Entity.News>();
            this.new_list_hy = new List<Entity.News>();
        }

        /// <summary>
        /// 轮播图
        /// </summary>
        public List<Entity.Banner> banner_list { get; set; }

        /// <summary>
        /// 朗形动态
        /// </summary>
        public List<Entity.News> new_list_lx { get; set; }

        /// <summary>
        /// 行业新闻
        /// </summary>
        public List<Entity.News> new_list_hy { get; set; }

    }
}
