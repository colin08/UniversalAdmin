using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 医学通识
    /// </summary>
    public class News
    {
        public News()
        {
            this.Status = true;
            this.AddTime = DateTime.Now;
            this.Weight = 99;
            this.TResource = "厚德医疗";
        }

        public int ID { get; set; }

        [Display(Name = "所属分类"), Required(ErrorMessage = "必选")]
        public int NewsCategoryID { get; set; }

        public virtual NewsCategory NewsCategory { get; set; }


        [Display(Name ="标题"),MaxLength(255,ErrorMessage ="不能超过255个字"),Required(ErrorMessage ="不能为空")]
        public string Title { get; set; }

        [Display(Name ="出处"),MaxLength(30,ErrorMessage ="不能超过30个字符"),Required(ErrorMessage ="不能为空")]
        public string TResource { get; set; }

        [Display(Name ="封面图"),MaxLength(255)]
        public string ImgUrl { get; set; }

        [Display(Name = "状态")]
        public bool Status { get; set; }

        [Display(Name ="排序数字")]
        public int Weight { get; set; }

        [Display(Name ="摘要"),MaxLength(500,ErrorMessage ="不能超过500个字")]
        public string Summary { get; set; }

        [Display(Name ="文章内容")]
        public string Content { get; set; }

        [Display(Name ="跳转地址"),MaxLength(500,ErrorMessage ="不能超过500个字")]
        public string LinkUrl { get; set; }
        
        public DateTime AddTime { get; set; }

        [Display(Name ="拥有的标签")]
        public virtual ICollection<NewsTag> NewsTags { get; set; }

    }
}
