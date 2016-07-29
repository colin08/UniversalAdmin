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
    public class SysRoleController : BaseAdminController
    {
        public ActionResult Index(int page = 1,string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelSysRoleList response_model = new Models.ViewModelSysRoleList();
            response_model.page = page;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie("sysroleindex"), SiteKey.AdminDefaultPageSize);
            var db = new DataCore.EFDBContext();
            //查询分页
            IQueryable<DataCore.Entity.SysRole> query = db.SysRoles;
            if (!string.IsNullOrWhiteSpace(word))
                query = query.Where(p => p.RoleName.Contains(word));

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


        public ActionResult Edit(int? id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                DataCore.Entity.SysRole entity = new DataCore.Entity.SysRole();
                int num = TypeHelper.ObjectToInt(id, 0);
                if (num != 0)
                {
                    entity = db.SysRoles.Find(num);
                    if (entity == null)
                    {
                        entity = new DataCore.Entity.SysRole();
                        entity.Msg = -1;
                        entity.MsgBox = "信息不存在或已被删除";
                        entity.RedirectUrl = "/admin/SysRole";
                        return View(entity);
                    }
                }
                return View(entity);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DataCore.Entity.SysRole entity)
        {
            var isAdd = entity.ID == 0 ? true : false;

            var db = new DataCore.EFDBContext();

            //数据验证
            if (isAdd)
            {
                if (db.SysRoles.Count(p => p.RoleName == entity.RoleName) > 0)
                {
                    ModelState.AddModelError("RoleName", "该组名已存在");
                }

            }
            else
            {
                if (db.SysRoles.Count(p => p.ID == entity.ID) == 0)
                {
                    entity.Msg = -1;
                    entity.MsgBox = "要编辑的信息不存在或已被删除";
                    entity.RedirectUrl = "/admin/SysRole";
                    db.Dispose();
                    return View(entity);
                }
            }
            var role = db.SysRoles.Find(entity.ID);

            if (ModelState.IsValid)
            {
                if(!isAdd)
                {
                    //验证组名是否存在
                    if (role.RoleName != entity.RoleName)
                    {
                        if (db.SysRoles.Count(p => p.RoleName == entity.RoleName) > 0)
                        {
                            ModelState.AddModelError("RoleName", "该组名已存在");
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    entity.AddTime = DateTime.Now;
                    db.SysRoles.Add(entity);

                }
                else //修改
                {  
                    role.RoleName = entity.RoleName;
                    role.RoleDesc = entity.RoleDesc;
                    role.IsAdmin = entity.IsAdmin;
                }

                db.SaveChanges();

                entity.Msg = 1;
                entity.MsgBox = "操作成功";
                entity.RedirectUrl = "/admin/SysRole";
            }
            else
            {
                entity.Msg = -2;
            }

            db.Dispose();
            return View(entity);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpPost]
        public JsonResult Del(string ids)
        {
            WorkContext.AjaxStringEntity.msgbox = "暂不能删除用户组";
            return Json(WorkContext.AjaxStringEntity);

            //if (string.IsNullOrWhiteSpace(ids))
            //{
            //    WorkContext.AjaxStringEntity.msgbox = "缺少参数";
            //    return Json(WorkContext.AjaxStringEntity);
            //}
            //var db = new DataCore.EFDBContext();
            //foreach (var item in ids.Split(','))
            //{
            //    int id = TypeHelper.ObjectToInt(item);
            //    var entity = db.SysUsers.Find(id);
            //    if (entity != null)
            //    {
            //        db.SysUsers.Remove(entity);
            //        AddAdminLogs(db, DataCore.Entity.SysLogMethodType.Delete, "删除后台用户：" + item + ",登录名：" + entity.UserName + ",昵称:" + entity.NickName);
            //    }
            //}
            //db.SaveChanges();
            //db.Dispose();
            //WorkContext.AjaxStringEntity.msg = 1;
            //WorkContext.AjaxStringEntity.msgbox = "success";
            //return Json(WorkContext.AjaxStringEntity);
        }

    }
}