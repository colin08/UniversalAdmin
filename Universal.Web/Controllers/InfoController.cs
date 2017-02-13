using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using System.Data.Entity;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 信息详情
    /// </summary>
    [BasicUserAuth]
    public class InfoController : BaseHBLController
    {
        public ActionResult NotFound()
        {
            return View();
        }

        /// <summary>
        /// APP更新
        /// </summary>
        /// <param name="id">to id</param>
        /// <param name="msg">消息ID</param>
        /// <returns></returns>
        public ActionResult AppUpdate(int id,int? msg)
        {
            BLL.BaseBLL<Entity.AppVersion> bll = new BLL.BaseBLL<Entity.AppVersion>();
            var entity = bll.GetModel(p => p.ID == id);
            if (entity == null)
            {
                return View("NotFound");
            }

            SetMsgRead(msg);
            return View(entity);
        }

        /// <summary>
        /// 公告信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg">消息ID</param>
        /// <returns></returns>
        public ActionResult Notice(int id,int? msg)
        {
            BLL.BaseBLL<Entity.CusNotice> bll = new BLL.BaseBLL<Entity.CusNotice>();
            var entity = bll.GetModel(p => p.ID == id, p => p.CusUser);
            if (entity == null)
            {
                return View("NotFound");
            }
            SetMsgRead(msg);
            return View(entity);
        }

        /// <summary>
        /// 文件分享
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg">消息ID</param>
        /// <returns></returns>
        public ActionResult FileShare(int id,int? msg)
        {
            //TODO 详情展示-文件分享
            return View();
        }

        /// <summary>
        /// 项目展示（判断是否审核、审核状态）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg">消息ID</param>
        /// <returns></returns>
        public ActionResult Project(int id,int? msg)
        {
            BLL.BaseBLL<Entity.Project> bll = new BLL.BaseBLL<Entity.Project>();
            var entity = bll.GetModel(p => p.ID == id, p => p.ApproveUser);
            if (entity == null)
            {
                return View("NotFound");
            }
            SetMsgRead(msg);


            ViewData["ShowApprove"] = 0;
            //如果是项目添加者，则只显示项目审批状态
            if(entity.CusUserID == WorkContext.UserInfo.ID)
            {
                ViewData["ShowApprove"] = 1;
            }
            else if(entity.ApproveUserID == WorkContext.UserInfo.ID)
            {
                //如果是审批人，则显示审批按钮和文本框
                ViewData["ShowApprove"] = 2;
            }
            else
            {
                
            }

            return View(entity);
        }
        
        /// <summary>
        /// 任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg">消息ID</param>
        /// <returns></returns>
        public ActionResult Job(int id, int? msg)
        {
            ViewData["tabLeft"] = "msg";
            ViewData["BackUrl"] = "/User/Message";
            ViewData["BackTitle"] = "消息列表";

            var entity = BLL.BLLWorkJob.GetModel(id);
            if (entity == null)
            {
                return View("NotFound");
            }
            SetMsgRead(msg);
            if(msg== null)
            {
                ViewData["tabLeft"] = "workjob";
                ViewData["BackUrl"] = "/WorkJob/Index";
                ViewData["BackTitle"] = "任务指派列表";
            }
            //是否显示点击完成的按钮
            ViewData["CanJoin"] = 0;
            foreach (var item in entity.WorkJobUsers.ToList())
            {
                if (item.CusUserID == WorkContext.UserInfo.ID && item.IsConfirm)
                    ViewData["CanJoin"] = 1;
            }
            return View(entity);
        }

        /// <summary>
        /// 计划
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg">消息ID</param>
        /// <returns></returns>
        public ActionResult Plan(int id, int? msg)
        {
            ViewData["tabLeft"] = "msg";
            ViewData["BackUrl"] = "/User/Message";
            ViewData["BackTitle"] = "消息列表";

            Entity.WorkPlan entity = BLL.BLLWorkPlan.GetModel(id);
            if (entity == null)
                return View("NotFound");
            ViewData["ApproveUser"] = BLL.BLLCusUser.GetUserDepartmentAdminText(entity.CusUserID);

            BLL.BaseBLL<Entity.CusDepartmentAdmin> bll_admin = new BLL.BaseBLL<Entity.CusDepartmentAdmin>();
            bool isAdmin = bll_admin.Exists(p => p.CusUserID == WorkContext.UserInfo.ID);
            ViewData["IsDepartmentAdmin"] = isAdmin;

            SetMsgRead(msg);

            if (msg == null)
            {
                ViewData["BackTitle"] = "待审批计划列表";
                ViewData["tabLeft"] = "gzjh";
                ViewData["BackUrl"] = "/WorkPlan/Approve";
            }
            return View(entity);
        }

        /// <summary>
        /// 流程
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg">消息ID</param>
        /// <returns></returns>
        public ActionResult Flow(int id, int? msg)
        {
            return View();
        }

        /// <summary>
        /// 秘籍
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg">消息ID</param>
        /// <returns></returns>
        public ActionResult Document(int id, int? msg)
        {
            ViewData["tabLeft"] = "msg";
            ViewData["BackUrl"] = "/User/Message";
            ViewData["BackTitle"] = "消息列表";
            var entity = BLL.BLLDocument.GetModel(id);
            if (entity == null)
            {
                return View("NotFound");
            }
            SetMsgRead(msg);
            if(msg ==null)
            {
                ViewData["BackTitle"] = "秘籍收藏列表";
                ViewData["tabLeft"] = "file";
                ViewData["BackUrl"] = "/User/DocFavorites";
            }
            return View(entity);
        }

        /// <summary>
        /// 秘籍-仅做展示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Doc(int id)
        {
            var entity = BLL.BLLDocument.GetModel(id);
            if (entity == null)
                return View("NotFound");
            return View(entity);
        }

        /// <summary>
        /// 会议召集
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg">消息ID</param>
        /// <returns></returns>
        public ActionResult Meeting(int id, int? msg)
        {
            ViewData["tabLeft"] = "msg";
            ViewData["BackUrl"] = "/User/Message";
            ViewData["BackTitle"] = "消息列表";
            var entity = BLL.BLLWorkMeeting.GetModel(id);
            if (entity == null)
            {
                return View("NotFound");
            }
            SetMsgRead(msg);
            if (msg == null)
            {
                ViewData["BackTitle"] = "会议列表";
                ViewData["tabLeft"] = "workmeeting";
                ViewData["BackUrl"] = "/WorkMeeting/Index";
            }
            //是否显示点击参会的按钮
            ViewData["CanJoin"] = 0;
            foreach (var item in entity.WorkMeetingUsers.ToList())
            {
                if (item.CusUserID == WorkContext.UserInfo.ID && item.IsConfirm)
                    ViewData["CanJoin"] = 1;
            }
            return View(entity);
        }


        /// <summary>
        /// 设置消息已读
        /// </summary>
        private void SetMsgRead(int? msg_id)
        {
            if(msg_id != null)
            {
                int id = TypeHelper.ObjectToInt(msg_id);
                BLL.BaseBLL<Entity.CusUserMessage> bll_msg = new BLL.BaseBLL<Entity.CusUserMessage>();
                var entity_msg = bll_msg.GetModel(p => p.ID == id && p.CusUserID == WorkContext.UserInfo.ID);
                if (entity_msg != null)
                {
                    if (!entity_msg.IsRead)
                    {
                        entity_msg.IsRead = true;
                        bll_msg.Modify(entity_msg, "IsRead");
                    }
                }

                GetMessage();
            }
        }

    }
}