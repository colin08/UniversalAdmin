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
            this.see_entity = new List<ViewModelDocumentCategory>();
        }

        public int id { get; set; }

        [Required(ErrorMessage ="标题不能为空"), MaxLength(100,ErrorMessage ="标题不能超过100个字符")]
        public string title { get; set; }

        [MaxLength(1000,ErrorMessage ="内容不能超过1000个字符")]
        public string content { get; set; }
        
        /// <summary>
        /// 秘籍权限
        /// </summary>
        public Entity.DocPostSee post_see { get; set; }

        /// <summary>
        /// 可以看的用户或部门id，逗号分割
        /// </summary>
        public string see_ids { get; set; }

        /// <summary>
        /// 可以看的用户或部门信息，用于还原数据
        /// </summary>
        public List<ViewModelDocumentCategory> see_entity { get; set; }

    }

    public class ViewModelNoticeUser
    {
        public int id { get; set; }

        public string telphone { get; set; }

        public string nick_name { get; set; }
    }

}