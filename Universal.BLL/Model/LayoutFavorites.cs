using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL.Model
{
    /// <summary>
    /// 用户收藏
    /// </summary>
    public class LayoutFavorites
    {
        public int id { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        public string link_url { get; set; }

        public string title { get; set; }

        /// <summary>
        /// 二级标题
        /// </summary>
        public string er_title { get; set; }
    }
}
