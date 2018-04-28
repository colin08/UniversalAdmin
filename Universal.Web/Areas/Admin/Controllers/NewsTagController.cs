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
    /// 医学通识标签云
    /// </summary>
    public class NewsTagController : BaseAdminController
    {
        /// <summary>
        /// 标签云首页
        /// </summary>
        /// <returns></returns>
        [AdminPermissionAttribute("医学通识标签", "标签首页")]
        public ActionResult Index(int role = 0)
        {
            Models.ViewModelNewsTagList viewModelNewsTagList = new Models.ViewModelNewsTagList();
            viewModelNewsTagList.role = role;
            List<Models.ViewModelNewsTags> list_result = new List<Models.ViewModelNewsTags>();
            var db_list = BLL.BLLNewsTag.GetListByCategoryID(role);
            foreach (var item in db_list)
            {
                list_result.Add(new Models.ViewModelNewsTags(item.Title, item.ID));
            }
            string json = JsonHelper.ToJson(list_result);
            ViewData["JSON"] = json;

            LoadCategory();

            return View(viewModelNewsTagList);
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="category_id"></param>
        /// <returns></returns>
        [AdminPermission("医学通识标签", "添加标签")]
        [HttpGet]
        public ActionResult Modify(int id = 0)
        {
            List<int> select_ids = new List<int>();

            Entity.NewsTag entity = new Entity.NewsTag();
            if (id > 0)
            {
                entity = BLL.BLLNewsTag.GetModel(id, out select_ids);
                if (entity == null) return PromptView("Error", "标签不存在", "");
            }
            ViewData["CategoryIDs"] = string.Join(",", select_ids.ToArray());
            LoadCategory(select_ids);
            return View(entity);
        }

        /// <summary>
        /// 添加标签json
        /// </summary>
        /// <param name="category_id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Modify(int id, string category_ids, string title,int weight)
        {
            if (string.IsNullOrWhiteSpace(category_ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "请选择分类";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                WorkContext.AjaxStringEntity.msgbox = "请填写标签名";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (id == 0)
            {
                var rseult = BLL.BLLNewsTag.AddTags(title, weight, category_ids);
                if (rseult > 0)
                {
                    WorkContext.AjaxStringEntity.msg = 1;
                    WorkContext.AjaxStringEntity.msgbox="标签添加成功";
                    return Json(WorkContext.AjaxStringEntity);
                }
                else
                {
                    WorkContext.AjaxStringEntity.msgbox = "标签添加失败";
                    return Json(WorkContext.AjaxStringEntity);
                }
            }
            else
            {
                var status = BLL.BLLNewsTag.Modify(id, title, weight, category_ids);
                if (status)
                {
                    WorkContext.AjaxStringEntity.msg = 1;
                    WorkContext.AjaxStringEntity.msgbox = "标签修改成功";
                    return Json(WorkContext.AjaxStringEntity);
                }
                else
                {
                    WorkContext.AjaxStringEntity.msgbox = "标签修改失败";
                    return Json(WorkContext.AjaxStringEntity);
                }
            }
        }


        ///// <summary>
        ///// 添加标签
        ///// </summary>
        ///// <param name="title"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[AdminPermission("医学通识标签", "修改标签")]
        //public JsonResult Modify(int id,string title)
        //{
        //    BLL.BaseBLL<Entity.DoctorsSpecialty> bll = new BLL.BaseBLL<Entity.DoctorsSpecialty>();
        //    var entity = bll.GetModel(p => p.ID == id, "ID ASC");
        //    if (entity == null)
        //    {
        //        WorkContext.AjaxStringEntity.msgbox = "该标签不存在";
        //        return Json(WorkContext.AjaxStringEntity);
        //    }
        //    if(bll.Exists(p => p.ID != id && p.Title == title))
        //    {
        //        WorkContext.AjaxStringEntity.msgbox = "【" + title + "】标签已存在";
        //        return Json(WorkContext.AjaxStringEntity);
        //    }

        //    entity.Title = title;
        //    bll.Modify(entity, "Title");
        //    WorkContext.AjaxStringEntity.msg = 1;
        //    WorkContext.AjaxStringEntity.msgbox = "ok";
        //    return Json(WorkContext.AjaxStringEntity);
        //}

        /// <summary>
        /// 加载多选的分类
        /// </summary>
        /// <param name="select_ids"></param>
        private void LoadCategory(List<int> select_ids)
        {
            if (select_ids == null) select_ids = new List<int>();
            BLL.BaseBLL<Entity.NewsCategory> bll = new BLL.BaseBLL<Entity.NewsCategory>();
            var db_list = bll.GetListBy(0, p => p.Status, "Weight DESC");
            List<SelectListItem> dataList = new List<SelectListItem>();
            foreach (var item in db_list)
            {
                if (select_ids.Contains(item.ID))
                    dataList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString(), Selected = true });
                else
                    dataList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString() });
            }
            ViewData["CategoryList"] = dataList;
        }

        /// <summary>
        /// 加载多选的分类
        /// </summary>
        /// <param name="select_ids"></param>
        private void LoadCategory()
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

    }
}