using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 关于朗形
    /// </summary>
    public class AboutController : BaseWebController
    {
        /// <summary>
        /// 公司简介
        /// </summary>
        /// <returns></returns>
        public ActionResult Summary()
        {
            WorkContext.CategoryMark = "About";
            WorkContext.CategoryErMark = "Company-Profile";
            var banner_list = BLL.BLLAbout.GetCompanyProfileBannerList();
            Models.CompanyProfile result_model = new Models.CompanyProfile();

            var config_model = ConfigHelper.LoadConfig<CompanyProfileModel>(ConfigFileEnum.CompanyProfile, false);
            config_model.JJDesc = WebHelper.UrlDecode(config_model.JJDesc);
            config_model.JJBGDesc = WebHelper.UrlDecode(config_model.JJBGDesc);
            config_model.JJOneLeftDesc = WebHelper.UrlDecode(config_model.JJOneLeftDesc);
            config_model.JJOneRightDesc = WebHelper.UrlDecode(config_model.JJOneRightDesc);
            config_model.JJTwoLeftDesc = WebHelper.UrlDecode(config_model.JJTwoLeftDesc);
            config_model.JJTwoRightDesc = WebHelper.UrlDecode(config_model.JJTwoRightDesc);

            result_model.banner_list = banner_list;
            result_model.SiteConfig = config_model;
            return View(result_model);
        }

        /// <summary>
        /// 企业文化
        /// </summary>
        /// <returns></returns>
        public ActionResult Culture()
        {
            WorkContext.CategoryMark = "About";
            WorkContext.CategoryErMark = "Company-Culture";

            return View();
        }

        /// <summary>
        /// 团队介绍
        /// </summary>
        /// <returns></returns>
        public ActionResult TeamIntroduction()
        {
            WorkContext.CategoryMark = "About";
            WorkContext.CategoryErMark = "Team-Introduction";

            return View();
        }

        /// <summary>
        /// 公司荣誉
        /// </summary>
        /// <returns></returns>
        public ActionResult Honor()
        {
            WorkContext.CategoryMark = "About";
            WorkContext.CategoryErMark = "Company-Honor";

            var result_model = BLL.BLLAbout.GetHonourList();
            return View(result_model);
        }

        /// <summary>
        /// 大事记
        /// </summary>
        /// <returns></returns>
        public ActionResult Memorabilia()
        {
            WorkContext.CategoryMark = "About";
            WorkContext.CategoryErMark = "Company-Memorabilia";

            var result_model = BLL.BLLAbout.GetTimeLineList();
            return View(result_model);
        }

        /// <summary>
        /// 未来&愿景
        /// </summary>
        /// <returns></returns>
        public ActionResult FutureVision()
        {
            WorkContext.CategoryMark = "About";
            WorkContext.CategoryErMark = "Future-Vision";

            var result_model = BLL.BLLAbout.GetFutureVisionList();
            return View(result_model);
        }


    }
}