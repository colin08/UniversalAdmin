using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            return View();
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