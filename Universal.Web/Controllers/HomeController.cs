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
            //Tools.Crypto3DES des = new Tools.Crypto3DES(SiteKey.DES3KEY);
            //var ss = des.DESEnCode("1");
            return Content("");
        }
        
        [BasicAuth]
        public ContentResult test()
        { 
            return Content("哈哈哈");
        }

    }

}