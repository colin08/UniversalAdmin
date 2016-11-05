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
    /// 秘籍
    /// </summary>
    [BasicAdminAuth]
    public class NodeController : BaseHBLController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}