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
            this.year2 = DateTime.Now.Year.ToString();
            this.month2 = DateTime.Now.Month.ToString();
            this.day2 = DateTime.Now.Day.ToString();
            this.plan_item = new List<Entity.WorkPlanItem>();
            var entity = new Entity.WorkPlanItem();
            entity.Status = Entity.WorkStatus.ing;
            this.plan_item.Add(entity);
        }

        public int id { get; set; }

        /// <summary>
        /// 计划名称
        /// </summary>
        [Required(ErrorMessage ="计划名称不能为空")]
        public string week_text { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public bool approve_status { get; set; }
        
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

        /// <summary>
        /// 年
        /// </summary>
        public string year2 { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public string month2 { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        public string day2 { get; set; }

        public int approve_user_id { get; set; }

        public string approve_user_name { get; set; }

        public List<Entity.WorkPlanItem> plan_item { get; set; }

    }
}