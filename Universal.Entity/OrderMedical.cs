using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatus : byte
    {
        [Description("临时订单")]
        临时订单 = 0,//加项使用
        [Description("待支付")]
        等待支付 = 1,
        [Description("已取消")]
        已取消,
        [Description("已完成")]
        已完成,
        [Description("已支付")]
        已支付
    }

    /// <summary>
    /// 订单支付类别
    /// </summary>
    public enum OrderPayType : byte
    {
        [Description("微信支付")]
        微信支付 = 1,
        [Description("账户余额")]
        账户余额 = 2
    }

    /// <summary>
    /// 订单-体检套餐
    /// </summary>
    public class OrderMedical
    {
        public OrderMedical()
        {
            this.Status = OrderStatus.临时订单;
            this.PayType = OrderPayType.微信支付;
            this.IDCardType = MPUserIDCardType.IDCard;
            this.AddTime = DateTime.Now;
            this.Gender = MPUserGenderType.unknown;
            this.YuYueDate = DateTime.Now.AddDays(1);
            this.OrderMedicalItems = new List<OrderMedicalItem>();
        }

        public int ID { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Index(IsUnique = true), MaxLength(100)]
        public string OrderNum { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; set; }

        public string GetStatusStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<OrderStatus>(Status);
            }
        }

        /// <summary>
        /// 是否可以完成
        /// </summary>
        [NotMapped]
        public bool CanDone
        {
            get
            {
                return Status == OrderStatus.已支付;
            }
        }

        /// <summary>
        /// 订单说明
        /// </summary>
        [MaxLength(255)]
        public string Desc { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public OrderPayType PayType { get; set; }

        public string GetPayTypeStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<OrderPayType>(PayType);
            }
        }

        /// <summary>
        /// 订单金额
        /// </summary>
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        [Column(TypeName = "money")]
        public decimal RelAmount { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public int MPUserID { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual MPUser MPUser { get; set; }

        /// <summary>
        /// 套餐ID
        /// </summary>
        public int MedicalID { get; set; }

        /// <summary>
        /// 体检套餐名称
        /// </summary>
        [MaxLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// 套餐原价
        /// </summary>
        [Column(TypeName ="money")]
        public decimal MYPrice { get; set; }

        /// <summary>
        /// 套餐实际价格
        /// </summary>
        [Column(TypeName = "money")]
        public decimal MPrice { get; set; }

        /// <summary>
        /// 体检套餐封面图
        /// </summary>
        [MaxLength(255)]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 预约用户姓名
        /// </summary>
        [MaxLength(100)]
        public string RealName { get; set; }

        /// <summary>
        /// 体检用户-身份证类别
        /// </summary>
        public MPUserIDCardType IDCardType { get; set; }

        /// <summary>
        /// 体检用户-身份证号码
        /// </summary>
        [MaxLength(30)]
        public string IDCardNumber { get; set; }

        /// <summary>
        /// 体检用户-电话，大陆+86，香港+852，澳门+853
        /// </summary>
        [MaxLength(50)]
        public string Telphone { get; set; }

        /// <summary>
        /// 体检用户-性别
        /// </summary>
        public MPUserGenderType Gender { get; set; }

        public string GetGenderStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<MPUserGenderType>(Gender);
            }
        }

        /// <summary>
        /// 体检用户-生日
        /// </summary>
        [Column(TypeName = "Date")]
        public DateTime? Brithday { get; set; }

        public string GetBrithday
        {
            get
            {
                if (Brithday == null) return "";
                return Tools.TypeHelper.ObjectToDateTime(Brithday).ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 预约体检日期
        /// </summary>
        public DateTime YuYueDate { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        public string GetPayTimeStr
        {
            get
            {
                if (PayTime == null) return "/";
                else return Tools.TypeHelper.ObjectToDateTime(PayTime).ToString("yyyy-MM-dd HH:mm");
            }
        }

        public DateTime AddTime { get; set; }

        /// <summary>
        /// 体检项
        /// </summary>
        public virtual ICollection<OrderMedicalItem> OrderMedicalItems { get; set; }
    }
}
