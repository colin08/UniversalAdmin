using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 获取公司荣誉页面数据
    /// </summary>
    public class Honour
    {
        public Honour()
        {
            this.banner_list = new List<Banner>();
            this.honor_list = new List<Entity.Honour>();
        }

        /// <summary>
        /// 轮播图
        /// </summary>
        public List<Entity.Banner> banner_list { get; set; }

        /// <summary>
        /// 荣誉列表
        /// </summary>
        public List<Entity.Honour> honor_list { get; set; }

    }
}
