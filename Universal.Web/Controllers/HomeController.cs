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
            //BLL.BLLMPUserState.SetOpenID("oHP6jw6WoaY5MyVOBUfuRxSnWHlg");
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