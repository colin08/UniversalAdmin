using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 医生特长标签
    /// </summary>
    public class DoctorsSpecialty
    {
        public DoctorsSpecialty()
        {
            this.AddTime = DateTime.Now;
            this.DoctorsList = new List<MPUserDoctors>();
        }

        public int ID { get; set; }

        /// <summary>
        /// 特长名称
        /// </summary>
        [Required(ErrorMessage = "必填"),MaxLength(50,ErrorMessage ="不能超过50个字符")]
        public string Title { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 医生列表
        /// </summary>
        public virtual ICollection<MPUserDoctors> DoctorsList { get; set; }

    }
}
