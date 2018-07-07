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
    /// 体检套餐
    /// </summary>
    public class MedicalController : BaseAdminController
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role"></param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("体检套餐", "体检套餐首页")]
        public ActionResult Index(int page = 1,int role=0, string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelMedicalList response_model = new Models.ViewModelMedicalList();
            response_model.page = page;
            response_model.word = word;
            response_model.role = role;
            Load();
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);
            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (role != 0)
                filter.Add(new BLL.FilterSearch("Status", role.ToString(), BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like));
            }


            BLL.BaseBLL<Entity.Medical> bll = new BLL.BaseBLL<Entity.Medical>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "Weight desc");
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("体检套餐", "首页禁用体检套餐")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            var db_ids = string.Join(",", id_list);
            BLL.BLLMedical.DisEnble(db_ids);
            AddAdminLogs(Entity.SysLogMethodType.Delete, "禁用套餐：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [AdminPermissionAttribute("体检套餐", "体检套餐编辑页面")]
        public ActionResult Edit(int? id)
        {
            Load();
            if (id == null)
                GetTree();
            else
                GetTree(TypeHelper.ObjectToInt(id, 0));
            BLL.BaseBLL<Entity.Medical> bll = new BLL.BaseBLL<Entity.Medical>();
            Entity.Medical entity = new Entity.Medical();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/Medical", "404", "Not Found", "信息不存在或已被删除", 5);
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
        [ValidateAntiForgeryToken,ValidateInput(false)]
        [AdminPermissionAttribute("体检套餐", "保存体检套餐编辑信息")]
        public ActionResult Edit(Entity.Medical entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            Load();
            GetTree(entity.ID);
            //选中的项
            var select_items = WebHelper.GetFormString("hid_items");
            BLL.BaseBLL<Entity.Medical> bll = new BLL.BaseBLL<Entity.Medical>();
            //数据验证
            if (isAdd)
            {
                if (bll.Exists(p => p.Title == entity.Title))
                {
                    ModelState.AddModelError("Title", "该名称已存在");
                }
            }
            else
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/Medical", "404", "Not Found", "信息不存在或已被删除", 5);
                }

            }

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    BLL.BLLMedical.Add(entity, select_items);
                }
                else //修改
                {
                    BLL.BLLMedical.Modify(entity, select_items);
                }

                return PromptView("/admin/Medical", "OK", "Success", "操作成功", 5);
            }
            else
                return View(entity);
        }

        /// <summary>
        /// 加载用户组
        /// </summary>
        private void Load()
        {
            List<SelectListItem> userRoleList = new List<SelectListItem>();
            userRoleList.Add(new SelectListItem() { Text = "选择状态", Value = "0" });
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.MedicalStatus)))
            {
                string text = EnumHelper.GetDescription((Entity.MedicalStatus)item.Key);
                userRoleList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["StatusList"] = userRoleList;
        }

        /// <summary>
        /// 获取树数据
        /// </summary>
        /// <param name="id">当前组ID，没有传0</param>
        private void GetTree(int id = 0)
        {
            BLL.BaseBLL<Entity.MedicalItem> bll_item = new BLL.BaseBLL<Entity.MedicalItem>();
            var db_item = bll_item.GetListBy(0, p => p.Status, "Weight DESC");
            var db_data = BLL.BLLMedicalItemCategory.GetAllData();
            List<Entity.MedicalItem> check_list = new List<Entity.MedicalItem>();
            if(id != 0) check_list = BLL.BLLMedicalItem.GetItemByMedicalID(id);
            List<Models.ViewModelTree> list = new List<Models.ViewModelTree>();
            decimal total_amount = 0;
            System.Text.StringBuilder ids = new System.Text.StringBuilder();
            foreach (var category in db_data)
            {
                Models.ViewModelTree model = new Models.ViewModelTree();
                model.id = category.ID;
                model.price = 0;
                model.name = category.Title;
                model.open = true;
                model.pId = 0;
                list.Add(model);

                foreach (var item in category.MedicalItems.Where(p=>p.Status).OrderByDescending(p=>p.Weight))
                {
                    Models.ViewModelTree model_item = new Models.ViewModelTree();
                    model_item.id = item.ID;
                    model_item.price = item.Price;
                    model_item.name = item.Title + "  ￥：" + Tools.WebHelper.FormatDecimalMoney(item.Price);
                    model_item.open = false;
                    model_item.pId = category.ID;
                    if (check_list.Any(p => p.ID == item.ID))
                    {
                        ids.Append(item.ID.ToString() + ",");
                        model_item.is_checked = true;
                        total_amount += item.Price;
                    }
                    list.Add(model_item);
                }

            }
            //foreach (var item in db_item)
            //{
            //    Models.ViewModelTree model = new Models.ViewModelTree();
            //    model.id = item.ID;
            //    model.price = item.Price;
            //    model.name = item.Title + "  ￥：" + item.Price.ToString("F2");
            //    model.open = false;
            //    model.pId = 0;
            //    if (check_list.Any(p => p.ID == item.ID))
            //    {
            //        ids.Append(item.ID.ToString() + ",");
            //        model.is_checked = true;
            //        total_amount += item.Price;
            //    }
            //    list.Add(model);
            //}
            if(ids.Length>0)
            {
                ViewData["SelectIDS"] = ids.Remove(ids.Length - 1, 1).ToString();
            }
            
            ViewData["TotalAmount"] = WebHelper.FormatDecimalMoney(total_amount);
            ViewData["Tree"] = JsonHelper.ToJson(list).Replace("is_checked", "checked");
        }

    }
}