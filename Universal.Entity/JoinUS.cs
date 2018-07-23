using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 加入我们
    /// </summary>
    public class JoinUS:BaseAdminEntity
    {
        public JoinUS()
        {
            this.Status = true;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 所属职位分类
        /// </summary>
        [Display(Name ="所属分类")]
        public int JoinUSCategoryID { get; set; }

        public virtual JoinUSCategory Category { get; set; }

        [Display(Name = "职位名称"), MaxLength(30, ErrorMessage = "不能超过30个字符"), Required(ErrorMessage = "职位名称不能为空")]
        public string Title { get; set; }

        [Display(Name = "工作地点"), MaxLength(30, ErrorMessage = "不能超过30个字符"), Required(ErrorMessage = "工作地点不能为空")]
        public string Address { get; set; }
        
        [Display(Name = "状态")]
        public bool Status { get; set; }
        
        /// <summary>
        /// 优先级，越大，同级显示的时候越靠前
        /// </summary>
        [Display(Name = "权重")]
        public int Weight { get; set; }

        [Display(Name = "内容")]
        public string Content { get; set; }

        [Display(Name = "过期日期")]
        public DateTime? TimeOut { get; set; }

        public string GetTimeOut
        {
            get
            {
                if (TimeOut == null) return "永久有效";
                return Tools.TypeHelper.ObjectToDateTime(TimeOut).ToShortDateString();
            }
        }

    }
}
