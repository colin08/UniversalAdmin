using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 添加我的会议需要的参数
    /// </summary>
    public class WorkMeetingR
    {
        public WorkMeetingR()
        {
            this.file_list = new List<Response.ProjectFile>();
        }

        public int id { get; set; }

        public int user_id { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 议程
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 开会时间
        /// </summary>
        public DateTime begin_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }

        /// <summary>
        /// 会议地点
        /// </summary>
        public string location { get; set; }

        /// <summary>
        /// 与会人员，英文逗号分割
        /// </summary>
        public string user_ids { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<Models.Response.ProjectFile> file_list { get; set; }

    }
}