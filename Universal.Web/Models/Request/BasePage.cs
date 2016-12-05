using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    public class BasePage
    {
        /// <summary>
        /// 当前登录的用户ID
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int page_size { get; set; }

        /// <summary>
        /// 当前第几页
        /// </summary>
        public int page_index { get; set; }

    }
}