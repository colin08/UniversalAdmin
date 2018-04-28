using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 文章Banner
    /// </summary>
    public class NewsBanner
    {
        public NewsBanner()
        {
            this.Status = true;
            this.Weight = 99;
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Display(Name = "标题"), MaxLength(20, ErrorMessage = "不能超过20个字符"), Required(ErrorMessage = "不能为空")]
        public string Title { get; set; }

        [Display(Name = "封面图"), MaxLength(255)]
        public string ImgUrl { get; set; }

        [Display(Name ="状态")]
        public bool Status { get; set; }

        /// <summary>
        /// 事件类别
        /// </summary>
        public MedicalBannerLinkType LinkType { get; set; }

        public string GetLinkType
        {
            get
            {
                return Tools.EnumHelper.GetDescription<MedicalBannerLinkType>(LinkType);
            }
        }

        /// <summary>
        /// 事件目标，套餐ID或网页地址地址
        /// </summary>
        [MaxLength(500), DisplayFormat(ConvertEmptyStringToNull = true)]
        public string LinkVal { get; set; }


        [Display(Name ="排序数字")]
        public int Weight { get; set; }

        public DateTime AddTime { get; set; }

    }
}
