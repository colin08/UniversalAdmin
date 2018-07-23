using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 公司大事件
    /// </summary>
    public class TimeLine: BaseAdminEntity
    {
        public TimeLine()
        {
            this.Status = true;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Display(Name = "事件名称"), MaxLength(30, ErrorMessage = "不能超过30个字符"), Required(ErrorMessage = "事件名称不能为空")]
        public string Title { get; set; }

        [Display(Name = "图片"), MaxLength(300)]
        public string ImgUrl { get; set; }

        [Display(Name = "描述"), MaxLength(2000, ErrorMessage = "不能超过2000个字符"), Required(ErrorMessage = "描述不能为空")]
        public string Content { get; set; }
        
        [Display(Name = "状态")]
        public bool Status { get; set; }

        /// <summary>
        /// 优先级，越大，同级显示的时候越靠前
        /// </summary>
        [Display(Name = "权重")]
        public int Weight { get; set; }

    }
}
