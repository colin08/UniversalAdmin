using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;
using System.Data.Entity;

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
            return View();
        }

        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDeparment(int? pid)
        {
            WebAjaxEntity<List<Models.ViewModelDepartment>> result = new WebAjaxEntity<List<Models.ViewModelDepartment>>();
            List<Models.ViewModelDepartment> list = new List<Models.ViewModelDepartment>();
            BLL.BaseBLL<Entity.CusDepartment> bll = new BLL.BaseBLL<Entity.CusDepartment>();
            var db_list = new List<Entity.CusDepartment>();
            if (TypeHelper.ObjectToInt(pid, 0) >= 0)
                db_list = bll.GetListBy(0, p => p.PID == pid, "Priority Desc", p => p.CusDepartmentAdmins.Select(s => s.CusUser));
            else
                db_list = bll.GetListBy(0, new List<BLL.FilterSearch>(), "Priority Desc", p => p.CusDepartmentAdmins.Select(s => s.CusUser));
            foreach (var item in db_list)
            {
                Models.ViewModelDepartment model = new Models.ViewModelDepartment();
                model.department_id = item.ID;
                model.parent_id = item.PID == null ? 0 : item.PID;
                model.title = item.Title;
                List<Models.ViewModelDepartmentAdmin> admin_list = new List<Models.ViewModelDepartmentAdmin>();
                foreach (var ad_list in item.CusDepartmentAdmins)
                {
                    if (ad_list.CusUser != null)
                    {
                        Models.ViewModelDepartmentAdmin admin_model = new Models.ViewModelDepartmentAdmin();

                        admin_model.user_id = ad_list.CusUserID;
                        admin_model.name = ad_list.CusUser.NickName;
                        admin_list.Add(admin_model);

                    }
                }

                model.admin_list = admin_list;
                list.Add(model);
            }
            result.data = list;
            result.msg = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加部门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(int pid, string title, string user_ids)
        {
            if (pid < 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "父级ID错误";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (!BLL.BLLCusRoute.CheckUserAuthority(BLL.CusRouteType.部门管理, WorkContext.UserInfo.ID))
            {
                WorkContext.AjaxStringEntity.msgbox = "没有权限操作";
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
            if (!BLL.BLLCusRoute.CheckUserAuthority(BLL.CusRouteType.部门管理, WorkContext.UserInfo.ID))
            {
                WorkContext.AjaxStringEntity.msgbox = "没有权限操作";
                return Json(WorkContext.AjaxStringEntity);
            }

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

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Del(int id)
        {
            if (!BLL.BLLCusRoute.CheckUserAuthority(BLL.CusRouteType.部门管理, WorkContext.UserInfo.ID))
            {
                WorkContext.AjaxStringEntity.msgbox = "没有权限操作";
                return Json(WorkContext.AjaxStringEntity);
            }

            string msg = "";
            bool is_ok = BLL.BLLDepartment.Del(id, out msg);
            WorkContext.AjaxStringEntity.msg = is_ok ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity);
        }

    }
}