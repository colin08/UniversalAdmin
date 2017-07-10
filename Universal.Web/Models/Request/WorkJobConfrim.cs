using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 任务指派完成所需参数
    /// </summary>
    public class WorkJobConfrim
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public int work_job_id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 确认文本
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 附件，type填写1，不会取这个值
        /// </summary>
        public List<Response.ProjectFile> files { get; set; }
    }
}