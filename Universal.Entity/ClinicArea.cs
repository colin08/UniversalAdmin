using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 地区设置
    /// </summary>
    public class ClinicArea
    {
        public ClinicArea()
        {
            this.Status = true;
            this.Weight = 99;
            this.AddTime = DateTime.Now;
        }

        public ClinicArea(string title)
        {
            this.Title = title;
            this.Status = true;
            this.Weight = 99;
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50,ErrorMessage ="不能超过50个字符"),Required(ErrorMessage ="地区名称必填")]
        public string Title { get; set; }

        public bool Status { get; set; }

        /// <summary>
        /// 排序数字
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

    }
}
