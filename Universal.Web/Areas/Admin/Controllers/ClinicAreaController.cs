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
    /// 诊所地区
    /// </summary>
    public class ClinicAreaController : BaseAdminController
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role">筛选：组ID</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("诊所地区", "诊所地区首页")]
        public ActionResult Index(int page = 1, string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelClinicAreaList response_model = new Models.ViewModelClinicAreaList();
            response_model.page = page;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);
            
            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like));
            }


            BLL.BaseBLL<Entity.ClinicArea> bll = new BLL.BaseBLL<Entity.ClinicArea>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "Weight desc");
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
        [AdminPermissionAttribute("诊所地区", "诊所地区编辑页面")]
        public ActionResult Edit(int? id)
        {
            BLL.BaseBLL<Entity.ClinicArea> bll = new BLL.BaseBLL<Entity.ClinicArea>();
            Entity.ClinicArea entity = new Entity.ClinicArea();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/ClinicArea", "404", "Not Found", "信息不存在或已被删除", 5);
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
        [AdminPermissionAttribute("诊所地区", "保存诊所地区编辑信息")]
        public ActionResult Edit(Entity.ClinicArea entity)
        {
            var isAdd = entity.ID == 0 ? true : false;

            BLL.BaseBLL<Entity.ClinicArea> bll = new BLL.BaseBLL<Entity.ClinicArea>();
            //数据验证
            if (isAdd)
            {
                
            }
            else
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/ClinicArea", "404", "Not Found", "信息不存在或已被删除", 5);
                }

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
                    data.Weight = entity.Weight;
                    bll.Modify(data);
                }

                return PromptView("/admin/ClinicArea", "OK", "Success", "操作成功", 5);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 禁用
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("诊所地区", "禁用地区")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            var db_ids = string.Join(",", id_list);
            BLL.BLLClinicArea.DisEnble(db_ids);
            AddAdminLogs(Entity.SysLogMethodType.Delete, "禁用诊所地区：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

    }
}