using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Universal.Entity
{
    /// <summary>
    /// 工作计划
    /// </summary>
    public class WorkPlan
    {
        public WorkPlan()
        {
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 周期
        /// </summary>
        [MaxLength(200)]
        public string WeekText { get; set; }

        /// <summary>
        /// 本周工作记录
        /// </summary>
        [MaxLength(1000)]
        public string NowJob { get; set; }
        

        /// <summary>
        /// 下周工作计划
        /// </summary>
        [MaxLength(1000)]
        public string NextPlan { get; set; }
        
        /// <summary>
        /// 是否审批
        /// </summary>
        public bool IsApprove { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime? ApproveTime { get; set; }

        /// <summary>
        /// 审批人，昵称逗号分割
        /// </summary>
        [NotMapped]
        public string ApproveNickName { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime DoneTime { get; set; }
        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

    }
}
