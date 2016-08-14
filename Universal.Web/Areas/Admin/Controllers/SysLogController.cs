using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Areas.Admin.Controllers
{
    public class SysLogController : BaseAdminController
    {
        [AdminPermission("日志","系统异常日志列表")]
        public ActionResult LogException()
        {
            var db = new DataCore.EFDBContext();
            return View(db.SysLogExceptions.ToList());
        }

        [AdminPermission("日志", "系统操作日志列表")]
        public ActionResult LogMethod(int page = 1,int type= 0, string word = "")
        {

            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Text = "所有日志", Value = "0" });
            foreach (var item in EnumHelper.EnumToDictionary(typeof(DataCore.Entity.SysLogMethodType)))
            {
                string text = EnumHelper.GetDescription<DataCore.Entity.SysLogMethodType>((DataCore.Entity.SysLogMethodType)item.Key);
                typeList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["LogMethod_Type"] = typeList;


            word = WebHelper.UrlDecode(word);
            Models.ViewModelLogMethod response_model = new Models.ViewModelLogMethod();
            response_model.page = page;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie("logmethodindex"), SiteKey.AdminDefaultPageSize);
            var db = new DataCore.EFDBContext();
            
            //查询分页
            IQueryable<DataCore.Entity.SysLogMethod> query = db.SysLogMethods;
            if (type != 0)
                query = query.Where(p => p.Type == (DataCore.Entity.SysLogMethodType)type);
            if (!string.IsNullOrWhiteSpace(word))
                query = query.Where(p => p.Detail.Contains(word));

            //总数
            int total = query.Count();

            query = query.Include(p=>p.SysUser).OrderByDescending(p => p.AddTime);
            query = query.Skip(response_model.page_size * (page - 1)).Take(response_model.page_size);

            response_model.DataList = query.ToList();
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            db.Dispose();
            return View(response_model);
        }

        [HttpPost]
        [AdminPermissionAttribute("日志", "删除异常日志")]
        public JsonResult DelException(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            var db = new DataCore.EFDBContext();
            foreach (var item in ids.Split(','))
            {
                int id = TypeHelper.ObjectToInt(item);
                var entity = db.SysLogExceptions.Find(id);
                if (entity != null)
                {
                    db.SysLogExceptions.Remove(entity);
                    AddAdminLogs(db, DataCore.Entity.SysLogMethodType.Delete, "删除异常日志:" + entity.ID);
                }
            }
            db.SaveChanges();
            db.Dispose();
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        [HttpPost]
        [AdminPermissionAttribute("日志", "删除操作日志")]
        public JsonResult DelMethod(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            var db = new DataCore.EFDBContext();
            foreach (var item in ids.Split(','))
            {
                int id = TypeHelper.ObjectToInt(item);
                var entity = db.SysLogMethods.Find(id);
                if (entity != null)
                {
                    db.SysLogMethods.Remove(entity);
                    AddAdminLogs(db, DataCore.Entity.SysLogMethodType.Delete, "删除操作日志:" + entity.ID);
                }
            }
            db.SaveChanges();
            db.Dispose();
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }
    }
}