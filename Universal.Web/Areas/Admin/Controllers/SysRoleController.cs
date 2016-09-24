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
            if (!string.IsNullOrWhiteSpace(word))
                filter.Add(new BLL.FilterSearch("RoleName", word, BLL.FilterSearchContract.like));


            BLL.BaseBLL<Entity.SysRole> bll = new BLL.BaseBLL<Entity.SysRole>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, p => p.AddTime, false);
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }


        [AdminPermissionAttribute("其他", "更新权限数据")]
        [HttpPost]
        public JsonResult UpdateRoute()
        {
            List<Models.ModelRoute> route_list = new List<Models.ModelRoute>();

            #region 反射获取所有的控制路由

            string path = IOHelper.GetMapPath("~/bin/Universal.Web.dll");
            byte[] buffer = System.IO.File.ReadAllBytes(path);
            Assembly assembly = Assembly.Load(buffer);

            foreach (var type in assembly.ExportedTypes)
            {
                System.Reflection.MemberInfo[] properties = type.GetMembers();
                foreach (var item in properties)
                {
                    string controllerName = item.ReflectedType.Name.Replace("Controller", "").ToString();
                    string actionName = item.Name.ToString();
                    //访问路由
                    string route_map = controllerName.ToLower() + "/" + actionName.ToLower();
                    //是否是HttpPost请求
                    bool IsHttpPost = item.GetCustomAttributes(typeof(System.Web.Mvc.HttpPostAttribute), true).Count() > 0 ? true : false;

                    object[] attrs = item.GetCustomAttributes(typeof(Framework.AdminPermissionAttribute), true);
                    if (attrs.Length == 1)
                    {
                        Framework.AdminPermissionAttribute attr = (Framework.AdminPermissionAttribute)attrs[0];
                        route_list.Add(new Models.ModelRoute
                        {
                            Tag = attr.Tag,
                            Desc = attr.Desc,
                            IsPost = IsHttpPost,
                            Route = route_map
                        });
                    }
                }
            }
            #endregion

            BLL.BaseBLL<Entity.SysRoute> bll = new BLL.BaseBLL<Entity.SysRoute>();
            var db_list = bll.GetListBy(new List<BLL.FilterSearch>());

            foreach (var item in db_list)
            {
                var entity = route_list.Where(p => p.IsPost == item.IsPost && p.Route == item.Route).FirstOrDefault();
                //如果数据库对应程序中不存在，则删除数据库里的
                if (entity == null)
                {
                    bll.Del(item);

                }
                else
                {
                    //否则修改数据库里的DES之类的辅助说明              
                    item.Desc = entity.Desc;
                    item.Tag = entity.Tag;
                    bll.Modify(item);
                }
            }

            foreach (var item in route_list)
            {
                var entity = bll.GetModel(p => p.IsPost == item.IsPost && p.Route == item.Route);
                if (entity == null)
                {
                    var route = new Entity.SysRoute();
                    route.AddTime = DateTime.Now;
                    route.Desc = item.Desc;
                    route.IsPost = item.IsPost;
                    route.Route = item.Route;
                    route.Tag = item.Tag;
                    bll.Add(route);
                }
            }
            AddAdminLogs(Entity.SysLogMethodType.Update, "更新权限数据");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
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
                entity = bll.GetModel(p => p.ID == num);
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
                if (bll.GetCount(p => p.RoleName == entity.RoleName) > 0)
                    ModelState.AddModelError("RoleName", "该组名已存在");
            }
            else
            {
                if (bll.GetCount(p => p.ID == entity.ID) == 0)
                    return PromptView("/admin/SysRole", "404", "Not Found", "该组不存在或已被删除", 5);

                var old_entity = bll.GetModel(p => p.ID == entity.ID);
                //验证组名是否存在
                if (old_entity.RoleName != entity.RoleName)
                {
                    if (bll.GetCount(p => p.RoleName == entity.RoleName) > 0)
                        ModelState.AddModelError("RoleName", "该组名已存在");
                }
            }

            if (ModelState.IsValid)
            {
                BLL.BLLSysRole bll_role = new BLL.BLLSysRole();
                if (entity.ID == 0)//添加
                    bll_role.Add(entity, qx);
                else //修改
                    bll_role.Modify(entity, qx);

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
                role = bll_route.GetModel(p => p.ID == id);

            List<Models.ViewModelTree> list = new List<Models.ViewModelTree>();
            var route_group = new BLL.BLLSysRoute().GetListGroupByTag();
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
                        model2.is_checked = bll_role_route.GetCount(p => p.SysRoleID == role.ID && p.SysRouteID == item.ID) > 0 ? true : false;

                    list.Add(model2);
                }

            }

            ViewData["Tree"] = Newtonsoft.Json.JsonConvert.SerializeObject(list).Replace("is_checked", "checked");
        }

    }
}