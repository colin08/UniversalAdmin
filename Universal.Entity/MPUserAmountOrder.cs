using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Universal.Entity
{
    /// <summary>
    /// 用户充值订单
    /// </summary>
    public class MPUserAmountOrder
    {

        public MPUserAmountOrder()
        {
            this.AddTime = DateTime.Now;
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="order_num"></param>
        /// <param name="amount"></param>
        /// <param name="user_id"></param>
        /// <param name="desc"></param>
        public MPUserAmountOrder(string order_num,decimal amount,int user_id,string desc)
        {
            this.AddTime = DateTime.Now;
            this.OrderNum = order_num;
            this.Amount = amount;
            this.MPUserID = user_id;
            this.Desc = desc;
        }

        public int ID { get; set; }

        [MaxLength(50)]
        public string OrderNum { get; set; }

        /// <summary>
        /// 微信订单号
        /// </summary>
        [MaxLength(50)]
        public string OrderNumWX { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public string GetAmount
        {
            get
            {
                return Tools.WebHelper.FormatDecimalMoney(Amount);
            }
        }

        /// <summary>
        /// 所属用户
        /// </summary>
        public int MPUserID { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual MPUser MPUser { get; set; }
        
        /// <summary>
        /// 支付用户的open_id
        /// </summary>
        [MaxLength(100)]
        public string OpenID { get; set; }

        /// <summary>
        /// 订单说明
        /// </summary>
        [MaxLength(255)]
        public string Desc { get; set; }

        public bool Status { get; set; }
        
        public DateTime? PayTime { get; set; }

        public DateTime AddTime { get; set; }

    }
}
