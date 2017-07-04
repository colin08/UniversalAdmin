using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

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
        /// 计划名称
        /// </summary>
        [MaxLength(200)]
        public string WeekText { get; set; }
        
        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime? ApproveTime { get; set; }

        /// <summary>
        /// 审核人员ID
        /// </summary>
        public int? ApproveUserID { get; set; }

        /// <summary>
        /// 审核人员信息
        /// </summary>
        [ForeignKey("ApproveUserID")]
        public virtual CusUser ApproveUser { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApproveStatusType ApproveStatus { get; set; }

        /// <summary>
        /// 审核状态文本
        /// </summary>
        [NotMapped]
        public string ApproveStatusTxt
        {
            get
            {
                return Tools.EnumHelper.GetBEnumShowName(typeof(ApproveStatusType), (byte)this.ApproveStatus);
            }
        }

        /// <summary>
        /// 审核状态备注
        /// </summary>
        [MaxLength(500)]
        public string ApproveRemark { get; set; }
        
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
                
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 计划条目
        /// </summary>
        public virtual ICollection<WorkPlanItem> WorkPlanItemList { get; set; }

    }
}
