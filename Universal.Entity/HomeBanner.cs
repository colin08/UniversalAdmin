using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    public enum HomeBannerLinkType
    {
        [Description("外部网页")]
        web_url = 1,
    }

    /// <summary>
    /// 首页Banner图
    /// </summary>
    public class HomeBanner : BaseAdminEntity
    {
        public HomeBanner()
        {
            this.Status = true;
            this.Weight = 99;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
            this.Remark = "";
        }

        public int ID { get; set; }

        [Display(Name = "标题"), MaxLength(255, ErrorMessage = "不能超过255个字符")]
        public string Title { get; set; }

        [Display(Name = "事件目标"), Required(ErrorMessage = "不能为空")]
        public HomeBannerLinkType LinkType { get; set; }

        /// <summary>
        /// 获取事件类别
        /// </summary>
        public string GetLinkType
        {
            get
            {
                return Tools.EnumHelper.GetDescription(LinkType);
            }
        }

        [Display(Name = "目标参数"), MaxLength(500, ErrorMessage = "不能超过500个字符"), Required(ErrorMessage = "不能为空")]
        public string LinkVal { get; set; }

        [Display(Name = "状态")]
        public bool Status { get; set; }

        [Display(Name = "排序数字")]
        public int Weight { get; set; }

        [Display(Name = "图片"), MaxLength(300)]
        public string ImgUrl { get; set; }

        [Display(Name = "备注"), MaxLength(500, ErrorMessage = "不能超过500个字符")]
        public string Remark { get; set; }

    }
}
