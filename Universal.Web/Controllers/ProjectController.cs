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
    /// 项目管理(普通用户)
    /// </summary>
    [BasicUserAuth]
    public class ProjectController : BaseHBLController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 项目基本信息
        /// </summary>
        /// <returns></returns>
        public ActionResult BasicInfo(int id)
        {
            if (id <= 0)
                return Content("项目不存在");

            return View();
        }

        /// <summary>
        /// 项目信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Info(int id)
        {
            if (id <= 0)
                return Content("项目不存在");

            return View();
        }

        /// <summary>
        /// 流程信息
        /// </summary>
        /// <returns></returns>
        public ActionResult FlowInfo(int id)
        {
            if (id <= 0)
                return Content("项目不存在");

            return View();
        }

        /// <summary>
        /// 流拆迁信息
        /// </summary>
        /// <returns></returns>
        public ActionResult FlowStage(int id)
        {
            if (id <= 0)
                return Content("项目不存在");

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

    }
}