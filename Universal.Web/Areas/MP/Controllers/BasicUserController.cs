using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;
using System.Data;
using System.Data.Entity;

namespace Universal.Web.Areas.MP.Controllers
{
    /// <summary>
    /// 用户基本信息
    /// </summary>
    [OnlyBasicUser]
    public class BasicUserController : BaseMPController
    {
        // GET: MP/BasicUser
        public ActionResult Index()
        {
            if(WorkContext.UserInfo.Identity == Entity.MPUserIdentity.Normal)
            {
                ViewData["RechText"] = "升级VIP";
                ViewData["RechUrl"] = "/MP/BasicUser/ToVIP";
            }
            else
            {
                ViewData["RechText"] = "充值";
                ViewData["RechUrl"] = "/MP/BasicUser/Recharge";
            }
            var user = BLL.BLLMPUser.GetUserInfoOrAdd(WorkContext.open_id);
            ViewData["AccountBalance"] = user.AccountBalance.ToString("F2");
            return View();
        }

        /// <summary>
        /// 用户资料详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Info()
        {
            var gender = Universal.Tools.EnumHelper.GetDescription<Universal.Entity.MPUserGenderType>(WorkContext.UserInfo.Gender);
            ViewData["Gender"] = gender;
            return View();
        }

        /// <summary>
        /// 修改用户资料
        /// </summary>
        /// <returns></returns>
        public ActionResult Modify(string back)
        {
            if (WorkContext.UserInfo.IsFullInfo)
            {
                return PromptView("/MP/BasicUser/Info", "资料不能再修改了");
            }

            if (string.IsNullOrWhiteSpace(back))
            {
                ViewData["BackTxt"] = "修改个人资料";
                ViewData["BackUrl"] = "/mp/BasicUser/Info";
            }
            else
            {
                ViewData["BackTxt"] = "完善个人资料";
                ViewData["BackUrl"] = back;
            }
            return View();
        }

        /// <summary>
        /// 修改用户基本资料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Modify(string realname,string idnumber,string telphone,int gender,string bri,string code)
        {

            if (WorkContext.UserInfo.IsFullInfo)
            {
                WorkContext.AjaxStringEntity.msgbox = "资料不能再修改";
                return Json(WorkContext.AjaxStringEntity);
            }

            #region 数据验证

            if (realname.Length > 5 || realname.Length == 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "姓名长度在0~5之间";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (idnumber.Length > 18 || idnumber.Length == 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "身份证号码长度在0~18之间";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (!Tools.ValidateHelper.IsMobile(telphone))
            {
                WorkContext.AjaxStringEntity.msgbox = "手机号码格式不正确";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (gender != 1 && gender != 2)
            {
                WorkContext.AjaxStringEntity.msgbox = "性别数据非法";
                return Json(WorkContext.AjaxStringEntity);
            }
            var brithday = Tools.TypeHelper.ObjectToDateTime(bri, DateTime.Now);

            if (string.IsNullOrWhiteSpace(code))
            {
                WorkContext.AjaxStringEntity.msgbox = "请输入验证码";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (code.Length != 4)
            {
                WorkContext.AjaxStringEntity.msgbox = "验证码只有4位";
                return Json(WorkContext.AjaxStringEntity);
            }

            #endregion

            BLL.BaseBLL<Entity.MPUser> bll = new BLL.BaseBLL<Entity.MPUser>();
            var entity = bll.GetModel(p => p.OpenID == WorkContext.open_id, "ID DESC");
            if(entity == null)
            {
                WorkContext.AjaxStringEntity.msgbox= "要修改的信息不存在";
                return Json(WorkContext.AjaxStringEntity) ;
            }
            if(!string.IsNullOrWhiteSpace(entity.IDCardNumber))
            {
                //修改,判断身份证号码是否存在
                if (bll.Exists(p => p.ID != entity.ID && p.IDCardNumber == idnumber))
                {
                    WorkContext.AjaxStringEntity.msgbox = "身份证号码已存在";
                    return Json(WorkContext.AjaxStringEntity);
                }
            }
            else
            {
                //新增，判断身份证号码是否存在
                if(bll.Exists(p => p.IDCardNumber == idnumber))
                {
                    WorkContext.AjaxStringEntity.msgbox = "身份证号码已存在";
                    return Json(WorkContext.AjaxStringEntity);
                }
            }
            if (!string.IsNullOrWhiteSpace(entity.Telphone))
            {
                //修改,判断手机号码是否存在
                if (bll.Exists(p => p.ID != entity.ID && p.Telphone == telphone))
                {
                    WorkContext.AjaxStringEntity.msgbox = "手机号码已存在";
                    return Json(WorkContext.AjaxStringEntity);
                }
            }
            else
            {
                //新增，判断手机号码是否存在
                if (bll.Exists(p => p.Telphone == telphone))
                {
                    WorkContext.AjaxStringEntity.msgbox = "手机号码已存在";
                    return Json(WorkContext.AjaxStringEntity);
                }
            }
            string ver_msg = "";
            var ver_s = BLL.BLLVerificationCode.VerCode(telphone, Entity.VerificationCodeType.FullInfo, code, out ver_msg);
            if (!ver_s)
            {
                WorkContext.AjaxStringEntity.msgbox = ver_msg;
                return Json(WorkContext.AjaxStringEntity);
            }

            entity.RealName = realname;
            entity.IDCardNumber = idnumber;
            entity.Telphone = telphone;
            entity.Gender = (Entity.MPUserGenderType)gender;
            entity.Brithday = brithday;
            entity.IsFullInfo = true;
            bll.Modify(entity, "RealName", "NickName", "IDCardNumber", "Telphone", "Gender", "Brithday", "IsFullInfo");
            //更新Session中的用户信息
            BLL.BLLMPUserState.SetLogin(entity.OpenID);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "修改成功";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 升级VIP 须知
        /// </summary>
        /// <returns></returns>
        public ActionResult ToVIP()
        {
            return View();
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge(string back)
        {
            if (string.IsNullOrWhiteSpace(back)) ViewData["BackUrl"] = "/MP/BasicUser/Index";
            else ViewData["BackUrl"] = back;
            return View();
        }

    }
}