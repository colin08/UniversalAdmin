using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Areas.Admin.Controllers
{
    public class ToolsController : BaseAdminController
    {
        /// <summary>
        /// 设置分页大小Cookie
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ContentResult SetPageCookie(string cname,int num)
        {
            if(!string.IsNullOrWhiteSpace(cname) && num >3)
            {
                WebHelper.SetCookie(cname, num.ToString(), 10000);
            }
            return Content("");
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [AdminPermission("其他","上传文件")]
        public ActionResult UploadFile(Admin_Upload_Type type, string folder, int num, string call_back_ele,string call_func)
        {
            string file_ext = "";
            string file_txt = "";
            string file_size = "0KB";
            bool multi = false; //是否可上传多个
            if (type == Admin_Upload_Type.MorePicture)
            {
                multi = true;
            }
            //可上传文件后缀
            switch (type)
            {
                case Admin_Upload_Type.OnePicture:
                case Admin_Upload_Type.MorePicture:
                    file_ext = "image/*";
                    file_txt = "图片";
                    file_size = "5MB";
                    break;
                case Admin_Upload_Type.APK:
                    file_ext = "*.apk;*.APK";
                    file_txt = "apk文件";
                    file_size = "50MB";
                    break;
                case Admin_Upload_Type.IPA:
                    file_ext = "*.ipa;*.IPA";
                    file_txt = "ipa文件";
                    file_size = "50MB";
                    break;
                case Admin_Upload_Type.ZIP:
                    file_ext = "*.zip;*.ZIP";
                    file_txt = "zip压缩包";
                    file_size = "50MB";
                    break;
                default:
                    break;
            }
            ViewData["call_back_ele"] = call_back_ele;
            ViewData["file_ext"] = file_ext;
            ViewData["multi"] = multi.ToString().ToLower();
            ViewData["file_txt"] = file_txt;
            ViewData["file_size"] = file_size;
            ViewData["folder"] = folder;
            ViewData["num"] = num;
            ViewData["call_func"] = call_func;
            ViewData["upload_type"] = type;
            return View();
        }


        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public JsonResult UploadAction(HttpPostedFileBase fileData)
        {
            if (fileData == null)
            {
                fileData = HttpContext.Request.Files["imgFile"];
            }

            string file_name = fileData.FileName;
            string file_ext = "";
            if (!string.IsNullOrWhiteSpace(file_name))
                file_ext = IOHelper.GetFileExt(file_name).ToLower();

            //上传文件夹
            string operation = WebHelper.GetFormString("operation", "");
            if (string.IsNullOrWhiteSpace(operation))
                operation = WebHelper.GetQueryString("operation");
            //上传类别
            string upload_type = WebHelper.GetFormString("upload_type", "");
            if (string.IsNullOrWhiteSpace(upload_type))
                upload_type = WebHelper.GetQueryString("upload_type");

            if (string.IsNullOrWhiteSpace(operation))
            {
                WorkContext.AjaxStringEntity.msgbox = "保存位置不明确";
                return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrWhiteSpace(upload_type))
            {
                WorkContext.AjaxStringEntity.msgbox = "上传类别不明确";
                return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
            }


            //保存的目录
            string filePath = "/uploads/" + operation + "/";

            UploadHelper up_helper = new UploadHelper();

            switch ((Admin_Upload_Type)Enum.Parse(typeof(Admin_Upload_Type), upload_type))
            {
                case Admin_Upload_Type.APK:
                    Hashtable ht = new Hashtable();
                    if (!string.IsNullOrWhiteSpace(file_ext))
                    {
                        if (file_ext != "apk")
                        {
                            ht["msg"] = 0;
                            ht["msgbox"] = "请上传APK格式的文件";
                            return Json(ht, JsonRequestBehavior.AllowGet);
                        }
                    }
                    ht = up_helper.Upload_APK(fileData);
                    return Json(ht, JsonRequestBehavior.AllowGet);
                case Admin_Upload_Type.IPA:
                    if (file_ext != "ipa")
                    {
                        WorkContext.AjaxStringEntity.msgbox = "请上传ipa格式的文件";
                        return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
                    }
                    WorkContext.AjaxStringEntity = up_helper.Upload(fileData, filePath);
                    return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
                case Admin_Upload_Type.ZIP://附件
                    if (!string.IsNullOrWhiteSpace(file_ext))
                    {
                        if (file_ext != "zip")
                        {
                            WorkContext.AjaxStringEntity.msgbox = "请上传zip格式的文件";
                            return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
                        }
                    }
                    WorkContext.AjaxStringEntity = up_helper.Upload_Zip(fileData, filePath);
                    return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
                case Admin_Upload_Type.TxtArea: //富文本编辑器中的上传
                    WorkContext.AjaxStringEntity = up_helper.Upload(fileData, filePath);
                    SimditorResult result = new SimditorResult();
                    if (WorkContext.AjaxStringEntity.msg == 1)
                    {
                        result.success = true;
                        result.msg = "上传成功";
                        result.file_path = WorkContext.AjaxStringEntity.data;
                    }
                    else
                    {
                        result.success = false;
                        result.msg = WorkContext.AjaxStringEntity.msgbox;
                    }
                    return Json(result, JsonRequestBehavior.AllowGet);
                case Admin_Upload_Type.OnePicture:
                case Admin_Upload_Type.MorePicture:
                    if (!string.IsNullOrWhiteSpace(file_ext))
                    {
                        if (file_ext != "jpg" && file_ext != "jpeg" && file_ext != "png" && file_ext != "bmp" && file_ext != "gif")
                        {
                            WorkContext.AjaxStringEntity.msgbox = "请上传图片格式的文件";
                            return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
                        }
                    }
                    WorkContext.AjaxStringEntity = up_helper.Upload(fileData, filePath);
                    return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
                default:
                    WorkContext.AjaxStringEntity = up_helper.Upload(fileData, filePath);
                    return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
            }
        }

        

    }

    /// <summary>
    /// 富文本编辑器上传需要返回的格式
    /// </summary>
    public class SimditorResult
    {
        public bool success { get; set; }

        public string msg { get; set; }

        public string file_path { get; set; }
    }
}