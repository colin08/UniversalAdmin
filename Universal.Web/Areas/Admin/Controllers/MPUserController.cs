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
    /// <summary>
    /// 微信用户
    /// </summary>
    public class MPUserController : BaseAdminController
    {
        /// <summary>
        /// 用户分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role">筛选：组ID</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("微信用户", "微信用户首页")]
        public ActionResult Index(int page = 1, int role = 0, string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelMPUserList response_model = new Models.ViewModelMPUserList();
            response_model.page = page;
            response_model.role = role;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);

            Load();

            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (role != 0)
                filter.Add(new BLL.FilterSearch("Identity", role.ToString(), BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("RealName", word, BLL.FilterSearchContract.like));
                filter.Add(new BLL.FilterSearch("IDCardNumber", word, BLL.FilterSearchContract.like, BLL.FilterRelate.Or));
                filter.Add(new BLL.FilterSearch("Telphone", word, BLL.FilterSearchContract.like, BLL.FilterRelate.Or));
            }


            BLL.BaseBLL<Entity.MPUser> bll = new BLL.BaseBLL<Entity.MPUser>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "AddTime desc");
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [AdminPermissionAttribute("微信用户", "微信用户编辑页面")]
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return PromptView("/admin/MPUser", "Error", "Error", "微信用户不支持后台添加", 5);
            }

            BLL.BaseBLL<Entity.MPUser> bll = new BLL.BaseBLL<Entity.MPUser>();
            Load();
            Entity.MPUser entity = new Entity.MPUser();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/MPUser", "404", "Not Found", "信息不存在或已被删除", 5);
                }
            }
            return View(entity);
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminPermissionAttribute("微信用户", "保存微信用户编辑信息")]
        public ActionResult Edit(Entity.MPUser entity)
        {
            var isAdd = entity.ID == 0 ? true : false;

            BLL.BaseBLL<Entity.MPUser> bll = new BLL.BaseBLL<Entity.MPUser>();
            Load();

            if (entity.Identity == 0)
            {
                ModelState.AddModelError("Identity", "请选择用户身份");
            }

            //数据验证
            if (isAdd)
            {

            }
            else
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/MPUser", "404", "Not Found", "信息不存在或已被删除", 5);
                }

            }

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    

                }
                else //修改
                {
                    var user = bll.GetModel(p => p.ID == entity.ID, null);
                    user.Brithday = entity.Brithday;
                    user.IDCardNumber = entity.IDCardNumber;
                    user.IsFullInfo = true;
                    user.RealName = entity.RealName;
                    user.Telphone = entity.Telphone;
                    user.Weight = entity.Weight;
                    user.Gender = entity.Gender;
                    user.Status = entity.Status;
                    user.Avatar = entity.Avatar;
                    user.Identity = entity.Identity;
                    bll.Modify(user);
                }

                return PromptView("/admin/MPUser", "OK", "Success", "操作成功", 5);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 禁用用户
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("微信用户", "首页禁用用户")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            var db_ids = string.Join(",", id_list);
            BLL.BLLMPUser.DisEnbleUser(db_ids);
            AddAdminLogs(Entity.SysLogMethodType.Delete, "禁用微信用户：" + ids + "");

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
            userRoleList.Add(new SelectListItem() { Text = "选择身份", Value = "0" });
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.MPUserIdentity)))
            {
                string text = EnumHelper.GetDescription((Entity.MPUserIdentity)item.Key);
                userRoleList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["IdentityList"] = userRoleList;
        }

    }
}