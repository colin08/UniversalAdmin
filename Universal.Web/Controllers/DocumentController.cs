using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;
using System.Data.Entity;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 秘籍
    /// </summary>
    [BasicAdminAuth]
    public class DocumentController : BaseHBLController
    {
        public ActionResult Index()
        {
            int default_id = 0;
            string data = BLL.BLLDocCategory.CreateDocCategoryTreeData(out default_id);
            ViewData["TreeData"] = data;
            ViewData["DefaultID"] = default_id;
            return View();
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="doc_id">部门ID</param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DocData(int page_size, int page_index, int doc_id, string keyword)
        {
            BLL.BaseBLL<Entity.DocPost> bll = new BLL.BaseBLL<Entity.DocPost>();
            BLL.BaseBLL<Entity.CusUserDocFavorites> bll_fav = new BLL.BaseBLL<Entity.CusUserDocFavorites>();
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            filter.Add(new BLL.FilterSearch("DocCategoryID", doc_id.ToString(), BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(keyword))
                filter.Add(new BLL.FilterSearch("Title", keyword, BLL.FilterSearchContract.like));

            List<Entity.DocPost> list = bll.GetPagedList(page_index, page_size, ref rowCount, filter, "LastUpdateTime desc", p => p.CusUser);

            foreach (var item in list)
            {
                item.IsFavorites = bll_fav.Exists(p=>p.CusUserID == WorkContext.UserInfo.ID && p.DocPostID == item.ID);// db.CusUserDocFavorites.Any(p => p.CusUserID == user_id && p.DocPostID == item.ID);
            }
            WebAjaxEntity<List<Entity.DocPost>> result = new WebAjaxEntity<List<Entity.DocPost>>();
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
        public JsonResult DelDoc(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            if(BLL.BLLDocument.Del(ids))
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "删除成功";
                return Json(WorkContext.AjaxStringEntity);
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = "删除失败";
                return Json(WorkContext.AjaxStringEntity);
            }

        }


        public ActionResult Modify(int? id)
        {
            LoadCategory();
            int ids = TypeHelper.ObjectToInt(id);
            Models.ViewModelDocument model = new Models.ViewModelDocument();
            if (ids != 0)
            {
                BLL.BaseBLL<Entity.DocPost> bll = new BLL.BaseBLL<Entity.DocPost>();
                Entity.DocPost entity = bll.GetModel(p => p.ID == id);
                if (entity != null)
                {
                    model.category_id = entity.DocCategoryID;
                    model.filepath = entity.FilePath;
                    model.filesize = entity.FileSize;
                    model.title = entity.Title;
                    model.id = entity.ID;
                    model.post_see = entity.See;
                    System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
                    switch (entity.See)
                    {
                        case Entity.DocPostSee.everyone:
                            break;
                        case Entity.DocPostSee.department:
                            foreach (var item in BLL.BLLDepartment.GetListByIds(entity.TOID))
                            {
                                str_ids.Append(item.ID.ToString() + ",");
                                model.see_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.Title));
                            }
                            break;
                        case Entity.DocPostSee.user:
                            foreach (var item in BLL.BLLCusUser.GetListByIds(entity.TOID))
                            {
                                str_ids.Append(item.ID.ToString() + ",");
                                model.see_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.Telphone + "(" + item.NickName + ")"));
                            }
                            break;
                        default:
                            break;
                    }
                    if(str_ids.Length>0)
                    {
                        str_ids = str_ids.Remove(str_ids.Length - 1, 1);
                    }
                    model.see_ids = str_ids.ToString();
                }
                else
                {
                    model.Msg = 2;
                    model.MsgBox = "数据不存在";
                }

            }
            return View(model);
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Info(int id)
        {
            Models.ViewModelDocument model = new Models.ViewModelDocument();
            BLL.BaseBLL<Entity.DocPost> bll = new BLL.BaseBLL<Entity.DocPost>();
            Entity.DocPost entity = bll.GetModel(p => p.ID == id);
            if (entity != null)
            {
                model.category_id = entity.DocCategoryID;
                model.filepath = entity.FilePath;
                model.filesize = entity.FileSize;
                model.title = entity.Title;
                model.id = entity.ID;
            }
            return View(model);
        }

        /// <summary>
        ///  秘籍收藏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DocFavorites(int id)
        {
            if(id <=0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            string msg = "";
            bool isOK = BLL.BllCusUserFavorites.AddDocFav(id, WorkContext.UserInfo.ID,out msg);
            if(!isOK)
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
                return Json(WorkContext.AjaxStringEntity);
            }

            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);

        }


        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Modify(Models.ViewModelDocument entity)
        {
            var isAdd = entity.id == 0 ? true : false;

            LoadCategory();
            
            BLL.BaseBLL<Entity.DocPost> bll = new BLL.BaseBLL<Entity.DocPost>();
            if (!isAdd)
            {
                if (!bll.Exists(p => p.ID == entity.id))
                {
                    entity.Msg = 2;
                    ModelState.AddModelError("title", "信息不存在");
                }
            }

            #region 处理用户或部门的ID
            System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
            entity.see_entity = new List<Models.ViewModelDocumentCategory>();
            switch (entity.post_see)
            {
                case Entity.DocPostSee.everyone:
                    break;
                case Entity.DocPostSee.department:
                    foreach (var item in BLL.BLLDepartment.GetListByIds(entity.see_ids))
                    {
                        str_ids.Append(item.ID.ToString() + ",");
                        entity.see_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.Title));
                    }
                    break;
                case Entity.DocPostSee.user:
                    foreach (var item in BLL.BLLCusUser.GetListByIds(entity.see_ids))
                    {
                        str_ids.Append(item.ID.ToString() + ",");
                        entity.see_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.Telphone + "(" + item.NickName + ")"));
                    }
                    break;
                default:
                    break;
            }
            string final_ids = "";
            if (str_ids.Length > 0)
            {
                final_ids = "," + str_ids.ToString();
            } 
            #endregion

            if (ModelState.IsValid)
            {
                Entity.DocPost model = null;
                if (isAdd)
                {
                    model = new Entity.DocPost();
                    model.CusUserID = WorkContext.UserInfo.ID;
                }
                else
                    model = bll.GetModel(p => p.ID == entity.id);
                
                model.DocCategoryID = entity.category_id;
                model.FilePath = entity.filepath;
                model.FileSize = entity.filesize;
                model.Title = entity.title;
                model.TOID = final_ids;
                model.See = entity.post_see;
                if (isAdd)
                    bll.Add(model);
                else
                    bll.Modify(model);

                entity.Msg = 1;
            }
            
            return View(entity);
        }


        public ActionResult Some()
        {
            return View();
        }


        /// <summary>
        /// 加载分类
        /// </summary>
        private void LoadCategory()
        {
            //List<Models.ViewModelDocumentCategory> result = new List<Models.ViewModelDocumentCategory>();
            BLL.BaseBLL<Entity.DocCategory> bll = new BLL.BaseBLL<Entity.DocCategory>();
            List<Entity.DocCategory> list = bll.GetListBy(0, p => p.Status == true, "Priority Desc", true);

            List<SelectListItem> userRoleList = new List<SelectListItem>();
            userRoleList.Add(new SelectListItem() { Text = "全部分类", Value = "0" });
            foreach (var item in list)
            {
                userRoleList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString() });
            }
            ViewData["category"] = userRoleList;
            //foreach (var one in list.Where(p => p.Depth == 1))
            //{
            //    Models.ViewModelDocumentCategory one_model = new Models.ViewModelDocumentCategory();
            //    one_model.id = one.ID;
            //    one_model.title = one.Title;
            //    result.Add(one_model);
            //    foreach (var two in list.Where(p => p.Depth == 2 && p.PID == one.ID))
            //    {
            //        Models.ViewModelDocumentCategory two_model = new Models.ViewModelDocumentCategory();
            //        two_model.id = two.ID;
            //        two_model.title = two.Title;
            //        result.Add(two_model);
            //        foreach (var three in list.Where(p => p.Depth == 3 && p.PID == two.ID))
            //        {
            //            Models.ViewModelDocumentCategory three_model = new Models.ViewModelDocumentCategory();
            //            three_model.id = three.ID;
            //            three_model.title = three.Title;
            //            result.Add(three_model);
            //        }
            //    }
            //}
        }

    }
}