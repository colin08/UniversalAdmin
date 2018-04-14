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
    /// 体检套餐项操作
    /// </summary>
    public class MedicalItemController : BaseAdminController
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("体检套餐项", "体检套餐项首页")]
        public ActionResult Index(int page = 1,string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelMedicalItemList response_model = new Models.ViewModelMedicalItemList();
            response_model.page = page;
            response_model.word = word;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);
            

            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("OnlyID", word, BLL.FilterSearchContract.like));
                filter.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like, BLL.FilterRelate.Or));
            }


            BLL.BaseBLL<Entity.MedicalItem> bll = new BLL.BaseBLL<Entity.MedicalItem>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "Weight desc");
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        /// <summary>
        /// 禁用用户
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("体检套餐项", "首页禁用项")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            var db_ids = string.Join(",", id_list);
            BLL.BLLMedicalItem.DisEnble(db_ids);
            AddAdminLogs(Entity.SysLogMethodType.Delete, "禁用套餐项：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [AdminPermissionAttribute("体检套餐项", "体检套餐项编辑页面")]
        public ActionResult Edit(int? id)
        {
            BLL.BaseBLL<Entity.MedicalItem> bll = new BLL.BaseBLL<Entity.MedicalItem>();
            Entity.MedicalItem entity = new Entity.MedicalItem();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/MedicalItem", "404", "Not Found", "信息不存在或已被删除", 5);
                }
            }
            return View(entity);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminPermissionAttribute("体检套餐项", "保存体检套餐项编辑信息")]
        public ActionResult Edit(Entity.MedicalItem entity)
        {
            var isAdd = entity.ID == 0 ? true : false;

            BLL.BaseBLL<Entity.MedicalItem> bll = new BLL.BaseBLL<Entity.MedicalItem>();

            //数据验证
            if (isAdd)
            {
                if(bll.Exists(p=>p.Title == entity.Title))
                {
                    ModelState.AddModelError("Title", "该名称已存在");
                }
                if(bll.Exists(p=>p.OnlyID == entity.OnlyID))
                {
                    ModelState.AddModelError("OnlyID", "编号已存在");
                }
            }
            else
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/MedicalItem", "404", "Not Found", "信息不存在或已被删除", 5);
                }

            }

            if (ModelState.IsValid)
            {
                var pinyin = Hz2Py.ConvertToPin(entity.Title);
                if(!string.IsNullOrWhiteSpace(pinyin))
                {
                    entity.SZM = pinyin.Substring(0, 1);
                }else
                {
                    entity.SZM = "&";
                }
                //添加
                if (entity.ID == 0)
                {
                    bll.Add(entity);
                }
                else //修改
                {
                    var user = bll.GetModel(p => p.ID == entity.ID, null);
                    user.Weight = entity.Weight;
                    user.Status = entity.Status;
                    user.OnlyID = entity.OnlyID;
                    user.Title = entity.Title;
                    user.Price = entity.Price;
                    user.Weight = entity.Weight;
                    user.Desc = entity.Desc;

                    bll.Modify(user);
                }

                return PromptView("/admin/MedicalItem", "OK", "Success", "操作成功", 5);
            }
            else
                return View(entity);
        }



    }
}