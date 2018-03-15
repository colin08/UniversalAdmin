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
    public class DemoController : BaseAdminController
    {
        // GET: Admin/Demo
        public ActionResult Index(int page = 1, string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelDemo response_model = new Models.ViewModelDemo();
            response_model.page = page;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);

            List<BLL.FilterSearch> filters = new List<BLL.FilterSearch>();
            if (!string.IsNullOrWhiteSpace(word))
                filters.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like));
            int total = 0;
            BLL.BaseBLL<Entity.Demo> bll = new BLL.BaseBLL<Entity.Demo>();
            List<Entity.Demo> list = bll.GetPagedList(page, response_model.page_size, ref total, filters, "AddTime desc");

            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        [HttpPost]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.Demo> bll = new BLL.BaseBLL<Entity.Demo>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','),int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            AddAdminLogs(Entity.SysLogMethodType.Delete, "删除Demo："+ids);

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        public ActionResult Edit(int? id)
        {
            BLL.BLLDemo bll = new BLL.BLLDemo();
            Entity.Demo entity = new Entity.Demo();
            entity.Depts.Add(new Entity.DemoDept()
            {
                ImgUrl = "",
                Num = 0,
                Title = ""
            });

            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(num);
                if (entity == null)
                {
                    return PromptView("/admin/demo", "404", "Not Found", "信息不存在或已被删除", 5);
                }
            }

            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Entity.Demo entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            BLL.BaseBLL<Entity.Demo> bll = new BLL.BaseBLL<Entity.Demo>();
            string str_albums = WebHelper.GetFormString("StrAlbums");
            if (!isAdd)
            {
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/demo", "404", "Not Found", "信息不存在或已被删除", 5);
                }
            }
            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    entity.AddTime = DateTime.Now;
                    entity.LastUpdateTime = DateTime.Now;
                    entity.AddUserID = WorkContext.UserInfo.ID;
                    entity.LastUpdateUserID = WorkContext.UserInfo.ID;
                    bll.Add(entity);

                }
                else //修改
                {
                    entity.LastUpdateTime = DateTime.Now;
                    entity.LastUpdateUserID = WorkContext.UserInfo.ID;
                    new BLL.BLLDemo().Modify(entity);
                }
                
                return PromptView("/admin/demo", "OK", "Success", "操作成功", 5);
            }
            else
            {
                return View(entity);
            }
        }

    }
}