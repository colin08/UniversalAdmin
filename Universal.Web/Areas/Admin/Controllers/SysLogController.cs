﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Areas.Admin.Controllers
{
    public class SysLogController : BaseAdminController
    {
        [AdminPermission("日志", "系统异常日志列表")]
        public ActionResult LogException()
        {
            BLL.BaseBLL<Entity.SysLogException> bll = new BLL.BaseBLL<Entity.SysLogException>();
            return View(bll.GetListBy(new List<BLL.FilterSearch>(), p => p.AddTime, false));
        }

        [AdminPermission("日志", "系统操作日志列表")]
        public ActionResult LogMethod(int page = 1, int type = 0, string word = "")
        {

            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Text = "所有日志", Value = "0" });
            foreach (var item in EnumHelper.EnumToDictionary(typeof(Entity.SysLogMethodType)))
            {
                string text = EnumHelper.GetDescription<Entity.SysLogMethodType>((Entity.SysLogMethodType)item.Key);
                typeList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["LogMethod_Type"] = typeList;


            word = WebHelper.UrlDecode(word);
            Models.ViewModelLogMethod response_model = new Models.ViewModelLogMethod();
            response_model.page = page;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie("logmethodindex"), SiteKey.AdminDefaultPageSize);

            int total = 0;

            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (type != 0)
                filter.Add(new BLL.FilterSearch("Type",type.ToString(),BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(word))
                filter.Add(new BLL.FilterSearch("Detail", word, BLL.FilterSearchContract.like));


            BLL.BaseBLL<Entity.SysLogMethod> bll = new BLL.BaseBLL<Entity.SysLogMethod>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, p => p.AddTime, p => p.SysUser, false);
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
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
            BLL.BaseBLL<Entity.SysLogException> bll = new BLL.BaseBLL<Entity.SysLogException>();
            if ("all".Equals(ids.ToLower()))
            {
                bll.DelBy(new List<BLL.FilterSearch>());
                AddAdminLogs(Entity.SysLogMethodType.Delete, "清空异常日志");
            }
            else
            {
                int id = TypeHelper.ObjectToInt(ids);
                List<BLL.FilterSearch> filters = new List<BLL.FilterSearch>();
                filters.Add(new BLL.FilterSearch("ID", id.ToString(), BLL.FilterSearchContract.等于));
                bll.DelBy(filters);
                AddAdminLogs(Entity.SysLogMethodType.Delete, "删除异常日志:" + id.ToString());
            }
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
            BLL.BaseBLL<Entity.SysLogMethod> bll = new BLL.BaseBLL<Entity.SysLogMethod>();
            foreach (var item in ids.Split(','))
            {
                int id = TypeHelper.ObjectToInt(item);
                bll.DelBy(p => p.ID == id);
            }
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }
    }
}