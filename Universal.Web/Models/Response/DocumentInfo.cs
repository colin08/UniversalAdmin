using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 秘籍信息
    /// </summary>
    public class DocumentInfo
    {
        /// <summary>
        /// 秘籍ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string file_path { get; set; }

        /// <summary>
        /// 文件大小,KB或MB显示
        /// </summary>
        public string file_size { get; set; }

        /// <summary>
        /// 该用户是否收藏
        /// </summary>
        public bool is_favorites { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }

    }
}