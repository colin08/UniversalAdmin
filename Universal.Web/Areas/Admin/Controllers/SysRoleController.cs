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
    public class SysRoleController : BaseAdminController
    {
        [AdminPermissionAttribute("用户组", "用户组首页")]
        public ActionResult Index(int page = 1, string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelSysRoleList response_model = new Models.ViewModelSysRoleList();
            response_model.page = page;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie("sysroleindex"), SiteKey.AdminDefaultPageSize);

            int total = 0;

            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            filter.Add(new BLL.FilterSearch("SysMerchantID", WorkContext.UserInfo.SysMerchantID.ToString(), BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(word))
                filter.Add(new BLL.FilterSearch("RoleName", word, BLL.FilterSearchContract.like));


            BLL.BaseBLL<Entity.SysRole> bll = new BLL.BaseBLL<Entity.SysRole>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "AddTime desc");
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }
        

        [AdminPermissionAttribute("用户组", "用户组信息编辑页面")]
        public ActionResult Edit(int? id)
        {
            BLL.BaseBLL<Entity.SysRole> bll = new BLL.BaseBLL<Entity.SysRole>();
            if (id == null)
                GetTree();
            else
                GetTree(TypeHelper.ObjectToInt(id, 0));

            Entity.SysRole entity = new Entity.SysRole();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/SysRole", "404", "Not Found", "信息不存在或已被删除", 5);
                }
            }
            return View(entity);
        }

        [AdminPermissionAttribute("用户组", "保存用户组编辑的信息")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Entity.SysRole entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            BLL.BaseBLL<Entity.SysRole> bll = new BLL.BaseBLL<Entity.SysRole>();
            GetTree(entity.ID);

            var qx = WebHelper.GetFormString("hid_qx");
            //数据验证
            if (isAdd)
            {
                if (bll.Exists(p => p.RoleName == entity.RoleName))
                    ModelState.AddModelError("RoleName", "该组名已存在");
            }
            else
            {
                if (!bll.Exists(p => p.ID == entity.ID))
                    return PromptView("/admin/SysRole", "404", "Not Found", "该组不存在或已被删除", 5);

                var old_entity = bll.GetModel(p => p.ID == entity.ID, null);
                //验证组名是否存在
                if (old_entity.RoleName != entity.RoleName)
                {
                    if (bll.Exists(p => p.RoleName == entity.RoleName && p.SysMerchantID == WorkContext.UserInfo.SysMerchantID))
                        ModelState.AddModelError("RoleName", "该组名已存在");
                }
                entity.SysMerchantID = old_entity.SysMerchantID;
            }

            if (ModelState.IsValid)
            {
                BLL.BLLSysRole bll_role = new BLL.BLLSysRole();
                if (entity.ID == 0)//添加
                {
                    entity.SysMerchantID = WorkContext.UserInfo.SysMerchantID;
                    bll_role.Add(entity, qx);
                    AddAdminLogs(Entity.SysLogMethodType.Add, "添加用户组:" + entity.RoleName);
                }
                else //修改
                {
                    bll_role.Modify(entity, qx);
                    AddAdminLogs(Entity.SysLogMethodType.Update, "修改用户组:" + entity.RoleName);
                }

                return PromptView("/admin/SysRole", "OK", "Success", "操作成功", 5);
            }
            else
                return View(entity);

        }

        [HttpPost]
        [AdminPermissionAttribute("用户组", "删除用户组信息")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BaseBLL<Entity.SysRole> bll = new BLL.BaseBLL<Entity.SysRole>();
            foreach (var item in ids.Split(','))
            {
                int id = TypeHelper.ObjectToInt(item);
                if (id != 1)
                {
                    bll.DelBy(p => p.ID == id);
                    AddAdminLogs(Entity.SysLogMethodType.Delete, "删除用户组：" + item);
                }
            }
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 获取树数据
        /// </summary>
        /// <param name="id">当前组ID，没有传0</param>
        private void GetTree(int id = 0)
        {
            BLL.BaseBLL<Entity.SysRole> bll_route = new BLL.BaseBLL<Entity.SysRole>();
            BLL.BaseBLL<Entity.SysRoleRoute> bll_role_route = new BLL.BaseBLL<Entity.SysRoleRoute>();
            Entity.SysRole role = null;
            if (id != 0)
                role = bll_route.GetModel(p => p.ID == id, null);

            List<Models.ViewModelTree> list = new List<Models.ViewModelTree>();
            bool is_super = BLL.BLLMerchant.CheckIsSuper(WorkContext.UserInfo.SysMerchantID);
            var route_group = new BLL.BLLSysRoute().GetListGroupByTag(is_super);
            for (int i = 0; i < route_group.Count; i++)
            {
                int top_id = i + 10000;
                Models.ViewModelTree model = new Models.ViewModelTree();
                model.id = top_id;
                model.name = route_group[i].Key;
                model.open = i < 4 ? true : false;
                model.pId = 0;
                list.Add(model);
                foreach (var item in route_group[i].ToList())
                {
                    Models.ViewModelTree model2 = new Models.ViewModelTree();
                    model2.id = item.ID;
                    model2.name = item.Desc;
                    model2.open = false;
                    model2.pId = top_id;
                    if (role != null)
                        model2.is_checked = bll_role_route.Exists(p => p.SysRoleID == role.ID && p.SysRouteID == item.ID);

                    list.Add(model2);
                }

            }

            ViewData["Tree"] = JsonHelper.ToJson(list).Replace("is_checked", "checked");
        }

    }
}