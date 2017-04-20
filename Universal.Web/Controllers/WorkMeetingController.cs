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
    public class WorkMeetingController : BaseHBLController
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
            BLL.BaseBLL<Entity.WorkMeeting> bll = new BLL.BaseBLL<Entity.WorkMeeting>();
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            filter.Add(new BLL.FilterSearch("CusUserID", WorkContext.UserInfo.ID.ToString(), BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(keyword))
                filter.Add(new BLL.FilterSearch("Title", keyword, BLL.FilterSearchContract.like));
            List<Entity.WorkMeeting> list = bll.GetPagedList(page_index, page_size, ref rowCount, filter, "AddTime desc");
            WebAjaxEntity<List<Entity.WorkMeeting>> result = new WebAjaxEntity<List<Entity.WorkMeeting>>();
            result.msg = 1;
            result.msgbox = CalculatePage(rowCount, page_size).ToString();
            result.data = list;
            result.total = rowCount;

            return Json(result);
        }

        /// <summary>
        /// 用户点击参会
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Join(int id)
        {
            BLL.BLLWorkMeeting.Join(id, WorkContext.UserInfo.ID);
            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);
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
            BLL.BaseBLL<Entity.WorkMeeting> bll = new BLL.BaseBLL<Entity.WorkMeeting>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);

        }

        ///// <summary>
        ///// 会议详情
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public ActionResult Info(int id)
        //{
        //    var entity = BLL.BLLWorkMeeting.GetModel(id);
        //    if (entity == null)
        //    {
        //        return View("NotFound");
        //    }


        //    return View(entity);
        //}


        public ActionResult Modify(int? id)
        {
            LoadStatus();
            int ids = TypeHelper.ObjectToInt(id);
            Models.ViewModelWorkMeeting model = new Models.ViewModelWorkMeeting();
            if (ids != 0)
            {
                Entity.WorkMeeting entity = BLL.BLLWorkMeeting.GetModel(ids);
                if (entity != null)
                {
                    model.content = entity.Content;
                    model.title = entity.Title;
                    model.begin_time = entity.BeginTime;
                    model.location = entity.Location;
                    model.end_time = entity.EndTime;
                    model.id = ids;
                    System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
                    foreach (var item in entity.WorkMeetingUsers)
                    {
                        str_ids.Append(item.ID.ToString() + ",");
                        model.users_entity.Add(new Models.ViewModelDocumentCategory(item.CusUser.ID, item.CusUser.NickName));
                    }
                    if (str_ids.Length > 0)
                    {
                        str_ids = str_ids.Remove(str_ids.Length - 1, 1);
                    }
                    model.user_ids = str_ids.ToString();

                    model.BuildViewModelListFile(entity.FileList.ToList());

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
        public ActionResult Modify(Models.ViewModelWorkMeeting entity)
        {
            LoadStatus();
            var isAdd = entity.id == 0 ? true : false;
            

            BLL.BaseBLL<Entity.WorkMeeting> bll = new BLL.BaseBLL<Entity.WorkMeeting>();
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
                entity.users_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.NickName));
            }
            string final_ids = "";
            if (str_ids.Length > 0)
            {
                final_ids = str_ids.Remove(str_ids.Length - 1, 1).ToString();
            }
            #endregion



            if (ModelState.IsValid)
            {

                Entity.WorkMeeting model = null;

                if (isAdd)
                {
                    model = new Entity.WorkMeeting();
                    model.CusUserID = WorkContext.UserInfo.ID;
                }
                else
                {
                    model = bll.GetModel(p => p.ID == entity.id);
                }
                model.Location = entity.location;
                model.Content = entity.content;
                model.Title = entity.title;
                model.EndTime = entity.end_time;
                model.BeginTime = entity.begin_time;
                model.FileList = entity.BuildFileList();
                if (isAdd)
                    BLL.BLLWorkMeeting.Add(model,final_ids);
                else
                    BLL.BLLWorkMeeting.Modify(model, final_ids);

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