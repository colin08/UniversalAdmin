using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 添加秘籍所需参数
    /// </summary>
    public class AddDocument
    {
        public AddDocument()
        {
            this.file_list = new List<Response.ProjectFile>();
        }

        public int user_id { get; set; }

        /// <summary>
        /// 可见类别
        /// </summary>
        public Entity.DocPostSee see_type { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public int category_id { get; set; }

        /// <summary>
        /// 部门或用户ID,逗号分割
        /// </summary>
        public string toid { get; set; }

        /// <summary>
        /// 项目附件
        /// </summary>
        public List<Models.Response.ProjectFile> file_list { get; set; }
    }
}