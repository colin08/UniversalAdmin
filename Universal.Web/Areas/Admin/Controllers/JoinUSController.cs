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
    public class JoinUSController : BaseAdminController
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role">筛选：组ID</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("职位管理", "职位管理首页")]
        public ActionResult Index(int page = 1,string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelJoinUSList response_model = new Models.ViewModelJoinUSList();
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


            BLL.BaseBLL<Entity.JoinUS> bll = new BLL.BaseBLL<Entity.JoinUS>();
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
        [AdminPermissionAttribute("职位管理", "职位编辑页面")]
        public ActionResult Edit(int? id)
        {
            BLL.BaseBLL<Entity.JoinUS> bll = new BLL.BaseBLL<Entity.JoinUS>();
            Entity.JoinUS entity = new Entity.JoinUS();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/JoinUS", "404", "Not Found", "信息不存在或已被删除", 3);
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
        [AdminPermissionAttribute("职位管理", "保存职位编辑信息")]
        public ActionResult Edit(Entity.JoinUS entity)
        {
            var isAdd = entity.ID == 0 ? true : false;

            BLL.BaseBLL<Entity.JoinUS> bll = new BLL.BaseBLL<Entity.JoinUS>();
            

            //数据验证
            if (isAdd)
            {
                

            }
            else
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/JoinUS", "404", "Not Found", "信息不存在或已被删除", 3);
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
                    AddAdminLogs(Entity.SysLogMethodType.Add, "添加职位：" + entity.Title + "");

                }
                else //修改
                {
                    var ddd = bll.GetModel(p => p.ID == entity.ID, null);
                    ddd.Status = entity.Status;
                    ddd.Title = entity.Title;
                    ddd.Address = entity.Address;
                    ddd.TimeOut = entity.TimeOut;
                    ddd.Weight = entity.Weight;
                    ddd.Content = entity.Content;
                    ddd.AddUserID = WorkContext.UserInfo.ID;
                    ddd.LastUpdateUserID = WorkContext.UserInfo.ID;
                    ddd.ImgUrl = entity.ImgUrl;
                    ddd.ImgUrlBig = entity.ImgUrlBig;
                    ddd.Department = entity.Department;

                    bll.Modify(ddd);
                    AddAdminLogs(Entity.SysLogMethodType.Update, "修改职位管理：" + ddd.Title + "");
                }

                return PromptView("/admin/JoinUS", "OK", "Success", "操作成功", 3);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("职位管理", "删除职位")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.JoinUS> bll = new BLL.BaseBLL<Entity.JoinUS>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            AddAdminLogs(Entity.SysLogMethodType.Delete, "删除职位：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }
    }
}