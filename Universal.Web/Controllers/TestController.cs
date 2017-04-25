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
    /// 测试
    /// </summary>
    public class TestController : BaseHBLController
    {
        // GET: Test
        public ActionResult JPush(string alias)
        {
            Tools.JPush.PushALl(alias, "后台手动推送" + alias, 2, "4");

            return Content("已推送");
        }
    }
}