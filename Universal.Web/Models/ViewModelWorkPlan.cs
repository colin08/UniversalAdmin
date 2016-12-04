using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 工作计划
    /// </summary>
    public class ViewModelWorkPlan: ViewModelCustomFormBase
    {

        public ViewModelWorkPlan()
        {
            this.year = DateTime.Now.Year.ToString();
            this.month = DateTime.Now.Month.ToString();
            this.day = DateTime.Now.Day.ToString();
        }

        public int id { get; set; }

        /// <summary>
        /// 周期
        /// </summary>
        [Required(ErrorMessage ="请选择周期")]
        public string week_text { get; set; }

        /// <summary>
        /// 本周工作记录
        /// </summary>
        [Required(ErrorMessage ="请输入本周工作记录"),MaxLength(1000,ErrorMessage ="本周工作记录不能超过1000个字符")]
        public string now_job { get; set; }

        /// <summary>
        /// 下周工作计划
        /// </summary>
        [Required(ErrorMessage = "请输入下周工作计划"), MaxLength(1000, ErrorMessage = "下周工作计划不能超过1000个字符")]
        public string next_plan { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public string year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public string month { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        public string day { get; set; }

    }
}