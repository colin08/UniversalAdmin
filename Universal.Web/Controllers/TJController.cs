using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using System.Data.Entity;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    public class TJController : BaseHBLController
    {
        // GET: Msg
        public ActionResult Index()
        {
            return View();
        }
    }
}