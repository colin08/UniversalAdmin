using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 合作企业
    /// </summary>
    public class TeamWork :BaseAdminEntity
    {
        public TeamWork()
        {
            this.Status = true;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Display(Name = "企业名称"), MaxLength(30, ErrorMessage = "不能超过30个字符"), Required(ErrorMessage = "企业名称不能为空")]
        public string Title { get; set; }

        [Display(Name = "灰色LOGO")]
        public string ImgUrl { get; set; }

        [Display(Name = "彩色LOGO")]
        public string ImgUrl2 { get; set; }

        [Display(Name = "状态")]
        public bool Status { get; set; }

        /// <summary>
        /// 优先级，越大，同级显示的时候越靠前
        /// </summary>
        [Display(Name = "权重")]
        public int Weight { get; set; }
        
        [Display(Name = "备注"), MaxLength(500, ErrorMessage = "不能超过500个字符")]
        public string Remark { get; set; }

        /// <summary>
        /// 经典案例，多对多关系
        /// </summary>
        public ICollection<CaseShow> CaseShows { get; set; }
    }
}
