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
    /// 
    /// </summary>
    public class NewsController : BaseAdminController
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("新闻资讯", "新闻资讯首页")]
        public ActionResult Index(int page = 1,int role=0,string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelNewsList response_model = new Models.ViewModelNewsList();
            response_model.page = page;
            response_model.role = role;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);
            Load();
            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (role > 0)
            {
                filter.Add(new BLL.FilterSearch("Type", role.ToString(), BLL.FilterSearchContract.等于));
            }
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like));
            }


            BLL.BaseBLL<Entity.News> bll = new BLL.BaseBLL<Entity.News>();
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
        [AdminPermissionAttribute("新闻资讯", "新闻资讯编辑页面")]
        public ActionResult Edit(int? id)
        {
            Load();
            BLL.BaseBLL<Entity.News> bll = new BLL.BaseBLL<Entity.News>();
            Entity.News entity = new Entity.News();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/News", "404", "Not Found", "信息不存在或已被删除", 3);
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
        [ValidateAntiForgeryToken,ValidateInput(false)]
        [AdminPermissionAttribute("新闻资讯", "保存新闻资讯编辑信息")]
        public ActionResult Edit(Entity.News entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            Load();
            BLL.BaseBLL<Entity.News> bll = new BLL.BaseBLL<Entity.News>();


            if (string.IsNullOrWhiteSpace(entity.Title))
            {
                ModelState.AddModelError("Title", "标题必填");
            }
            if(entity.Type == 0)
            {
                ModelState.AddModelError("Type", "类别必选");
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
                    return PromptView("/admin/News", "404", "Not Found", "信息不存在或已被删除", 3);
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
                    AddAdminLogs(Entity.SysLogMethodType.Add, "添加新闻资讯：" + entity.Title + "");
                }
                else //修改
                {
                    var ddd = bll.GetModel(p => p.ID == entity.ID, null);
                    ddd.Status = entity.Status;
                    ddd.Title = entity.Title;
                    ddd.Source = entity.Source;
                    ddd.Type = entity.Type;
                    ddd.Author = entity.Author;
                    ddd.Weight = entity.Weight;
                    ddd.ImgUrl = entity.ImgUrl;
                    ddd.ImgUrlBig = entity.ImgUrlBig;
                    ddd.Summary = entity.Summary;
                    ddd.Content = entity.Content;
                    ddd.AddUserID = WorkContext.UserInfo.ID;
                    ddd.LastUpdateUserID = WorkContext.UserInfo.ID;
                    ddd.LastUpdateTime = DateTime.Now;
                    bll.Modify(ddd);
                    AddAdminLogs(Entity.SysLogMethodType.Update, "修改新闻资讯：" + entity.ID.ToString() + "");
                }

                return PromptView("/admin/News", "OK", "Success", "操作成功", 3);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("新闻资讯", "删除新闻资讯")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.News> bll = new BLL.BaseBLL<Entity.News>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            AddAdminLogs(Entity.SysLogMethodType.Delete, "删除新闻资讯：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 加载用户组
        /// </summary>
        private void Load()
        {
            List<SelectListItem> dataList = new List<SelectListItem>();
            dataList.Add(new SelectListItem() { Text = "全部分类", Value = "0" });
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.NewsType)))
            {
                string text = EnumHelper.GetDescription<Entity.NewsType>((Entity.NewsType)item.Key);
                dataList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["News_role"] = dataList;
        }
    }
}