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
        [Description("案例展示")]
        case_show,
        [Description("公司简介")]
        CompanyProfile,
        [Description("企业文化")]
        CompanyCulture,
        [Description("团队介绍")]
        TeamIntroduction,
        [Description("公司荣誉")]
        CompanyHonor,
        [Description("大事记")]
        Memorabilia,
        [Description("未来愿景")]
        FutureVision,
        [Description("职位招聘")]
        JoinUS,
        [Description("新闻资讯")]
        News
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

        /// <summary>
        /// 获取前端点击跳转的页面地址
        /// </summary>
        public string GetClickUrl
        {
            get
            {
                switch (LinkType)
                {
                    case BannerLinkType.web_url:
                        return " href='" + LinkVal + "' target='_blank'";
                    case BannerLinkType.case_show:
                        return " href='/CaseShow/Detail?id="+LinkVal+"'";
                    case BannerLinkType.CompanyProfile:
                        return " href='/About/Summary'";
                    case BannerLinkType.CompanyCulture:
                        return " href='/About/Culture'";
                    case BannerLinkType.TeamIntroduction:
                        return " href='/About/TeamIntroduction'";
                    case BannerLinkType.CompanyHonor:
                        return " href='/About/Honor'";
                    case BannerLinkType.Memorabilia:
                        return " href='/About/Memorabilia'";
                    case BannerLinkType.FutureVision:
                        return " href='/About/FutureVision'";
                    case BannerLinkType.JoinUS:
                        return " href='/Contact/Job?id="+LinkVal+"'";
                    case BannerLinkType.News:
                        return " href='/Contact/NewsDetail?id=" + LinkVal + "'";
                    default:
                        return "";
                }
            }
        }

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
