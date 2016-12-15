using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 公告列表信息
    /// </summary>
    public class NoticeBasic
    {
        /// <summary>
        /// 公告ID
        /// </summary>
        public int notice_id { get; set; }

        public string title { get; set; }

        /// <summary>
        /// 概要
        /// </summary>
        public string summary { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime time { get; set; }

    }
}