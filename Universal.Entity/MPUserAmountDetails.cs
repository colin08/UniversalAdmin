using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    public enum MPUserAmountDetailsType : byte
    {
        [Description("增加")]
        Add= 1,
        [Description("支出")]
        Less=2
    }

    /// <summary>
    /// 用户账户资金明细
    /// </summary>
    public class MPUserAmountDetails
    {
        public MPUserAmountDetails()
        {
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public int MPUserID { get; set; }

        public virtual MPUser MPUser { get; set; }
        
        /// <summary>
        /// 内容
        /// </summary>
        [MaxLength(500)]
        public string Title { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public MPUserAmountDetailsType Type { get; set; }

        /// <summary>
        /// 获取类别String
        /// </summary>
        public string GetTypeStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<MPUserAmountDetailsType>(Type);
            }
        }

        /// <summary>
        /// 发生金额
        /// </summary>
        [Column(TypeName ="money")]
        public decimal Amount { get; set; }

        public DateTime AddTime { get; set; }

        public string GetAddTime
        {
            get
            {
                return AddTime.ToString("yyy-MM-dd HH:mm");
            }
        }

    }
}
