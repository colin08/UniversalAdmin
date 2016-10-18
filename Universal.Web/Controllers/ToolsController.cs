using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    public class ToolsController : BaseHBLController
    {
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendCode(string guid,string telphone)
        {
            if(string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(telphone))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            if(!bll.Exists(p => p.Telphone == telphone))
            {
                WorkContext.AjaxStringEntity.msgbox = "该手机号不存在";
                return Json(WorkContext.AjaxStringEntity);
            }

            string msg = "";
            BLL.BLLVerification.Send(telphone, new Guid(guid), Entity.CusVerificationType.RestPwd, out msg);
            if (msg.Equals("OK"))
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "success";
                return Json(WorkContext.AjaxStringEntity);
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
                return Json(WorkContext.AjaxStringEntity);
            }
        }

        /// <summary>
        /// 上传用户头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadAvatar(HttpPostedFileBase file)
        {
            
            UploadHelper uh = new UploadHelper();
            WebAjaxEntity<string> result = uh.Upload(file, "/uploads/avatar/");
            return Json(result);
        }

    }
}