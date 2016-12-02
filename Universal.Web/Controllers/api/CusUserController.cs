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

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public class CusUserController : BaseAPIController
    {
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
            if(string.IsNullOrWhiteSpace(req.phone_email) || string.IsNullOrWhiteSpace(req.pwd))
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
            if (user_id<=0)
            {
                response_entity.msgbox = "非法参数";
                return response_entity;
            }

            var model = BLL.BLLCusUser.GetModel(user_id);
            if(model == null)
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
        /// 构造用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Models.Response.ModelUserInfo BuilderAPPUser(Entity.CusUser model)
        {
            if (model == null)
                return null;
            Models.Response.ModelUserInfo entity = new Models.Response.ModelUserInfo();
            entity.about_me = model.AboutMe;
            entity.avatar = GetSiteUrl() + model.Avatar;
            entity.brithday = model.Brithday;
            entity.department_id = model.CusDepartmentID;
            entity.department_name = model.CusDepartment.Title;
            entity.email = model.Email;
            entity.gender = model.Gender;
            entity.id = model.ID;
            entity.job_id = model.CusUserJobID;
            entity.job_name = model.CusUserJob.Title;
            entity.nick_name = model.NickName;
            entity.shor_num = model.ShorNum;
            entity.telphone = model.Telphone;
            return entity;
        }

    }
}
