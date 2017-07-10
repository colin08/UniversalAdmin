using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using System.Data.Entity;
using Universal.Web.Framework;
using Newtonsoft.Json;

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
            int rowCount = 0;
            var list = BLL.BLLWorkJob.GetPageList(page_size, page_index, WorkContext.UserInfo.ID, keyword, out rowCount);
            WebAjaxEntity<List<Entity.WorkJob>> result = new WebAjaxEntity<List<Entity.WorkJob>>();
            result.msg = 1;
            result.msgbox = CalculatePage(rowCount, page_size).ToString();
            result.data = list;
            result.total = rowCount;
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = "yyyy-MM-dd"
            };

            var ret = JsonConvert.SerializeObject(result, setting);

            return Json(ret);
        }

        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Info(int id)
        {
            Entity.WorkJob entity = BLL.BLLWorkJob.GetModel(id);
            if (entity == null)
                return View("NotFound");
            BLL.BaseBLL<Entity.WorkJobUserFile> bll = new BLL.BaseBLL<Entity.WorkJobUserFile>();
            foreach (var item in entity.WorkJobUsers)
            {
                var files = bll.GetListBy(0, p => p.WorkJobUserID == item.ID, "ID ASC");
                foreach (var file in files)
                {
                    file.FilePath = "/Tools/Down?path=" + file.FilePath + "&name=" + file.FileName;
                }
                item.ConfrimFiles = files;
            }
            
            return View(entity);
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

        /// <summary>
        /// 用户点击完成
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Confirm(int id,string text,string files)
        {
            List<Entity.WorkJobUserFile> list = new List<Entity.WorkJobUserFile>();
            foreach (var item in files.Split('|'))
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                var file = item.Split(',');
                if (file.Length != 3)
                    continue;
                Entity.WorkJobUserFile entity_file = new Entity.WorkJobUserFile();
                entity_file.FilePath = file[0];
                entity_file.FileName = file[1];
                entity_file.FileSize = file[2];
                list.Add(entity_file);
            }
            BLL.BLLWorkJob.Confirm(id, WorkContext.UserInfo.ID, text, list);
            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);
        }

        public ActionResult Modify(int? id)
        {
            int ids = TypeHelper.ObjectToInt(id);
            Models.ViewModelWorkJob model = new Models.ViewModelWorkJob();
            if (ids != 0)
            {
                Entity.WorkJob entity = BLL.BLLWorkJob.GetModel(ids);
                if (entity != null)
                {
                    model.content = entity.Content;
                    model.title = entity.Title;
                    DateTime dt = TypeHelper.ObjectToDateTime(entity.DoneTime);
                    model.year = dt.Year.ToString();
                    model.month = dt.Month.ToString();
                    model.day = dt.Day.ToString();
                    model.id = ids;
                    System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
                    foreach (var item in entity.WorkJobUsers)
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
        public ActionResult Modify(Models.ViewModelWorkJob entity)
        {
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

                Entity.WorkJob model = null;

                if (isAdd)
                {
                    model = new Entity.WorkJob();
                    model.CusUserID = WorkContext.UserInfo.ID;
                }
                else
                    model = bll.GetModel(p => p.ID == entity.id);

                model.Content = entity.content;
                model.Title = entity.title;
                if (!string.IsNullOrWhiteSpace(entity.year) && !string.IsNullOrWhiteSpace(entity.month) && !string.IsNullOrWhiteSpace(entity.day))
                    model.DoneTime = TypeHelper.ObjectToDateTime(entity.year + "/" + entity.month + "/" + entity.day);

                model.FileList = entity.BuildFileList();
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
        
    }
}