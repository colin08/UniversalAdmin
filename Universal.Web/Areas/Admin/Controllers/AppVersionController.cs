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
    public class AppVersionController : BaseAdminController
    {
        [AdminPermissionAttribute("APP版本", "APP版本首页")]
        public ActionResult Index(int page = 1, int platform = 0)
        {
            LoadPlatform();

            Models.ViewModelAppVersion response_model = new Models.ViewModelAppVersion();
            response_model.page = page;
            response_model.platform = platform;

            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);

            List<BLL.FilterSearch> filters = new List<BLL.FilterSearch>();
            if (platform != 0)
                filters.Add(new BLL.FilterSearch("Platforms", platform.ToString(), BLL.FilterSearchContract.等于));
            int total = 0;
            BLL.BaseBLL<Entity.AppVersion> bll = new BLL.BaseBLL<Entity.AppVersion>();
            List<Entity.AppVersion> list = bll.GetPagedList(page, response_model.page_size, ref total, filters, "AddTime desc");
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        [HttpPost]
        [AdminPermission("App版本", "删除版本")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BaseBLL<Entity.AppVersion> bll = new BLL.BaseBLL<Entity.AppVersion>();
            foreach (var item in ids.Split(','))
            {
                int id = TypeHelper.ObjectToInt(item);
                bll.DelBy(p => p.ID == id);
            }

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 编辑安卓
        /// </summary>
        /// <returns></returns>
        [AdminPermissionAttribute("App版本", "安卓版本编辑页面")]
        public ActionResult EditAndroid(int? id)
        {
            int num = TypeHelper.ObjectToInt(id, 0);
            Entity.AppVersion entity = new Entity.AppVersion();
            if (num != 0)
            {
                BLL.BaseBLL<Entity.AppVersion> bll = new BLL.BaseBLL<Entity.AppVersion>();
                entity = bll.GetModel(p => p.ID == num,null);
                if (entity == null)
                    return PromptView("/admin/AppVersion", "404", "Not Found", "信息不存在或已被删除", 5);
                if (entity.Platforms != Entity.APPVersionPlatforms.Android)
                    return PromptView("/admin/AppVersion", "400", "数据非法", "此信息非安卓版本", 5);

            }
            return View(entity);
        }

        [AdminPermission("App版本", "保存安卓编辑信息")]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        [HttpPost]
        public ActionResult EditAndroid(Entity.AppVersion entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            BLL.BaseBLL<Entity.AppVersion> bll = new BLL.BaseBLL<Entity.AppVersion>();
            
            entity.APPType = Entity.APPVersionType.Standard;
            entity.Platforms = Entity.APPVersionPlatforms.Android;

            //数据验证
            if (isAdd)
            {
                //判断版本是否存在
                if (bll.Exists(p => p.Platforms == Entity.APPVersionPlatforms.Android && p.APPType == Entity.APPVersionType.Standard && p.Version == entity.Version))
                {
                    ModelState.AddModelError("Content", "该版本存在");
                }

            }
            else
            {
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/AppVersion", "404", "Not Found", "信息不存在或已被删除", 5);
                }
            }

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    entity.AddTime = DateTime.Now;
                    bll.Add(entity);

                }
                else //修改
                    bll.Modify(entity);
                
                return PromptView("/admin/AppVersion", "OK", "Success", "操作成功", 5);
            }
            else
                return View(entity);
        }

        /// <summary>
        /// 编辑IOS
        /// </summary>
        /// <returns></returns>
        [AdminPermissionAttribute("App版本", "IOS版本编辑页面")]
        public ActionResult EditIOS(int? id)
        {
            LoadPlatform();
            Entity.AppVersion entity = new Entity.AppVersion();
            int num = TypeHelper.ObjectToInt(id, 0);
            BLL.BaseBLL<Entity.AppVersion> bll = new BLL.BaseBLL<Entity.AppVersion>();

            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                    return PromptView("/admin/AppVersion", "404", "Not Found", "信息不存在或已被删除", 5);
                if (entity.Platforms != Entity.APPVersionPlatforms.IOS)
                    return PromptView("/admin/AppVersion", "400", "数据非法", "此信息非IOS版本", 5);
            }

            return View(entity);
        }

        [AdminPermission("App版本", "保存苹果编辑信息")]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        [HttpPost]
        public ActionResult EditIOS(Entity.AppVersion entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            LoadPlatform();
            BLL.BaseBLL<Entity.AppVersion> bll = new BLL.BaseBLL<Entity.AppVersion>();

            entity.Platforms = Entity.APPVersionPlatforms.IOS;
            entity.LogoImg = "null";
            entity.MD5 = "null";

            ModelState.Remove("LogoImg");
            ModelState.Remove("MD5");



            //数据验证
            if (isAdd)
            {
                //判断版本是否存在
                if (bll.Exists(p => p.Platforms == Entity.APPVersionPlatforms.IOS && p.APPType == entity.APPType && p.Version == entity.Version))
                {
                    ModelState.AddModelError("Version", "该版本存在");
                }

            }
            else
            {
                if (bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/AppVersion", "404", "Not Found", "信息不存在或已被删除", 5);
                }
            }

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    var new_ver = bll.GetModel(p => p.Platforms == Entity.APPVersionPlatforms.IOS && p.APPType == entity.APPType, "VersionCode desc");
                    entity.VersionCode = new_ver == null ? 1 : new_ver.VersionCode + 1;
                    entity.AddTime = DateTime.Now;

                    bll.Add(entity);

                }
                else //修改
                {
                    //var old_entity = db.AppVersions.Find(entity.ID);
                    //db.Entry(old_entity).CurrentValues.SetValues(entity);
                    bll.Modify(entity);
                }
                return PromptView("/admin/AppVersion", "OK", "Success", "操作成功", 5);
            }
            else
                return View(entity);
        }

        /// <summary>
        /// 加载平台列表
        /// </summary>
        private void LoadPlatform()
        {
            List<SelectListItem> platformList = new List<SelectListItem>();
            platformList.Add(new SelectListItem() { Text = "所有平台", Value = "0" });
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.APPVersionPlatforms)))
            {
                string text = EnumHelper.GetDescription<Entity.APPVersionPlatforms>((Entity.APPVersionPlatforms)item.Key);
                platformList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["PlatformsList"] = platformList;

            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Text = "所有类别", Value = "0" });
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.APPVersionType)))
            {
                string text = EnumHelper.GetDescription<Entity.APPVersionType>((Entity.APPVersionType)item.Key);
                typeList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["TypeList"] = typeList;
        }
    }
}