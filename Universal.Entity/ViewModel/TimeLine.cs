using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 获取大事记页面数据
    /// </summary>
    public class TimeLine
    {
        public TimeLine()
        {
            this.banner_list = new List<Banner>();
            this.time_line_list = new List<Entity.TimeLine>();
        }

        /// <summary>
        /// 轮播图
        /// </summary>
        public List<Entity.Banner> banner_list { get; set; }

        /// <summary>
        /// 大事记列表
        /// </summary>
        public List<Entity.TimeLine> time_line_list { get; set; }

    }
}
