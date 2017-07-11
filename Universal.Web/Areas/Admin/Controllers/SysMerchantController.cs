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
    /// <summary>
    /// 商户管理
    /// </summary>
    public class SysMerchantController : BaseAdminController
    {
        /// <summary>
        /// 用户分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role">筛选：组ID</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("商户管理", "商户管理首页",true)]
        public ActionResult Index(int page = 1, string word = "")
        {
            ViewData["Domain"] = GetSiteUrl();
            ViewData["DESKEY"] = SiteKey.DES3KEY;
            word = WebHelper.UrlDecode(word);
            Models.ViewModelSysMerchantList response_model = new Models.ViewModelSysMerchantList();
            response_model.page = page;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie("sysmchindex"), SiteKey.AdminDefaultPageSize);
            
            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like));
            }


            BLL.BaseBLL<Entity.SysMerchant> bll = new BLL.BaseBLL<Entity.SysMerchant>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "AddTime desc");
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }


        [AdminPermissionAttribute("商户管理", "更新权限路由数据", true)]
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
                            IsSuperMch = attr.IsSuperMch,
                            IsPost = IsHttpPost,
                            Route = route_map
                        });
                    }
                }
            }
            #endregion

            BLL.BaseBLL<Entity.SysRoute> bll = new BLL.BaseBLL<Entity.SysRoute>();
            var db_list = bll.GetListBy(0, new List<BLL.FilterSearch>(), null);

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
                    item.IsSuperMch = entity.IsSuperMch;
                    bll.Modify(item);
                }
            }

            foreach (var item in route_list)
            {
                var entity = bll.GetModel(p => p.IsPost == item.IsPost && p.Route == item.Route, null);
                if (entity == null)
                {
                    var route = new Entity.SysRoute();
                    route.AddTime = DateTime.Now;
                    route.Desc = item.Desc;
                    route.IsPost = item.IsPost;
                    route.Route = item.Route;
                    route.Tag = item.Tag;
                    route.IsSuperMch = item.IsSuperMch;
                    bll.Add(route);
                }
            }
            AddAdminLogs(Entity.SysLogMethodType.Update, "更新权限数据");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [AdminPermissionAttribute("商户管理", "商户编辑页面",true)]
        public ActionResult Edit(int? id)
        {
            BLL.BaseBLL<Entity.SysMerchant> bll = new BLL.BaseBLL<Entity.SysMerchant>();
            Entity.SysMerchant entity = new Entity.SysMerchant();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/SysMerchant", "404", "Not Found", "信息不存在或已被删除", 5);
                }
            }
            return View(entity);
        }

        /// <summary>
        /// 保存商户
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminPermissionAttribute("商户管理", "保存商户编辑的信息",true)]
        public ActionResult Edit(Entity.SysMerchant entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            BLL.BaseBLL<Entity.SysMerchant> bll = new BLL.BaseBLL<Entity.SysMerchant>();

            //数据验证
            if (isAdd)
            {
                //判断用户名是否存在
                if (bll.Exists(p => p.Title == entity.Title))
                {
                    ModelState.AddModelError("Title", "该商户名已存在");
                }

            }
            else
            {
                //如果要编辑的用户不存在
                if (bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/SysMerchant", "404", "Not Found", "信息不存在或已被删除", 5);
                }
                ModelState.Remove("Title");
            }

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    int id = BLL.BLLMerchant.Add(entity);
                    entity.ID = id;
                    AddAdminLogs(Entity.SysLogMethodType.Add, "添加商户：" + entity.Title);
                }
                else //修改
                {
                    var user = bll.GetModel(p => p.ID == entity.ID, null);
                    user.Status = entity.Status;
                    user.Remark = entity.Remark;
                    //user.IsSuperMch = entity.IsSuperMch;
                    bll.Modify(user);
                    AddAdminLogs(Entity.SysLogMethodType.Update, "修改商户：" + entity.Title);
                }

                return PromptView("/admin/SysMerchant", "OK", "Success", "操作成功", 5);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 禁用商户
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("商户管理", "禁用商户",true)]
        public JsonResult ShutDown(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.SysMerchant> bll = new BLL.BaseBLL<Entity.SysMerchant>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            var temp_entity = new Entity.SysMerchant();
            temp_entity.Status = false;
            bll.ModifyBy(temp_entity, p => id_list.Contains(p.ID), "Status");
            AddAdminLogs(Entity.SysLogMethodType.Delete, "禁用商户：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }
    }
}