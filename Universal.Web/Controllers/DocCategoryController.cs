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
    public class DocCategoryController : BaseHBLController
    {
        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// 获取秘秘籍分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDocCategory(int? pid)
        {
            WebAjaxEntity<List<Models.ViewModelDepartment>> result = new WebAjaxEntity<List<Models.ViewModelDepartment>>();
            List<Models.ViewModelDepartment> list = new List<Models.ViewModelDepartment>();
            BLL.BaseBLL<Entity.DocCategory> bll = new BLL.BaseBLL<Entity.DocCategory>();
            var db_list = new List<Entity.DocCategory>();
            if (TypeHelper.ObjectToInt(pid, 0) >= 0)
                db_list = bll.GetListBy(0, p => p.PID == pid, "Priority Desc");
            else
                db_list = bll.GetListBy(0, new List<BLL.FilterSearch>(), "Priority Desc");
            foreach (var item in db_list)
            {
                Models.ViewModelDepartment model = new Models.ViewModelDepartment();
                model.department_id = item.ID;
                model.parent_id = item.PID == null ? 0 : item.PID;
                model.title = item.Title;
                list.Add(model);
            }
            result.data = list;
            result.msg = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// 添加秘籍分类
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddDocCategory(int pid, string title)
        {
            WebAjaxEntity<int> result = new WebAjaxEntity<int>();
            int re_id = BLL.BLLDocCategory.Add(pid, title);
            if (re_id <= 0)
                result.msg = 0;
            result.msg = 1;
            result.data = re_id;
            return Json(result);
        }

        /// <summary>
        /// 修改分类数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyDocCategory(int id, string title)
        {
            bool isOK = BLL.BLLDocCategory.Modify(id, title);
            if (isOK)
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "修改成功";
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = "失败，请检查数据";
            }
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Del(int id)
        {
            if (BLL.BLLDocCategory.Del(id))
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "删除成功";
                return Json(WorkContext.AjaxStringEntity);
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = "删除失败";
                return Json(WorkContext.AjaxStringEntity);
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Sort(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            string msg = "";
            if(!BLL.BLLDocCategory.Sort(ids,out msg))
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
                return Json(WorkContext.AjaxStringEntity);
            }

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }


    }
}