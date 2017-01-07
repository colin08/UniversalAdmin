using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 反馈所需参数
    /// </summary>
    public class FeebBack
    {
        public int user_id { get; set; }

        /// <summary>
        /// 内容，500字内
        /// </summary>
        public string content { get; set; }
    }
}