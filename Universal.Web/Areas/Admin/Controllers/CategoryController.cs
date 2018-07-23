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
    /// <summary>
    /// 栏目管理
    /// </summary>
    public class CategoryController : BaseAdminController
    {

        [AdminPermissionAttribute("栏目管理", "栏目管理首页")]
        public ActionResult Index()
        {
            return View(LoadCategory());
        }

        [AdminPermissionAttribute("栏目管理", "删除栏目")]
        [HttpPost]
        public JsonResult Del(string ids)
        {
            int id = TypeHelper.ObjectToInt(ids);
            if (id<=0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            string str= BLL.BLLCategory.GetChildIDStr(TypeHelper.ObjectToInt(ids));
            if(string.IsNullOrWhiteSpace(str))
            {
                WorkContext.AjaxStringEntity.msgbox = "找不到相关数据";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.Category> bll = new BLL.BaseBLL<Entity.Category>();
            var id_list = Array.ConvertAll<string, int>(str.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            AddAdminLogs(Entity.SysLogMethodType.Delete, "删除栏目：" + str);

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }


        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [AdminPermissionAttribute("栏目管理", "栏目管理编辑页面")]
        public ActionResult Edit(int? id)
        {
            BLL.BaseBLL<Entity.Category> bll = new BLL.BaseBLL<Entity.Category>();
            LoadCategorySelect();
            Entity.Category entity = new Entity.Category();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num,null);
                if (entity == null)
                {
                    return PromptView("/admin/Category", "404", "Not Found", "信息不存在或已被删除", 3);
                }
            }
            return View(entity);
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminPermissionAttribute("栏目管理", "保存栏目管理编辑信息")]
        public ActionResult Edit(Entity.Category entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            LoadCategorySelect();
            BLL.BaseBLL<Entity.Category> bll = new BLL.BaseBLL<Entity.Category>();
            
            //数据验证
            if (!isAdd)
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/Category", "404", "Not Found", "信息不存在或已被删除", 3);
                }
            }

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    BLL.BLLCategory.Add(entity);

                }
                else //修改
                {
                    entity.LastUpdateUserID = WorkContext.UserInfo.ID;
                    entity.LastUpdateTime = DateTime.Now;
                    BLL.BLLCategory.Modify(entity);
                }

                return PromptView("/admin/Category", "OK", "Success", "操作成功", 3);
            }
            else
                return View(entity);
        }


        /// <summary>
        /// 加载所有栏目
        /// </summary>
        /// <returns></returns>
        public List<Entity.Category> LoadCategory()
        {
            BLL.BaseBLL<Entity.Category> bll = new BLL.BaseBLL<Entity.Category>();
            var oldData = bll.GetListBy(0, new List<BLL.FilterSearch>(), "Weight Desc");
            List<Entity.Category> newData = new List<Entity.Category>();
            GetChilds(oldData, newData, null);
            foreach (var item in newData)
            {
                item.Title = StringHelper.StringOfChar(item.Depth - 1, "&nbsp;&nbsp;") + "├ " + StringHelper.StringOfChar(item.Depth - 1, "&nbsp;&nbsp;&nbsp;&nbsp;") + item.Title;
            }
            return newData;
        }

        /// <summary>
        /// 加载所有栏目select
        /// </summary>
        /// <returns></returns>
        public void LoadCategorySelect()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            selectList.Add(new SelectListItem() { Text = "选择栏目", Value = "0" });
            BLL.BaseBLL<Entity.Category> bll = new BLL.BaseBLL<Entity.Category>();
            var oldData = bll.GetListBy(0, new List<BLL.FilterSearch>(), "Weight Desc");
            List<Entity.Category> newData = new List<Entity.Category>();
            GetChilds(oldData, newData, null);
            foreach (var item in newData)
            {
                string text = StringHelper.StringOfChar(item.Depth - 1, "|--") +  StringHelper.StringOfChar(item.Depth - 1, "|--") + item.Title;
                selectList.Add(new SelectListItem() { Text = text, Value = item.ID.ToString() });
            }
            ViewData["CategoryList"] = selectList;
        }

        private void GetChilds(List<Entity.Category> oldData, List<Entity.Category> newData, int? pid)
        {

            List<Entity.Category> list = new List<Entity.Category>();
            if (pid == null)
                list = oldData.Where(p => p.PID == null).ToList();
            else
                list = oldData.Where(p => p.PID == pid).ToList();
            foreach (var item in list)
            {
                Entity.Category entity = new Entity.Category();
                entity.AddTime = item.AddTime;
                entity.Depth = item.Depth;
                entity.ID = item.ID;
                entity.PID = item.PID;
                entity.Weight = item.Weight;
                entity.Status = item.Status;
                entity.Title = item.Title;
                entity.CallName = item.CallName;
                newData.Add(entity);
                this.GetChilds(oldData, newData, item.ID);
            }
        }

    }
}