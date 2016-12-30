using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 添加下载记录所需参数
    /// </summary>
    public class AddDownLog
    {
        public int user_id { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string file_name { get; set; }

    }
}