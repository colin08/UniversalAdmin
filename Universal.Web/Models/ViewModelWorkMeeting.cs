using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 工作任务
    /// </summary>
    public class ViewModelWorkMeeting:ViewModelCustomFormBase
    {
        public ViewModelWorkMeeting()
        {
            this.year = DateTime.Now.Year.ToString();
            this.month = DateTime.Now.Month.ToString();
            this.day = DateTime.Now.Day.ToString();
            this.users_entity = new List<ViewModelDocumentCategory>();
        }

        public int id { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [Required(ErrorMessage = "不能为空"), MaxLength(500, ErrorMessage = "不能超过500个字符")]
        public string title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "不能为空"), MaxLength(500, ErrorMessage = "不能超过500个字符")]
        public string content { get; set; }

        /// <summary>
        /// 会议地点
        /// </summary>
        [Required(ErrorMessage = "不能为空"), MaxLength(100, ErrorMessage = "不能超过100个字符")]
        public string location { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Entity.WorkStatus status { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public string year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public string month { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        public string day { get; set; }

        /// <summary>
        /// 参与用户
        /// </summary>
        public string user_ids { get; set; }

        /// <summary>
        /// 参与用户信息
        /// </summary>
        public List<ViewModelDocumentCategory> users_entity { get; set; }

    }
}