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
    public class HonourController : BaseAdminController
    {
        /// <summary>
        /// 用户分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("朗形荣誉", "朗形荣誉首页")]
        public ActionResult Index(int page = 1,string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelHonourList response_model = new Models.ViewModelHonourList();
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


            BLL.BaseBLL<Entity.Honour> bll = new BLL.BaseBLL<Entity.Honour>();
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
        [AdminPermissionAttribute("朗形荣誉", "朗形荣誉编辑页面")]
        public ActionResult Edit(int? id)
        {
            BLL.BaseBLL<Entity.Honour> bll = new BLL.BaseBLL<Entity.Honour>();
            Entity.Honour entity = new Entity.Honour();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/Honour", "404", "Not Found", "信息不存在或已被删除", 3);
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
        [AdminPermissionAttribute("朗形荣誉", "保存朗形荣誉编辑信息")]
        public ActionResult Edit(Entity.Honour entity)
        {
            var isAdd = entity.ID == 0 ? true : false;

            BLL.BaseBLL<Entity.Honour> bll = new BLL.BaseBLL<Entity.Honour>();

            
            //数据验证
            if (isAdd)
            {

            }
            else
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/Honour", "404", "Not Found", "信息不存在或已被删除", 3);
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
                    AddAdminLogs(Entity.SysLogMethodType.Add, "添加朗形荣誉：" + entity.Title + "");
                }
                else //修改
                {
                    var ddd = bll.GetModel(p => p.ID == entity.ID, null);
                    ddd.Status = entity.Status;
                    ddd.Title = entity.Title;
                    ddd.Weight = entity.Weight;
                    ddd.ImgUrl = entity.ImgUrl;
                    entity.AddUserID = WorkContext.UserInfo.ID;
                    entity.LastUpdateUserID = WorkContext.UserInfo.ID;
                    bll.Modify(ddd);
                    AddAdminLogs(Entity.SysLogMethodType.Update, "修改朗形荣誉：" + entity.ID.ToString() + "");
                }

                return PromptView("/admin/Honour", "OK", "Success", "操作成功", 3);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("朗形荣誉", "删除朗形荣誉")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.Honour> bll = new BLL.BaseBLL<Entity.Honour>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            AddAdminLogs(Entity.SysLogMethodType.Delete, "删除朗形荣誉：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

    }
}