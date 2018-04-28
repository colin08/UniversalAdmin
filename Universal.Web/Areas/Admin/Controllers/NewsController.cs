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
    /// 医学通识
    /// </summary>
    public class NewsController : BaseAdminController
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role"></param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("医学通识", "医学通识首页")]
        public ActionResult Index(int page = 1, int role = 0, string word = "")
        {
            LoadCagegory();
            word = WebHelper.UrlDecode(word);
            Models.ViewModelNewsList response_model = new Models.ViewModelNewsList();
            response_model.page = page;
            response_model.word = word;
            response_model.role = role;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);
            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (role != 0)
                filter.Add(new BLL.FilterSearch("NewsCategoryID", role.ToString(), BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like));
            }


            BLL.BaseBLL<Entity.News> bll = new BLL.BaseBLL<Entity.News>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "Weight desc", "NewsCategory");
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("医学通识", "首页禁用医学通识")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            var db_ids = string.Join(",", id_list);
            BLL.BLLNews.DisEnble(db_ids);
            AddAdminLogs(Entity.SysLogMethodType.Delete, "禁用医学通识：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [AdminPermissionAttribute("医学通识", "医学通识编辑页面")]
        public ActionResult Edit(int? id)
        {
            LoadCagegory();
            BLL.BaseBLL<Entity.News> bll = new BLL.BaseBLL<Entity.News>();
            Entity.News entity = new Entity.News();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null, "NewsTags");
                if (entity == null)
                {
                    return PromptView("/admin/News", "404", "Not Found", "信息不存在或已被删除", 5);
                }

                LoadTags(entity.NewsCategoryID, entity.NewsTags.ToList());
            }
            return View(entity);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        [AdminPermissionAttribute("医学通识", "保存医学通识编辑信息")]
        public ActionResult Edit(Entity.News entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            LoadCagegory();
            BLL.BaseBLL<Entity.News> bll = new BLL.BaseBLL<Entity.News>();
            //数据验证
            if (isAdd)
            {
            }
            else
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/News", "404", "Not Found", "信息不存在或已被删除", 5);
                }

            }
            var tag_ids = Tools.WebHelper.GetFormString("hid_tag_ids");
            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    BLL.BLLNews.Add(entity, tag_ids);
                }
                else //修改
                {
                    BLL.BLLNews.Edit(entity, tag_ids);
                }

                return PromptView("/admin/News", "OK", "Success", "操作成功", 5);
            }
            else
                return View(entity);
        }

        /// <summary>
        /// 根据分类获取下面的标签
        /// </summary>
        /// <param name="category_id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoadTags(int category_id)
        {
            UnifiedResultEntity<List<Entity.ViewModel.NewsTags>> resultEntity = new UnifiedResultEntity<List<Entity.ViewModel.NewsTags>>();
            if (category_id <= 0)
            {
                resultEntity.msgbox = "请先选择分类";
                return Json(resultEntity);
            }

            var db_data = BLL.BLLNewsTag.GetListByCategoryID(category_id);
            resultEntity.msg = 1;
            resultEntity.msgbox = "ok";
            resultEntity.data = db_data;
            return Json(resultEntity);
        }

        private void LoadCagegory()
        {
            BLL.BaseBLL<Entity.NewsCategory> bll = new BLL.BaseBLL<Entity.NewsCategory>();
            var db_list = bll.GetListBy(0, p => p.Status, "Weight DESC");
            List<SelectListItem> dataList = new List<SelectListItem>();
            dataList.Add(new SelectListItem() { Text = "选择类别", Value = "0" });
            foreach (var item in db_list)
            {
                dataList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString() });
            }
            ViewData["CategoryList"] = dataList;
        }

        /// <summary>
        /// 加载标签数据
        /// </summary>
        private void LoadTags(int category_id, List<Entity.NewsTag> select_list)
        {
            if (category_id > 0)
            {
                var db_ca_list = BLL.BLLNewsTag.GetListByCategoryID(category_id);
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                stringBuilder.Append("<option value=\"\"></option>");
                foreach (var item in db_ca_list)
                {
                    if (select_list != null)
                    {
                        if(select_list.Where(p=>p.ID == item.ID).FirstOrDefault() != null)
                        {
                            stringBuilder.Append("<option value=\"" + item.ID + "\" selected>" + item.Title + "</option>");
                        }
                        else
                            stringBuilder.Append("<option value=\"" + item.ID + "\">" + item.Title + "</option>");
                    }
                    else
                        stringBuilder.Append("<option value=\"" + item.ID + "\">" + item.Title + "</option>");
                }
                ViewData["DefaultOption"] = stringBuilder.ToString();
            }
            if (select_list != null)
            {
                System.Text.StringBuilder builder_ID = new System.Text.StringBuilder();
                //System.Text.StringBuilder builder_Title = new System.Text.StringBuilder();
                foreach (var item in select_list)
                {
                    builder_ID.Append(item.ID + ",");
                    //builder_Title.Append(item.Title + ",");
                }
                if (builder_ID.Length > 0)
                {
                    builder_ID.Remove(builder_ID.Length - 1, 1);
                    //builder_Title.Remove(builder_Title.Length - 1, 1);
                }
                ViewData["TagSelectID"] = builder_ID.ToString();
                //ViewData["TagSelectTitle"] = builder_Title.ToString();
            }
        }

    }
}