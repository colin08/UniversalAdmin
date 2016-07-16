using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;

namespace Universal.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        // GET: Admin/Home
        [AdminPermissionAttribute("其他","后台管理首页")]
        public ActionResult Index()
        {
            return View();
        }
    }
}