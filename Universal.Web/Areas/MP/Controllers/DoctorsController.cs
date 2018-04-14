using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;

namespace Universal.Web.Areas.MP.Controllers
{
    /// <summary>
    /// 医生端首页
    /// </summary>
    [OnlyDoctors]
    public class DoctorsController : BaseMPController
    {
        // GET: MP/Doctors
        public ActionResult Index()
        {
            return Content("医生");
        }
    }
}