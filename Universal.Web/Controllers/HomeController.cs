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
        
    }

}