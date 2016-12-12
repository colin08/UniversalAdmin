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
            
            view_model.TopNotice = new BLL.BaseBLL<Entity.CusNotice>().GetModel(p => p.See == Entity.DocPostSee.everyone, "AddTime DESC");
            return View(view_model);
        }

        /// <summary>
        /// 添加下载记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddDownLog(string title)
        {
            BLL.BLLCusUser.AddDownLog(WorkContext.UserInfo.ID, title);
            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);
        }

    }

}