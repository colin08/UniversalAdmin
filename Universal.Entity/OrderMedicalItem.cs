using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{

    /// <summary>
    /// 订单-体检项类别
    /// </summary>
    public enum OrderMedicalItemType : byte
    {
        套餐内=1,
        额外自选=2
    }

    /// <summary>
    /// 订单-体检套餐-体检具体项目
    /// </summary>
    public class OrderMedicalItem
    {
        public OrderMedicalItem()
        {

        }

        public int ID { get; set; }

        /// <summary>
        /// 所属订单
        /// </summary>
        public int OrderMedicalID { get; set; }

        public virtual OrderMedical OrderMedical { get; set; }

        /// <summary>
        /// 项目类别
        /// </summary>
        public OrderMedicalItemType Type { get; set; }

        /// <summary>
        /// 体检项ID，不做关联约束
        /// </summary>
        public int MedicalID { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [MaxLength(255)]
        public  string Title { get; set; }

        /// <summary>
        /// 排序数字
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        [MaxLength(30)]
        public string OnlyID { get; set; }

        /// <summary>
        /// 项目价格
        /// </summary>
        [Column(TypeName ="money")]
        public decimal Price { get; set; }

        /// <summary>
        /// 项目说明（项目意义）
        /// </summary>
        [MaxLength(500)]
        public string Desc { get; set; }

        public string GetTipsDesc
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Desc)) return "无介绍";
                else
                {
                    return Desc.Replace("\r\n", "");
                }
            }
        }

    }
}
