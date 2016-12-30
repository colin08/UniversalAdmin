using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Universal.Entity
{
    /// <summary>
    /// 工作计划之周期计划
    /// </summary>
    public class WorkPlanItem
    {
        public WorkPlanItem()
        {
            this.Status = WorkStatus.ing;
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
        public WorkStatus Status { get; set; }

        /// <summary>
        /// 状态文本
        /// </summary>
        [NotMapped]
        public string StatusText
        {
            get
            {
                switch (this.Status)
                {
                    case WorkStatus.ing:
                        return "进行中";
                    case WorkStatus.done:
                        return "已完成";
                    case WorkStatus.cancel:
                        return "已取消";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(255)]
        public string Remark { get; set; }

    }
}
