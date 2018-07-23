using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 加入我们分类
    /// </summary>
    public class JoinUSCategory: BaseAdminEntity
    {

        public JoinUSCategory()
        {
            this.Status = true;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Display(Name = "职位类别"), MaxLength(30, ErrorMessage = "不能超过30个字符"), Required(ErrorMessage = "职位类别不能为空")]
        public string Title { get; set; }

        [Display(Name = "背景图"), MaxLength(300), Required(ErrorMessage = "背景图不能为空")]
        public string ImgUrl { get; set; }

        [Display(Name = "状态")]
        public bool Status { get; set; }

        /// <summary>
        /// 优先级，越大，同级显示的时候越靠前
        /// </summary>
        [Display(Name = "权重")]
        public int Weight { get; set; }

        [Display(Name = "备注"), MaxLength(500, ErrorMessage = "不能超过500个字符")]
        public string Remark { get; set; }
    }
}
