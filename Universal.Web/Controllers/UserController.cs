using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    [BasicUserAuth]
    public class UserController : BaseHBLController
    {
        /// <summary>
        /// 基本资料
        /// </summary>
        /// <returns></returns>
        public ActionResult Basic()
        {
            var model = new Models.ViewModelUserBasic();
            model.about_me = WorkContext.UserInfo.AboutMe;
            model.gender = WorkContext.UserInfo.Gender;
            model.nick_name = WorkContext.UserInfo.NickName;
            model.short_num = WorkContext.UserInfo.ShorNum;
            DateTime dt = DateTime.Now;
            if (WorkContext.UserInfo.Brithday != null)
                dt = TypeHelper.ObjectToDateTime(WorkContext.UserInfo.Brithday);

            model.year = dt.Year.ToString();
            model.month = dt.Month.ToString();
            model.day = dt.Day.ToString();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Basic(Models.ViewModelUserBasic req)
        {
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();

            if (string.IsNullOrWhiteSpace(req.year) || string.IsNullOrWhiteSpace(req.month) || string.IsNullOrWhiteSpace(req.day))
            {
                ModelState.AddModelError("year", "请填写完整");
            }

            if (ModelState.IsValid)
            {
                var model = bll.GetModel(p => p.ID == WorkContext.UserInfo.ID);
                model.AboutMe = req.about_me;
                model.Gender = req.gender;
                model.NickName = req.nick_name;
                model.ShorNum = req.short_num;
                if (!string.IsNullOrWhiteSpace(req.year) && !string.IsNullOrWhiteSpace(req.month) && !string.IsNullOrWhiteSpace(req.day))
                    model.Brithday = TypeHelper.ObjectToDateTime(req.year + "/" + req.month + "/" + req.day);
                else
                    model.Brithday = null;
                bll.Modify(model, "AboutMe", "Gender", "NickName", "ShorNum", "Brithday");
                BLL.BLLCusUser.ModifySession(BLL.BLLCusUser.GetModel(WorkContext.UserInfo.ID));


                req.State = true;
            }

            return View(req);
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <returns></returns>
        public ActionResult Avatar()
        {
            return View();
        }

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="avatar"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Avatar(string avatar)
        {
            if (WorkContext.UserInfo == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "Time Out";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (string.IsNullOrWhiteSpace(avatar))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            var model = bll.GetModel(p => p.ID == WorkContext.UserInfo.ID);
            if (model != null)
            {
                model.Avatar = avatar;
                bll.Modify(model, "Avatar");
                WorkContext.AjaxStringEntity.msg = 1;
                BLL.BLLCusUser.ModifySession(BLL.BLLCusUser.GetModel(WorkContext.UserInfo.ID));
                return Json(WorkContext.AjaxStringEntity);
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return Json(WorkContext.AjaxStringEntity);
            }
        }

        /// <summary>
        /// 安全设置
        /// </summary>
        /// <returns></returns>
        public ActionResult Safe()
        {
            return View();
        }

        /// <summary>
        /// 验证码的校验
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ActionResult CheckCode()
        {
            //生成验证码
            ValidateCode validateCode = new ValidateCode();
            string code = validateCode.CreateValidateCode(4);
            Session["ValidateCode"] = code;
            byte[] bytes = validateCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        /// <summary>
        /// 修改邮箱-检查密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckPwd(string pwd, string code)
        {
            if (WorkContext.UserInfo == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "Time Out";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (Session["ValidateCode"] == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "验证码已过期";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (!SecureHelper.MD5(pwd).Equals(WorkContext.UserInfo.Password))
            {
                WorkContext.AjaxStringEntity.msgbox = "密码错误";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (!Session["ValidateCode"].ToString().Equals(code))
            {
                WorkContext.AjaxStringEntity.msgbox = "验证码错误";
                return Json(WorkContext.AjaxStringEntity);
            }
            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyEmail(string email)
        {
            if (WorkContext.UserInfo == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "Time Out";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            if (!ValidateHelper.IsEmail(email))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法邮箱格式";
                return Json(WorkContext.AjaxStringEntity);
            }

            var model = bll.GetModel(p => p.ID == WorkContext.UserInfo.ID);
            if (email.Equals(model.Email))
            {
                WorkContext.AjaxStringEntity.msgbox = "新邮箱和原来的邮箱不能一样";
                return Json(WorkContext.AjaxStringEntity);
            }
            model.Email = email;            
            bll.Modify(model, "Email");
            BLL.BLLCusUser.ModifySession(model);
            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 检查邮箱输入是否正确
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckTel(string tel)
        {
            if (WorkContext.UserInfo == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "Time Out";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (string.IsNullOrWhiteSpace(tel))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法手机号";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (!WorkContext.UserInfo.Telphone.Equals(tel))
            {
                WorkContext.AjaxStringEntity.msgbox = "错误的手机号";
                return Json(WorkContext.AjaxStringEntity);
            }

            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);

        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendCode(string tel)
        {
            if (WorkContext.UserInfo == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "Time Out";
                return Json(WorkContext.AjaxStringEntity);
            }


            if (string.IsNullOrWhiteSpace(tel))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法手机号";
                return Json(WorkContext.AjaxStringEntity);
            }

            string msg = "";
            Guid guid = Guid.NewGuid();
            BLL.BLLVerification.Send(tel, guid, Entity.CusVerificationType.Modify, out msg);
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
        /// 校验手机验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckTelCode(string guid, string code)
        {
            if (WorkContext.UserInfo == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "Time Out";
                return Json(WorkContext.AjaxStringEntity);
            }


            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(code))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            Guid new_guid = new Guid();
            try
            {
                new_guid = new Guid(guid);
            }
            catch (Exception)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法guid";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (BLL.BLLVerification.Check(new_guid, Entity.CusVerificationType.Modify, code))
            {
                WorkContext.AjaxStringEntity.msg = 1;
                return Json(WorkContext.AjaxStringEntity);
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = "验证码错误";
                return Json(WorkContext.AjaxStringEntity);
            }

        }

        /// <summary>
        /// 验证手机号是否存在
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckTelExists(string tel)
        {
            if (WorkContext.UserInfo == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "Time Out";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (string.IsNullOrWhiteSpace(tel))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法手机号";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (tel.Equals(WorkContext.UserInfo.Telphone))
            {
                WorkContext.AjaxStringEntity.msgbox = "新手机号不能和原手机号一样";
                return Json(WorkContext.AjaxStringEntity);
            }


            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            if (bll.Exists(p => p.Telphone == tel))
            {
                WorkContext.AjaxStringEntity.msgbox = "手机号已被使用";
                return Json(WorkContext.AjaxStringEntity);
            }

            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 修改手机号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyTel(string tel)
        {
            if (WorkContext.UserInfo == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "Time Out";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (string.IsNullOrWhiteSpace(tel))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法手机号";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (!ValidateHelper.IsMobile(tel))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法手机号2";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            if (bll.Exists(p => p.Telphone == tel))
            {
                WorkContext.AjaxStringEntity.msgbox = "手机号已被使用";
                return Json(WorkContext.AjaxStringEntity);
            }

            var model = bll.GetModel(p => p.ID == WorkContext.UserInfo.ID);
            if (model == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return Json(WorkContext.AjaxStringEntity);
            }

            model.Telphone = tel;
            bll.Modify(model, "Telphone");
            BLL.BLLCusUser.LoginOut();
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "修改成功，请使用新手机号重新登录";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="old"></param>
        /// <param name="newp"></param>
        /// <returns></returns>
        public JsonResult ModifyPwd(string old, string newp)
        {
            if (WorkContext.UserInfo == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "Time Out";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (string.IsNullOrWhiteSpace(old) || string.IsNullOrWhiteSpace(newp))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (newp.Length < 3 || newp.Length > 30)
            {
                WorkContext.AjaxStringEntity.msgbox = "密码在3-30位之间";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            var model = bll.GetModel(p => p.ID == WorkContext.UserInfo.ID);
            if (model == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return Json(WorkContext.AjaxStringEntity);
            }
            string old_pwd = SecureHelper.MD5(old);
            string new_pwd = SecureHelper.MD5(newp);
            if (!model.Password.Equals(old_pwd))
            {
                WorkContext.AjaxStringEntity.msgbox = "旧密码不正确";
                return Json(WorkContext.AjaxStringEntity);
            }

            model.Password = new_pwd;
            bll.Modify(model, "Password");
            BLL.BLLCusUser.LoginOut();
            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);
        }
                
        /// <summary>
        /// 项目收藏
        /// </summary>
        /// <returns></returns>
        public ActionResult XMSC()
        {
            return View();
        }

        /// <summary>
        /// 文件(秘籍)收藏
        /// </summary>
        /// <returns></returns>
        public ActionResult DocFavorites()
        {
            int default_id = 0;
            string data = BLL.BLLDocCategory.CreateDocCategoryTreeData(out default_id);
            ViewData["TreeData"] = data;
            ViewData["DefaultID"] = default_id;
            return View();
        }

        /// <summary>
        /// 文件(秘籍)收藏Json数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DocFavoritesData(int page_size, int page_index, int doc_id, string keyword)
        {
            List<Entity.DocPost> list = new List<Entity.DocPost>();
            int rowCount = 0;
            list = BLL.BllCusUserFavorites.GetPageData(page_index, page_size, ref rowCount, WorkContext.UserInfo.ID, keyword, doc_id);
            WebAjaxEntity<List<Entity.DocPost>> result = new WebAjaxEntity<List<Entity.DocPost>>();
            result.msg = 1;
            result.msgbox = CalculatePage(rowCount, page_size).ToString();
            result.data = list;
            result.total = rowCount;

            return Json(result);
        }

        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelDocFavorites(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.CusUserDocFavorites> bll = new BLL.BaseBLL<Entity.CusUserDocFavorites>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 我的下载
        /// </summary>
        /// <returns></returns>
        public ActionResult MyDown()
        {
            return View();
        }

        /// <summary>
        /// 我的消息
        /// </summary>
        /// <returns></returns>
        public ActionResult MyMsg()
        {
            return View();
        }

    }
}