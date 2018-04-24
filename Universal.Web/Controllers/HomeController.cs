using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //TODO 手动设置openid
            //BLL.BLLMPUserState.SetOpenID("abcd");
            //return Redirect("/Admin/Home/");
            return Content("");
            //return View();
        }
        
        [BasicAuth]
        public ContentResult test()
        { 
            return Content("哈哈哈");
        }

    }

}