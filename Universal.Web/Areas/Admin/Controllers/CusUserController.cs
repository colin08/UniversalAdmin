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
    public class CusUserController : BaseAdminController
    {
        // GET: Admin/CusUser
        [AdminPermissionAttribute("前端用户", "前端用户首页")]
        public ActionResult Index(int page = 1, string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelCusUser response_model = new Models.ViewModelCusUser();
            response_model.page = page;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie("cususerndex"), SiteKey.AdminDefaultPageSize);

            List<BLL.FilterSearch> filters = new List<BLL.FilterSearch>();
            if (!string.IsNullOrWhiteSpace(word))
            {
                filters.Add(new BLL.FilterSearch("Telphone", word, BLL.FilterSearchContract.like));
                filters.Add(new BLL.FilterSearch("NickName", word, BLL.FilterSearchContract.like));
            }
            int total = 0;
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            List<Entity.CusUser> list = bll.GetPagedList(page, response_model.page_size, ref total, filters, "RegTime desc");

            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        [HttpPost]
        [AdminPermissionAttribute("前端用户", "删除用户")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            AddAdminLogs(Entity.SysLogMethodType.Delete, "删除CusUser：" + ids);

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        [AdminPermissionAttribute("前端用户", "前端用户编辑页面")]
        public ActionResult Edit(int? id)
        {
            var entity = new Entity.CusUser();
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num);
                if (entity == null)
                {
                    return PromptView("/admin/"+WorkContext.Controller.ToLower(), "404", "Not Found", "信息不存在或已被删除", 5);
                }
            }

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminPermissionAttribute("前端用户", "保存前端用户编辑信息")]
        public ActionResult Edit(Entity.CusUser entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            if (!isAdd)
            {
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/" + WorkContext.Controller.ToLower(), "404", "Not Found", "信息不存在或已被删除", 5);
                }
                ModelState.Remove("Telphone");
            }
            else
            {
                if (bll.Exists(p => p.Telphone == entity.Telphone))
                {
                    ModelState.AddModelError("Telphone", "该手机号已存在");
                }
            }
            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    entity.RegTime = DateTime.Now;
                    entity.LastLoginTime = DateTime.Now;
                    entity.Password = SecureHelper.MD5(entity.Password);
                    if (string.IsNullOrWhiteSpace(entity.Avatar))
                        entity.Avatar = "/Content/images/default_avatar.jpg";
                    bll.Add(entity);

                }
                else //修改
                {
                    if (entity.Password != "litdev")
                        entity.Password = SecureHelper.MD5(entity.Password);
                    bll.Modify(entity);
                }

                return PromptView("/admin/" + WorkContext.Controller.ToLower(), "OK", "Success", "操作成功", 5);
            }
            else
            {
                return View(entity);
            }
        }

    }
}