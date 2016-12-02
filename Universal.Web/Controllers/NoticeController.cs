﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using System.Data.Entity;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 秘籍
    /// </summary>
    [BasicAdminAuth]
    public class NoticeController : BaseHBLController
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
        public JsonResult NoticeData(int page_size, int page_index, string keyword)
        {
            BLL.BaseBLL<Entity.CusNotice> bll = new BLL.BaseBLL<Entity.CusNotice>();
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (!string.IsNullOrWhiteSpace(keyword))
                filter.Add(new BLL.FilterSearch("Title", keyword, BLL.FilterSearchContract.like));
            List<Entity.CusNotice> list = bll.GetPagedList(page_index, page_size, ref rowCount, filter, "AddTime desc", p => p.CusUser);
            WebAjaxEntity<List<Entity.CusNotice>> result = new WebAjaxEntity<List<Entity.CusNotice>>();
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
        public JsonResult DelNotice(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.CusNotice> bll = new BLL.BaseBLL<Entity.CusNotice>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);

        }

        public ActionResult Modify(int? id)
        {
            int ids = TypeHelper.ObjectToInt(id);
            Models.ViewModelNotice model = new Models.ViewModelNotice();
            if (ids != 0)
            {
                BLL.BaseBLL<Entity.CusNotice> bll = new BLL.BaseBLL<Entity.CusNotice>();
                Entity.CusNotice entity = bll.GetModel(p => p.ID == id);
                if (entity != null)
                {
                    model.content = entity.Content;
                    model.title = entity.Title;
                    model.post_see = entity.See;
                    System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
                    switch (entity.See)
                    {
                        case Entity.DocPostSee.everyone:
                            break;
                        case Entity.DocPostSee.department:
                            foreach (var item in BLL.BLLDepartment.GetListByIds(entity.TOID))
                            {
                                str_ids.Append(item.ID.ToString() + ",");
                                model.see_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.Title));
                            }
                            break;
                        case Entity.DocPostSee.user:
                            foreach (var item in BLL.BLLCusUser.GetListByIds(entity.TOID))
                            {
                                str_ids.Append(item.ID.ToString() + ",");
                                model.see_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.Telphone + "(" + item.NickName + ")"));
                            }
                            break;
                        default:
                            break;
                    }
                    if (str_ids.Length > 0)
                    {
                        str_ids = str_ids.Remove(str_ids.Length - 1, 1);
                    }
                    model.see_ids = str_ids.ToString();
                }
                else
                {
                    model.Msg = 2;
                    model.MsgBox = "数据不存在";
                }

            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Modify(Models.ViewModelNotice entity)
        {
            var isAdd = entity.id == 0 ? true : false;
            

            BLL.BaseBLL<Entity.CusNotice> bll = new BLL.BaseBLL<Entity.CusNotice>();
            if (!isAdd)
            {
                if (!bll.Exists(p => p.ID == entity.id))
                {
                    entity.Msg = 2;
                    ModelState.AddModelError("title", "信息不存在");
                }
            }

            #region 处理用户或部门的ID
            System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
            entity.see_entity = new List<Models.ViewModelDocumentCategory>();
            switch (entity.post_see)
            {
                case Entity.DocPostSee.everyone:
                    break;
                case Entity.DocPostSee.department:
                    foreach (var item in BLL.BLLDepartment.GetListByIds(entity.see_ids))
                    {
                        str_ids.Append(item.ID.ToString() + ",");
                        entity.see_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.Title));
                    }
                    break;
                case Entity.DocPostSee.user:
                    foreach (var item in BLL.BLLCusUser.GetListByIds(entity.see_ids))
                    {
                        str_ids.Append(item.ID.ToString() + ",");
                        entity.see_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.Telphone + "(" + item.NickName + ")"));
                    }
                    break;
                default:
                    break;
            }
            string final_ids = "";
            if (str_ids.Length > 0)
            {
                final_ids = "," + str_ids.ToString();
            }
            #endregion

            if (ModelState.IsValid)
            {

                Entity.CusNotice model = null;

                if (isAdd)
                {
                    model = new Entity.CusNotice();
                    model.CusUserID = WorkContext.UserInfo.ID;
                }
                else
                    model = bll.GetModel(p => p.ID == entity.id);

                model.Content = entity.content;
                model.Title = entity.title;
                model.See = entity.post_see;
                model.TOID = final_ids;

                if (isAdd)
                    bll.Add(model);
                else
                    bll.Modify(model);

                entity.Msg = 1;
            }

            return View(entity);
        }
    }
}