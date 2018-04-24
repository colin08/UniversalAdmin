using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 诊所列表
    /// </summary>
    public class Clinic
    {
        public Clinic()
        {
            this.AddTime = DateTime.Now;
            this.Weight = 99;
            this.Status = true;
        }

        public int ID { get; set; }

        /// <summary>
        /// 所属地区
        /// </summary>
        public int ClinicAreaID { get; set; }

        /// <summary>
        /// 地区信息
        /// </summary>
        public virtual ClinicArea ClinicArea { get; set; }

        /// <summary>
        /// 诊所名称
        /// </summary>
        [Required(ErrorMessage ="必填"),MaxLength(100,ErrorMessage ="不能超过100个字符")]
        public string Title { get; set; }

        /// <summary>
        /// 诊所图标
        /// </summary>
        [MaxLength(255)]
        public string ICON { get; set; }
        
        /// <summary>
        /// 工作时间 门诊：周一~周六 8：30-17：30 周日休诊
        /// </summary>
        [MaxLength(255)]
        public string WorkHours { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Required(ErrorMessage = "必填"), MaxLength(255)]
        public string Address { get; set; }

        /// <summary>
        /// 诊所电话
        /// </summary>
        [MaxLength(100)]
        public string Telphone { get; set; }

        /// <summary>
        /// 服务项目
        /// </summary>
        [MaxLength(500)]
        public string FuWuXiangMu { get; set; }

        /// <summary>
        /// 服务语言
        /// </summary>
        [MaxLength(300)]
        public string FuWuYuYan { get; set; }

        /// <summary>
        /// 乘车路线
        /// </summary>
        [MaxLength(1000)]
        public string ChengCheLuXian { get; set; }
        
        /// <summary>
        /// 排序数字
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 科室数据
        /// </summary>
        public virtual ICollection<ClinicDepartment> DepartmentList { get; set; }
    }
}
