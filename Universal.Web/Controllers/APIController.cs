﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 测试接口
    /// </summary>
    public class APIController : BaseAPIController
    {
        /// <summary>
        /// 测试接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/test/a")]
        public WebAjaxEntity<string> Test()
        {
            WorkContext.AjaxStringEntity.msgbox = "消息";
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 修改用户头像，From表单中头像标识：user_id
        /// </summary>
        /// <returns></returns>
        [Route("api/test/upload")]
        public async Task<HttpResponseMessage> UpdateUserAvatar()
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
            string SaveTempPath = "/uploads/Users/Avatar";
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
            if (file_type_list[0].ToLower() != "image")
            {
                WorkContext.AjaxStringEntity.msgbox = "只能上传图片";
                response.Content = new StringContent(JsonConvert.SerializeObject(WorkContext.AjaxStringEntity), Encoding.GetEncoding("UTF-8"), "application/json");
                return response;
            }

            FileInfo fileinfo = new FileInfo(provider.FileData[0].LocalFileName);
            string io_path = fileinfo.FullName;  //保存的完整绝对路径
            string md5 = Tools.IOHelper.GetMD5HashFromFile(io_path);
            string new_path = dirTempPath + "/" + md5 + "." + file_type_list[1];
            string server_path = (SaveTempPath + "/" + md5 + "." + file_type_list[1]).Replace(" ", "");
            if (System.IO.File.Exists(new_path))
                System.IO.File.Delete(io_path); //把刚刚上传的给删掉，只用原有的文件
            else //不存在，改名为md5值保存
                System.IO.File.Move(io_path, dirTempPath + "/" + md5 + "." + file_type_list[1]); //给文件改名}

            string user_id_temp = provider.FormData["user_id"];

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            WorkContext.AjaxStringEntity.data = GetSiteUrl() + server_path;
            response.Content = new StringContent(JsonConvert.SerializeObject(WorkContext.AjaxStringEntity), Encoding.GetEncoding("UTF-8"), "application/json");
            return response;
        }


    }
}
