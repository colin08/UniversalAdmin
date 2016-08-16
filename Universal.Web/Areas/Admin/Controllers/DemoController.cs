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
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie("demoindex"), SiteKey.AdminDefaultPageSize);
            var db = new DataCore.EFDBContext();
            //查询分页
            IQueryable<DataCore.Entity.Demo> query = db.Demo;
            if (!string.IsNullOrWhiteSpace(word))
                query = query.Where(p => p.Title.Contains(word));

            //总数
            int total = query.Count();

            query = query.OrderByDescending(p => p.AddTime);
            query = query.Skip(response_model.page_size * (page - 1)).Take(response_model.page_size);

            response_model.DataList = query.ToList();
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            db.Dispose();
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
            var db = new DataCore.EFDBContext();
            foreach (var item in ids.Split(','))
            {
                int id = TypeHelper.ObjectToInt(item);
                var entity = db.Demo.Find(id);
                db.Demo.Remove(entity);
            }
            db.SaveChanges();
            db.Dispose();
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        public ActionResult Edit(int? id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                DataCore.Entity.Demo entity = new DataCore.Entity.Demo();
                entity.Depts.Add(new DataCore.Entity.DemoDept()
                {
                    ImgUrl = "",
                    Num = 0,
                    Title = ""
                });

                int num = TypeHelper.ObjectToInt(id, 0);
                if (num != 0)
                {
                    entity = db.Demo.Where(p => p.ID == num).Include(p => p.LastUpdateUser).Include(p => p.AddUser).Include(p => p.Albums).Include(p => p.Depts).FirstOrDefault();
                    if (entity == null)
                    {
                        return PromptView("/admin/demo", "404", "Not Found", "信息不存在或已被删除",5);
                    }
                }
                
                return View(entity);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DataCore.Entity.Demo entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            var db = new DataCore.EFDBContext();
            string str_albums = WebHelper.GetFormString("StrAlbums");
            if (!isAdd)
            {
                if (db.Demo.Count(p => p.ID == entity.ID) == 0)
                {
                    return PromptView("/admin/demo", "404", "Not Found", "信息不存在或已被删除",5);
                }
            }
            //var temp = db.Demo.Find(entity.ID);

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    var sys_user = db.SysUsers.Find(WorkContext.UserInfo.ID);
                    entity.AddTime = DateTime.Now;
                    entity.LastUpdateTime = DateTime.Now;
                    entity.AddUser = sys_user;
                    entity.LastUpdateUser = sys_user;

                    db.Demo.Add(entity);

                }
                else //修改
                {
                    entity.LastUpdateTime = DateTime.Now;
                    entity.LastUpdateUser = db.SysUsers.Find(WorkContext.UserInfo.ID);

                    //删除旧数据
                    db.DemoAlbums.Where(p => p.DemoID == entity.ID).ToList().ForEach(p => db.Entry(p).State = EntityState.Deleted);
                    db.DemoDepts.Where(p => p.DemoID == entity.ID).ToList().ForEach(p => db.Entry(p).State = EntityState.Deleted);

                    var old_entity = db.Demo.Find(entity.ID);
                    db.Entry(old_entity).CurrentValues.SetValues(entity);
                    ((List<DataCore.Entity.DemoAlbum>)entity.Albums).ForEach(p => db.Entry(p).State = EntityState.Added);
                    foreach (var item in entity.Depts)
                    {
                        item.DemoID = entity.ID;
                        db.Entry(item).State = EntityState.Added;
                    }
                }

                db.SaveChanges();
                return PromptView("/admin/demo", "OK", "Success", "操作成功",5);
            }
            else
            {
                return View(entity);
            }            
        }

    }
}