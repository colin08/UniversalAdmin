using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 微信用户-医生身份拓展信息
    /// </summary>
    public class MPUserDoctors
    {
        public MPUserDoctors()
        {
            this.AddTime = DateTime.Now;
            this.CanAdvisory = true;
            this.AdvisoryPrice = 99;
            this.TouXian = "医师";
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 主用户信息
        /// </summary>
        [ForeignKey("ID")]
        public virtual MPUser MPUser { get; set; }

        /// <summary>
        /// 所属诊所ID
        /// </summary>
        public int? ClinicID { get; set; }

        /// <summary>
        /// 所属诊所数据
        /// </summary>
        public virtual Clinic Clinic { get; set; }
        
        /// <summary>
        /// 获取诊所名称
        /// </summary>
        [NotMapped]
        public string GetClinicTitle
        {
            get
            {
                if (Clinic == null) return "";
                return Clinic.Title;
            }
        }

        /// <summary>
        /// 所属科室、会有多个
        /// </summary>
        public virtual ICollection<ClinicDepartment> ClinicDepartmentList { get; set; }

        /// <summary>
        /// 特长列表
        /// </summary>
        public virtual ICollection<DoctorsSpecialty> DoctorsSpecialtyList { get; set; }

        /// <summary>
        /// 头衔
        /// </summary>
        [MaxLength(100,ErrorMessage ="头衔不能超过100个字符"),Required(ErrorMessage = "必填")]
        [DisplayFormat(ConvertEmptyStringToNull =true)]
        public string TouXian { get; set; }

        /// <summary>
        /// 医生介绍
        /// </summary>
        [MaxLength(200),DisplayFormat(ConvertEmptyStringToNull = true)]
        public string ShowMe { get; set; }

        /// <summary>
        /// 接收咨询
        /// </summary>
        public bool CanAdvisory { get; set; }

        /// <summary>
        /// 咨询价格
        /// </summary>
        [Column(TypeName ="money")]
        public decimal AdvisoryPrice { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

    }
}
