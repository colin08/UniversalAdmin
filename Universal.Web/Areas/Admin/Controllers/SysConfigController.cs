using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 系统配置文件
    /// </summary>
    public class SysConfigController : BaseAdminController
    {
        /// <summary>
        /// 建这个Controller只是为了添加一项权限
        /// </summary>
        /// <returns></returns>
        [AdminPermission("系统操作","查看高级信息")]
        public ActionResult Sys()
        {
            return Content("没有意义");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AdminPermission("站点配置文件", "编辑配置文件页面")]
        public ActionResult Modify()
        {
            return View(WebSite);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [AdminPermission("站点配置文件", "保存修改的配置文件")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify(WebSiteModel entity)
        {
            if (ModelState.IsValid)
            {
                if (ConfigHelper.SaveConfig(ConfigFileEnum.SiteConfig, entity))
                {
                    AddAdminLogs(Entity.SysLogMethodType.Update, "修改配置文件");
                    return PromptView("/admin/SysConfig/Modify", "OK", "Success", "修改成功", 3);
                }else
                {
                    return PromptView("/admin/SysConfig/Modify", "500", "Error", "保存失败，可能是没有权限操作数据库", 3);
                }
            }
            else
            {
                return View(entity);
            }
        }


        //[AdminPermission("站点配置文件","回收应用程序池")]
        [HttpPost]
        public JsonResult AppPoolRecycle()
        {
            var is_ok = IISHelper.AppPool(IISHelperAppPoolMethod.Recycle, WebSite.AppPoolName);
            if(is_ok)
            {
                AddAdminLogs(Entity.SysLogMethodType.Resotre, "回收应用程序池");
                WorkContext.AjaxStringEntity.msg = 1;
            }
            return Json(WorkContext.AjaxStringEntity);
        }

    }
}