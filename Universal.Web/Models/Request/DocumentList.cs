using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 请求秘籍列表所需参数
    /// </summary>
    public class DocumentList:BasePage
    {
        /// <summary>
        /// 具体分类
        /// </summary>
        public int category_id { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string search_word { get; set; }

    }
}