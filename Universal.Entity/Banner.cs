using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    public enum BannerLinkType
    {
        [Description("无动作")]
        none=0,
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
        News,
        [Description("数字品牌馆")]
        ShuZiPin,
        [Description("数字体验馆")]
        ShuZiTi,
        [Description("城市规划馆")]
        ChengShiGH,
        [Description("数字仿真工程")]
        ShuZiFang,
        [Description("视觉动画")]
        ShiJue,
        [Description("创意广告")]
        ChuangYI,
        [Description("新媒体互动")]
        XinMeiTi,
        [Description("建筑可视化")]
        JianZhu
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
            this.LinkVal = "N";
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
        /// 获取前端点击跳转的页面地址 a标签的href 事件
        /// </summary>
        public string GetClickUrl
        {
            get
            {
                switch (LinkType)
                {
                    case BannerLinkType.none:
                        return "";
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
                    case BannerLinkType.ShuZiPin:
                        return " href='/CaseShow/Index?t=Digital-Display&e=Digital-Brand-Pavilion'";
                    case BannerLinkType.ShuZiTi:
                        return " href='/CaseShow/Index?t=Digital-Display&e=Digital-Experience-Hall'";
                    case BannerLinkType.ChengShiGH:
                        return " href='/CaseShow/Index?t=Digital-Display&e=City-Planning-Hall'";
                    case BannerLinkType.ShuZiFang:
                        return " href='/CaseShow/Index?t=Digital-Display&e=Digital-Engineering-Simulation'";
                    case BannerLinkType.ShiJue:
                        return " href='/CaseShow/Index?t=Creative-Vision&e=Visual-Animation'";
                    case BannerLinkType.ChuangYI:
                        return " href='/CaseShow/Index?t=Creative-Vision&e=Creative-Advertising'";
                    case BannerLinkType.XinMeiTi:
                        return " href='/CaseShow/Index?t=Creative-Vision&e=New-Media-Interaction'";
                    case BannerLinkType.JianZhu:
                        return " href='/CaseShow/Index?t=Creative-Vision&e=Architectural-Visualization'";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 获取前端点击跳转的页面地址 DIV的CLick事件
        /// </summary>
        public string GetDivClickUrl
        {
            get
            {
                switch (LinkType)
                {
                    case BannerLinkType.none:
                        return "";
                    case BannerLinkType.web_url:
                        return "window.open('" + LinkVal + "' target='_blank')";
                    case BannerLinkType.case_show:
                        return "window.open('/CaseShow/Detail?id=" + LinkVal + "')";
                    case BannerLinkType.CompanyProfile:
                        return "window.open('/About/Summary')";
                    case BannerLinkType.CompanyCulture:
                        return "window.open('/About/Culture')";
                    case BannerLinkType.TeamIntroduction:
                        return "window.open('/About/TeamIntroduction')";
                    case BannerLinkType.CompanyHonor:
                        return "window.open('/About/Honor')";
                    case BannerLinkType.Memorabilia:
                        return "window.open('/About/Memorabilia')";
                    case BannerLinkType.FutureVision:
                        return "window.open('/About/FutureVision')";
                    case BannerLinkType.JoinUS:
                        return "window.open('/Contact/Job?id=" + LinkVal + "')";
                    case BannerLinkType.News:
                        return "window.open('/Contact/NewsDetail?id=" + LinkVal + "')";
                    case BannerLinkType.ShuZiPin:
                        return "window.open('/CaseShow/Index?t=Digital-Display&e=Digital-Brand-Pavilion')";
                    case BannerLinkType.ShuZiTi:
                        return "window.open('/CaseShow/Index?t=Digital-Display&e=Digital-Experience-Hall')";
                    case BannerLinkType.ChengShiGH:
                        return "window.open('/CaseShow/Index?t=Digital-Display&e=City-Planning-Hall')";
                    case BannerLinkType.ShuZiFang:
                        return "window.open('/CaseShow/Index?t=Digital-Display&e=Digital-Engineering-Simulation')";
                    case BannerLinkType.ShiJue:
                        return "window.open('/CaseShow/Index?t=Creative-Vision&e=Visual-Animation')";
                    case BannerLinkType.ChuangYI:
                        return "window.open('/CaseShow/Index?t=Creative-Vision&e=Creative-Advertising')";
                    case BannerLinkType.XinMeiTi:
                        return "window.open('/CaseShow/Index?t=Creative-Vision&e=New-Media-Interaction')";
                    case BannerLinkType.JianZhu:
                        return "window.open('/CaseShow/Index?t=Creative-Vision&e=Architectural-Visualization')";
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
