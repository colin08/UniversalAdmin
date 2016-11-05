using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    public class AccountController : BaseHBLController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            var viewModelLogin = new Models.ViewModelUserLogin();
            if (WorkContext.UserInfo != null)
                return Redirect("/");
            //如果保存了cookie，则为用户做自动登录
            if (!string.IsNullOrWhiteSpace(WebHelper.GetCookie(CookieKey.Web_Is_Remeber)))
            {
                if (WebHelper.GetCookie(CookieKey.Web_Is_Remeber) == "1")
                {
                    int uid = TypeHelper.ObjectToInt(WebHelper.GetCookie(CookieKey.Web_Login_UserID));
                    string upwd = WebHelper.GetCookie(CookieKey.Web_Login_UserPassword);
                    BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
                    List<BLL.FilterSearch> filters = new List<BLL.FilterSearch>();
                    filters.Add(new BLL.FilterSearch("ID", uid.ToString(), BLL.FilterSearchContract.等于));
                    filters.Add(new BLL.FilterSearch("Password", upwd, BLL.FilterSearchContract.等于));
                    Entity.CusUser model = bll.GetModel(filters, p => p.CusUserRoute.Select(s => s.CusRoute));
                    if (model != null)
                    {
                        if (model.Status)
                        {
                            Session[SessionKey.Web_User_Info] = model;
                            Session.Timeout = 60; //一小时不操作，session就过期
                            model.LastLoginTime = DateTime.Now;
                            bll.Modify(model, new string[] { "LastLoginTime" });
                            return Redirect("/");
                        }
                        else
                            return View(viewModelLogin);
                    }
                    else
                        return View(viewModelLogin);
                }
                else
                    return View(viewModelLogin);
            }

            return View(viewModelLogin);
        }

        /// <summary>
        /// 登录处理
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.ViewModelUserLogin req)
        {
            if(ModelState.IsValid)
            {
                BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
                string passworld = SecureHelper.MD5(req.password);
                Entity.CusUser model = null;
                
                //如果是邮箱
                if(ValidateHelper.IsEmail(req.user_name))
                    model = bll.GetModel(p => p.Email == req.user_name && p.Password == passworld, p => p.CusUserRoute.Select(s => s.CusRoute));
                else
                    model = bll.GetModel(p => p.Telphone == req.user_name && p.Password == passworld, p => p.CusUserRoute.Select(s => s.CusRoute));
                if(model == null)
                {
                    ModelState.AddModelError("user_name", "用户不存在或密码错误");
                    return View(req);
                }
                if(!model.Status)
                {
                    ModelState.AddModelError("user_name", "用户已被禁用");
                    return View(req);
                }

                Session[SessionKey.Web_User_Info] = model;
                Session.Timeout = 60;
                if (req.is_rember)
                {
                    WebHelper.SetCookie(CookieKey.Web_Is_Remeber, "1", 14400);
                    WebHelper.SetCookie(CookieKey.Web_Login_UserID, model.ID.ToString(), 14400);
                    WebHelper.SetCookie(CookieKey.Web_Login_UserPassword, model.Password, 14400);
                }
                else
                {
                    WebHelper.SetCookie(CookieKey.Web_Login_UserID, model.ID.ToString());
                    WebHelper.SetCookie(CookieKey.Web_Login_UserPassword, model.Password);
                }
                model.LastLoginTime = DateTime.Now;
                bll.Modify(model, new string[] { "LastLoginTime" });
                return Redirect("/");

            }
            return View(req);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            BLL.BLLCusUser.LoginOut();
            return Redirect("/");
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPwd()
        { 
            return View(new Models.ViewModelSendPwd());
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPwd(Models.ViewModelSendPwd req)
        {
            if(string.IsNullOrWhiteSpace(WebSite.ResetPwd))
                ModelState.AddModelError("guid", "站点配置文件有误");

            if(ModelState.IsValid)
            {                
                if(BLL.BLLVerification.Check(req.guid, Entity.CusVerificationType.RestPwd, req.code))
                {
                    BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
                    Entity.CusUser model = bll.GetModel(p => p.Telphone == req.telphone);
                    if(model != null)
                    {
                        string passworld = SecureHelper.MD5(WebSite.ResetPwd);
                        model.Password = passworld;
                        bll.Modify(model, new string[] { "Password" });
                    }
                    TempData["Telphone"] = req.telphone;
                    return RedirectToAction("ResetSuc", "Account");
                }
            }

            return View(req);
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendCode(string gui,string tel)
        {
            if (string.IsNullOrWhiteSpace(tel))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法手机号";
                return Json(WorkContext.AjaxStringEntity);
            }

            string msg = "";
            Guid guid = new Guid();
            try
            {
                guid = new Guid(gui);
            }
            catch (Exception)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法Gui";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            if(!bll.Exists(p=>p.Telphone == tel))
            {
                WorkContext.AjaxStringEntity.msgbox = "没有该用户";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BLLVerification.Send(tel, guid, Entity.CusVerificationType.RestPwd, out msg);
            if (msg.Equals("OK"))
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.data = guid.ToString();
                return Json(WorkContext.AjaxStringEntity);
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
                return Json(WorkContext.AjaxStringEntity);
            }

        }

        /// <summary>
        /// 重置密码成功
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetSuc()
        {
            ViewBag.Telphone = TempData["Telphone"].ToString();
            return View();
        }

    }
}