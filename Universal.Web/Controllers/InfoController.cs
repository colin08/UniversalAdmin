﻿using System;
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
            //TODO 详情展示-待审核项目
            return View();
        }
        
        /// <summary>
        /// 任务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg">消息ID</param>
        /// <returns></returns>
        public ActionResult Job(int id, int? msg)
        {
            BLL.BaseBLL<Entity.WorkJob> bll = new BLL.BaseBLL<Entity.WorkJob>();
            var entity = bll.GetModel(p => p.ID == id, p => p.CusUser);
            if (entity == null)
            {
                return View("NotFound");
            }
            SetMsgRead(msg);
            return View();
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
                ViewData["BackTitle"] = "待计划列表";
                ViewData["tabLeft"] = "gzjh";
                ViewData["BackUrl"] = "/WorkPlan/Index";
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