using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 套餐状态
    /// </summary>
    public enum MedicalStatus : byte
    {
        [Description("上架")]
        Up =1,
        [Description("下架")]
        Down=2
    }

    /// <summary>
    /// 体检套餐
    /// </summary>
    public class Medical
    {
        public Medical()
        {
            this.AddTime = DateTime.Now;
            this.Status = MedicalStatus.Up;
            this.Weight = 99;
            this.MedicalItems = new List<MedicalItem>();
        }

        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage ="套餐名称必填"),MaxLength(255,ErrorMessage ="不能超过255个字符")]
        public string Title { get; set; }

        /// <summary>
        /// 套餐状态
        /// </summary>
        public MedicalStatus Status { get; set; }

        /// <summary>
        /// 获取状态String
        /// </summary>
        public string GetStatus
        {
            get
            {
                return Tools.EnumHelper.GetDescription<MedicalStatus>(Status);
            }
        }

        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        [MaxLength(255),DisplayFormat(ConvertEmptyStringToNull = true)]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        [Column(TypeName = "money")]
        public decimal YPrice { get; set; }

        public string GetYPrice
        {
            get
            {
                return Tools.WebHelper.FormatDecimalMoney(YPrice);
            }
        }

        /// <summary>
        /// 实际价格
        /// </summary>
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public string GetPrice
        {
            get
            {
                return Tools.WebHelper.FormatDecimalMoney(Price);
            }
        }

        
        /// <summary>
        /// VIP价格（预留）
        /// </summary>
        [Column(TypeName = "money")]
        public decimal VIPPrice { get; set; }

        /// <summary>
        /// 套餐介绍
        /// </summary>
        [MaxLength(2000), DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Desc { get; set; }

        
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 包含的项
        /// </summary>
        public virtual ICollection<MedicalItem> MedicalItems { get; set; }

    }
}
