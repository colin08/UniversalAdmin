using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Areas.MP.Controllers
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class ToolsController : BaseMPController
    {

        /// <summary>
        /// 下载微信图片
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> DownloadWXImages(string mediaId)
        {
            UnifiedResultEntity<string> result = new UnifiedResultEntity<string>();

            var web_site = ConfigHelper.GetSiteModel();
            var accessToken = AccessTokenContainer.GetAccessToken(web_site.WeChatAppID);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                await Senparc.Weixin.MP.AdvancedAPIs.MediaApi.GetAsync(accessToken, mediaId, ms);
                if(ms.Length == 0)
                {
                    result.msgbox = "图片下载失败";
                    return Json(result);
                }

                //保存到文件
                string file_folder = "/uploads/mpimg/";
                string file_io_folder = IOHelper.GetMapPath(file_folder);
                if (!System.IO.Directory.Exists(file_io_folder)) System.IO.Directory.CreateDirectory(file_io_folder);

                string file_server_path = file_folder + DateTime.Now.ToFileTime() + ".jpg";
                string file_io_path = IOHelper.GetMapPath(file_server_path);

                using (System.IO.FileStream fs = new System.IO.FileStream(file_io_path, System.IO.FileMode.Create))
                {
                    ms.Position = 0;
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;
                    while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        fs.Write(buffer, 0, bytesRead);
                    }
                    fs.Flush();
                }

                result.msg = 1;
                result.msgbox = "ok";
                result.data = file_server_path;
                return Json(result);
            }
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadAvatar(HttpPostedFileBase fileData)
        {
            fileData = fileData ?? HttpContext.Request.Files["imgFile"];

            string file_name = fileData.FileName;
            string file_ext = "";
            if (!string.IsNullOrWhiteSpace(file_name))
                file_ext = IOHelper.GetFileExt(file_name).ToLower();

            if (!string.IsNullOrWhiteSpace(file_ext))
            {
                if (file_ext != "jpg" && file_ext != "jpeg" && file_ext != "png" && file_ext != "bmp" && file_ext != "gif")
                {
                    WorkContext.AjaxStringEntity.msgbox = "请上传图片格式的文件";
                    return Json(WorkContext.AjaxStringEntity);
                }
            }

            string operation = "mpavatar";
            //保存的目录
            string filePath = "/uploads/" + operation + "/";
            UploadHelper up_helper = new UploadHelper();
            var upload_result = up_helper.UploadMobile(fileData, filePath);
            if(upload_result.msg==1)
            {
                //上传成功后直接修改头像
                BLL.BaseBLL<Entity.MPUser> bll = new BLL.BaseBLL<Entity.MPUser>();
                var uid= WorkContext.UserInfo.ID;
                var entity = bll.GetModel(p => p.ID == uid, "ID DESC");
                if(entity != null)
                {
                    entity.Avatar = upload_result.data;
                    bll.Modify(entity, "Avatar");
                    BLL.BLLMPUserState.SetLogin(entity.OpenID);
                }
            }
            WorkContext.AjaxStringEntity = upload_result;
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendVer(string telphone,int type)
        {

            if (string.IsNullOrWhiteSpace(telphone))
            {
                WorkContext.AjaxStringEntity.msgbox = "请传入手机号";
                return Json(WorkContext.AjaxStringEntity);
            }

            var tt = Entity.VerificationCodeType.FullInfo;
            try
            {
                tt = (Entity.VerificationCodeType)type;
            }
            catch
            {
                WorkContext.AjaxStringEntity.msgbox = "非法类别";
                return Json(WorkContext.AjaxStringEntity);
            }

            string msg = "";
            var status = BLL.BLLVerificationCode.Add(telphone, tt, out msg);

            WorkContext.AjaxStringEntity.msg = status ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity);
        }

    }
}