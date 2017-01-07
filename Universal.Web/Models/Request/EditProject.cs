using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 修改项目基本信息
    /// </summary>
    public class EditProject
    {
        public int project_id { get; set; }

        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        public int user_id { get; set; }

        public string title { get; set; }

        /// <summary>
        /// 流程ID
        /// </summary>
        public int flow_id { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public int approve_user_id { get; set; }
        
        /// <summary>
        /// 权限
        /// </summary>
        public Entity.DocPostSee post_see { get; set; }

        /// <summary>
        /// 可以看的用户或部门id，逗号分割
        /// </summary>
        public string see_ids { get; set; }


        /// <summary>
        /// 项目联系人id，逗号分割
        /// </summary>
        public string user_ids { get; set; }

        /// <summary>
        /// 项目附件
        /// </summary>
        public List<Models.Response.ProjectFile> file_list { get; set; }
    }
}