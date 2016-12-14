using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using System.Data.Entity;
using Universal.Web.Framework;
using System.Threading.Tasks;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 统计
    /// </summary>
    public class StatcticsController : BaseHBLController
    {
        //面积统计
        public ActionResult Domain()
        {
            LoadSelect();
            return View();
        }

        /// <summary>
        /// 加载项目
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadProject(int year, int jidu, int area, int gz, int node_id)
        {
            WebAjaxEntity<List<BLL.Model.AdminUserRoute>> result = new WebAjaxEntity<List<BLL.Model.AdminUserRoute>>();
            result.msg = 1;
            result.data = BLL.BLLProject.GetProjectTitle(year, jidu, area, gz, node_id);
            return Json(result);
        }

        /// <summary>
        /// 面积统计图
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ActionResult> DomainCharts(string ids)
        {
            Models.ViewModelStatics result = new Models.ViewModelStatics();
            BLL.Model.Statctics model = BLL.BLLStatctics.Domain(ids);
            result.x_data = model.x_data;
            result.y_data = model.y_data;
            return PartialView("_StaticsDomain", result);

        }

        private void LoadSelect()
        {
            BLL.BaseBLL<Entity.Node> bll = new BLL.BaseBLL<Entity.Node>();
            List<Entity.Node> list = bll.GetListBy(0, new List<BLL.FilterSearch>(), "ID ASC", true);

            ViewData["NodeList"] = list;
        }
    }
}