using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class FileController : Controller
    {
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownLoad()
        {
            string req_path = Tools.WebHelper.GetFormString("uri");
            string down_name = Tools.WebHelper.GetFormString("name");//下载的文件名
            if (string.IsNullOrWhiteSpace(req_path))
                return Content("<script>alert('不明确的文件');window.history.back();</script>");

            string io_path = req_path;
            if (req_path.StartsWith("/")) //相对路径
                io_path = Tools.IOHelper.GetMapPath(req_path);
            if (!System.IO.File.Exists(io_path))
                return Content("<script>alert('文件不存在');window.history.back();</script>");
            if (string.IsNullOrWhiteSpace(down_name))
                down_name = io_path.Substring(io_path.LastIndexOf(@"\") + 1);

            return File(io_path, "application/octet-stream", down_name);
        }
    }
}