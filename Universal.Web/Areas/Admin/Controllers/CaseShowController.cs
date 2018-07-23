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
    /// 
    /// </summary>
    public class CaseShowController : BaseAdminController
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="cid"></param>
        /// <param name="type"></param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("案例展示", "案例展示首页")]
        public ActionResult Index(int page = 1, int cid = 0, int type = 0, string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelCaseShowList response_model = new Models.ViewModelCaseShowList();
            response_model.page = page;
            response_model.word = word;
            response_model.cid = cid;
            response_model.type = type;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);
            Load();
            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (cid > 0)
            {
                filter.Add(new BLL.FilterSearch("CategoryID", cid.ToString(), BLL.FilterSearchContract.等于));
            }
            if (type > 0)
            {
                filter.Add(new BLL.FilterSearch("Type", type.ToString(), BLL.FilterSearchContract.等于));
            }
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like));
            }


            BLL.BaseBLL<Entity.CaseShow> bll = new BLL.BaseBLL<Entity.CaseShow>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "Weight desc", "Category");
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [AdminPermissionAttribute("案例展示", "案例展示编辑页面")]
        public ActionResult Edit(int? id)
        {
            Load();
            Entity.CaseShow entity = new Entity.CaseShow();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                string tw_ids = "";
                entity = BLL.BLLCaseShow.GetModel(num, out tw_ids);
                if (entity == null)
                {
                    return PromptView("/admin/CaseShow", "404", "Not Found", "信息不存在或已被删除", 3);
                }
                LoadTeamWork(tw_ids);
            }
            else
            {
                LoadTeamWork();
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
        [AdminPermissionAttribute("案例展示", "保存案例展示编辑信息")]
        public ActionResult Edit(Entity.CaseShow entity)
        {
            var isAdd = entity.ID == 0 ? true : false;
            
            if (string.IsNullOrWhiteSpace(entity.Title))
            {
                ModelState.AddModelError("Title", "标题必填");
            }
            if(entity.Type == 0)
            {
                ModelState.AddModelError("Type", "类别必选");
            }
            if(entity.CategoryID == 0)
            {
                ModelState.AddModelError("CategoryID", "所属栏目必选");
            }


            //数据验证
            if (isAdd)
            {

            }
            else
            {
                //如果要编辑的用户不存在
                if (!new BLL.BaseBLL<Entity.CaseShow>().Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/CaseShow", "404", "Not Found", "信息不存在或已被删除", 3);
                }
            }

            var tw_ids = WebHelper.GetFormString("hid_tw_ids");

            if (ModelState.IsValid)
            {
                //添加
                if (entity.ID == 0)
                {
                    entity.AddUserID = WorkContext.UserInfo.ID;
                    entity.LastUpdateUserID = WorkContext.UserInfo.ID;
                    BLL.BLLCaseShow.Add(entity, tw_ids);
                    AddAdminLogs(Entity.SysLogMethodType.Add, "添加案例展示：" + entity.Title + "");
                }
                else //修改
                {
                    entity.AddUserID = WorkContext.UserInfo.ID;
                    entity.LastUpdateUserID = WorkContext.UserInfo.ID;
                    BLL.BLLCaseShow.Modify(entity, tw_ids);
                    AddAdminLogs(Entity.SysLogMethodType.Update, "修改案例展示：" + entity.ID.ToString() + "");
                }

                return PromptView("/admin/CaseShow", "OK", "Success", "操作成功", 3);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("案例展示", "删除案例展示")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.CaseShow> bll = new BLL.BaseBLL<Entity.CaseShow>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            AddAdminLogs(Entity.SysLogMethodType.Delete, "删除案例展示：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }

        private void Load()
        {
            List<SelectListItem> dataList = new List<SelectListItem>();
            dataList.Add(new SelectListItem() { Text = "全部栏目", Value = "0" });            
            foreach (var item in BLL.BLLCategory.GetCaseShowCategory())
            {
                dataList.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["Category_role"] = dataList;


            List<SelectListItem> data2List = new List<SelectListItem>();
            data2List.Add(new SelectListItem() { Text = "全部类别", Value = "0" });
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.CaseShowType)))
            {
                string text = EnumHelper.GetDescription<Entity.CaseShowType>((Entity.CaseShowType)item.Key);
                data2List.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["Category_type"] = data2List;

        }

        /// <summary>
        /// 加载合作企业
        /// </summary>
        private void LoadTeamWork(string ids = "")
        {
            // 1,2,3
            if (!string.IsNullOrWhiteSpace(ids))
            {
                ids = "," + ids + ",";
                ids = ids.Replace(" ", "");
            }
            else
                ids = "";

            List<SelectListItem> dataList = new List<SelectListItem>();

            BLL.BaseBLL<Entity.TeamWork> bll_team_work = new BLL.BaseBLL<Entity.TeamWork>();
            System.Text.StringBuilder sb_ids = new System.Text.StringBuilder();
            foreach (var item in bll_team_work.GetListBy(0, p => p.Status, "Weight"))
            {
                bool is_select = ids.IndexOf("," + item.ID.ToString() + ",") != -1;
                if (is_select) sb_ids.Append(item.ID.ToString() + ",");
                dataList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString(), Selected = is_select });
            }
            if (sb_ids.Length > 0) sb_ids.Remove(sb_ids.Length - 1, 1);
            ViewData["TWIDS"] = sb_ids.ToString();
            ViewData["TeamWorkList"] = dataList;

        }

    }
}