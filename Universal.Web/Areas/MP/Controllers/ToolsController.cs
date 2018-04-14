using System;
using System.Collections.Generic;
using System.Linq;
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
            var upload_result = up_helper.Upload(fileData, filePath,true);
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
    }
}