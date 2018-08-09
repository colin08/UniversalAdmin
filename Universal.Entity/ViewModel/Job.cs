using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 职位页面数据
    /// </summary>
    public class Job
    {
        public Job()
        {
            this.banner_list = new List<Banner>();
            this.job_list = new List<JoinUS>();
        }

        /// <summary>
        /// 轮播图
        /// </summary>
        public List<Entity.Banner> banner_list { get; set; }

        /// <summary>
        /// 职位列表
        /// </summary>
        public List<Entity.JoinUS> job_list { get; set; }

    }
}
