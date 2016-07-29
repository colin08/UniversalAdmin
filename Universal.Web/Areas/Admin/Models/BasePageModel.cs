using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    /// <summary>
    /// 分页列表基类
    /// </summary>
    public class BasePageModel
    {
        /// <summary>
        /// 当前第几页
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 每页多少条数据
        /// </summary>
        public int page_size { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int total_page { get; set; }
    }
}