using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 任务指派信息
    /// </summary>
    public class WorkJob
    {
        public WorkJob()
        {
            this.users_list = new List<SelectUser>();
            this.file_list = new List<ProjectFile>();
        }

        public int id { get; set; }

        /// <summary>
        /// 状态 0:进行中；1：已完成；2：取消
        /// </summary>
        public Entity.WorkStatus status { get; set; }

        /// <summary>
        /// 状态文本
        /// </summary>
        public string status_text
        {
            get; set;
        }

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
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }

        /// <summary>
        /// 执行人
        /// </summary>
        public List<SelectUser> users_list { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<ProjectFile> file_list { get; set; }

    }
}