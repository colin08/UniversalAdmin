using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Universal.Entity
{
    public enum PlanStatus:byte
    {
        /// <summary>
        /// 未完成
        /// </summary>
        [Description("未完成")]
        ing = 0,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        done = 1
    }

    /// <summary>
    /// 工作计划之周期计划
    /// </summary>
    public class WorkPlanItem
    {
        public WorkPlanItem()
        {
            this.Status = PlanStatus.ing;
        }

        public int ID { get; set; }

        /// <summary>
        /// 计划ID
        /// </summary>
        public int WorkPlanID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual WorkPlan WorkPlan { get; set; }

        /// <summary>
        ///  标题
        /// </summary>
        [MaxLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// 工作内容
        /// </summary>
        [MaxLength(1000)]
        public string Content { get; set; }

        /// <summary>
        /// 预期目标 选填
        /// </summary>
        [MaxLength(255)]
        public string WantTaget { get; set; }

        /// <summary>
        /// 完成时限 选填
        /// </summary>
        [MaxLength(100)]
        public string DoneTime { get; set; }

        /// <summary>
        /// 完成情况
        /// </summary>
        public PlanStatus Status { get; set; }

        /// <summary>
        /// 状态文本
        /// </summary>
        [NotMapped]
        public string StatusText
        {
            get
            {
                return Tools.EnumHelper.GetDescription<PlanStatus>(this.Status);
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(255)]
        public string Remark { get; set; }

    }
}
