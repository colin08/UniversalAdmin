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

            var db = new DataCore.EFDBContext();
            var db_list = db.SysRoutes.ToList();

            foreach (var item in db_list)
            {
                var entity = route_list.Where(p => p.IsPost == item.IsPost && p.Route == item.Route).FirstOrDefault();
                //如果数据库对应程序中不存在，则删除数据库里的
                if(entity == null)
                {
                    db.SysRoutes.Remove(item);

                }else
                {
                    //否则修改数据库里的DES之类的辅助说明              
                    item.Desc = entity.Desc;
                    item.Tag = entity.Tag;
                }
            }

            foreach (var item in route_list)
            {
                var entity = db.SysRoutes.Where(p => p.IsPost == item.IsPost && p.Route == item.Route).FirstOrDefault();
                if (entity == null)
                {
                    var route = new DataCore.Entity.SysRoute();
                    route.AddTime = DateTime.Now;
                    route.Desc = item.Desc;
                    route.IsPost = item.IsPost;
                    route.Route = item.Route;
                    route.Tag = item.Tag;
                    db.SysRoutes.Add(route);
                }
            }
            
            db.SaveChanges();
            db.Dispose();

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        [AdminPermissionAttribute("用户组", "用户组信息编辑页面")]
        public ActionResult Edit(int? id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                if (id == null)
                    GetTree(db);
                else
                    GetTree(db, TypeHelper.ObjectToInt(id, 0));

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

        [AdminPermissionAttribute("用户组", "保存用户组编辑的信息")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DataCore.Entity.SysRole entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            var db = new DataCore.EFDBContext();
            GetTree(db, entity.ID);

            var qx = WebHelper.GetFormString("hid_qx");
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
                if (!isAdd)
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
                    if (!string.IsNullOrWhiteSpace(qx))
                    {
                        foreach (var item in qx.Split(','))
                        {
                            int route_id = TypeHelper.ObjectToInt(item);
                            db.SysRoleRoutes.Add(
                                new DataCore.Entity.SysRoleRoute() { SysRole = entity, SysRouteID = route_id }
                            );
                        }
                    }
                    db.SysRoles.Add(entity);

                }
                else //修改
                {
                    role.RoleName = entity.RoleName;
                    role.RoleDesc = entity.RoleDesc;
                    role.IsAdmin = entity.IsAdmin;
                    if (string.IsNullOrWhiteSpace(qx))
                    {
                        var list = db.SysRoleRoutes.Where(p => p.SysRoleID == role.ID).ToList();
                        list.ForEach(p => db.SysRoleRoutes.Remove(p));
                    }
                    else
                    {
                        List<int> new_id_list = qx.Split(',').Select(Int32.Parse).ToList();
                        var route_list = db.SysRoleRoutes.Where(p => p.SysRoleID == role.ID).ToList();
                        List<int> route_id_list = new List<int>();
                        foreach (var item in route_list)
                        {
                            route_id_list.Add(item.SysRouteID);
                        }
                        //判断存在的差
                        var route_del_list = route_id_list.Except(new_id_list).ToList();
                        foreach (var item in route_del_list)
                        {
                            //删除
                            var del_entity = db.SysRoleRoutes.Where(p => p.SysRouteID == item && p.SysRoleID == role.ID).FirstOrDefault();
                            db.SysRoleRoutes.Remove(del_entity);
                        }

                        var route_add_list = new_id_list.Except(route_id_list).ToList();
                        foreach (var item in route_add_list)
                        {
                            //做增加
                            db.SysRoleRoutes.Add(new DataCore.Entity.SysRoleRoute() { SysRoleID = entity.ID, SysRouteID = item });
                        }
                    }
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
        
        [HttpPost]
        [AdminPermissionAttribute("用户组", "删除用户组信息")]
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

        /// <summary>
        /// 获取树数据
        /// </summary>
        /// <param name="id">当前组ID，没有传0</param>
        private void GetTree(DataCore.EFDBContext db, int id = 0)
        {
            DataCore.Entity.SysRole role = null;
            if (id != 0)
            {
                role = db.SysRoles.Find(id);
            }
            List<Models.ViewModelTree> list = new List<Models.ViewModelTree>();
            var query = db.SysRoutes.GroupBy(p => p.Tag).ToList();
            for (int i = 0; i < query.Count; i++)
            {
                int top_id = i + 10000;
                Models.ViewModelTree model = new Models.ViewModelTree();
                model.id = top_id;
                model.name = query[i].Key;
                model.open = i < 4 ? true : false;
                model.pId = 0;
                list.Add(model);
                foreach (var item in query[i].ToList())
                {
                    Models.ViewModelTree model2 = new Models.ViewModelTree();
                    model2.id = item.ID;
                    model2.name = item.Desc;
                    model2.open = false;
                    model2.pId = top_id;
                    if (role != null)
                        model2.is_checked = db.SysRoleRoutes.Count(p => p.SysRoleID == role.ID && p.SysRouteID == item.ID) > 0 ? true : false;

                    list.Add(model2);
                }

            }

            ViewData["Tree"] = Newtonsoft.Json.JsonConvert.SerializeObject(list).Replace("is_checked", "checked");
        }

    }
}