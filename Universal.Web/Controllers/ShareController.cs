using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 分享
    /// </summary>
    public class ShareController : BaseHBLController
    {
        //秘籍分享
        public ActionResult doc(int id)
        {
            var entity = BLL.BLLDocument.GetModel(id);
            if (entity == null)
                return Content("");

            return View(entity);
        }
    }
}