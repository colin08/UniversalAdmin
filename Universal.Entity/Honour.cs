using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 荣誉证书
    /// </summary>
    public class Honour:BaseAdminEntity
    {
        public Honour()
        {
            this.Status = true;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Display(Name = "荣誉名称"), MaxLength(50, ErrorMessage = "不能超过50个字符"), Required(ErrorMessage = "荣誉名称不能为空")]
        public string Title { get; set; }

        [Display(Name = "图片"), MaxLength(300)]
        public string ImgUrl { get; set; }

        [Display(Name = "状态")]
        public bool Status { get; set; }

        /// <summary>
        /// 优先级，越大，同级显示的时候越靠前
        /// </summary>
        [Display(Name = "权重")]
        public int Weight { get; set; }

    }
}
