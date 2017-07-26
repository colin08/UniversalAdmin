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
            model.brithday = WorkContext.UserInfo.Brithday;


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Basic(Models.ViewModelUserBasic req)
        {
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();

           
            if (ModelState.IsValid)
            {
                var model = bll.GetModel(p => p.ID == WorkContext.UserInfo.ID);
                model.AboutMe = req.about_me;
                model.Gender = req.gender;
                model.ShorNum = req.short_num;
                model.Brithday = req.brithday;
                bll.Modify(model, "AboutMe", "Gender", "ShorNum", "Brithday");
                BLL.BLLCusUser.ModifySession(BLL.BLLCusUser.GetModel(WorkContext.UserInfo.ID));

                req.nick_name = model.NickName;
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
            BLL.BLLCusUser.ModifySession(BLL.BLLCusUser.GetModel(model.ID));
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
        public ActionResult ProjectFavorites()
        {
            return View();
        }

        /// <summary>
        /// 文件(秘籍)收藏Json数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProjectFavoritesData(int page_size, int page_index, string keyword)
        {
            List<Entity.Project> list = new List<Entity.Project>();
            int rowCount = 0;
            list = BLL.BllCusUserFavorites.GetProjectPageData(page_index, page_size, ref rowCount, WorkContext.UserInfo.ID, keyword);
            WebAjaxEntity<List<Entity.Project>> result = new WebAjaxEntity<List<Entity.Project>>();
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
        public JsonResult DelProjectFavorites(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.CusUserProjectFavorites> bll = new BLL.BaseBLL<Entity.CusUserProjectFavorites>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ProjectID) && p.CusUserID == WorkContext.UserInfo.ID);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);
        }


        /// <summary>
        /// 文件(秘籍)收藏
        /// </summary>
        /// <returns></returns>
        public ActionResult DocFavorites()
        {
            //int default_id = 0;
            //string data = BLL.BLLDocCategory.CreateDocCategoryTreeData(out default_id);
            //ViewData["TreeData"] = data;
            //ViewData["DefaultID"] = default_id;
            return View();
        }

        /// <summary>
        /// 文件(秘籍)收藏Json数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DocFavoritesData(int page_size, int page_index,string keyword)
        {
            List<Entity.DocPost> list = new List<Entity.DocPost>();
            int rowCount = 0;
            list = BLL.BllCusUserFavorites.GetDocPageData(page_index, page_size, ref rowCount, WorkContext.UserInfo.ID, keyword, 0);
            WebAjaxEntity<List<Entity.DocPost>> result = new WebAjaxEntity<List<Entity.DocPost>>();
            result.msg = 1;
            result.msgbox = CalculatePage(rowCount, page_size).ToString();
            result.data = list;
            result.total = rowCount;

            return Json(result);
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserData(int page_size, int page_index, string keyword)
        {
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            int rowCount = 0;
            List<Entity.CusUser> list = new List<Entity.CusUser>();
            if (!string.IsNullOrWhiteSpace(keyword))
                list = bll.GetPagedList(page_index, page_size, ref rowCount, p => p.NickName.Contains(keyword) || p.Telphone.Contains(keyword), "RegTime desc", p => p.CusUserJob);
            else
                list = bll.GetPagedList(page_index, page_size, ref rowCount, new List<BLL.FilterSearch>(), "RegTime desc", p => p.CusUserJob);
            WebAjaxEntity<List<Entity.CusUser>> result = new WebAjaxEntity<List<Entity.CusUser>>();
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
            bll.DelBy(p => id_list.Contains(p.DocPostID) && p.CusUserID == WorkContext.UserInfo.ID);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 我的下载
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadLog()
        {
            return View();
        }

        /// <summary>
        /// 我的下载 分页数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DownLogData(int page_size, int page_index, string keyword)
        {
            BLL.BaseBLL<Entity.DownloadLog> bll = new BLL.BaseBLL<Entity.DownloadLog>();
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            filter.Add(new BLL.FilterSearch("CusUserID", WorkContext.UserInfo.ID.ToString(), BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(keyword))
                filter.Add(new BLL.FilterSearch("Title", keyword, BLL.FilterSearchContract.like));

            List<Entity.DownloadLog> list = bll.GetPagedList(page_index, page_size, ref rowCount, filter, "AddTime desc");
            WebAjaxEntity<List<Entity.DownloadLog>> result = new WebAjaxEntity<List<Entity.DownloadLog>>();
            result.msg = 1;
            result.msgbox = CalculatePage(rowCount, page_size).ToString();
            result.data = list;
            result.total = rowCount;

            return Json(result);
        }

        /// <summary>
        /// 删除下载记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelDownLog(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.DownloadLog> bll = new BLL.BaseBLL<Entity.DownloadLog>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);
        }


        /// <summary>
        /// 我的消息
        /// </summary>
        /// <returns></returns>
        public ActionResult Message()
        {
            return View();
        }

        /// <summary>
        /// 我的消息 分页数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MessageData(int page_size, int page_index, string keyword)
        {
            BLL.BaseBLL<Entity.CusUserMessage> bll = new BLL.BaseBLL<Entity.CusUserMessage>();
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            filter.Add(new BLL.FilterSearch("CusUserID", WorkContext.UserInfo.ID.ToString(), BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(keyword))
                filter.Add(new BLL.FilterSearch("Content", keyword, BLL.FilterSearchContract.like));

            List<Entity.CusUserMessage> list = bll.GetPagedList(page_index, page_size, ref rowCount, filter, "IsRead ASC,AddTime desc");
            WebAjaxEntity<List<Entity.CusUserMessage>> result = new WebAjaxEntity<List<Entity.CusUserMessage>>();
            result.msg = 1;
            result.msgbox = CalculatePage(rowCount, page_size).ToString();
            result.data = list;
            result.total = rowCount;

            return Json(result);
        }

        /// <summary>
        /// 消息设置为已读,id=-1为将所有消息置为已读
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MessageSetRead(int id)
        {
            if (id < -1 || id == 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.CusUserMessage> bll = new BLL.BaseBLL<Entity.CusUserMessage>();
            if (id == -1)
            {
                //将所有消息置为已读
                BLL.BLLCusUserMessage.SetAllRead(WorkContext.UserInfo.ID);
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "ok";
                return Json(WorkContext.AjaxStringEntity);
            }

            var entity = bll.GetModel(p => p.ID == id);
            if (entity == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "消息不存在";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (!entity.IsRead)
            {
                entity.IsRead = true;
                bll.Modify(entity, "IsRead");
            }
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelMessage(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.CusUserMessage> bll = new BLL.BaseBLL<Entity.CusUserMessage>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);
        }

    }
}