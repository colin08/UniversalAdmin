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
    public class SysUserController : BaseAdminController
    {
        /// <summary>
        /// 用户分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role">筛选：组ID</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int role = 0, string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelSysUserList response_model = new Models.ViewModelSysUserList();
            response_model.page = page;
            response_model.role = role;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie("sysuserindex"), SiteKey.AdminDefaultPageSize);

            var db = new DataCore.EFDBContext();
            Load(db);
            //查询分页
            IQueryable<DataCore.Entity.SysUser> query = db.SysUsers;
            if (role != 0)
                query = query.Where(p => p.SysRoleID == role);
            if (!string.IsNullOrWhiteSpace(word))
                query = query.Where(p => p.NickName.Contains(word) || p.UserName.Contains(word));

            //总数
            int total = query.Count();

            query = query.Include(p => p.SysRole).OrderByDescending(p => p.RegTime);
            query = query.Skip(response_model.page_size * (page - 1)).Take(response_model.page_size);

            response_model.DataList = query.ToList();
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            db.Dispose();
            return View(response_model);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {

            using (var db = new DataCore.EFDBContext())
            {
                Load(db);
                DataCore.Entity.SysUser entity = new DataCore.Entity.SysUser();
                int num = TypeHelper.ObjectToInt(id, 0);
                if (num != 0)
                {
                    entity = db.SysUsers.Find(num);
                    if (entity == null)
                    {
                        entity = new DataCore.Entity.SysUser();
                        entity.Msg = -1;
                        entity.MsgBox = "信息不存在或已被删除";
                        entity.RedirectUrl = "/admin/SysUser";
                        return View(entity);
                    }
                }
                return View(entity);
            }
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DataCore.Entity.SysUser entity)
        {
            var isAdd = entity.ID == 0 ? true : false;

            var db = new DataCore.EFDBContext();
            Load(db);

            if (entity.SysRoleID == 0)
            {
                ModelState.AddModelError("SysRoleID", "请选择用户组");
            }

            //数据验证
            if (isAdd)
            {
                //判断用户名是否存在
                if (db.SysUsers.Count(p => p.UserName == entity.UserName) > 0)
                {
                    ModelState.AddModelError("UserName", "该用户名已存在");
                }

            }
            else
            {
                //如果要编辑的用户不存在
                if (db.SysUsers.Count(p => p.ID == entity.ID) == 0)
                {
                    entity.Msg = -1;
                    entity.MsgBox = "要编辑的信息不存在或已被删除";
                    entity.RedirectUrl = "/admin/SysUser";
                    db.Dispose();
                    return View(entity);
                }
                ModelState.Remove("UserName");
            }

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    entity.RegTime = DateTime.Now;
                    entity.Password = SecureHelper.MD5(entity.Password);
                    entity.Avatar = "";
                    entity.LastLoginTime = DateTime.Now;
                    db.SysUsers.Add(entity);

                }
                else //修改
                {
                    var user = db.SysUsers.Find(entity.ID);
                    if (entity.Password != "litdev")
                        user.Password = SecureHelper.MD5(entity.Password);
                    user.NickName = entity.NickName;
                    user.IsAdmin = entity.IsAdmin;
                    user.Gender = entity.Gender;
                    user.Status = entity.Status;
                    user.SysRoleID = entity.SysRoleID;
                }

                db.SaveChanges();

                entity.Msg = 1;
                entity.MsgBox = "操作成功";
                entity.RedirectUrl = "/admin/SysUser";
            }else
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
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            var db = new DataCore.EFDBContext();
            foreach (var item in ids.Split(','))
            {
                int id = TypeHelper.ObjectToInt(item);
                var entity = db.SysUsers.Find(id);
                if (entity != null)
                {
                    db.SysUsers.Remove(entity);
                    AddAdminLogs(db, DataCore.Entity.SysLogMethodType.Delete, "删除后台用户：" + item + ",登录名：" + entity.UserName + ",昵称:" + entity.NickName);
                }
            }
            db.SaveChanges();
            db.Dispose();
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }


        /// <summary>
        /// 加载用户组
        /// </summary>
        private void Load(DataCore.EFDBContext db)
        {
            List<SelectListItem> userRoleList = new List<SelectListItem>();
            userRoleList.Add(new SelectListItem() { Text = "全部组", Value = "0" });
            var lis = db.SysRoles;
            foreach (var item in lis.ToList())
            {
                userRoleList.Add(new SelectListItem() { Text = item.RoleName, Value = item.ID.ToString() });
            }
            ViewData["sysuser_role"] = userRoleList;
        }

    }
}