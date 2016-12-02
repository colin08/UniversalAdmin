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
            this.see_entity = new List<ViewModelDocumentCategory>();
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

    public class ViewModelDocumentCategory
    {
        public ViewModelDocumentCategory()
        {

        }

        public ViewModelDocumentCategory(int id,string title)
        {
            this.id = id;
            this.title = title;
        }

        public int id { get; set; }

        public string title { get; set; }
    }

}