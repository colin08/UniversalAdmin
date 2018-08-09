using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 数字展示-创意视觉数据
    /// </summary>
    public class CaseShow
    {
        public CaseShow()
        {
            this.banner_list = new List<Banner>();
            this.case_list_new = new List<Entity.CaseShow>();
            this.case_list_classic = new List<Entity.CaseShow>();
        }

        /// <summary>
        /// 当前分类ID
        /// </summary>
        public int category_id { get; set; }

        /// <summary>
        /// 分类标题
        /// </summary>
        public string category_title { get; set; }

        /// <summary>
        /// 分类标识
        /// </summary>
        public string category_call_name { get; set; }

        /// <summary>
        /// 轮播图
        /// </summary>
        public List<Entity.Banner> banner_list { get; set; }

        /// <summary>
        /// 最新案例
        /// </summary>
        public List<Entity.CaseShow> case_list_new { get; set; }

        /// <summary>
        /// 经典案例
        /// </summary>
        public List<Entity.CaseShow> case_list_classic { get; set; }

    }
}
