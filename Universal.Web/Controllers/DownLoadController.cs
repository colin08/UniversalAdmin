using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;
using Universal.Tools;

namespace Universal.Web.Controllers
{
    public class DownLoadController : BaseHBLController
    {
        // GET: DownLoad
        public ActionResult APP(string b)
        {
            string os_type = WebHelper.GetOSType();
            //IOHelper.WriteLogs("平台:"+os_type);
            bool is_android = true;
            if (os_type == "IPhone" || os_type == "Mac") is_android = false;

            Entity.AppVersion entity = null;
            //安卓
            if (b == "1")
            {
                entity = BLL.BLLAppVersion.GetLatest(Entity.APPVersionPlatforms.Android);
                if (entity == null)
                    return Content("无APP可供下载");

                string apk_path = IOHelper.GetMapPath(entity.DownUrl);
                string down_name = "ChengTaiAPP" + entity.Version + ".apk";
                return File(apk_path, "application/octet-stream", down_name);
            }//IOS
            else if(b =="2")
            {
                entity = BLL.BLLAppVersion.GetLatest(Entity.APPVersionPlatforms.IOS);
                if (entity == null)
                    return Content("无APP可供下载");
                return Redirect(entity.LinkUrl);
            }else
            {
                //判断平台
                if (is_android)
                    entity = BLL.BLLAppVersion.GetLatest(Entity.APPVersionPlatforms.Android);
                else
                    entity = BLL.BLLAppVersion.GetLatest(Entity.APPVersionPlatforms.IOS);

                if (entity == null)
                    return Content("无APP可供下载");

                if (!is_android)
                    return Redirect(entity.LinkUrl);

                string apk_path = IOHelper.GetMapPath(entity.DownUrl);
                string down_name = "ChengTaiAPP" + entity.Version + ".apk";
                return File(apk_path, "application/octet-stream", down_name);
            }

        }
    }
}