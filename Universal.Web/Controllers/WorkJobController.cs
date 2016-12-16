using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using System.Data.Entity;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 秘籍
    /// </summary>
    [BasicUserAuth]
    public class WorkJobController : BaseHBLController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PageData(int page_size, int page_index, string keyword)
        {
            BLL.BaseBLL<Entity.WorkJob> bll = new BLL.BaseBLL<Entity.WorkJob>();
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (!string.IsNullOrWhiteSpace(keyword))
                filter.Add(new BLL.FilterSearch("Title", keyword, BLL.FilterSearchContract.like));
            List<Entity.WorkJob> list = bll.GetPagedList(page_index, page_size, ref rowCount, filter, "AddTime desc");
            WebAjaxEntity<List<Entity.WorkJob>> result = new WebAjaxEntity<List<Entity.WorkJob>>();
            result.msg = 1;
            result.msgbox = CalculatePage(rowCount, page_size).ToString();
            result.data = list;
            result.total = rowCount;

            return Json(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.WorkJob> bll = new BLL.BaseBLL<Entity.WorkJob>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);

        }

        public ActionResult Modify(int? id)
        {
            LoadStatus();
            int ids = TypeHelper.ObjectToInt(id);
            Models.ViewModelWorkJob model = new Models.ViewModelWorkJob();
            if (ids != 0)
            {
                BLL.BaseBLL<Entity.WorkJob> bll = new BLL.BaseBLL<Entity.WorkJob>();
                Entity.WorkJob entity = bll.GetModel(p => p.ID == id,p=>p.WorkJobUsers.Select(s=>s.CusUser));
                if (entity != null)
                {
                    model.content = entity.Content;
                    model.title = entity.Title;
                    DateTime dt = TypeHelper.ObjectToDateTime(entity.DoneTime);
                    model.year = dt.Year.ToString();
                    model.month = dt.Month.ToString();
                    model.day = dt.Day.ToString();
                    model.status = entity.Status;
                    model.id = ids;
                    System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
                    foreach (var item in entity.WorkJobUsers)
                    {
                        str_ids.Append(item.ID.ToString() + ",");
                        model.users_entity.Add(new Models.ViewModelDocumentCategory(item.CusUser.ID, item.CusUser.Telphone + "(" + item.CusUser.NickName + ")"));
                    }
                    if (str_ids.Length > 0)
                    {
                        str_ids = str_ids.Remove(str_ids.Length - 1, 1);
                    }
                    model.user_ids = str_ids.ToString();
                }
                else
                {
                    model.Msg = 2;
                    model.MsgBox = "数据不存在";
                }

            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Modify(Models.ViewModelWorkJob entity)
        {
            LoadStatus();
            var isAdd = entity.id == 0 ? true : false;
            

            BLL.BaseBLL<Entity.WorkJob> bll = new BLL.BaseBLL<Entity.WorkJob>();
            if (!isAdd)
            {
                if (!bll.Exists(p => p.ID == entity.id))
                {
                    entity.Msg = 2;
                    ModelState.AddModelError("title", "信息不存在");
                }
            }

            #region 处理用户ID
            System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
            entity.users_entity = new List<Models.ViewModelDocumentCategory>();
            foreach (var item in BLL.BLLCusUser.GetListByIds(entity.user_ids))
            {
                str_ids.Append(item.ID.ToString() + ",");
                entity.users_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.Telphone + "(" + item.NickName + ")"));
            }
            string final_ids = "";
            if (str_ids.Length > 0)
            {
                final_ids = str_ids.Remove(str_ids.Length - 1, 1).ToString();
            }
            #endregion

            if (ModelState.IsValid)
            {

                Entity.WorkJob model = null;

                if (isAdd)
                {
                    model = new Entity.WorkJob();
                    model.CusUserID = WorkContext.UserInfo.ID;
                }
                else
                {
                    model = bll.GetModel(p => p.ID == entity.id);
                    model.Status = entity.status;
                }

                model.Content = entity.content;
                model.Title = entity.title;
                if (!string.IsNullOrWhiteSpace(entity.year) && !string.IsNullOrWhiteSpace(entity.month) && !string.IsNullOrWhiteSpace(entity.day))
                    model.DoneTime = TypeHelper.ObjectToDateTime(entity.year + "/" + entity.month + "/" + entity.day);

                if (isAdd)
                    BLL.BLLWorkJob.Add(model,final_ids);
                else
                    BLL.BLLWorkJob.Modify(model, final_ids);

                entity.Msg = 1;
            }
            else
            {
                entity.Msg = 3;
            }

            return View(entity);
        }


        private void LoadStatus()
        {
            List<SelectListItem> StatusList = new List<SelectListItem>();
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.WorkStatus)))
            {
                string text = EnumHelper.GetDescription<Entity.WorkStatus>((Entity.WorkStatus)item.Key);
                StatusList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["StatusList"] = StatusList;
        }

    }
}