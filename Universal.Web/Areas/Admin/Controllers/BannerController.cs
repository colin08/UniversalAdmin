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
    public class BannerController : BaseAdminController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cid"></param>
        /// <param name="ctitle"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        [AdminPermissionAttribute("轮播图", "轮播图首页")]
        public ActionResult Index(int page = 1,int cid=0,string ctitle="",string word = "")
        {
            if(cid <= 0) return PromptView("/admin/Category", "404", "Not Found", "非法分类参数", 3);
            ViewData["CID"] = cid;
            ViewData["CTitle"] = ctitle.Replace(" ", "").Replace("├", "");
            word = WebHelper.UrlDecode(word);
            Models.ViewModelBannerList response_model = new Models.ViewModelBannerList();
            response_model.page = page;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);
            
            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            filter.Add(new BLL.FilterSearch("CategoryID", cid.ToString(), BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like));
            }


            BLL.BaseBLL<Entity.Banner> bll = new BLL.BaseBLL<Entity.Banner>();
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
        [AdminPermissionAttribute("轮播图", "轮播图编辑页面")]
        public ActionResult Edit(int? id,int cid=0,string ctitle="")
        {
            if (cid <= 0) return PromptView("/admin/Category", "404", "Not Found", "非法分类参数", 3);
            ViewData["CID"] = cid;
            ViewData["CTitle"] = ctitle;
            Load();
            BLL.BaseBLL<Entity.Banner> bll = new BLL.BaseBLL<Entity.Banner>();
            Entity.Banner entity = new Entity.Banner();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/Banner/Index?page=1&cid=" + cid + "&ctitle=" + ctitle, "404", "Not Found", "信息不存在或已被删除", 3);
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
        [AdminPermissionAttribute("轮播图", "保存轮播图首页编辑信息")]
        public ActionResult Edit(Entity.Banner entity, int cid = 0, string ctitle = "")
        {
            if (cid <= 0) return PromptView("/admin/Category", "404", "Not Found", "非法分类参数", 3);
            ViewData["CID"] = cid;
            ViewData["CTitle"] = ctitle;

            var isAdd = entity.ID == 0 ? true : false;

            BLL.BaseBLL<Entity.Banner> bll = new BLL.BaseBLL<Entity.Banner>();

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
                    return PromptView("/admin/Banner/Index?page=1&cid=" + cid + "&ctitle=" + ctitle, "404", "Not Found", "信息不存在或已被删除", 3);
                }
            }

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    entity.AddUserID = WorkContext.UserInfo.ID;
                    entity.LastUpdateUserID = WorkContext.UserInfo.ID;
                    entity.CategoryID = cid;
                    bll.Add(entity);
                    AddAdminLogs(Entity.SysLogMethodType.Add, "添加轮播图：" + entity.Title + "");
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
                    AddAdminLogs(Entity.SysLogMethodType.Update, "修改轮播图：" + entity.ID.ToString() + "");
                }

                return PromptView("/admin/Banner/Index?page=1&cid=" + cid + "&ctitle=" + ctitle, "OK", "Success", "操作成功", 3);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("轮播图", "删除轮播图")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.Banner> bll = new BLL.BaseBLL<Entity.Banner>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            AddAdminLogs(Entity.SysLogMethodType.Delete, "删除轮播图：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        private void Load()
        {
            List<SelectListItem> dataList = new List<SelectListItem>();
            dataList.Add(new SelectListItem() { Text = "选择事件类别", Value = "0" });
            foreach (var item in EnumHelper.EnumToDictionary(typeof(Entity.BannerLinkType)))
            {
                string text = EnumHelper.GetDescription<Entity.BannerLinkType>((Entity.BannerLinkType)item.Key);
                dataList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["LinkTypeList"] = dataList;
        }

    }
}