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
        /// Android版本升级
        /// </summary>
        /// <param name="vcode">当前升级号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/appversion/check")]
        public WebAjaxEntity<Entity.AppVersion> AppUpdate(int vcode)
        {
            WebAjaxEntity<Entity.AppVersion> response_entity = new WebAjaxEntity<Entity.AppVersion>();
            var entity = BLL.BLLAppVersion.GetLatest(Entity.APPVersionPlatforms.Android);
            if (entity == null)
            {
                response_entity.msgbox = "无版本可用";
                return response_entity;
            }
            if (entity.VersionCode <= vcode)
            {
                response_entity.msgbox = "当前已是最新版本";
                return response_entity;
            }
            entity.DownUrl = GetSiteUrl() + entity.DownUrl;
            entity.LogoImg = GetSiteUrl() + entity.LogoImg;
            response_entity.msg = 1;
            response_entity.msgbox = "有新版本可用";
            response_entity.data = entity;
            return response_entity;
        }

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
            //if (!BLL.BLLCusUser.Exists(tel))
            //{
            //    WorkContext.AjaxStringEntity.msgbox = "手机号不存在";
            //    return WorkContext.AjaxStringEntity;
            //}
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
        [Route("api/v1/user/modify/resetpwd")]
        public WebAjaxEntity<string> ResetPwd(string guid, string code, string tel)
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
        /// 校验密码
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="pwd">密码，经过3DES加密</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/check/pwd")]
        public WebAjaxEntity<string> CheckPwd(int user_id, string pwd)
        {
            if (user_id <= 0 || string.IsNullOrWhiteSpace(pwd))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            Entity.CusUser model = bll.GetModel(p => p.ID == user_id);
            if (model == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return WorkContext.AjaxStringEntity;
            }
            Crypto3DES des = new Crypto3DES(SiteKey.DES3KEY);
            string password = des.DESDeCode(pwd);
            string md5_pwd = SecureHelper.MD5(password);
            if (model.Password.Equals(md5_pwd))
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "验证成功";
                return WorkContext.AjaxStringEntity;
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = "验证失败";
                return WorkContext.AjaxStringEntity;
            }
        }

        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="new_email">新邮箱</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/modify/email")]
        public WebAjaxEntity<string> ModifyEmail(int user_id, string new_email)
        {
            if (new_email.IndexOf('@') == -1)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法邮箱";
                return WorkContext.AjaxStringEntity;
            }
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            Entity.CusUser model = bll.GetModel(p => p.ID == user_id);
            if (model == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return WorkContext.AjaxStringEntity;
            }

            model.Email = new_email;
            bll.Modify(model, "Email");
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "邮箱已重置为:" + new_email;
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user_id">用户id</param>
        /// <param name="old_pwd">旧密码，经过3DES加密</param>
        /// <param name="new_pwd">新密码，经过3DES加密</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/modify/email")]
        public WebAjaxEntity<string> ModifyPwd(int user_id, string old_pwd, string new_pwd)
        {
            Crypto3DES des = new Crypto3DES(SiteKey.DES3KEY);
            string old_password = des.DESDeCode(old_pwd);
            string new_password = des.DESDeCode(new_pwd);
            string db_old_pwd = SecureHelper.MD5(old_password);
            string db_new_pwd = SecureHelper.MD5(new_password);
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            Entity.CusUser model = bll.GetModel(p => p.ID == user_id);
            if (model == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return WorkContext.AjaxStringEntity;
            }
            if (!model.Password.Equals(db_old_pwd))
            {
                WorkContext.AjaxStringEntity.msgbox = "原密码不正确";
                return WorkContext.AjaxStringEntity;
            }

            model.Password = db_new_pwd;
            bll.Modify(model, "Password");
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "密码修改成功";
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 校验验证码是否正确(校验之后验证码就失效),正确与否在data里，bool值
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="code"></param>
        /// <param name="tel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/check/code")]
        public WebAjaxEntity<bool> CheckCode(string guid, string code, string tel)
        {
            WebAjaxEntity<bool> result = new WebAjaxEntity<bool>();

            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(code))
            {
                result.msgbox = "非法参数";
                return result;
            }
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            Entity.CusUser model = bll.GetModel(p => p.Telphone == tel);
            if (model == null)
            {
                result.msgbox = "用户不存在";
                return result;
            }

            Guid new_guid = new Guid();
            try
            {
                new_guid = new Guid(guid);
            }
            catch (Exception)
            {
                result.msgbox = "非法guid";
                return result;
            }


            if (BLL.BLLVerification.Check(new_guid, Entity.CusVerificationType.Modify, code))
            {
                result.msg = 1;
                result.msgbox = "ok";
                result.data = true;
                return result;
            }
            else
            {
                result.msgbox = "验证码错误";
                return result;
            }
        }

        /// <summary>
        /// 修改手机号
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="code"></param>
        /// <param name="user_id">用户ID</param>
        /// <param name="new_tel">新手机号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/modify/tel")]
        public WebAjaxEntity<string> ModifyTelphone(string guid, string code, int user_id, string new_tel)
        {
            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(code))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }

            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            if (bll.GetModel(p => p.Telphone == new_tel) != null)
            {
                WorkContext.AjaxStringEntity.msgbox = "该手机号已存在";
                return WorkContext.AjaxStringEntity;
            }
            Entity.CusUser model = bll.GetModel(p => p.ID == user_id);
            if (model == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "该用户不存在";
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
                model.Telphone = new_tel;
                bll.Modify(model, "Telphone");
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "手机号修改成功";
                return WorkContext.AjaxStringEntity;
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = "验证码错误";
                return WorkContext.AjaxStringEntity;
            }
        }

        /// <summary>
        /// 判断用户是否是部门管理员，用于该用户是否必选选择审批人
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        //[HttpGet]
        //[Route("api/v1/user/check/isadmin")]
        //public WebAjaxEntity<bool> UserIsAdmin (int user_id)
        //{
        //    WebAjaxEntity<bool> result = new WebAjaxEntity<bool>();
        //    result.data = BLL.BLLCusUser.CheckUserIsAdmin(user_id);
        //    result.msgbox = "ok";
        //    result.msg = 1;
        //    return result;
        //}

        /// <summary>
        /// 设置用户消息推送状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Route("api/v1/user/push/trun")]
        [HttpPost]
        public WebAjaxEntity<string> SetPushTurn([FromBody]Models.Request.UserPushTrun req)
        {
            var entity_user = BLL.BLLCusUser.GetSimpleModel(req.user_id);
            if (entity_user == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return WorkContext.AjaxStringEntity;
            }
            string data = "";
            if (!string.IsNullOrWhiteSpace(req.data))
                data = "," + req.data + ",";

            BLL.BaseBLL<Entity.CusUserPushTurn> bll = new BLL.BaseBLL<Entity.CusUserPushTurn>();
            var entity = bll.GetModel(p => p.CusUserID == req.user_id);
            if (entity == null)
            {
                entity = new Entity.CusUserPushTurn();
                entity.OnStr = data;
                entity.CusUserID = req.user_id;
                bll.Add(entity);
            }
            else
            {
                entity.OnStr = data;
                bll.Modify(entity, "OnStr");
            }

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "设置成功";
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 获取用户设置的推送开关状态
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/push/get")]
        public WebAjaxEntity<string> GetPushTrun(int user_id)
        {
            BLL.BaseBLL<Entity.CusUserPushTurn> bll = new BLL.BaseBLL<Entity.CusUserPushTurn>();
            var entity = bll.GetModel(p => p.CusUserID == user_id);
            if (entity == null)
                WorkContext.AjaxStringEntity.data = null;
            else
            {
                string temp_str = entity.OnStr;
                if (!string.IsNullOrWhiteSpace(temp_str))
                {
                    if (temp_str.StartsWith(","))
                        temp_str = temp_str.Substring(1, temp_str.Length - 1);
                    if (temp_str.EndsWith(","))
                        temp_str = temp_str.Substring(0, temp_str.Length - 1);
                }
                WorkContext.AjaxStringEntity.data = temp_str;
            }

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return WorkContext.AjaxStringEntity;
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

            List<Models.Response.ModelUserInfo> response_list = new List<Models.Response.ModelUserInfo>();
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            int to = 0;
            var db_list = BLL.BLLCusUser.GetPageData(1, 1000, ref to, department_id, search_word);
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
                response_list.Add(BuilderSelectUser(item));
            }
            response_entity.data = response_list;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }

        #region 工作计划
        /// <summary>
        /// 获取我的工作计划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/user/workplan/list")]
        public WebAjaxEntity<List<Models.Response.WorkPlan>> GetJobPlan([FromBody]Models.Request.BasePage req)
        {
            WebAjaxEntity<List<Models.Response.WorkPlan>> response_entity = new WebAjaxEntity<List<Models.Response.WorkPlan>>();
            List<Models.Response.WorkPlan> response_list = new List<Models.Response.WorkPlan>();
            int rowCount = 0;
            List<Entity.WorkPlan> db_list = BLL.BLLWorkPlan.GetPagedList(req.page_index, req.page_size, ref rowCount, req.user_id);
            foreach (var item in db_list)
                response_list.Add(BuildWorkPlan(item));

            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = response_list;
            response_entity.total = rowCount;
            return response_entity;
        }

        /// <summary>
        /// 获取工作计划详情
        /// </summary>
        /// <param name="id">工作计划id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/workplan/info")]
        public WebAjaxEntity<Models.Response.WorkPlan> GetJobPlanInfo(int id)
        {
            WebAjaxEntity<Models.Response.WorkPlan> response_entity = new WebAjaxEntity<Models.Response.WorkPlan>();
            Entity.WorkPlan entity = BLL.BLLWorkPlan.GetModel(id);
            if (entity == null)
            {
                response_entity.msgbox = "工作计划不存在";
                return response_entity;
            }
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = BuildWorkPlan(entity);
            return response_entity;
        }

        /// <summary>
        /// 构建工作计划单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static Models.Response.WorkPlan BuildWorkPlan(Entity.WorkPlan entity)
        {
            if (entity == null)
                return null;
            Models.Response.WorkPlan model = new Models.Response.WorkPlan();

            model.approve_time = entity.ApproveTime;
            model.approve_user_id = TypeHelper.ObjectToInt(entity.ApproveUserID, 0);
            model.approve_user_name = entity.ApproveUser == null ? "" : entity.ApproveUser.NickName;
            model.begin_time = entity.BeginTime;
            model.end_time = entity.EndTime;
            model.id = entity.ID;
            model.approve_status = entity.ApproveStatus;
            model.approve_status_text = entity.ApproveStatusTxt;
            model.approve_status_text = entity.ApproveRemark;
            model.week_text = entity.WeekText;
            if (entity.WorkPlanItemList != null)
            {
                foreach (var plan_item in entity.WorkPlanItemList)
                {
                    Models.Response.WorkPlanItem model_item = new Models.Response.WorkPlanItem();
                    model_item.content = plan_item.Content;
                    model_item.done_time = plan_item.DoneTime;
                    model_item.remark = plan_item.Remark;
                    model_item.status = plan_item.Status;
                    model_item.status_text = plan_item.StatusText;
                    model_item.title = plan_item.Title;
                    model_item.want_taget = plan_item.WantTaget;
                    model.plan_item.Add(model_item);
                }
            }
            return model;
        }


        /// <summary>
        /// 添加我的工作计划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/user/workplan/modify")]
        public WebAjaxEntity<string> AddJobPlan([FromBody]Models.Request.WorkPlanR req)
        {
            var entity_user = new BLL.BaseBLL<Entity.CusUser>().GetModel(p => p.ID == req.user_id);
            if (entity_user == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return WorkContext.AjaxStringEntity;
            }
            BLL.BaseBLL<Entity.WorkPlan> bll = new BLL.BaseBLL<Entity.WorkPlan>();
            var entity = new Entity.WorkPlan();
            if (req.id > 0)
                entity = bll.GetModel(p => p.ID == req.id);
            entity.BeginTime = req.begin_time;
            entity.CusUserID = req.user_id;
            entity.EndTime = req.end_time;
            entity.WeekText = req.week_text;
            if (req.approve_user_id <= 0)
                entity.ApproveUserID = null;
            else
            {
                if (req.approve_user_id == req.user_id)
                {
                    WorkContext.AjaxStringEntity.msgbox = "不能审批自己的计划";
                    return WorkContext.AjaxStringEntity;
                }
                entity.ApproveUserID = req.approve_user_id;

            }
            if (req.plan_item != null)
            {
                if (entity.WorkPlanItemList == null)
                    entity.WorkPlanItemList = new List<Entity.WorkPlanItem>();

                foreach (var item in req.plan_item)
                {
                    Entity.WorkPlanItem model = new Entity.WorkPlanItem();
                    model.Content = item.content;
                    model.DoneTime = item.done_time;
                    model.Remark = item.remark;
                    model.Status = item.status;
                    model.Title = item.title;
                    model.WantTaget = item.want_taget;
                    entity.WorkPlanItemList.Add(model);
                }
            }
            if (req.id > 0)
            {
                BLL.BLLWorkPlan.Modify(entity);
            }
            else
            {
                entity.ApproveStatus = Entity.ApproveStatusType.nodo;
                bll.Add(entity);
            }
            if (req.approve_user_id > 0)
                BLL.BLLMsg.PushMsg(req.approve_user_id, Entity.CusUserMessageType.waitapproveplan, string.Format(BLL.BLLMsgTemplate.WaitApprovePlan, entity_user.NickName, entity.WeekText), entity.ID,entity_user.NickName);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 审批工作计划
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/user/workplan/approve")]
        public WebAjaxEntity<string> ApproveWorkPlan([FromBody]Models.Request.ApproveWorkPlan req)
        {
            if (req.user_id == 0 || req.work_plan_id == 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }
            if (req.approve_status == Entity.ApproveStatusType.no && string.IsNullOrWhiteSpace(req.approve_no_text))
            {
                WorkContext.AjaxStringEntity.msgbox = "审核不通过时请填写不通过原因";
                return WorkContext.AjaxStringEntity;
            }

            string msg = "";
            bool isOK = BLL.BLLWorkPlan.Approve(req.user_id, req.work_plan_id, req.approve_status, req.approve_no_text, out msg);
            WorkContext.AjaxStringEntity.msg = isOK ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return WorkContext.AjaxStringEntity;
        }

        #endregion

        #region 会议召集
        /// <summary>
        /// 获取我的会议召集
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/user/workmeeting/list")]
        public WebAjaxEntity<List<Models.Response.WorkMeeting>> GetMeeting([FromBody]Models.Request.BasePage req)
        {
            WebAjaxEntity<List<Models.Response.WorkMeeting>> response_entity = new WebAjaxEntity<List<Models.Response.WorkMeeting>>();
            List<Models.Response.WorkMeeting> response_list = new List<Models.Response.WorkMeeting>();
            BLL.BaseBLL<Entity.WorkMeeting> bll = new BLL.BaseBLL<Entity.WorkMeeting>();
            int rowCount = 0;
            var db_list = bll.GetPagedList(req.page_index, req.page_size, ref rowCount, p => p.CusUserID == req.user_id, "AddTime desc", p => p.WorkMeetingUsers.Select(s => s.CusUser));

            foreach (var item in db_list)
                response_list.Add(BuildWorkMeeting(item, req.user_id));

            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = response_list;
            response_entity.total = rowCount;
            return response_entity;
        }

        /// <summary>
        /// 获取会议召集详细信息
        /// </summary>
        /// <param name="id">会议ID</param>
        /// <param name="user_id">当前登录的用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/workmeeting/info")]
        public WebAjaxEntity<Models.Response.WorkMeeting> GetMeetingInfo(int id, int user_id)
        {
            WebAjaxEntity<Models.Response.WorkMeeting> response_entity = new WebAjaxEntity<Models.Response.WorkMeeting>();
            var entity = BLL.BLLWorkMeeting.GetModel(id);
            if (entity == null)
            {
                response_entity.msgbox = "会议不存在";
                return response_entity;
            }

            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = BuildWorkMeeting(entity, user_id);
            return response_entity;
        }

        /// <summary>
        /// 点击参会
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/workmeeting/join")]
        public WebAjaxEntity<string> WorkMeetingJoin(int id, int user_id)
        {
            BLL.BLLWorkMeeting.Join(id, user_id);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return WorkContext.AjaxStringEntity;
        }


        /// <summary>
        /// 构建会议召集实体信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user_id">当前登录的用户ID</param>
        /// <returns></returns>
        private Models.Response.WorkMeeting BuildWorkMeeting(Entity.WorkMeeting entity, int user_id)
        {
            if (entity == null)
                return null;
            Models.Response.WorkMeeting model = new Models.Response.WorkMeeting();
            model.add_time = entity.AddTime;
            model.begin_time = entity.BeginTime;
            model.end_time = entity.EndTime;
            model.content = entity.Content;
            model.id = entity.ID;
            model.location = entity.Location;
            model.status_text = entity.StatusText;
            model.title = entity.Title;
            List<Models.Response.SelectUser> users_list = new List<Models.Response.SelectUser>();
            if (entity.WorkMeetingUsers != null)
            {
                foreach (var user in entity.WorkMeetingUsers)
                {
                    users_list.Add(BuilderSelectUser(user.CusUser, user.IsConfirm));
                    if (user.CusUserID == user_id && !user.IsConfirm && model.end_time > DateTime.Now)
                        model.can_join = true;

                }
            }
            model.meeting_users = users_list;

            List<Entity.WorkMeetingFile> file_list = null;
            if (entity.FileList.Count == 0)
                file_list = new BLL.BaseBLL<Entity.WorkMeetingFile>().GetListBy(0, p => p.WorkMeetingID == entity.ID, "ID ASC");

            if (file_list != null)
            {
                foreach (var file in file_list)
                {
                    Models.Response.ProjectFile model_file = new Models.Response.ProjectFile();
                    model_file.file_name = file.FileName;
                    model_file.file_path = GetSiteUrl() + file.FilePath;
                    model_file.file_size = file.FileSize;
                    model_file.type = Entity.ProjectFileType.file;
                    model.file_list.Add(model_file);
                }
            }

            return model;
        }


        /// <summary>
        /// 添加/修改我的会议召集
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/user/workmeeting/modify")]
        public WebAjaxEntity<string> ModifyMeeting([FromBody]Models.Request.WorkMeetingR req)
        {
            Entity.WorkMeeting entity = new Entity.WorkMeeting();
            if (req.id > 0)
                entity = new BLL.BaseBLL<Entity.WorkMeeting>().GetModel(p => p.ID == req.id);

            entity.CusUserID = req.user_id;
            entity.Location = req.location;
            entity.Content = req.content;
            entity.Title = req.title;
            entity.BeginTime = req.begin_time;
            entity.EndTime = req.end_time;
            if (req.file_list != null)
            {
                foreach (var item in req.file_list)
                {
                    Entity.WorkMeetingFile entity_file = new Entity.WorkMeetingFile();
                    entity_file.FileName = item.file_name;
                    entity_file.FilePath = item.file_path;
                    entity_file.FileSize = item.file_size;
                    entity.FileList.Add(entity_file);
                }
            }
            if (req.id > 0)
                BLL.BLLWorkMeeting.Modify(entity, req.user_ids);
            else
                BLL.BLLWorkMeeting.Add(entity, req.user_ids);

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return WorkContext.AjaxStringEntity;
        }

        #endregion

        #region 任务指派
        /// <summary>
        /// 获取我的任务指派
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/user/workjob/list")]
        public WebAjaxEntity<List<Models.Response.WorkJob>> GetWorkJob([FromBody]Models.Request.BasePage req)
        {
            WebAjaxEntity<List<Models.Response.WorkJob>> response_entity = new WebAjaxEntity<List<Models.Response.WorkJob>>();
            List<Models.Response.WorkJob> response_list = new List<Models.Response.WorkJob>();
            int rowCount = 0;
            var db_list = BLL.BLLWorkJob.GetPageList(req.page_size, req.page_index, req.user_id, "", out rowCount);
            foreach (var item in db_list)
                response_list.Add(BuildWorkJob(item, req.user_id));

            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = response_list;
            response_entity.total = rowCount;
            return response_entity;
        }

        /// <summary>
        /// 获取任务指派详情
        /// </summary>
        /// <param name="id">任务iD</param>
        /// <param name="user_id">当前登录的用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/workjob/info")]
        public WebAjaxEntity<Models.Response.WorkJob> GetWorkJobInfo(int id, int user_id)
        {
            WebAjaxEntity<Models.Response.WorkJob> response_entity = new WebAjaxEntity<Models.Response.WorkJob>();
            var entity = BLL.BLLWorkJob.GetModel(id);
            if (entity == null)
            {
                response_entity.msgbox = "任务不存在";
                return response_entity;
            }

            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = BuildWorkJob(entity, user_id);
            return response_entity;
        }

        /// <summary>
        /// 完成任务指派
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/user/workjob/comp")]
        public WebAjaxEntity<string> WorkJobComp([FromBody]Models.Request.WorkJobConfrim req)
        {
            if(req == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "未收到参数";
                return WorkContext.AjaxStringEntity;
            }
            List<Entity.WorkJobUserFile> file_list = new List<Entity.WorkJobUserFile>();
            foreach (var item in req.files)
            {
                Entity.WorkJobUserFile entity = new Entity.WorkJobUserFile();
                entity.FileName = item.file_name;
                entity.FilePath = item.file_path;
                entity.FileSize = item.file_size;
                file_list.Add(entity);
            }
            BLL.BLLWorkJob.Confirm(req.work_job_id, req.user_id, req.text, file_list);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 构建任务指派实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        private Models.Response.WorkJob BuildWorkJob(Entity.WorkJob entity, int user_id)
        {
            if (entity == null)
                return null;

            Models.Response.WorkJob model = new Models.Response.WorkJob();
            model.add_time = entity.AddTime;
            model.content = entity.Content;
            model.id = entity.ID;
            model.done_time = entity.DoneTime;
            model.create_user_id = entity.CusUserID;
            model.status_text = BLL.BLLWorkJob.GetJobStatus(entity.ID);
            model.title = entity.Title;
            model.work_type = (entity.CusUserID == user_id) ? 1 : 2;
            BLL.BaseBLL<Entity.CusUser> bll_user = new BLL.BaseBLL<Entity.CusUser>();
            List<Models.Response.WorkJobConfrimUser> users_list = new List<Models.Response.WorkJobConfrimUser>();
            List<Entity.WorkJobUser> db_job_user = new List<Entity.WorkJobUser>();
            if (entity.WorkJobUsers != null)
            {
                db_job_user = entity.WorkJobUsers.ToList();
            }
            else
            {
                db_job_user = new BLL.BaseBLL<Entity.WorkJobUser>().GetListBy(0, p => p.WorkJobID == entity.ID, "ID ASC", p => p.ConfrimFiles);
            }
            foreach (var user in db_job_user)
            {
                Models.Response.WorkJobConfrimUser temp_model = new Models.Response.WorkJobConfrimUser();
                if (user.CusUser == null)
                    user.CusUser = bll_user.GetModel(p => p.ID == user.CusUserID);
                if (user.CusUser != null)
                {
                    temp_model.avatar = GetSiteUrl() + user.CusUser.Avatar;
                    temp_model.nick_name = user.CusUser.NickName;
                    temp_model.short_num = user.CusUser.ShorNum;
                    temp_model.telphone = user.CusUser.Telphone;
                    temp_model.user_id = user.CusUserID;
                }
                temp_model.confrim_text = user.ConfirmText;
                temp_model.is_join = user.IsConfirm;
                temp_model.file_list = new List<Models.Response.ProjectFile>();
                if (user.ConfrimFiles == null)
                {
                    user.ConfrimFiles = new BLL.BaseBLL<Entity.WorkJobUserFile>().GetListBy(0, p => p.WorkJobUserID == user.ID, "ID ASC").ToList();
                }
                foreach (var file in user.ConfrimFiles.ToList())
                {
                    Models.Response.ProjectFile model_file = new Models.Response.ProjectFile();
                    model_file.file_name = file.FileName;
                    model_file.file_path = GetSiteUrl() + file.FilePath;
                    model_file.file_size = file.FileSize;
                    model_file.type = Entity.ProjectFileType.file;
                    temp_model.file_list.Add(model_file);
                }

                users_list.Add(temp_model);
                if (user.CusUserID == user_id && !user.IsConfirm)
                    model.is_comp = true;
            }

            model.users_list = users_list;

            List<Entity.WorkJobFile> file_list = null;
            if (entity.FileList.Count == 0)
                file_list = new BLL.BaseBLL<Entity.WorkJobFile>().GetListBy(0, p => p.WorkJobID == entity.ID, "ID ASC");
            else
                file_list = entity.FileList.ToList();

            if (file_list != null)
            {
                foreach (var file in file_list)
                {
                    Models.Response.ProjectFile model_file = new Models.Response.ProjectFile();
                    model_file.file_name = file.FileName;
                    model_file.file_path = GetSiteUrl() + file.FilePath;
                    model_file.file_size = file.FileSize;
                    model_file.type = Entity.ProjectFileType.file;
                    model.file_list.Add(model_file);
                }
            }

            return model;
        }


        /// <summary>
        /// 添加/修改我的任务指派
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/user/workjob/modify")]
        public WebAjaxEntity<string> ModifyWorkJob([FromBody]Models.Request.WorkJobR req)
        {
            Entity.WorkJob entity = new Entity.WorkJob();
            if (req.id > 0)
                entity = new BLL.BaseBLL<Entity.WorkJob>().GetModel(p => p.ID == req.id);

            entity.Content = req.content;
            entity.Title = req.title;
            entity.DoneTime = req.done_time;
            entity.CusUserID = req.user_id;
            entity.ID = req.id;
            if (req.file_list != null)
            {
                foreach (var item in req.file_list)
                {
                    Entity.WorkJobFile entity_file = new Entity.WorkJobFile();
                    entity_file.FileName = item.file_name;
                    entity_file.FilePath = item.file_path;
                    entity_file.FileSize = item.file_size;
                    entity.FileList.Add(entity_file);
                }
            }

            if (req.id > 0)
                BLL.BLLWorkJob.Modify(entity, req.user_ids);
            else
                BLL.BLLWorkJob.Add(entity, req.user_ids);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return WorkContext.AjaxStringEntity;
        }

        #endregion

        /// <summary>
        /// 获取我的消息列表【设计图上写的是公告，实际上是消息列表】
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/user/message/list")]
        public WebAjaxEntity<List<Models.Response.MessageInfo>> GetMessage([FromBody]Models.Request.MessageList req)
        {
            WebAjaxEntity<List<Models.Response.MessageInfo>> response_entity = new WebAjaxEntity<List<Models.Response.MessageInfo>>();
            List<Models.Response.MessageInfo> response_list = new List<Models.Response.MessageInfo>();

            int rowCount = 0;
            var db_list = BLL.BLLCusUser.GetUseMessagePageList(req.page_size, req.page_index, req.user_id, req.msg_type, req.searh_word, out rowCount);

            BLL.BaseBLL<Entity.CusUser> bll_user = new BLL.BaseBLL<Entity.CusUser>();
            foreach (var item in db_list)
            {
                Models.Response.MessageInfo model = new Models.Response.MessageInfo();
                model.add_time = item.AddTime;
                model.content = item.Content;
                model.id = item.ID;
                model.link_id = item.LinkID;
                model.type = item.Type;
                model.type_name = item.TypeName;
                model.add_user_name = item.AddUserName;

                response_list.Add(model);
            }

            response_entity.total = rowCount;
            response_entity.data = response_list;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }

        /// <summary>
        /// 获取我的待办列表【实际上也是消息】
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/user/jobtask/list")]
        public WebAjaxEntity<List<Models.Response.MessageInfo>> GetDaiBan([FromBody]Models.Request.BasePage req)
        {
            WebAjaxEntity<List<Models.Response.MessageInfo>> response_entity = new WebAjaxEntity<List<Models.Response.MessageInfo>>();
            List<Models.Response.MessageInfo> response_list = new List<Models.Response.MessageInfo>();
            int rowCount = 0;
            var db_list = BLL.BLLCusUser.GetJobTaskPageList(req.page_size, req.page_index, req.user_id, out rowCount);
            BLL.BaseBLL<Entity.CusUser> bll_user = new BLL.BaseBLL<Entity.CusUser>();
            foreach (var item in db_list)
            {
                Models.Response.MessageInfo model = new Models.Response.MessageInfo();
                model.add_time = item.AddTime;
                model.content = item.Content;
                model.id = item.ID;
                model.link_id = item.LinkID;
                model.type = item.Type;
                model.type_name = item.TypeName;
                model.add_user_name = item.AddUserName;
                response_list.Add(model);
            }

            response_entity.total = rowCount;
            response_entity.data = response_list;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }


        /// <summary>
        /// 设置全部消息为已读
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/message/setread/{user_id:int}")]
        public WebAjaxEntity<string> SetMessageRead(int user_id)
        {
            BLL.BLLCusUserMessage.SetAllRead(user_id);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 获取我的下载记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/user/downlog/list")]
        public WebAjaxEntity<List<Models.Response.SeeModel>> GetDownLog([FromBody]Models.Request.BasePage req)
        {
            WebAjaxEntity<List<Models.Response.SeeModel>> response_entity = new WebAjaxEntity<List<Models.Response.SeeModel>>();
            List<Models.Response.SeeModel> response_list = new List<Models.Response.SeeModel>();
            int total = 0;
            BLL.BaseBLL<Entity.DownloadLog> bll = new BLL.BaseBLL<Entity.DownloadLog>();
            var db_list = bll.GetPagedList(req.page_index, req.page_size, ref total, p => p.CusUserID == req.user_id, "AddTime DESC");
            foreach (var item in db_list)
                response_list.Add(new Models.Response.SeeModel(item.ID, item.Title));

            response_entity.data = response_list;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.total = total;
            return response_entity;
        }

        /// <summary>
        /// 用户反馈
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/user/feedback")]
        public WebAjaxEntity<string> AddFeedBack([FromBody]Models.Request.FeebBack req)
        {
            if (req.user_id == 0 || string.IsNullOrWhiteSpace(req.content))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }

            Entity.CusUser entity_user = new BLL.BaseBLL<Entity.CusUser>().GetModel(p => p.ID == req.user_id);
            if (entity_user == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }

            Entity.Feedback entity = new Entity.Feedback();
            entity.Content = req.content;
            entity.Email = entity_user.Email;
            entity.Name = entity_user.NickName;
            entity.Telphone = entity_user.Telphone;
            new BLL.BaseBLL<Entity.Feedback>().Add(entity);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return WorkContext.AjaxStringEntity;
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
            if (model.CusUserJobID == null)
            {
                entity.job_id = 0;
                entity.job_name = "";
            }
            else
            {
                int temp_job_id = TypeHelper.ObjectToInt(model.CusUserJobID);
                entity.job_id = temp_job_id;
                if (model.CusUserJob == null)
                {
                    Entity.CusUserJob job = new BLL.BaseBLL<Entity.CusUserJob>().GetModel(p => p.ID == temp_job_id);
                    if (job != null)
                        entity.job_name = job.Title;
                }
                else
                    entity.job_name = model.CusUserJob.Title;
            }

            entity.nick_name = model.NickName;
            entity.shor_num = model.ShorNum == null ? "" : model.ShorNum;
            entity.telphone = model.Telphone;
            entity.is_department_manager = BLL.BLLDepartment.CheckUserIsManager(entity.id, entity.department_id);
            entity.permission_contacts = BLL.BLLCusUser.CheckPermissionContacts(entity.id);
            return entity;
        }

        /// <summary>
        /// 构造用户基本信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Models.Response.SelectUser BuilderSelectUser(Entity.CusUser entity, bool is_join = false)
        {
            if (entity == null)
                return null;
            Models.Response.SelectUser model = new Models.Response.SelectUser();
            model.user_id = entity.ID;
            model.telphone = entity.Telphone;
            model.nick_name = entity.NickName;
            model.short_num = entity.ShorNum;
            model.avatar = GetSiteUrl() + entity.Avatar;
            model.is_join = is_join;
            return model;
        }

    }
}
