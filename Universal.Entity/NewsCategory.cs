using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 医学通识 大类
    /// </summary>
    public class NewsCategory
    {
        public NewsCategory()
        {
            this.Status = true;
            this.Weight = 99;
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Display(Name ="分类名称"),MaxLength(10,ErrorMessage ="不能超过10个字符"),Required(ErrorMessage ="不能为空")]
        public string Title { get; set; }

        [Display(Name = "状态")]
        public bool Status { get; set; }

        [Display(Name = "排序数字")]
        public int Weight { get; set; }

        public DateTime AddTime { get; set; }

        [Display(Name = "拥有的标签")]
        public virtual ICollection<NewsTag> NewsTags { get; set; }

    }
}
