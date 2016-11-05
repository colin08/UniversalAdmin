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
    /// 部门
    /// </summary>
    [BasicAdminAuth]
    public class DepartmentController : BaseHBLController
    {
        /// <summary>
        /// 部门管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<Models.ViewModelDepartment> response_entity = new List<Models.ViewModelDepartment>();
            BLL.BaseBLL<Entity.CusDepartment> bll = new BLL.BaseBLL<Entity.CusDepartment>();
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            List<Entity.CusDepartment> db_list = bll.GetListBy(0, filter, "Priority desc", p => p.DepartmentAdminUsers, true);
            foreach (var item in db_list)
            {
                Models.ViewModelDepartment model = new Models.ViewModelDepartment();
                model.department_id = item.ID;
                model.parent_id = item.PID == null ? 0 : item.PID;
                model.title = item.Title;
                List<Models.ViewModelDepartmentAdmin> admin_list = new List<Models.ViewModelDepartmentAdmin>();
                foreach (var ad_list in item.DepartmentAdminUsers)
                {
                    Models.ViewModelDepartmentAdmin admin_model = new Models.ViewModelDepartmentAdmin();
                    admin_model.user_id = ad_list.ID;
                    admin_model.name = ad_list.NickName;
                    admin_list.Add(admin_model);
                }

                model.admin_list = admin_list;
                response_entity.Add(model);
            }

            ViewData["response_model"] = Newtonsoft.Json.JsonConvert.SerializeObject(response_entity);
            return View();
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(int pid, string title, string user_ids)
        {
            if (pid <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "父级ID错误";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                WorkContext.AjaxStringEntity.msgbox = "部门不能为空";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (BLL.BLLDepartment.Add(pid, title, user_ids))
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "添加成功";
                return Json(WorkContext.AjaxStringEntity);
            }
            WorkContext.AjaxStringEntity.msgbox = "添加失败";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Modify(int id, string title, string user_ids)
        {
            if (id <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "部门错误";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                WorkContext.AjaxStringEntity.msgbox = "部门不能为空";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (BLL.BLLDepartment.Modify(id, title, user_ids))
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "修改成功";
                return Json(WorkContext.AjaxStringEntity);
            }
            WorkContext.AjaxStringEntity.msgbox = "修改失败";
            return Json(WorkContext.AjaxStringEntity);
        }
    }
}