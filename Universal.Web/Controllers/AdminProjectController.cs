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
    /// 项目管理（管理员）
    /// </summary>
    [BasicAdminAuth]
    public class AdminProjectController : BaseHBLController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}