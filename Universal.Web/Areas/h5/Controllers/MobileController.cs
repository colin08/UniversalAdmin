﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Areas.h5.Controllers
{
    public class MobileController : BaseHBLMobileController
    {
        /// <summary>
        /// 做登录
        /// </summary>
        /// <returns></returns>
        public ActionResult login(string sign)
        {
            Tools.Crypto3DES des = new Tools.Crypto3DES(Tools.SiteKey.DES3KEY);
            string[] vals = des.DESDeCode(sign).Split('&');
            if (vals.Length != 3)
                return Content("Error Request.");
            DateTime dt_now = DateTime.Now;
            DateTime dt_old = Tools.WebHelper.GetTime(vals[2], dt_now);
            double diff = Tools.WebHelper.DateTimeDiff(dt_old, dt_now, "as");
            if (diff > 10)
                return Content("过时的请求.");

            int user_id = TypeHelper.ObjectToInt(vals[0], 0);
            int project_id = TypeHelper.ObjectToInt(vals[1], 0);
            if (user_id == 0 || project_id == 0)
                return Content("非法参数");

            //做登录
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            var entity = bll.GetModel(p => p.ID == user_id,p=>p.CusUserJob);
            if (entity == null)
                return Content("用户不存在");

            Session[SessionKey.Web_User_Info] = entity;
            WebHelper.SetCookie(CookieKey.Web_Login_UserID, entity.ID.ToString());
            WebHelper.SetCookie(CookieKey.Web_Login_UserPassword, entity.Password);
            Session.Timeout = 60; //一小时不操作，session就过期
            entity.LastLoginTime = DateTime.Now;
            bll.Modify(entity, new string[] { "LastLoginTime" });
            return Redirect("/H5/Mobile/Flow?project_id=" + project_id);
        }

        /// <summary>
        /// 流程页面
        /// </summary>
        /// <param name="project_id"></param>
        /// <returns></returns>
        public ActionResult Flow(string project_id)
        {
            ViewData["can_edit"] = false;
            int project = TypeHelper.ObjectToInt(project_id, 0);
            BLL.BaseBLL<Entity.Project> bll = new BLL.BaseBLL<Entity.Project>();
            var entity = bll.GetModel(p => p.ID == project, p => p.ProjectUsers);
            if (entity == null)
            {
                return Content("无此流程");
            }
            ViewData["project_id"] = entity.ID;
            if (entity.ProjectUsers.ToList().Any(p => p.CusUserID == WorkContext.UserInfo.ID))
                ViewData["can_edit"] = true;
            return View();
        }
    }
}