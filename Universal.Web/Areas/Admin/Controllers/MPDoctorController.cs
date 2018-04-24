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
    /// 医生管理
    /// </summary>
    public class MPDoctorController : BaseAdminController
    {
        /// <summary>
        /// 用户分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role">筛选：组ID</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("医生管理", "医生管理首页")]
        public ActionResult Index(int page = 1,int role=0, string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelMPDoctorList response_model = new Models.ViewModelMPDoctorList();
            response_model.page = page;
            response_model.word = word;
            response_model.role = role;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);

            Load();

            int total = 0;

            var list = BLL.BLLMPDoctor.GetPageList(response_model.page_size, response_model.page, role, word, out total);
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
        [AdminPermissionAttribute("医生管理", "医生管理编辑页面")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return PromptView("/admin/MPDoctor", "Error", "Error", "医生管理不支持后台添加", 5);
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
                    return PromptView("/admin/MPDoctor", "404", "Not Found", "信息不存在或已被删除", 5);
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
        [AdminPermissionAttribute("医生管理", "保存医生管理编辑信息")]
        public ActionResult Edit(Entity.MPUser entity)
        {
            var isAdd = entity.ID == 0 ? true : false;

            BLL.BaseBLL<Entity.MPUser> bll = new BLL.BaseBLL<Entity.MPUser>();
            Load();

            entity.Identity = Entity.MPUserIdentity.Doctors;

            //数据验证
            if (isAdd)
            {

            }
            else
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/MPDoctor", "404", "Not Found", "信息不存在或已被删除", 5);
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

                return PromptView("/admin/MPDoctor", "OK", "Success", "操作成功", 5);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 禁用用户
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("医生管理", "首页禁用用户")]
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
            AddAdminLogs(Entity.SysLogMethodType.Delete, "禁用医生管理：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }



        /// <summary>
        /// 加载医院列表
        /// </summary>
        private void Load()
        {
            List<SelectListItem> dataList = new List<SelectListItem>();
            dataList.Add(new SelectListItem() { Text = "选择诊所", Value = "0" });
            var db_list = new BLL.BaseBLL<Entity.Clinic>().GetListBy(0, p => p.Status, "Weight DESC");

            foreach (var item in db_list)
            {
                dataList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString() });
            }
            ViewData["ClinicList"] = dataList;
        }

    }
}