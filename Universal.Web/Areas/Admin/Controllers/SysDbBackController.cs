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
    /// <summary>
    /// 数据库备份
    /// </summary>
    public class SysDbBackController : BaseAdminController
    {

        [AdminPermissionAttribute("数据库", "数据库管理首页")]
        public ActionResult Index(int page = 1, string word = "", int type = 0)
        {
            LoadType();

            word = WebHelper.UrlDecode(word);
            Models.ViewModelSysDbBack response_model = new Models.ViewModelSysDbBack();
            response_model.page = page;
            response_model.word = word;
            response_model.type = type;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);

            List<BLL.FilterSearch> filters = new List<BLL.FilterSearch>();
            if (type != 0)
            {
                filters.Add(new BLL.FilterSearch("BackType", type.ToString(), BLL.FilterSearchContract.等于));
            }
            if (!string.IsNullOrWhiteSpace(word))
            {
                filters.Add(new BLL.FilterSearch("DbName", word, BLL.FilterSearchContract.like));
                filters.Add(new BLL.FilterSearch("BackName", word, BLL.FilterSearchContract.like));
            }
            int total = 0;
            BLL.BaseBLL<Entity.SysDbBack> bll = new BLL.BaseBLL<Entity.SysDbBack>();
            List<Entity.SysDbBack> list = bll.GetPagedList(page, response_model.page_size, ref total, filters, "AddTime desc");

            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        [AdminPermissionAttribute("数据库", "删除备份的数据库")]
        [HttpPost]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BLLSysDbBack bll = new BLL.BLLSysDbBack();
            string del_file = "";
            string msg = "";
            var is_ok = bll.Del(ids, out msg, out del_file);
            if (is_ok)
            {
                AddAdminLogs(Entity.SysLogMethodType.Delete, "删除备份的数据库：" + del_file);
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "删除成功";
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
            }

            return Json(WorkContext.AjaxStringEntity);
        }

        [AdminPermission("数据库", "新增备份页面")]
        public ActionResult Add()
        {
            LoadType();
            Entity.SysDbBack entity = new Entity.SysDbBack();
            entity.BackName = DateTime.Now.ToString("yyyyMMdd") + WebHelper.GenerateRandomIntNumber(4);
            return View(entity);
        }

        [AdminPermission("数据库", "保存新增备份")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(Entity.SysDbBack entity)
        {
            LoadType();
            if (entity.BackType == 0)
                ModelState.AddModelError("BackType", "备份类别必选");
            if (ModelState.IsValid)
            {
                string msg = "";
                BLL.BLLSysDbBack bll = new BLL.BLLSysDbBack();
                entity.AddTime = DateTime.Now;
                entity.LastUpdateTime = DateTime.Now;
                entity.AddUserID = WorkContext.UserInfo.ID;
                entity.LastUpdateUserID = WorkContext.UserInfo.ID;
                bool is_ok = bll.BackDb(entity, out msg);
                if (is_ok)
                {
                    AddAdminLogs(Entity.SysLogMethodType.Add, "新增备份的数据库：" + entity.FilePath);
                    return PromptView("/admin/SysDbBack", "OK", "Success", "备份成功", 5);
                }
                else
                    return PromptView("/admin/SysDbBack", "404", "Error", msg, 10);

            }
            return View(entity);
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminPermission("数据库", "备份的详情信息")]
        public ActionResult Info(int id)
        {
            BLL.BaseBLL<Entity.SysDbBack> bll = new BLL.BaseBLL<Entity.SysDbBack>();
            var entity = bll.GetModel(p => p.ID == id, null,"AddUser");
            if (entity == null)
                return PromptView("/admin/SysDbBack", "404", "Not Found", "信息不存在或已被删除", 5);

            return View(entity);
        }

        /// <summary>
        /// 数据库还原
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminPermission("数据库", "还原数据库")]
        [HttpPost]
        public JsonResult Restore(int id)
        {
            BLL.BaseBLL<Entity.SysDbBack> bll = new BLL.BaseBLL<Entity.SysDbBack>();
            var entity = bll.GetModel(p => p.ID == id, null);
            if (entity == null)
                WorkContext.AjaxStringEntity.msgbox = "备份信息不存在";
            else
            {
                string msg = "";
                bool is_ok = new BLL.BLLSysDbBack().RestoreDb(entity, out msg);
                if (is_ok)
                {
                    AddAdminLogs(Entity.SysLogMethodType.Resotre, "还原数据库：" + entity.FilePath);
                    WorkContext.AjaxStringEntity.msg = 1;
                    WorkContext.AjaxStringEntity.msgbox = "还原成功";
                }
                else
                    WorkContext.AjaxStringEntity.msgbox = msg;
            }

            return Json(WorkContext.AjaxStringEntity);
        }


        /// <summary>
        /// 加载平台列表
        /// </summary>
        private void LoadType()
        {
            List<SelectListItem> typeList = new List<SelectListItem>();
            //typeList.Add(new SelectListItem() { Text = "所有类别", Value = "0" });
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.SysDbBackType)))
            {
                string text = EnumHelper.GetDescription((Entity.SysDbBackType)item.Key);
                typeList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["TypeList"] = typeList;

            BLL.BLLSysDbBack bll = new BLL.BLLSysDbBack();
            var db_list = bll.GetSysDbList();
            List<SelectListItem> dbList = new List<SelectListItem>();
            dbList.Add(new SelectListItem() { Text = "请选择数据库", Value = "" });
            foreach (var item in db_list)
            {
                dbList.Add(new SelectListItem() { Text = item.name, Value = item.name });
            }
            ViewData["dbList"] = dbList;

        }
    }
}