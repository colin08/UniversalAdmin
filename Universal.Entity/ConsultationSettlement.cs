using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{

    /// <summary>
    /// 咨询结算状态
    /// </summary>
    public enum ConsultationSettlementStatus : byte
    {
        [Description("等待审核")]
        等待审核 = 1,
        [Description("审核通过")]
        审核通过,
        [Description("审核不通过")]
        审核不通过
    }

    /// <summary>
    /// 咨询结算打款状态
    /// </summary>
    public enum ConsultationSettlementPayStatus : byte
    {
        [Description("等待打款")]
        等待打款 = 1,
        [Description("打款成功")]
        打款成功,
        [Description("打款失败")]
        打款失败
    }

    /// <summary>
    /// 咨询结算
    /// </summary>
    public class ConsultationSettlement
    {
        public ConsultationSettlement()
        {
            Status = ConsultationSettlementStatus.等待审核;
            StatusDESC = "等待管理员审核";
            PayStatus = ConsultationSettlementPayStatus.等待打款;
            AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 医生ID
        /// </summary>
        public int MPUserID { get; set; }

        /// <summary>
        /// 医生信息
        /// </summary>
        public virtual MPUser MPDoctorInfo { get; set; }

        /// <summary>
        /// 操作管理员ID
        /// </summary>
        public int? SysUserID { get; set; }

        /// <summary>
        /// 操作管理员信息
        /// </summary>
        public virtual SysUser SysUserInfo { get; set; }
                
        /// <summary>
        /// 订单号
        /// </summary>
        [MaxLength(255)]
        public string OrderNum { get; set; }

        /// <summary>
        /// 原金额
        /// </summary>
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 实际打款金额-平台要抽成
        /// </summary>
        [Column(TypeName ="money")]
        public decimal RelAmount { get; set; }

        /// <summary>
        /// 结算状态
        /// </summary>
        public ConsultationSettlementStatus Status { get; set; }

        /// <summary>
        /// 获取结算状态文本
        /// </summary>
        [NotMapped]
        public string GetStatusStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<ConsultationSettlementStatus>(Status);
            }
        }

        /// <summary>
        /// 打款状态
        /// </summary>
        public ConsultationSettlementPayStatus PayStatus { get; set; }

        /// <summary>
        /// 获取打款状态文本
        /// </summary>
        [NotMapped]
        public string GetPayStatusStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<ConsultationSettlementPayStatus>(PayStatus);
            }
        }

        /// <summary>
        /// 结算状态说明
        /// </summary>
        [MaxLength(255)]
        public string StatusDESC { get; set; }

        /// <summary>
        /// 打款状态说明
        /// </summary>
        [MaxLength(255)]
        public string PayStatusDESC { get; set; }

        /// <summary>
        /// 平台抽成说明
        /// </summary>
        [MaxLength(255)]
        public string CCDESC { get; set; }


        public DateTime AddTime { get; set; }

        /// <summary>
        /// 管理员操作时间
        /// </summary>
        public DateTime? AdminTime { get; set; }

        /// <summary>
        /// 具体咨询项,可以多个合并一起结算
        /// </summary>
        public virtual ICollection<ConsultationSettlementItem> ConsultationSettlementItem { get; set; }

    }
}
