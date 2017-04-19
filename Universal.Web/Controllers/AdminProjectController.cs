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
    /// 项目管理（管理员）
    /// </summary>
    [BasicAdminAuth]
    public class AdminProjectController : BaseHBLController
    {
        public ActionResult Index()
        {
            ViewData["IsAdmin"] = !(BLL.BLLCusUser.CheckUserIsAdmin(WorkContext.UserInfo.ID));
            LoadNodes();
            return View();
        }

        public void LoadNodes()
        {
            BLL.BaseBLL<Entity.NodeCategory> bll = new BLL.BaseBLL<Entity.NodeCategory>();
            List<Entity.NodeCategory> list = bll.GetListBy(0, new List<BLL.FilterSearch>(), "ID ASC", true);

            ViewData["NodeCategoryList"] = list;
        }
    }
}