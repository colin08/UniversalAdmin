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
    public class ProjectController : BaseHBLController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Modify()
        {
            return View();
        }

        /// <summary>
        /// 新增项目-临时
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }


        /// <summary>
        /// 项目(用户对应有权限的才会显示)
        /// </summary>
        /// <returns></returns>
        public ActionResult Some()
        {
            return View();
        }

    }
}