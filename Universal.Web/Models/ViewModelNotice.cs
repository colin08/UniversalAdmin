using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    public class ViewModelNotice:ViewModelCustomFormBase
    {
        public ViewModelNotice()
        {
            this.user_id_str = "";
            this.user_list = new List<ViewModelNoticeUser>();
        }

        public int id { get; set; }

        [Required(ErrorMessage ="标题不能为空"), MaxLength(100,ErrorMessage ="标题不能超过100个字符")]
        public string title { get; set; }

        [MaxLength(1000,ErrorMessage ="内容不能超过1000个字符")]
        public string content { get; set; }

        /// <summary>
        /// 通知用户id串
        /// </summary>
        public string user_id_str { get; set; }

        public List<ViewModelNoticeUser> user_list { get; set; }

    }

    public class ViewModelNoticeUser
    {
        public int id { get; set; }

        public string telphone { get; set; }

        public string nick_name { get; set; }
    }
}