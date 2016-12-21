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
        /// 内容，html格式
        /// </summary>
        public string content { get; set; }


        /// <summary>
        /// 文件路径
        /// </summary>
        public string file_path { get; set; }

        /// <summary>
        /// 文件大小,KB或MB显示
        /// </summary>
        public string file_size { get; set; }

        /// <summary>
        /// 收藏的ID，如果没有收藏，则为0
        /// </summary>
        public int favorites_id { get; set; }

        /// <summary>
        /// 上传者
        /// </summary>
        public string add_user { get; set; }

        /// <summary>
        /// 所属分类ID
        /// </summary>
        public int category_id { get; set; }

        /// <summary>
        /// 所属分类名称
        /// </summary>
        public string category_name { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }

    }
}