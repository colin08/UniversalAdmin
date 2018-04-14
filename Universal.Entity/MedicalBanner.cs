using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Universal.Entity
{
    /// <summary>
    /// 轮播图跳转类别
    /// </summary>
    public enum MedicalBannerLinkType : byte
    {
        [Description("套餐")]
        Medical=1,
        [Description("网页")]
        WebSite =2
    }

    /// <summary>
    /// 套餐轮播图
    /// </summary>
    public class MedicalBanner
    {
        public MedicalBanner()
        {
            this.Status = true;
            this.Weight = 99;
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [MaxLength(255), DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Title { get; set; }
        
        /// <summary>
        /// 封面图
        /// </summary>
        [MaxLength(255), DisplayFormat(ConvertEmptyStringToNull = true)]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
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

        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }


        public DateTime AddTime { get; set; }
    }
}
