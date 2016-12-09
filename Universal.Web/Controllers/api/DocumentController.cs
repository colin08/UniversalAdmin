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
            }
            response_entity.data = response_list;
            response_entity.msg = 1;
            response_entity.msgbox = "success";
            return response_entity;
        }

        /// <summary>
        /// 添加秘籍,表单上传。参数：user_id(登录的用户ID),see_type(可见类别,0:所有人,1:某些部门,2:某些用户),toid(对应的部门或用户ID,逗号分割),title(秘籍标题)
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/document/add")]
        public async Task<HttpResponseMessage> AddFavorites()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.OK);
            // 检查是否是 multipart/form-data 
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少 enctype='multipart/form-data";
                response.Content = new StringContent(JsonConvert.SerializeObject(WorkContext.AjaxStringEntity), Encoding.GetEncoding("UTF-8"), "application/json");
                return response;
            }
            //文件保存目录路径 
            string SaveTempPath = "/uploads/doc";
            String dirTempPath = Tools.IOHelper.GetMapPath(SaveTempPath);
            if (!Directory.Exists(dirTempPath))
            {
                Directory.CreateDirectory(dirTempPath);
            }
            // 设置上传目录 
            var provider = new MultipartFormDataStreamProvider(dirTempPath);

            // Read the form data.  这一步文件已经保存了
            await Request.Content.ReadAsMultipartAsync(provider);

            if (provider.FileData.Count != 1)
            {
                WorkContext.AjaxStringEntity.msgbox = "一次只能上传一个文件";
                response.Content = new StringContent(JsonConvert.SerializeObject(WorkContext.AjaxStringEntity), Encoding.GetEncoding("UTF-8"), "application/json");
                return response;
            }

            string file_type = provider.FileData[0].Headers.ContentType.MediaType.ToString(); //image/png
            string[] file_type_list = file_type.Split('/');
            //if (file_type_list[0].ToLower() != "image")
            //{
            //    WorkContext.AjaxStringEntity.msgbox = "只能上传图片";
            //    response.Content = new StringContent(JsonConvert.SerializeObject(WorkContext.AjaxStringEntity), Encoding.GetEncoding("UTF-8"), "application/json");
            //    return response;
            //}

            FileInfo fileinfo = new FileInfo(provider.FileData[0].LocalFileName);
            long size = fileinfo.Length;
            string size_str = "0";
            if (size >= 1024)
                size_str = (size / 1024).ToString() + "MB";
            else
                size_str = size.ToString() + "KB";
            string io_path = fileinfo.FullName;  //保存的完整绝对路径
            string md5 = Tools.IOHelper.GetMD5HashFromFile(io_path);
            string new_path = dirTempPath + "/" + md5 + "." + file_type_list[1];
            string server_path = (SaveTempPath + "/" + md5 + "." + file_type_list[1]).Replace(" ", "");
            if (System.IO.File.Exists(new_path))
                System.IO.File.Delete(io_path); //把刚刚上传的给删掉，只用原有的文件
            else //不存在，改名为md5值保存
                System.IO.File.Move(io_path, dirTempPath + "/" + md5 + "." + file_type_list[1]); //给文件改名
            //用户ID
            int user_id = TypeHelper.ObjectToInt(provider.FormData["user_id"], 0);
            //可见类别
            Entity.DocPostSee see_type = (Entity.DocPostSee)TypeHelper.ObjectToInt(provider.FormData["see_type"], 0);
            //标题
            string title = "";
            if (provider.FormData["title"] != null)
                title = provider.FormData["title"].ToString();
            int category_id = TypeHelper.ObjectToInt(provider.FormData["category_id"], 0);
            //部门ID或用户ID，逗号分割
            string toid = "";
            if (provider.FormData["toid"] != null)
                title = provider.FormData["toid"].ToString();

            #region 处理用户或部门的ID
            StringBuilder str_ids = new StringBuilder();
            switch (see_type)
            {
                case Entity.DocPostSee.everyone:
                    break;
                case Entity.DocPostSee.department:
                    foreach (var item in BLL.BLLDepartment.GetListByIds(toid))
                        str_ids.Append(item.ID.ToString() + ",");
                    break;
                case Entity.DocPostSee.user:
                    foreach (var item in BLL.BLLCusUser.GetListByIds(toid))
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
            entity.CusUserID = user_id;
            entity.DocCategoryID = category_id;
            entity.FilePath = server_path;
            entity.FileSize = size_str;
            entity.Title = title;
            entity.TOID = final_ids;
            entity.See = see_type;
            BLL.BaseBLL<Entity.DocPost> bll = new BLL.BaseBLL<Entity.DocPost>();
            bll.Add(entity);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            response.Content = new StringContent(JsonConvert.SerializeObject(WorkContext.AjaxStringEntity), Encoding.GetEncoding("UTF-8"), "application/json");
            return response;

        }

        /// <summary>
        /// 获取可见秘籍分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/document/list")]
        public WebAjaxEntity<List<Models.Response.DocumentInfo>> GetDocList([FromBody]Models.Request.DocumentList req)
        {
            WebAjaxEntity<List<Models.Response.DocumentInfo>> response_entity = new WebAjaxEntity<List<Models.Response.DocumentInfo>>();
            if(req.page_index <=0 || req.page_size <=0)
            {
                response_entity.msgbox = "非法参数";
                return response_entity;
            }
            if (req.user_id <= 0 || req.category_id <= 0)
            {
                response_entity.msgbox = "非法参数";
                return response_entity;
            }
            int rowCount = 0;
            List<Models.Response.DocumentInfo> response_list = new List<Models.Response.DocumentInfo>();
            BLL.BaseBLL<Entity.CusUserDocFavorites> bll_fav = new BLL.BaseBLL<Entity.CusUserDocFavorites>();
            foreach (var item in BLL.BLLDocument.GetPowerPageData(req.page_index,req.page_size,ref rowCount,req.user_id,req.search_word,req.category_id))
            {
                Models.Response.DocumentInfo model = new Models.Response.DocumentInfo();
                model.add_time = item.AddTime;
                model.file_path = GetSiteUrl() + item.FilePath;
                model.file_size = item.FileSize;
                model.id = item.ID;
                model.title = item.Title;
                model.is_favorites = bll_fav.Exists(p => p.CusUserID == req.user_id && p.DocPostID == item.ID);
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
            if(req.user_id <=0 || string.IsNullOrWhiteSpace(req.doc_ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }
            BLL.BaseBLL<Entity.DocPost> bll_doc = new BLL.BaseBLL<Entity.DocPost>();
            BLL.BaseBLL<Entity.DownloadLog> bll_down = new BLL.BaseBLL<Entity.DownloadLog>();
            foreach (var item in req.doc_ids.Split(','))
            {
                int id = TypeHelper.ObjectToInt(item,0);
                if(id<=0)
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
        [Route("api/v1/favorites/list")]
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
            foreach (var item in BLL.BllCusUserFavorites.GetDocPageData(req.page_index, req.page_size, ref rowCount, req.user_id, "", 0))
            {
                Models.Response.DocumentInfo model = new Models.Response.DocumentInfo();
                model.add_time = item.AddTime;
                model.file_path = GetSiteUrl() + item.FilePath;
                model.file_size = item.FileSize;
                model.id = item.ID;
                model.title = item.Title;
                model.is_favorites = true;
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
            }
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
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
            if(string.IsNullOrWhiteSpace(req.ids))
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
                
    }
}
