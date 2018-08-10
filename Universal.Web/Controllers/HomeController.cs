using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : BaseWebController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //设置导航标识
            WorkContext.CategoryMark = "HOME";
            WorkContext.CategoryErMark = "";

            var result_model = BLL.BLLHome.GetData();
            return View(result_model);
        }

        /// <summary>
        /// 数字展示-创意视觉子分类
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public ActionResult Category(string t)
        {
            if (string.IsNullOrWhiteSpace(t) || string.IsNullOrWhiteSpace(t)) return ErrorView("找不到相关页面");
            //大分类必须是数字展示或创意视觉
            if (t.ToLower() != "digital-display" && t.ToLower() != "creative-vision") return ErrorView("找不到相关页面");
            //设置导航标识
            WorkContext.CategoryMark = t.Replace("-", "");
            WorkContext.CategoryErMark = "";
            ViewData["CallName"] = t;
            var result_model = BLL.BLLHome.GetChildeCategory(t);
            return View(result_model);
        }

    }

}