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
    /// <summary>
    /// 医生特长标签云
    /// </summary>
    public class DoctorsSpecialtyController : BaseAdminController
    {
        /// <summary>
        /// 标签云首页
        /// </summary>
        /// <returns></returns>
        [AdminPermissionAttribute("医生特长标签", "标签首页")]
        public ActionResult Index()
        {
            List<Models.ViewModelCloudTags> list_result = new List<Models.ViewModelCloudTags>();
            BLL.BaseBLL<Entity.DoctorsSpecialty> bll = new BLL.BaseBLL<Entity.DoctorsSpecialty>();
            var db_list = bll.GetListBy(0, p => p.ID > 0, "AddTime DESC");
            foreach (var item in db_list)
            {
                list_result.Add(new Models.ViewModelCloudTags(item.Title, item.ID));
            }
            string json = JsonHelper.ToJson(list_result);
            ViewData["JSON"] = json;
            return View();
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminPermission("医生特长标签", "添加标签")]
        public JsonResult Add(string title)
        {
            BLL.BaseBLL<Entity.DoctorsSpecialty> bll = new BLL.BaseBLL<Entity.DoctorsSpecialty>();
            var entity = bll.GetModel(p => p.Title == title, "ID ASC");
            if (entity != null)
            {
                WorkContext.AjaxStringEntity.msgbox = "【" + title + "】标签已存在";
                return Json(WorkContext.AjaxStringEntity);
            }
            entity = new Entity.DoctorsSpecialty();
            var pinyin = Hz2Py.ConvertToPin(title);
            if (!string.IsNullOrWhiteSpace(pinyin))
            {
                entity.SZM = pinyin.Substring(0, 1);
            }
            else
            {
                entity.SZM = "&";
            }
            entity.Title = title;
            bll.Add(entity);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminPermission("医生特长标签", "修改标签")]
        public JsonResult Modify(int id,string title)
        {
            BLL.BaseBLL<Entity.DoctorsSpecialty> bll = new BLL.BaseBLL<Entity.DoctorsSpecialty>();
            var entity = bll.GetModel(p => p.ID == id, "ID ASC");
            if (entity == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "该标签不存在";
                return Json(WorkContext.AjaxStringEntity);
            }
            if(bll.Exists(p => p.ID != id && p.Title == title))
            {
                WorkContext.AjaxStringEntity.msgbox = "【" + title + "】标签已存在";
                return Json(WorkContext.AjaxStringEntity);
            }

            entity.Title = title;
            bll.Modify(entity, "Title");
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return Json(WorkContext.AjaxStringEntity);
        }

    }
}