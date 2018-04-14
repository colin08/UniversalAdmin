using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;

namespace Universal.Web.Areas.MP.Controllers
{
    public class TestController : BaseMPController
    {
        // GET: MP/Test
        public ActionResult Index()
        {
            string open_id = Tools.WebHelper.GetCookie("MPOPENID");
            //if (!string.IsNullOrWhiteSpace(open_id))
            //    MPHelper.TemplateMessage.SendTestMsg(open_id, "第一个数据", "消息详细内容", "备注内容");
            return Content("用户OpenID:" + open_id);
        }
    }
}