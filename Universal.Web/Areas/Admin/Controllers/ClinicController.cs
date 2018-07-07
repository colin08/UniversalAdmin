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
    /// 诊所管理管理
    /// </summary>
    public class ClinicController : BaseAdminController
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role">筛选：组ID</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("诊所管理", "诊所管理首页")]
        public ActionResult Index(int page = 1,int role=0, string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelClinicList response_model = new Models.ViewModelClinicList();
            response_model.page = page;
            response_model.role = role;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);
            Load();
            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (role > 0)
            {
                filter.Add(new BLL.FilterSearch("ClinicAreaID", role.ToString(), BLL.FilterSearchContract.等于));
            }
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like));
            }


            BLL.BaseBLL<Entity.Clinic> bll = new BLL.BaseBLL<Entity.Clinic>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "Weight desc", "ClinicArea");
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
        [AdminPermissionAttribute("诊所管理", "诊所管理编辑页面")]
        public ActionResult Edit(int? id)
        {
            Load();
            BLL.BaseBLL<Entity.Clinic> bll = new BLL.BaseBLL<Entity.Clinic>();
            Entity.Clinic entity = new Entity.Clinic();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/Clinic", "404", "Not Found", "信息不存在或已被删除", 5);
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
        [ValidateAntiForgeryToken,ValidateInput(false)]
        [AdminPermissionAttribute("诊所管理", "保存诊所管理编辑信息")]
        public ActionResult Edit(Entity.Clinic entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            Load();
            BLL.BaseBLL<Entity.Clinic> bll = new BLL.BaseBLL<Entity.Clinic>();
            //数据验证
            if (isAdd)
            {

            }
            else
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/Clinic", "404", "Not Found", "信息不存在或已被删除", 5);
                }

            }
            if(entity.ClinicAreaID == 0)
            {
                ModelState.AddModelError("ClinicAreaID", "所属区域必选");
            }

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    bll.Add(entity);
                }
                else //修改
                {
                    var data = bll.GetModel(p => p.ID == entity.ID, null);
                    data.Title = entity.Title;
                    data.Status = entity.Status;
                    data.Address = entity.Address;
                    data.ChengCheLuXian = entity.ChengCheLuXian;
                    data.ClinicAreaID = entity.ClinicAreaID;
                    data.FuWuXiangMu = entity.FuWuXiangMu;
                    data.FuWuYuYan = entity.FuWuYuYan;
                    data.ICON = entity.ICON;
                    data.Telphone = entity.Telphone;
                    data.Weight = entity.Weight;
                    data.WorkHours = entity.WorkHours;
                    bll.Modify(data);
                }

                return PromptView("/admin/Clinic", "OK", "Success", "操作成功", 5);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 禁用
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("诊所管理", "禁用诊所")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            var db_ids = string.Join(",", id_list);
            BLL.BLLClinic.DisEnble(db_ids);
            AddAdminLogs(Entity.SysLogMethodType.Delete, "禁用诊所：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        private void Load()
        {
            List<SelectListItem> dataList = new List<SelectListItem>();
            dataList.Add(new SelectListItem() { Text = "选择地区", Value = "0" });
            var db_list = new BLL.BaseBLL<Entity.ClinicArea>().GetListBy(0, p => p.Status, "Weight DESC");
            foreach (var item in db_list)
            {
                dataList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString() });
            }
            ViewData["AreaList"] = dataList;
        }

    }
}