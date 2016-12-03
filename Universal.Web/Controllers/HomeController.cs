using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    public class HomeController : BaseHBLController
    {
        public ActionResult Index()
        {
            Models.ViewModelIndex view_model = new Models.ViewModelIndex();
            int rowCount = 0;
            view_model.DocumentList = BLL.BLLDocument.GetPowerPageData(1, 5, ref rowCount, WorkContext.UserInfo.ID, "", 0);
            return View(view_model);
        }
    }

}