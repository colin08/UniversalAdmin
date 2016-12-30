using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 添加任务指派所需参数
    /// </summary>
    public class WorkJob
    {
        public WorkJob()
        {
            this.file_list = new List<Response.ProjectFile>();
        }

        public int user_id { get; set; }

        /// <summary>
        /// 状态 0:进行中；1：已完成；2：取消
        /// </summary>
        public Entity.WorkStatus status { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 任务内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime done_time { get; set; }
        

        /// <summary>
        /// 任务执行人，id逗号分割
        /// </summary>
        public string user_ids { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<Models.Response.ProjectFile> file_list { get; set; }
    }
}