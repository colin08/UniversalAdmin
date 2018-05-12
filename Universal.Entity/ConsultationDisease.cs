using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Entity
{
    /// <summary>
    /// 咨询疾病类型
    /// </summary>
    public class ConsultationDisease
    {
        public ConsultationDisease()
        {
            this.AddTime = DateTime.Now;
            this.Weight = 99;
            this.Status = true;
        }

        public int ID { get; set; }

        [Display(Name ="类别名称"),Required(ErrorMessage ="必填"),MaxLength(50,ErrorMessage ="不能超过50个字符")]
        public string Title { get; set; }

        [Display(Name ="排序数字")]
        public int Weight { get; set; }


        public bool Status { get; set; }


        public DateTime AddTime { get; set; }

    }
}
