using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 体检套餐项
    /// </summary>
    public class MedicalItem
    {
        public MedicalItem()
        {
            this.Status = true;
            this.Weight = 99;
            this.AddTime = DateTime.Now;
            this.Medicals = new List<Medical>();
        }

        public int ID { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        [Required(ErrorMessage = "项目编号必填"), MaxLength(30,ErrorMessage ="不超过30个字符")]
        public string OnlyID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }
        

        /// <summary>
        /// 项名称
        /// </summary>
        [Required(ErrorMessage ="项目名称必填"),MaxLength(100,ErrorMessage ="不超过100个字符")]
        public string Title { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        [MaxLength(30)]
        public string SZM { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [Column(TypeName = "money"), DisplayFormat(DataFormatString = "{0:0}")]
        public decimal Price { get; set; }

        /// <summary>
        /// 排序数字
        /// </summary>
        public int Weight { get; set; }


        /// <summary>
        /// 项目意义
        /// </summary>
        [MaxLength(500), DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Desc { get; set; }

        public DateTime AddTime { get; set; }

        /// <summary>
        /// 引用该项的套餐
        /// </summary>
        public virtual ICollection<Medical> Medicals { get; set; }
    }
}
