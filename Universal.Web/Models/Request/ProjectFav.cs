using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 获取我的项目收藏所需参数
    /// </summary>
    public class ProjectFav:BasePage
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string search_word { get; set; }


    }
}