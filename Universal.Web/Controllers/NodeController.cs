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
    /// 节点
    /// </summary>
    [BasicAdminAuth]
    public class NodeController : BaseHBLController
    {
        public ActionResult Index()
        {
            LoadCategory(1);
            return View();
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="category_id">搜索分类</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PageData(int page_size, int page_index, string keyword,int category_id)
        {
            BLL.BaseBLL<Entity.Node> bll = new BLL.BaseBLL<Entity.Node>();
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (category_id > 0)
                filter.Add(new BLL.FilterSearch("NodeCategoryID", category_id.ToString(), BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(keyword))
                filter.Add(new BLL.FilterSearch("Title", keyword, BLL.FilterSearchContract.like));
            List<Entity.Node> list = bll.GetPagedList(page_index, page_size, ref rowCount, filter, "LastUpdateTime desc",p=>p.NodeCategory);
            WebAjaxEntity<List<Entity.Node>> result = new WebAjaxEntity<List<Entity.Node>>();
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
        public JsonResult Del(string ids)
        {
            if (!BLL.BLLCusRoute.CheckUserAuthority(BLL.CusRouteType.节点设置, WorkContext.UserInfo.ID))
            {
                WorkContext.AjaxStringEntity.msgbox = "没有权限操作";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.Node> bll = new BLL.BaseBLL<Entity.Node>();
            BLL.BaseBLL<Entity.FlowNode> bll_flow = new BLL.BaseBLL<Entity.FlowNode>();
            foreach (var item in ids.Split(','))
            {
                int node_id = TypeHelper.ObjectToInt(item);
                string msg = "";
                var can_del = BLL.BLLNode.CanDel(node_id, out msg);
                if(!can_del)
                {
                    WorkContext.AjaxStringEntity.msgbox = "无法删除" + item + "，已有使用:\r\n" + msg;
                    return Json(WorkContext.AjaxStringEntity);
                }else
                {
                    bll.DelBy(p => p.ID == node_id);
                }
            }
            //var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            //bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);

        }
        
        public ActionResult Modify(int? id)
        {
            LoadCategory();
            int ids = TypeHelper.ObjectToInt(id);
            Models.ViewModelNode model = new Models.ViewModelNode();
            if (!BLL.BLLCusRoute.CheckUserAuthority(BLL.CusRouteType.节点设置, WorkContext.UserInfo.ID))
            {
                model.Msg = 4;
                return View(model);
            }
            if (ids != 0)
            {
                Entity.Node entity = BLL.BLLNode.GetMode(ids);
                if (entity != null)
                {
                    model.content = entity.Content;
                    model.title = entity.Title;
                    model.location = entity.Location;
                    model.id = ids;
                    model.is_factor = entity.IsFactor;
                    model.contacts_user = entity.ContactsUsers;
                    model.category_id = entity.NodeCategoryID;
                    //System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
                    //foreach (var item in entity.NodeUsers)
                    //{
                    //    str_ids.Append(item.ID.ToString() + ",");
                    //    model.users_entity.Add(new Models.ViewModelDocumentCategory(item.CusUser.ID, item.CusUser.NickName));
                    //}
                    //if (str_ids.Length > 0)
                    //{
                    //    str_ids = str_ids.Remove(str_ids.Length - 1, 1);
                    //}
                    //model.user_ids = str_ids.ToString();
                    
                    model.BuildViewModelListFile(entity.NodeFiles.ToList());
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
        public ActionResult Modify(Models.ViewModelNode entity)
        {
            var isAdd = entity.id == 0 ? true : false;
            LoadCategory();
            if (!BLL.BLLCusRoute.CheckUserAuthority(BLL.CusRouteType.节点设置, WorkContext.UserInfo.ID))
            {
                entity.Msg = 4;
                return View(entity);
            }
            BLL.BaseBLL<Entity.Node> bll = new BLL.BaseBLL<Entity.Node>();
            if (!isAdd)
            {
                if (!bll.Exists(p => p.ID == entity.id))
                {
                    entity.Msg = 2;
                    ModelState.AddModelError("title", "信息不存在");
                }
            }else
            {
                if(bll.Exists(p=>p.Title == entity.title))
                {
                    entity.Msg = 2;
                    ModelState.AddModelError("title", "已有同名节点");
                }
            }
            if(entity.category_id <=0)
            {
                ModelState.AddModelError("category_id", "分类必选");
            }

            //#region 处理用户ID
            //System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
            //entity.users_entity = new List<Models.ViewModelDocumentCategory>();
            //foreach (var item in BLL.BLLCusUser.GetListByIds(entity.user_ids))
            //{
            //    str_ids.Append(item.ID.ToString() + ",");
            //    entity.users_entity.Add(new Models.ViewModelDocumentCategory(item.ID,item.NickName));
            //}
            //string final_ids = "";
            //if (str_ids.Length > 0)
            //{
            //    final_ids = str_ids.Remove(str_ids.Length - 1, 1).ToString();
            //}
            //#endregion
            
            if (ModelState.IsValid)
            {

                Entity.Node model = null;

                if (isAdd)
                    model = new Entity.Node();
                else
                {
                    model = bll.GetModel(p => p.ID == entity.id);
                    model.LastUpdateTime = DateTime.Now;
                }

                model.Content = entity.content;
                model.Title = entity.title;
                model.IsFactor = entity.is_factor;
                model.Location = entity.location;
                model.ContactsUsers = entity.contacts_user;
                model.NodeCategoryID = entity.category_id;
                model.NodeFiles = entity.BuildFileList(WorkContext.UserInfo.ID);

                if (isAdd)
                    BLL.BLLNode.Add(model);
                else
                    BLL.BLLNode.Modify(model);

                entity.Msg = 1;
            }
            else
            {
                entity.Msg = 3;
            }

            return View(entity);
        }

        private void LoadCategory(int type=0)
        {
            List<SelectListItem> userRoleList = new List<SelectListItem>();
            if(type ==0)
                userRoleList.Add(new SelectListItem() { Text = "请选择分类", Value = "0" });
            foreach (var item in BLL.BLLNode.GetNodeCategory())
            {
                userRoleList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString() });
            }
            ViewData["category"] = userRoleList;
        }

    }
}