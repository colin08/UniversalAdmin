﻿using System;
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
    public class AppVersionController : BaseHBLController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionData(int page_size, int page_index, string keyword)
        {
            BLL.BaseBLL<Entity.AppVersion> bll = new BLL.BaseBLL<Entity.AppVersion>();
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (!string.IsNullOrWhiteSpace(keyword))
                filter.Add(new BLL.FilterSearch("Content", keyword, BLL.FilterSearchContract.like));
            List<Entity.AppVersion> list = bll.GetPagedList(page_index, page_size, ref rowCount, filter, "AddTime desc");
            WebAjaxEntity<List<Entity.AppVersion>> result = new WebAjaxEntity<List<Entity.AppVersion>>();
            result.msg = 1;
            result.msgbox = CalculatePage(rowCount, page_size).ToString();
            result.data = list;
            result.total = rowCount;

            return Json(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelVersion(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.AppVersion> bll = new BLL.BaseBLL<Entity.AppVersion>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);

        }

        public ActionResult Modify(int? id)
        {
            LoadPlatform();
            int num = TypeHelper.ObjectToInt(id, 0);
            Models.ViewModelAppVersion entity = new Models.ViewModelAppVersion();
            
            if (num != 0)
            {
                BLL.BaseBLL<Entity.AppVersion> bll = new BLL.BaseBLL<Entity.AppVersion>();
                Entity.AppVersion model = bll.GetModel(p => p.ID == num);
                if (entity == null)
                {
                    entity.Msg = 2;
                }else
                {
                    entity.APPType = model.APPType;
                    entity.Content = model.Content;
                    entity.DownUrl = model.DownUrl;
                    entity.ID = model.ID;
                    entity.LinkUrl = model.LinkUrl;
                    entity.LogoImg = model.LogoImg;
                    entity.MD5 = model.MD5;
                    entity.Platforms = model.Platforms;
                    entity.Size = model.Size;
                    entity.Version = model.Version;
                    entity.VersionCode = model.VersionCode;
                }

            }
            return View(entity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Modify(Models.ViewModelAppVersion entity)
        {
            LoadPlatform();
            var isAdd = entity.ID == 0 ? true : false;

            if(((int)entity.APPType) == 0)
            {
                ModelState.AddModelError("APPType", "请选择版本类别");
            }
            if (((int)entity.Platforms) == 0)
            {
                ModelState.AddModelError("APPType", "请选择平台");
            }
            if (entity.Platforms == Entity.APPVersionPlatforms.IOS && string.IsNullOrWhiteSpace(entity.Version))
            {
                ModelState.AddModelError("Version", "IOS系统必须填写");
            }
            if (entity.Platforms == Entity.APPVersionPlatforms.IOS && string.IsNullOrWhiteSpace(entity.LinkUrl))
            {
                ModelState.AddModelError("LinkUrl", "IOS系统必须填写");
            }
            if(entity.Platforms == Entity.APPVersionPlatforms.Android && string.IsNullOrWhiteSpace(entity.DownUrl))
            {
                ModelState.AddModelError("DownUrl", "必须上传APK文件");
            }

            BLL.BaseBLL<Entity.AppVersion> bll = new BLL.BaseBLL<Entity.AppVersion>();
            if (!isAdd)
            {
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    entity.Msg = 2;
                    ModelState.AddModelError("Version", "信息不存在");
                }
            }

            if (ModelState.IsValid)
            {

                Entity.AppVersion model = null;

                if (isAdd)
                {
                    model = new Entity.AppVersion();
                }
                else
                    model = bll.GetModel(p => p.ID == entity.ID);

                model.Content = entity.Content;
                model.APPType = entity.APPType;
                model.Content = entity.Content;
                model.DownUrl = entity.DownUrl;
                model.LinkUrl = entity.LinkUrl;
                model.LogoImg = entity.LogoImg;
                model.MD5 = entity.MD5;
                model.Platforms = entity.Platforms;
                model.Size = entity.Size;
                model.Version = entity.Version;
                model.VersionCode = entity.VersionCode;
                if (isAdd)
                    bll.Add(model);
                else
                    bll.Modify(model);

                entity.Msg = 1;
            }else
            {
                entity.Msg = 3;
            }

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