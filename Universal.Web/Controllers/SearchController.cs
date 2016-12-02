using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using System.Data.Entity;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 搜索
    /// </summary>
    public class SearchController : BaseHBLController
    {
        /// <summary>
        /// 项目
        /// </summary>
        /// <returns></returns>
        public ActionResult Project()
        {
            return View();
        }

        /// <summary>
        /// 秘籍
        /// </summary>
        /// <returns></returns>
        public ActionResult Document()
        {
            return View();
        }

        /// <summary>
        /// 节点
        /// </summary>
        /// <returns></returns>
        public ActionResult Node()
        {
            return View();
        }

        /// <summary>
        /// 公告
        /// </summary>
        /// <returns></returns>
        public ActionResult Notice()
        {
            return View();
        }

    }
}