using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Universal.Tools;
using Universal.Web.Framework;
using System.Data.Entity;

namespace Universal.Web.Controllers.api
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public class CusUserController : BaseAPIController
    {
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="tel">手机号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/code/send")]
        public WebAjaxEntity<string> SendCode(string tel)
        {
            if (string.IsNullOrWhiteSpace(tel))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法手机号";
                return WorkContext.AjaxStringEntity;
            }
            if (!BLL.BLLCusUser.Exists(tel))
            {
                WorkContext.AjaxStringEntity.msgbox = "手机号不存在";
                return WorkContext.AjaxStringEntity;
            }
            string msg = "";
            Guid guid = Guid.NewGuid();
            BLL.BLLVerification.Send(tel, guid, Entity.CusVerificationType.Modify, out msg);
            if (msg.Equals("OK"))
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.data = guid.ToString();
                return WorkContext.AjaxStringEntity;
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
                return WorkContext.AjaxStringEntity;
            }

        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="code">验证码</param>
        /// <param name="tel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/code/check")]
        public WebAjaxEntity<string> CheckCode(string guid, string code, string tel)
        {
            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(code))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            Entity.CusUser model = bll.GetModel(p => p.Telphone == tel);
            if (model == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return WorkContext.AjaxStringEntity;
            }

            Guid new_guid = new Guid();
            try
            {
                new_guid = new Guid(guid);
            }
            catch (Exception)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法guid";
                return WorkContext.AjaxStringEntity;
            }


            if (BLL.BLLVerification.Check(new_guid, Entity.CusVerificationType.Modify, code))
            {
                model.Password = SecureHelper.MD5(WebSite.ResetPwd);
                bll.Modify(model, "Password");
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "密码已重置为:" + WebSite.ResetPwd;
                return WorkContext.AjaxStringEntity;
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = "验证码错误";
                return WorkContext.AjaxStringEntity;
            }
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/user/login")]
        public WebAjaxEntity<Models.Response.ModelUserInfo> UserLogin([FromBody]Models.Request.ModelUserLogin req)
        {
            WebAjaxEntity<Models.Response.ModelUserInfo> response_entity = new WebAjaxEntity<Models.Response.ModelUserInfo>();
            if (string.IsNullOrWhiteSpace(req.phone_email) || string.IsNullOrWhiteSpace(req.pwd))
            {
                response_entity.msgbox = "参数不能有空";
                return response_entity;
            }

            string passworld = SecureHelper.MD5(req.pwd);
            Entity.CusUser model = null;

            //如果是邮箱
            if (ValidateHelper.IsEmail(req.phone_email))
                model = BLL.BLLCusUser.GetModel(req.phone_email, passworld);
            else
                model = BLL.BLLCusUser.GetModelTelphone(req.phone_email, passworld);
            if (model == null)
            {
                response_entity.msgbox = "用户名或密码错误";
                return response_entity;
            }
            if (!model.Status)
            {
                response_entity.msgbox = "账户已被禁用";
                return response_entity;
            }

            response_entity.data = BuilderAPPUser(model);
            response_entity.msg = 1;
            response_entity.msgbox = "登录成功";
            return response_entity;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/info/{user_id:int}")]
        public WebAjaxEntity<Models.Response.ModelUserInfo> GetUserInfo(int user_id)
        {
            WebAjaxEntity<Models.Response.ModelUserInfo> response_entity = new WebAjaxEntity<Models.Response.ModelUserInfo>();
            if (user_id <= 0)
            {
                response_entity.msgbox = "非法参数";
                return response_entity;
            }

            var model = BLL.BLLCusUser.GetModel(user_id);
            if (model == null)
            {
                response_entity.msgbox = "用户不存在";
                return response_entity;
            }

            if (!model.Status)
            {
                response_entity.msgbox = "账户已被禁用";
                return response_entity;
            }

            response_entity.data = BuilderAPPUser(model);
            response_entity.msg = 1;
            response_entity.msgbox = "登录成功";
            return response_entity;


        }

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/search")]
        public WebAjaxEntity<List<Models.Response.ModelUserInfo>> SearchUser(int department_id, string search_word)
        {
            WebAjaxEntity<List<Models.Response.ModelUserInfo>> response_entity = new WebAjaxEntity<List<Models.Response.ModelUserInfo>>();
            if(department_id <=0)
            {
                response_entity.msg = 0;
                response_entity.msgbox = "搜索需指定部门";
                return response_entity;
            }
            List<Models.Response.ModelUserInfo> response_list = new List<Models.Response.ModelUserInfo>();
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            var db_list = new List<Entity.CusUser>();
            if (string.IsNullOrWhiteSpace(search_word))
                db_list = bll.GetListBy(0, p => p.CusDepartmentID == department_id, "RegTime ASC", p => p.CusDepartment);
            else
                db_list = bll.GetListBy(0, p => p.CusDepartmentID == department_id && p.NickName.Contains(search_word) || p.Telphone.Contains(search_word) || p.Email.Contains(search_word), "RegTime ASC", p => p.CusDepartment);

            foreach (var item in db_list)
                response_list.Add(BuilderAPPUser(item));
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = response_list;
            return response_entity;
        }

        /// <summary>
        /// 获取所有用户列表,供秘籍选择用户使用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/all")]
        public WebAjaxEntity<List<Models.Response.SelectUser>> GetAllUser()
        {
            WebAjaxEntity<List<Models.Response.SelectUser>> response_entity = new WebAjaxEntity<List<Models.Response.SelectUser>>();
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            List<Models.Response.SelectUser> response_list = new List<Models.Response.SelectUser>();
            foreach (var item in bll.GetListBy(0, p => p.Status == true, "RegTime asc"))
            {
                Models.Response.SelectUser model = new Models.Response.SelectUser();
                model.user_id = item.ID;
                model.telphone = item.Telphone;
                model.nickname = item.NickName;
                response_list.Add(model);
            }
            response_entity.data = response_list;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }

        /// <summary>
        /// 构造用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Models.Response.ModelUserInfo BuilderAPPUser(Entity.CusUser model)
        {
            if (model == null)
                return null;
            Models.Response.ModelUserInfo entity = new Models.Response.ModelUserInfo();
            entity.about_me = model.AboutMe == null ? "" : model.AboutMe;
            entity.avatar = GetSiteUrl() + model.Avatar;
            entity.brithday = model.Brithday;
            entity.department_id = model.CusDepartmentID;
            if (model.CusDepartment == null)
            {
                Entity.CusDepartment depart = new BLL.BaseBLL<Entity.CusDepartment>().GetModel(p => p.ID == model.CusDepartmentID);
                if (depart != null)
                    entity.department_name = depart.Title;
            }
            else
                entity.department_name = model.CusDepartment.Title;
            entity.email = model.Email;
            entity.gender = model.Gender;
            entity.id = model.ID;
            entity.job_id = model.CusUserJobID;
            if (model.CusUserJob == null)
            {
                Entity.CusUserJob job = new BLL.BaseBLL<Entity.CusUserJob>().GetModel(p => p.ID == model.CusUserJobID);
                if (job != null)
                    entity.job_name = job.Title;
            }
            else
                entity.job_name = model.CusUserJob.Title;
            entity.nick_name = model.NickName;
            entity.shor_num = model.ShorNum == null? "":model.ShorNum;
            entity.telphone = model.Telphone;
            entity.is_department_manager = BLL.BLLDepartment.CheckUserIsManager(entity.id, entity.department_id);
            return entity;
        }

    }
}
