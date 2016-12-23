using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Controllers.api
{
    /// <summary>
    /// 秘籍接口
    /// </summary>
    public class DocumentController : BaseAPIController
    {
        /// <summary>
        /// 获取所有秘籍分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/document/category/all")]
        public WebAjaxEntity<List<Models.Response.TreeData>> GetDocCategory()
        {
            WebAjaxEntity<List<Models.Response.TreeData>> response_entity = new WebAjaxEntity<List<Models.Response.TreeData>>();
            BLL.BaseBLL<Entity.DocCategory> bll = new BLL.BaseBLL<Entity.DocCategory>();
            List<Models.Response.TreeData> response_list = new List<Models.Response.TreeData>();
            foreach (var item in bll.GetListBy(0, p => p.Status == true, "Priority Desc", false))
            {
                Models.Response.TreeData model = new Models.Response.TreeData();
                model.id = item.ID;
                model.pid = TypeHelper.ObjectToInt(item.PID, 0);
                model.title = item.Title;
                response_list.Add(model);
            }
            response_entity.data = response_list;
            response_entity.msg = 1;
            response_entity.msgbox = "success";
            return response_entity;
        }

        /// <summary>
        /// 添加秘籍
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/document/add")]
        public WebAjaxEntity<string> AddDocument([FromBody]Models.Request.AddDocument req)
        {
            if (req.user_id == 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "没有收到用户信息";
                return WorkContext.AjaxStringEntity;
            }
            BLL.BaseBLL<Entity.CusUser> bll_user = new BLL.BaseBLL<Entity.CusUser>();
            if (!bll_user.Exists(p => p.ID == req.user_id))
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return WorkContext.AjaxStringEntity;
            }

            #region 处理用户或部门的ID
            StringBuilder str_ids = new StringBuilder();
            switch (req.see_type)
            {
                case Entity.DocPostSee.everyone:
                    break;
                case Entity.DocPostSee.department:
                    if (string.IsNullOrWhiteSpace(req.toid))
                    {
                        WorkContext.AjaxStringEntity.msgbox = "当是可见是部门时，必须传入部门数据";
                        return WorkContext.AjaxStringEntity;
                    }
                    foreach (var item in BLL.BLLDepartment.GetListByIds(req.toid))
                        str_ids.Append(item.ID.ToString() + ",");
                    break;
                case Entity.DocPostSee.user:
                    if (string.IsNullOrWhiteSpace(req.toid))
                    {
                        WorkContext.AjaxStringEntity.msgbox = "当是可见是用户时，必须传入用户数据";
                        return WorkContext.AjaxStringEntity;
                    }
                    foreach (var item in BLL.BLLCusUser.GetListByIds(req.toid))
                        str_ids.Append(item.ID.ToString() + ",");
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
            
            Entity.DocPost entity = new Entity.DocPost();
            entity.CusUserID = req.user_id;
            entity.DocCategoryID = req.category_id;
            entity.Title = req.title;
            entity.TOID = final_ids;
            entity.Content = req.content;
            entity.See = req.see_type;
            if (req.file_list != null)
            {
                foreach (var item in req.file_list)
                {
                    Entity.DocFile entity_file = new Entity.DocFile();
                    entity_file.FileName = item.file_name;
                    entity_file.FilePath = item.file_path;
                    entity_file.FileSize = item.file_size;
                    entity.FileList.Add(entity_file);
                }
            }

            BLL.BaseBLL<Entity.DocPost> bll = new BLL.BaseBLL<Entity.DocPost>();
            bll.Add(entity);
            if (entity.ID > 0)
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "ok";
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = "秘籍添加失败";
            }
            return WorkContext.AjaxStringEntity;

        }



        /// <summary>
        /// 获取可见秘籍列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/document/list")]
        public WebAjaxEntity<List<Models.Response.DocumentInfo>> GetDocList([FromBody]Models.Request.DocumentList req)
        {
            WebAjaxEntity<List<Models.Response.DocumentInfo>> response_entity = new WebAjaxEntity<List<Models.Response.DocumentInfo>>();
            if (req.page_index <= 0 || req.page_size <= 0)
            {
                response_entity.msgbox = "非法参数";
                return response_entity;
            }
            if (req.user_id <= 0)
            {
                response_entity.msgbox = "非法参数";
                return response_entity;
            }
            int rowCount = 0;
            List<Models.Response.DocumentInfo> response_list = new List<Models.Response.DocumentInfo>();
            BLL.BaseBLL<Entity.CusUserDocFavorites> bll_fav = new BLL.BaseBLL<Entity.CusUserDocFavorites>();
            BLL.BaseBLL<Entity.DocCategory> bll_docc = new BLL.BaseBLL<Entity.DocCategory>();
            BLL.BaseBLL<Entity.DocFile> bll_file = new BLL.BaseBLL<Entity.DocFile>();
            foreach (var item in BLL.BLLDocument.GetPowerPageData(req.page_index, req.page_size, ref rowCount, req.user_id, req.search_word, req.category_id))
            {
                Models.Response.DocumentInfo model = new Models.Response.DocumentInfo();
                model.add_time = item.AddTime;
                model.id = item.ID;
                model.add_user = item.CusUser.NickName;
                model.category_id = item.DocCategoryID;
                var cat_entity = bll_docc.GetModel(p => p.ID == item.DocCategoryID);
                if (cat_entity != null)
                    model.category_name = cat_entity.Title;
                model.title = item.Title;
                model.content = ReplaceTextAreaImg(item.Content);
                var fav_entity = bll_fav.GetModel(p => p.CusUserID == req.user_id && p.DocPostID == item.ID);
                if (fav_entity != null)
                    model.favorites_id = fav_entity.ID;
                List<Entity.DocFile> file_list = bll_file.GetListBy(0, p => p.DocPostID == item.ID, "ID ASC");
                if (file_list != null)
                {
                    foreach (var file in file_list)
                    {
                        Models.Response.ProjectFile model_file = new Models.Response.ProjectFile();
                        model_file.file_name = file.FileName;
                        model_file.file_path = GetSiteUrl() + file.FilePath;
                        model_file.file_size = file.FileSize;
                        model_file.id = file.ID;
                        model_file.type = Entity.ProjectFileType.file;
                        model.file_list.Add(model_file);
                    }
                }
                response_list.Add(model);
            }
            if (rowCount > 0)
                response_entity.data = response_list;
            response_entity.total = rowCount;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }

        /// <summary>
        /// 添加下载记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/downloadlog/add")]
        public WebAjaxEntity<string> AddDownLog([FromBody]Models.Request.AddDownloadLog req)
        {
            if (req.user_id <= 0 || string.IsNullOrWhiteSpace(req.doc_ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }
            BLL.BaseBLL<Entity.DocPost> bll_doc = new BLL.BaseBLL<Entity.DocPost>();
            BLL.BaseBLL<Entity.DownloadLog> bll_down = new BLL.BaseBLL<Entity.DownloadLog>();
            foreach (var item in req.doc_ids.Split(','))
            {
                int id = TypeHelper.ObjectToInt(item, 0);
                if (id <= 0)
                    continue;
                var entity_doc = bll_doc.GetModel(p => p.ID == id);
                if (entity_doc == null)
                    continue;
                var entity_down = new Entity.DownloadLog();
                entity_down.CusUserID = req.user_id;
                entity_down.Title = entity_doc.Title;
                bll_down.Add(entity_down);
            }
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 获取秘籍收藏列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/favorites/document/list")]
        public WebAjaxEntity<List<Models.Response.DocumentInfo>> GetFavList([FromBody]Models.Request.BasePage req)
        {
            WebAjaxEntity<List<Models.Response.DocumentInfo>> response_entity = new WebAjaxEntity<List<Models.Response.DocumentInfo>>();
            if (req.page_size <= 0 || req.page_index <= 0 || req.user_id <= 0)
            {
                response_entity.msgbox = "非法参数";
                return response_entity;
            }
            List<Models.Response.DocumentInfo> response_list = new List<Models.Response.DocumentInfo>();
            int rowCount = 0;
            BLL.BaseBLL<Entity.CusUserDocFavorites> bll_fav = new BLL.BaseBLL<Entity.CusUserDocFavorites>();
            BLL.BaseBLL<Entity.DocCategory> bll_doccate = new BLL.BaseBLL<Entity.DocCategory>();
            BLL.BaseBLL<Entity.DocFile> bll_file = new BLL.BaseBLL<Entity.DocFile>();
            foreach (var item in BLL.BllCusUserFavorites.GetDocPageData(req.page_index, req.page_size, ref rowCount, req.user_id, "", 0))
            {
                Models.Response.DocumentInfo model = new Models.Response.DocumentInfo();
                model.add_time = item.AddTime;
                model.id = item.ID;
                model.title = item.Title;
                model.content = ReplaceTextAreaImg(item.Content);
                var fav_entity = bll_fav.GetModel(p => p.CusUserID == req.user_id && p.DocPostID == item.ID);
                if (fav_entity != null)
                    model.favorites_id = fav_entity.ID;
                var category_entity = bll_doccate.GetModel(p => p.ID == item.DocCategoryID);
                if (category_entity != null)
                {
                    model.category_name = category_entity.Title;
                }
                model.category_id = item.DocCategoryID;

                List<Entity.DocFile> file_list = bll_file.GetListBy(0, p => p.DocPostID == item.ID, "ID ASC");
                if (file_list != null)
                {
                    foreach (var file in file_list)
                    {
                        Models.Response.ProjectFile model_file = new Models.Response.ProjectFile();
                        model_file.file_name = file.FileName;
                        model_file.file_path = GetSiteUrl() + file.FilePath;
                        model_file.file_size = file.FileSize;
                        model_file.id = file.ID;
                        model_file.type = Entity.ProjectFileType.file;
                        model.file_list.Add(model_file);
                    }
                }
                response_list.Add(model);
            }
            if (rowCount > 0)
                response_entity.data = response_list;
            response_entity.total = rowCount;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }
        
        /// <summary>
        /// 添加秘籍收藏
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/favorites/add")]
        public WebAjaxEntity<string> AddFavorites([FromBody]Models.Request.AddDownloadLog req)
        {
            if (req.user_id <= 0 || string.IsNullOrWhiteSpace(req.doc_ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }
            BLL.BaseBLL<Entity.DocPost> bll_doc = new BLL.BaseBLL<Entity.DocPost>();
            BLL.BaseBLL<Entity.CusUserDocFavorites> bll_fav = new BLL.BaseBLL<Entity.CusUserDocFavorites>();
            StringBuilder str_ids = new StringBuilder();
            foreach (var item in req.doc_ids.Split(','))
            {
                int id = TypeHelper.ObjectToInt(item, 0);
                if (id <= 0)
                    continue;
                if (!bll_doc.Exists(p => p.ID == id))
                    continue;
                var entity_fav = new Entity.CusUserDocFavorites();
                entity_fav.CusUserID = req.user_id;
                entity_fav.DocPostID = id;
                bll_fav.Add(entity_fav);
                str_ids.Append(entity_fav.ID.ToString() + ",");
            }
            if (str_ids.Length > 0)
                str_ids.Remove(str_ids.Length - 1, 1);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            WorkContext.AjaxStringEntity.data = str_ids.ToString();
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 移除秘籍收藏
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/favorites/remove")]
        public WebAjaxEntity<string> RemoveFavorites([FromBody]Models.Request.RemoveFav req)
        {
            if (string.IsNullOrWhiteSpace(req.ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }
            BLL.BaseBLL<Entity.CusUserDocFavorites> bll = new BLL.BaseBLL<Entity.CusUserDocFavorites>();
            var id_list = Array.ConvertAll<string, int>(req.ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return WorkContext.AjaxStringEntity;
        }


        private string ReplaceTextAreaImg(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return "";
            return content.Replace("/uploads/TextArea/", GetSiteUrl() + "/uploads/TextArea/");
        }

    }
}
