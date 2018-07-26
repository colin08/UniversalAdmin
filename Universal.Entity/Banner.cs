using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    public enum BannerLinkType
    {
        [Description("外部网页")]
        web_url = 1,
    }

    /// <summary>
    /// 首页Banner图
    /// </summary>
    public class Banner : BaseAdminEntity
    {
        public Banner()
        {
            this.Status = true;
            this.Weight = 99;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
            this.Remark = "";
        }

        public int ID { get; set; }

        /// <summary>
        /// 所属分类
        /// </summary>
        [Display(Name = "所属分类")]
        public int CategoryID { get; set; }

        /// <summary>
        /// 分类信息
        /// </summary>
        public virtual Category Category { get; set; }

        [Display(Name = "标题"), MaxLength(255, ErrorMessage = "不能超过255个字符")]
        public string Title { get; set; }

        [Display(Name = "事件目标"), Required(ErrorMessage = "不能为空")]
        public BannerLinkType LinkType { get; set; }

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
