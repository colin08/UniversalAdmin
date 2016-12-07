using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 节点
    /// </summary>
    [BasicAdminAuth]
    public class NodeController : BaseHBLController
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
            BLL.BaseBLL<Entity.Node> bll = new BLL.BaseBLL<Entity.Node>();
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (!string.IsNullOrWhiteSpace(keyword))
                filter.Add(new BLL.FilterSearch("Title", keyword, BLL.FilterSearchContract.like));
            List<Entity.Node> list = bll.GetPagedList(page_index, page_size, ref rowCount, filter, "LastUpdateTime desc");
            WebAjaxEntity<List<Entity.Node>> result = new WebAjaxEntity<List<Entity.Node>>();
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
            BLL.BaseBLL<Entity.Node> bll = new BLL.BaseBLL<Entity.Node>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);

        }



        public ActionResult Modify(int? id)
        {
            int ids = TypeHelper.ObjectToInt(id);
            Models.ViewModelNode model = new Models.ViewModelNode();
            if (ids != 0)
            {
                Entity.Node entity = BLL.BLLNode.GetMode(ids);
                if (entity != null)
                {
                    model.content = entity.Content;
                    model.title = entity.Title;
                    model.location = entity.Location;
                    model.id = ids;
                    System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
                    foreach (var item in entity.NodeUsers)
                    {
                        str_ids.Append(item.ID.ToString() + ",");
                        model.users_entity.Add(new Models.ViewModelDocumentCategory(item.CusUser.ID, item.CusUser.Telphone + "(" + item.CusUser.NickName + ")"));
                    }
                    if (str_ids.Length > 0)
                    {
                        str_ids = str_ids.Remove(str_ids.Length - 1, 1);
                    }
                    model.user_ids = str_ids.ToString();

                    foreach (var item in entity.NodeFiles)
                    {
                        model.file_list.Add(new Models.ViewModelListFile(item.FilePath, item.FileName, item.FileSize));
                    }

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
        public ActionResult Modify(Models.ViewModelNode entity)
        {
            var isAdd = entity.id == 0 ? true : false;


            BLL.BaseBLL<Entity.Node> bll = new BLL.BaseBLL<Entity.Node>();
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

                Entity.Node model = null;

                if (isAdd)
                    model = new Entity.Node();
                else
                {
                    model = bll.GetModel(p => p.ID == entity.id);
                    model.LastUpdateTime = DateTime.Now;
                }

                model.Content = entity.content;
                model.Title = entity.title;
                model.Location = entity.location;

                foreach (var item in entity.file_list)
                {
                    var entity_file = new Entity.NodeFile();
                    entity_file.AddTime = DateTime.Now;
                    entity_file.CusUserID = WorkContext.UserInfo.ID;
                    entity_file.FileSize = item.file_size;
                    entity_file.FilePath = item.file_path;
                    entity_file.FileName = item.file_name;
                    model.NodeFiles.Add(entity_file);
                }

                if (isAdd)
                    BLL.BLLNode.Add(model, final_ids);
                else
                    BLL.BLLNode.Modify(model, final_ids);

                entity.Msg = 1;
            }

            return View(entity);
        }

    }
}