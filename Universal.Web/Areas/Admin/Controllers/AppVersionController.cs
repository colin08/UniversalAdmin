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
        public ActionResult Index(int page = 1,int platform = 0)
        {
            LoadPlatform();

            Models.ViewModelAppVersion response_model = new Models.ViewModelAppVersion();
            response_model.page = page;
            response_model.platform = platform;

            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie("appversionindex"), SiteKey.AdminDefaultPageSize);

            var db = new DataCore.EFDBContext();
            //查询分页
            IQueryable<DataCore.Entity.AppVersion> query = db.AppVersions;
            if (platform != 0)
                query = query.Where(p => p.Platforms == (DataCore.Entity.APPVersionPlatforms)platform);

            //总数
            int total = query.Count();

            query = query.OrderByDescending(p => p.AddTime);
            query = query.Skip(response_model.page_size * (page - 1)).Take(response_model.page_size);

            response_model.DataList = query.ToList();
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            db.Dispose();
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

            using (var db = new DataCore.EFDBContext())
            {
                db.Database.ExecuteSqlCommand("delete AppVersion where ID in('" + ids + "')");
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
            using (var db = new DataCore.EFDBContext())
            {
                DataCore.Entity.AppVersion entity = new DataCore.Entity.AppVersion();
                int num = TypeHelper.ObjectToInt(id, 0);
                if (num != 0)
                {
                    entity = db.AppVersions.Find(num);
                    if (entity == null)
                        return PromptView("/admin/AppVersion", "404", "Not Found", "信息不存在或已被删除", 5);
                    if(entity.Platforms != DataCore.Entity.APPVersionPlatforms.Android)
                        return PromptView("/admin/AppVersion", "400", "数据非法", "此信息非安卓版本", 5);
                }
                
                return View(entity);
            }
        }

        [AdminPermission("App版本","保存安卓编辑信息")]
        [ValidateAntiForgeryToken,ValidateInput(false)]
        [HttpPost]
        public ActionResult EditAndroid(DataCore.Entity.AppVersion entity)
        {
            var isAdd = entity.ID == 0 ? true : false;

            var db = new DataCore.EFDBContext();
            entity.APPType = DataCore.Entity.APPVersionType.Standard;
            entity.Platforms = DataCore.Entity.APPVersionPlatforms.Android;

            //数据验证
            if (isAdd)
            {
                //判断版本是否存在
                if (db.AppVersions.Count(p => p.Platforms == DataCore.Entity.APPVersionPlatforms.Android && p.APPType == DataCore.Entity.APPVersionType.Standard && p.Version == entity.Version) > 0)
                {
                    ModelState.AddModelError("Content", "该版本存在");
                }

            }
            else
            {
                if (db.AppVersions.Count(p => p.ID == entity.ID) == 0)
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
                    db.AppVersions.Add(entity);

                }
                else //修改
                {
                    var old_entity = db.AppVersions.Find(entity.ID);
                    db.Entry(old_entity).CurrentValues.SetValues(entity);
                    
                }

                db.SaveChanges();
                db.Dispose();
                return PromptView("/admin/AppVersion", "OK", "Success", "操作成功", 5);
            }
            else
            {
                db.Dispose();
                return View(entity);
            }
        }

        /// <summary>
        /// 编辑IOS
        /// </summary>
        /// <returns></returns>
        [AdminPermissionAttribute("App版本", "IOS版本编辑页面")]
        public ActionResult EditIOS(int? id)
        {
            LoadPlatform();
            using (var db = new DataCore.EFDBContext())
            {
                DataCore.Entity.AppVersion entity = new DataCore.Entity.AppVersion();
                int num = TypeHelper.ObjectToInt(id, 0);
                if (num != 0)
                {
                    entity = db.AppVersions.Find(num);
                    if (entity == null)
                        return PromptView("/admin/AppVersion", "404", "Not Found", "信息不存在或已被删除", 5);
                    if (entity.Platforms != DataCore.Entity.APPVersionPlatforms.IOS)
                        return PromptView("/admin/AppVersion", "400", "数据非法", "此信息非IOS版本", 5);
                }

                return View(entity);
            }
        }

        [AdminPermission("App版本", "保存苹果编辑信息")]
        [ValidateAntiForgeryToken,ValidateInput(false)]
        [HttpPost]
        public ActionResult EditIOS(DataCore.Entity.AppVersion entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            LoadPlatform();
            var db = new DataCore.EFDBContext();
            entity.Platforms = DataCore.Entity.APPVersionPlatforms.IOS;
            entity.LogoImg = "null";
            entity.MD5 = "null";

            ModelState.Remove("LogoImg");
            ModelState.Remove("MD5");

            //数据验证
            if (isAdd)
            {
                //判断版本是否存在
                if (db.AppVersions.Count(p => p.Platforms == DataCore.Entity.APPVersionPlatforms.IOS &&  p.APPType == entity.APPType && p.Version == entity.Version) > 0)
                {
                    ModelState.AddModelError("Version", "该版本存在");
                }

            }
            else
            {
                if (db.AppVersions.Count(p => p.ID == entity.ID) == 0)
                {
                    return PromptView("/admin/AppVersion", "404", "Not Found", "信息不存在或已被删除", 5);
                }
            }

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    var new_ver = db.AppVersions.Where(p => p.Platforms == DataCore.Entity.APPVersionPlatforms.IOS && p.APPType == entity.APPType).OrderByDescending(p => p.VersionCode).FirstOrDefault();
                    
                    entity.VersionCode = new_ver == null ? 1 : new_ver.VersionCode + 1;
                    entity.AddTime = DateTime.Now;
                    
                    db.AppVersions.Add(entity);

                }
                else //修改
                {
                    var old_entity = db.AppVersions.Find(entity.ID);
                    db.Entry(old_entity).CurrentValues.SetValues(entity);

                }

                db.SaveChanges();
                db.Dispose();
                return PromptView("/admin/AppVersion", "OK", "Success", "操作成功", 5);
            }
            else
            {
                db.Dispose();
                return View(entity);
            }
        }

        /// <summary>
        /// 加载平台列表
        /// </summary>
        private void LoadPlatform()
        {
            List<SelectListItem> platformList = new List<SelectListItem>();
            platformList.Add(new SelectListItem() { Text = "所有平台", Value = "0" });
            foreach (var item in EnumHelper.EnumToDictionary(typeof(DataCore.Entity.APPVersionPlatforms)))
            {
                string text = EnumHelper.GetDescription<DataCore.Entity.APPVersionPlatforms>((DataCore.Entity.APPVersionPlatforms)item.Key);
                platformList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["PlatformsList"] = platformList;

            List<SelectListItem> typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Text = "所有类别", Value = "0" });
            foreach (var item in EnumHelper.EnumToDictionary(typeof(DataCore.Entity.APPVersionType)))
            {
                string text = EnumHelper.GetDescription<DataCore.Entity.APPVersionType>((DataCore.Entity.APPVersionType)item.Key);
                typeList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["TypeList"] = typeList;
        }
    }
}