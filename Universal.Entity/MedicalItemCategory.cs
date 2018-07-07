using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 套餐项分类
    /// </summary>
    public class MedicalItemCategory
    {

        public MedicalItemCategory()
        {
            this.AddTime = DateTime.Now;
            this.Weight = 99;
            this.Remark = "";
            this.Status = true;
        }

        public int ID { get; set; }

        [Display(Name ="分类名称"),MaxLength(100,ErrorMessage ="不能超过100个字符"),Required(ErrorMessage ="必填")]
        public string Title { get; set; }

        [Display(Name ="状态")]
        public bool Status { get; set; }

        [Display(Name ="排序数字")]
        public int Weight { get; set; }

        [Display(Name ="预留"),MaxLength(255)]
        public string Remark { get; set; }

        public DateTime AddTime { get; set; }

        /// <summary>
        /// 包含的体检项
        /// </summary>
        public virtual ICollection<Entity.MedicalItem> MedicalItems { get; set; }

    }
}
