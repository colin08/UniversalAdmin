using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 获取未来愿景页面数据
    /// </summary>
    public class FutureVision
    {
        public FutureVision()
        {
            this.banner_list = new List<Banner>();
            this.future_vision_list = new List<Entity.FutureVision>();
        }

        /// <summary>
        /// 轮播图
        /// </summary>
        public List<Entity.Banner> banner_list { get; set; }

        /// <summary>
        /// 愿景列表
        /// </summary>
        public List<Entity.FutureVision> future_vision_list { get; set; }

    }
}
