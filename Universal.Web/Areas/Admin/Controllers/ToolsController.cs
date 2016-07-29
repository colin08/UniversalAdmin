using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Areas.Admin.Controllers
{
    public class ToolsController : BaseAdminController
    {
        /// <summary>
        /// 设置分页大小Cookie
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult SetPageCookie(string cname,int num)
        {
            if(!string.IsNullOrWhiteSpace(cname) && num >3)
            {
                WebHelper.SetCookie(cname, num.ToString(), 10000);
            }
            return Content("");
        }
    }
}