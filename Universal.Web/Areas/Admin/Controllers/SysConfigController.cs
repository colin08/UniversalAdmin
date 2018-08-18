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
            WebSite.HonorDesc = WebHelper.UrlDecode(WebSite.HonorDesc);
            WebSite.HonorRightDesc = WebHelper.UrlDecode(WebSite.HonorRightDesc);
            return View(WebSite);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [AdminPermission("站点配置文件", "保存修改的配置文件")]
        [HttpPost]
        [ValidateAntiForgeryToken,ValidateInput(false)]
        public ActionResult Modify(WebSiteModel entity)
        {
            if (ModelState.IsValid)
            {
                entity.HonorDesc = WebHelper.UrlEncode(entity.HonorDesc);
                entity.HonorRightDesc = WebHelper.UrlEncode(entity.HonorRightDesc);
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AdminPermission("站点配置文件", "编辑公司介绍页面")]
        public ActionResult CompanyProfile()
        {
            var model = ConfigHelper.LoadConfig<CompanyProfileModel>(ConfigFileEnum.CompanyProfile,false);
            model.JJDesc = WebHelper.UrlDecode(model.JJDesc);
            model.JJBGDesc = WebHelper.UrlDecode(model.JJBGDesc);
            model.JJOneLeftDesc = WebHelper.UrlDecode(model.JJOneLeftDesc);
            model.JJOneRightDesc = WebHelper.UrlDecode(model.JJOneRightDesc);
            model.JJTwoLeftDesc = WebHelper.UrlDecode(model.JJTwoLeftDesc);
            model.JJTwoRightDesc = WebHelper.UrlDecode(model.JJTwoRightDesc);
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [AdminPermission("站点配置文件", "保存公司介绍信息")]
        [HttpPost]
        [ValidateAntiForgeryToken,ValidateInput(false)]
        public ActionResult CompanyProfile(CompanyProfileModel entity)
        {
            if (ModelState.IsValid)
            {

                entity.JJDesc = WebHelper.UrlEncode(entity.JJDesc);
                entity.JJBGDesc = WebHelper.UrlEncode(entity.JJBGDesc);
                entity.JJOneLeftDesc = WebHelper.UrlEncode(entity.JJOneLeftDesc);
                entity.JJOneRightDesc = WebHelper.UrlEncode(entity.JJOneRightDesc);
                entity.JJTwoLeftDesc = WebHelper.UrlEncode(entity.JJTwoLeftDesc);
                entity.JJTwoRightDesc = WebHelper.UrlEncode(entity.JJTwoRightDesc);

                if (ConfigHelper.SaveConfig(ConfigFileEnum.CompanyProfile, entity))
                {
                    AddAdminLogs(Entity.SysLogMethodType.Update, "修改公司介绍");
                    return PromptView("/admin/SysConfig/CompanyProfile", "OK", "Success", "修改成功", 3);
                }
                else
                {
                    return PromptView("/admin/SysConfig/CompanyProfile", "500", "Error", "保存失败，可能是没有权限操作数据库", 3);
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