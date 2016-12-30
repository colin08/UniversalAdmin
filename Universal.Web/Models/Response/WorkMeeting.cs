using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 获取我的会议召集返回的字段
    /// </summary>
    public class WorkMeeting
    {
        public WorkMeeting()
        {
            this.file_list = new List<ProjectFile>();
        }

        public int id { get; set; }
        
        /// <summary>
        /// 状态文本
        /// </summary>
        public string status_text { get; set; }

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
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }

        /// <summary>
        /// 参会人员
        /// </summary>
        public List<SelectUser> meeting_users { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<ProjectFile> file_list { get; set; }

    }
}