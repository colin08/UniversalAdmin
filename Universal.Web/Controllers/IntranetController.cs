using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Entity;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 供门诊系统调用的接口
    /// </summary>
    public class IntranetController : Controller
    {
        /// <summary>
        /// 用户体检报告
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserMedicalReport()
        {
            UnifiedResultEntity<string> result = new UnifiedResultEntity<string>();
            result.data = "";
            string idcardnumber = WebHelper.GetRequestString("id_number", "");
            if (string.IsNullOrWhiteSpace(idcardnumber))
            {
                result.msgbox = "身份证号码不能为空";
                return Json(result);
            }
            string report_title = WebHelper.GetRequestString("report_title", "");
            if (string.IsNullOrWhiteSpace(report_title))
            {
                result.msgbox = "报告名称不能为空";
                return Json(result);
            }

            string file_name = DateTime.Now.ToFileTime().ToString();
            string file_ext = "";
            string file_folder = "/uploads/report/";
            string server_path = file_folder + file_name;
            string file_io_folder = IOHelper.GetMapPath(file_folder);
            if (!System.IO.Directory.Exists(file_io_folder)) System.IO.Directory.CreateDirectory(file_io_folder);
            if(Request.Files.Count == 0)
            {
                result.msgbox = "缺少报告附件";
                return Json(result);
            }
            try
            {
                var file = Request.Files[0];
                file_ext = "." + IOHelper.GetFileExt(file.FileName).ToLower();
                server_path = server_path + file_ext;
                file.SaveAs(file_io_folder + file_name + file_ext);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("内网提交提交报告文件接收出错：" + ex.Message);
                result.msgbox = ex.Message;
                return Json(result);
            }
            string msg = "";
            var status = BLL.BLLMpUserMedicalReport.Add(report_title, idcardnumber, server_path, out msg);
            if(!status)
            {
                result.msgbox = msg;
                return Json(result);
            }
            result.msg = 1;
            result.msgbox = "ok";
            return Json(result);
        }
    }
}