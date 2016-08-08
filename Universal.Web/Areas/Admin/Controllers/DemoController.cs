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
                if (entity != null)
                {
                    var albums = db.DemoAlbums.Where(p => p.DemoID == id).ToList();
                    albums.ForEach(p => db.DemoAlbums.Remove(p));

                    var depts = db.DemoDepts.Where(p => p.DemoID == id).ToList();
                    depts.ForEach(p => db.DemoDepts.Remove(p));

                    db.Demo.Remove(entity);
                }
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
                int num = TypeHelper.ObjectToInt(id, 0);
                if (num != 0)
                {
                    entity = db.Demo.Where(p=>p.ID==num).Include(p=>p.LastUpdateUser).Include(p=>p.AddUser).Include(p => p.Albums).FirstOrDefault();
                    if (entity == null)
                    {
                        entity = new DataCore.Entity.Demo();
                        entity.Msg = -1;
                        entity.MsgBox = "信息不存在或已被删除";
                        entity.RedirectUrl = "/admin/Demo";
                        return View(entity);
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
            if(!isAdd)
            {
                if (db.Demo.Count(p => p.ID == entity.ID) == 0)
                {
                    entity.Msg = -1;
                    entity.MsgBox = "要编辑的信息不存在或已被删除";
                    entity.RedirectUrl = "/admin/Demo";
                    db.Dispose();
                    return View(entity);
                }
            }
            var temp = db.Demo.Find(entity.ID);
            
            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    entity.AddTime = DateTime.Now;
                    entity.LastUpdateTime = DateTime.Now;
                    entity.AddUser = WorkContext.UserInfo;
                    entity.LastUpdateUser = WorkContext.UserInfo;

                    db.Demo.Add(entity);

                }
                else //修改
                {
                    temp.LastUpdateTime = DateTime.Now;
                    temp.LastUpdateUser = db.SysUsers.Find(WorkContext.UserInfo.ID);
                    temp.Title = entity.Title;
                    temp.Telphone = entity.Telphone;
                    temp.Price = entity.Price;
                    temp.Num = entity.Num;
                    temp.Ran = entity.Ran;
                    temp.StrAlbums = entity.StrAlbums;

                    var old_albums = db.DemoAlbums.Where(p => p.DemoID == entity.ID).ToList();
                    old_albums.ForEach(p => db.DemoAlbums.Remove(p));
                }

                db.SaveChanges();

                entity.Msg = 1;
                entity.MsgBox = "操作成功";
                entity.RedirectUrl = "/admin/Demo";
            }
            else
            {
                entity.Msg = -2;
            }

            db.Dispose();
            return View(entity);
        }

    }
}