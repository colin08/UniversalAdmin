using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 任务执行人及完成情况
    /// </summary>
    public class WorkJobConfrimUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string telphone { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string nick_name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 短号
        /// </summary>
        public string short_num { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool is_join { get; set; }

        /// <summary>
        /// 完成的文本说明
        /// </summary>
        public string confrim_text { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<ProjectFile> file_list { get; set; }
    }
}