using System;
using System.Collections;
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
        public JsonResult SendCode(string guid, string telphone)
        {
            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(telphone))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            if (!bll.Exists(p => p.Telphone == telphone))
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
            if (file == null)
                file = Request.Files[0];
            UploadHelper uh = new UploadHelper();
            WebAjaxEntity<string> result = uh.Upload(file, "/uploads/avatar/");
            return Json(result);
        }

        /// <summary>
        /// 上传秘籍附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadDocument(HttpPostedFileBase file)
        {
            if (file == null)
                file = Request.Files[0];
            UploadHelper uh = new UploadHelper();
            WebAjaxEntity<string> result = uh.Upload(file, "/uploads/doc/");
            if (result.msg == 1)
            {
                int size = IOHelper.GetFileSize(result.data);
                if (size >= 1024)
                {
                    result.msgbox = (size / 1024).ToString() + "MB";
                }
                else
                    result.msgbox = size.ToString() + "KB";
            }
            return Json(result);
        }

        /// <summary>
        /// 上传APK
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadAPK(HttpPostedFileBase file)
        {
            if (file == null)
                file = Request.Files[0];
            UploadHelper uh = new UploadHelper();
            Hashtable ht = new Hashtable();
            ht = uh.Upload_APK(file);            
            return Json(ht);
        }


        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserList(string search)
        {
            WebAjaxEntity<List<Models.ViewModelNoticeUser>> result = new WebAjaxEntity<List<Models.ViewModelNoticeUser>>();
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            List<Models.ViewModelNoticeUser> list = new List<Models.ViewModelNoticeUser>();
            foreach (var item in bll.GetListBy(0, p=>p.Telphone.Contains(search)||p.NickName.Contains(search), "ID asc"))
            {
                Models.ViewModelNoticeUser model = new Models.ViewModelNoticeUser();
                model.id = item.ID;
                model.nick_name = item.NickName;
                model.telphone = item.Telphone;
                list.Add(model);
            }
            result.data = list;
            result.msg = 1;
            result.total = list.Count;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDepartmentList()
        {
            WebAjaxEntity<List<Models.ViewModelDepartment>> result = new WebAjaxEntity<List<Models.ViewModelDepartment>>();
            BLL.BaseBLL<Entity.CusDepartment> bll = new BLL.BaseBLL<Entity.CusDepartment>();
            List<Models.ViewModelDepartment> list = new List<Models.ViewModelDepartment>();
            foreach (var item in bll.GetListBy(0,new List<BLL.FilterSearch>(), "Priority desc"))
            {
                Models.ViewModelDepartment model = new Models.ViewModelDepartment();
                model.department_id = item.ID;
                model.parent_id = item.PID == null ? 0 : item.PID;
                model.title = item.Title;
                list.Add(model);
            }
            result.data = list;
            result.msg = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有职位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetJobList()
        {
            WebAjaxEntity<List<Models.ViewModelJob>> result = new WebAjaxEntity<List<Models.ViewModelJob>>();
            List<Models.ViewModelJob> list = new List<Models.ViewModelJob>();
            BLL.BaseBLL<Entity.CusUserJob> bll = new BLL.BaseBLL<Entity.CusUserJob>();
            foreach (var item in bll.GetListBy(0,new List<BLL.FilterSearch>(), "AddTime Asc"))
            {
                Models.ViewModelJob model = new Models.ViewModelJob();
                model.id = item.ID;
                model.title = item.Title;
                list.Add(model);
            }            
            result.data = list;
            result.msg = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}