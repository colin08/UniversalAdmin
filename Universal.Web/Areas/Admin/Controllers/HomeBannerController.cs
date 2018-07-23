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
    /// 首页Banner
    /// </summary>
    public class HomeBannerController : BaseAdminController
    {
        /// <summary>
        /// 用户分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("首页轮播图", "首页轮播图首页")]
        public ActionResult Index(int page = 1,string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelHomeBannerList response_model = new Models.ViewModelHomeBannerList();
            response_model.page = page;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);
            
            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like));
            }


            BLL.BaseBLL<Entity.HomeBanner> bll = new BLL.BaseBLL<Entity.HomeBanner>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "Weight desc");
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [AdminPermissionAttribute("首页轮播图", "首页轮播图编辑页面")]
        public ActionResult Edit(int? id)
        {
            Load();
            BLL.BaseBLL<Entity.HomeBanner> bll = new BLL.BaseBLL<Entity.HomeBanner>();
            Entity.HomeBanner entity = new Entity.HomeBanner();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/HomeBanner", "404", "Not Found", "信息不存在或已被删除", 3);
                }
            }
            return View(entity);
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminPermissionAttribute("首页轮播图", "保存首页轮播图首页编辑信息")]
        public ActionResult Edit(Entity.HomeBanner entity)
        {
            var isAdd = entity.ID == 0 ? true : false;

            BLL.BaseBLL<Entity.HomeBanner> bll = new BLL.BaseBLL<Entity.HomeBanner>();

            Load();

            if(entity.LinkType == 0)
            {
                ModelState.AddModelError("LinkType", "事件目标必选");
            }
            if (string.IsNullOrWhiteSpace(entity.Title))
            {
                ModelState.AddModelError("Title", "标题必填");
            }

            //数据验证
            if (isAdd)
            {

            }
            else
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/HomeBanner", "404", "Not Found", "信息不存在或已被删除", 3);
                }
            }

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    entity.AddUserID = WorkContext.UserInfo.ID;
                    entity.LastUpdateUserID = WorkContext.UserInfo.ID;
                    bll.Add(entity);
                    AddAdminLogs(Entity.SysLogMethodType.Add, "添加首页轮播图：" + entity.Title + "");
                }
                else //修改
                {
                    var ddd = bll.GetModel(p => p.ID == entity.ID, null);
                    ddd.Status = entity.Status;
                    ddd.Title = entity.Title;
                    ddd.Weight = entity.Weight;
                    ddd.ImgUrl = entity.ImgUrl;
                    ddd.Remark = entity.Remark;
                    ddd.LinkType = entity.LinkType;
                    ddd.LinkVal = entity.LinkVal;
                    entity.AddUserID = WorkContext.UserInfo.ID;
                    entity.LastUpdateUserID = WorkContext.UserInfo.ID;
                    bll.Modify(ddd);
                    AddAdminLogs(Entity.SysLogMethodType.Update, "修改首页轮播图：" + entity.ID.ToString() + "");
                }

                return PromptView("/admin/HomeBanner", "OK", "Success", "操作成功", 3);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("首页轮播图", "删除首页轮播图")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.HomeBanner> bll = new BLL.BaseBLL<Entity.HomeBanner>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            AddAdminLogs(Entity.SysLogMethodType.Delete, "删除首页轮播图：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        private void Load()
        {
            List<SelectListItem> dataList = new List<SelectListItem>();
            dataList.Add(new SelectListItem() { Text = "选择事件类别", Value = "0" });
            foreach (var item in EnumHelper.EnumToDictionary(typeof(Entity.HomeBannerLinkType)))
            {
                string text = EnumHelper.GetDescription<Entity.HomeBannerLinkType>((Entity.HomeBannerLinkType)item.Key);
                dataList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["LinkTypeList"] = dataList;
        }

    }
}