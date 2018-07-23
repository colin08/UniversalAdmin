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
        [AdminPermissionAttribute("后台用户", "后台用户首页")]
        public ActionResult Index(int page = 1, int role = 0, string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelSysUserList response_model = new Models.ViewModelSysUserList();
            response_model.page = page;
            response_model.role = role;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);

            Load();

            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (role != 0)
                filter.Add(new BLL.FilterSearch("SysRoleID", role.ToString(), BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("NickName", word, BLL.FilterSearchContract.like));
                filter.Add(new BLL.FilterSearch("UserName", word, BLL.FilterSearchContract.like));
            }


            BLL.BaseBLL<Entity.SysUser> bll = new BLL.BaseBLL<Entity.SysUser>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "RegTime desc","SysRole");
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
        [AdminPermissionAttribute("后台用户", "后台用户编辑页面")]
        public ActionResult Edit(int? id)
        {
            BLL.BaseBLL<Entity.SysUser> bll = new BLL.BaseBLL<Entity.SysUser>();
            Load();
            Entity.SysUser entity = new Entity.SysUser();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/SysUser", "404", "Not Found", "信息不存在或已被删除", 3);
                }
            }
            return View(entity);
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminPermissionAttribute("后台用户", "保存后台用户首页编辑信息")]
        public ActionResult Edit(Entity.SysUser entity)
        {
            var isAdd = entity.ID == 0 ? true : false;

            BLL.BaseBLL<Entity.SysUser> bll = new BLL.BaseBLL<Entity.SysUser>();
            Load();

            if (entity.SysRoleID == 0)
            {
                ModelState.AddModelError("SysRoleID", "请选择用户组");
            }

            //数据验证
            if (isAdd)
            {
                //判断用户名是否存在
                if (!bll.Exists(p => p.UserName == entity.UserName))
                {
                    ModelState.AddModelError("UserName", "该用户名已存在");
                }

            }
            else
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/SysUser", "404", "Not Found", "信息不存在或已被删除", 3);
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
                    entity.LastLoginTime = DateTime.Now;
                    AddAdminLogs(Entity.SysLogMethodType.Add, "添加后台用户：" + entity.UserName + "");
                    bll.Add(entity);

                }
                else //修改
                {
                    var user = bll.GetModel(p => p.ID == entity.ID, null);
                    if (entity.Password != "litdev")
                        user.Password = SecureHelper.MD5(entity.Password);
                    user.NickName = entity.NickName;
                    user.Gender = entity.Gender;
                    user.Status = entity.Status;
                    user.Avatar = entity.Avatar;
                    user.SysRoleID = entity.SysRoleID;
                    bll.Modify(user);
                    AddAdminLogs(Entity.SysLogMethodType.Update, "修改后台用户：" + user.UserName + "");
                }

                return PromptView("/admin/SysUser", "OK", "Success", "操作成功", 3);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("后台用户", "删除后台用户")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.SysUser> bll = new BLL.BaseBLL<Entity.SysUser>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            AddAdminLogs(Entity.SysLogMethodType.Delete, "删除后台用户：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }


        /// <summary>
        /// 加载用户组
        /// </summary>
        private void Load()
        {
            List<SelectListItem> userRoleList = new List<SelectListItem>();
            userRoleList.Add(new SelectListItem() { Text = "全部组", Value = "0" });
            BLL.BaseBLL<Entity.SysRole> bll = new BLL.BaseBLL<Entity.SysRole>();
            foreach (var item in bll.GetListBy(0,new List<BLL.FilterSearch>(),null))
            {
                userRoleList.Add(new SelectListItem() { Text = item.RoleName, Value = item.ID.ToString() });
            }
            ViewData["sysuser_role"] = userRoleList;
        }

    }
}