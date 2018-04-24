using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 诊所科室
    /// </summary>
    public class ClinicDepartment
    {
        public ClinicDepartment()
        {
            this.AddTime = DateTime.Now;
            this.Weight = 99;
            this.Status = true;
        }

        public int ID { get; set; }

        /// <summary>
        /// 所属诊所ID
        /// </summary>
        public int ClinicID { get; set; }

        /// <summary>
        /// 所属诊所数据
        /// </summary>
        public virtual Clinic Clinic { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        [Required(ErrorMessage = "必填"), MaxLength(100,ErrorMessage ="不能超过100个字符")]
        public string Title { get; set; }

        /// <summary>
        /// 科室介绍
        /// </summary>
        [MaxLength(255)]
        public string Desc { get; set; }


        public bool Status { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        [MaxLength(30)]
        public string SZM { get; set; }

        /// <summary>
        /// 排序数字，越大越靠前
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 科室下的医生、多对多
        /// </summary>
        public virtual ICollection<MPUserDoctors> DoctorsList { get; set; }

        public DateTime AddTime { get; set; }

    }
}
