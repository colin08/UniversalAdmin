using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    public class ViewModelDocument : ViewModelCustomFormBase
    {
        public ViewModelDocument()
        {
            this.category_list = new List<ViewModelDocumentCategory>();
        }

        public int id { get; set; }

        [Display(Name = "标题"), StringLength(255), Required(ErrorMessage = "标题不能为空")]
        public string title { get; set; }
        

        [MaxLength(500)]
        public string filepath { get; set; }

        [MaxLength(30)]
        public string filesize { get; set; }

        [Required(ErrorMessage ="请选择所属分类")]
        public int category_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ViewModelDocumentCategory> category_list { get; set; }

        /// <summary>
        /// 秘籍权限
        /// </summary>
        public Entity.DocPostSee post_see { get; set; }
        
    }

    public class ViewModelDocumentCategory
    {
        public int id { get; set; }

        public string title { get; set; }
    }

}