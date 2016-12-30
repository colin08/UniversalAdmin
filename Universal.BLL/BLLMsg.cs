using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    /// <summary>
    /// 用户消息模板
    /// </summary>
    public class MsgTemplate
    {
        /// <summary>
        /// app 新版本发布 APP【0】有新版本发布啦
        /// </summary>
        public static readonly string AppUpdate = "APP{0}有新版本发布啦";

        /// <summary>
        /// 发布公告 {0}发布了新的公告
        /// </summary>
        public static readonly string Notice = "{0}发布了新的公告";

        /// <summary>
        /// 文件分享 待定
        /// </summary>
        public static readonly string FileShare = "待定";

        /// <summary>
        /// 待审核项目 {0}创建了新的项目【{1}】，需要您的审核
        /// </summary>
        public static readonly string ApproveProject = "{0}创建了新的项目【{1}】，需要您的审核";

        /// <summary>
        /// 项目审核通过 您的项目【{0}】审核已经通过
        /// </summary>
        public static readonly string AppproveOK = "您的项目【{0}】审核已经通过";

        /// <summary>
        /// 项目审核未通过 您的项目【{0}】审核未通过，未通过原因：{1}
        /// </summary>
        public static readonly string AppproveNo = "您的项目【{0}】审核未通过，未通过原因：{1}";

        /// <summary>
        /// 待参加会议 {0}创建了一个会议，议题：【{1}】，等待您的参加
        /// </summary>
        public static readonly string WaitMeeting = "{0}创建了一个会议，议题：【{1}】，等待您的参加";

        /// <summary>
        /// 确认参加会议 {0}已确认参加主题为：【{1}】的会议
        /// </summary>
        public static readonly string ConfrimJoinMeeting = "{0}已确认参加主题为：【{1}】的会议";

        /// <summary>
        /// 会议取消提醒 {0}已取消会议【{1}】
        /// </summary>
        public static readonly string MeetingCancel = "{0}已取消会议【{1}】";

        /// <summary>
        /// 会议改期提醒 {0}已将会议【{1}】的开会时间由：{2} 改为：{3}
        /// </summary>
        public static readonly string MeetingChangeDate = "{0}已将会议【{1}】的开会时间由：{2} 改为：{3}";

        /// <summary>
        /// 待完成任务 【{0}】任务还未完成
        /// </summary>
        public static readonly string WaitMeetingDone = "【{0}】任务还未完成";

        /// <summary>
        /// 任务到期提示 【{0}】任务即将到期，到期时间：{1}
        /// </summary>
        public static readonly string MeetingTimeOut = "【{0}】任务即将到期，到期时间：{1}";

        /// <summary>
        /// 确认完成任务  {0}确认完成【{1}】任务
        /// </summary>
        public static readonly string ConfrimDoneJob = "{0}确认完成【{1}】任务";

        /// <summary>
        /// 待审核计划 {0}添加了一个工作计划，等待您的审批
        /// </summary>
        public static readonly string WaitApprovePlan = "{0}添加了一个工作计划，等待您的审批";

        /// <summary>
        /// 计划审核通过 您的工作计划已被{0}审核通过
        /// </summary>
        public static readonly string PlanApproveOK = "您的工作计划已被{0}审核通过";

        /// <summary>
        /// 计划审核未通过 您的工作计划审核未通过，原因：{1}
        /// </summary>
        public static readonly string PlanApproveNO = "您的工作计划审核未通过，原因：{1}";

        /// <summary>
        /// 收藏的项目更新提醒 您收藏的项目【{0}】于{2016/01/01}被{2}更新了
        /// </summary>
        public static readonly string FavProjectUpdate = "您收藏的项目【{0}】于{1}被{2}更新了";

        /// <summary>
        /// 收藏的秘籍更新提醒 您收藏的秘籍【{0}】于{2016/01/01}被{2}更新了
        /// </summary>
        public static readonly string FavDocUpdate = "您收藏的秘籍【{0}】于{1}被{2}更新了";

        /// <summary>
        /// 流程更新提醒 流程【{0}】于{1}被{2}更新了
        /// </summary>
        public static readonly string FlowUpdate = "流程【{0}】于{1}被{2}更新了";

        
    }


    /// <summary>
    /// 用户消息
    /// </summary>
    public class BLLMsg
    {



        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="user_id">消息所属用户ID</param>
        /// <param name="type">消息类别</param>
        /// <param name="content">消息内容,string.Format 拼接MsgTemplate类中的常量</param>
        /// <param name="link_id">链接id</param>
        /// <returns></returns>
        public bool AddMsg(int user_id,Entity.CusUserMessageType type,string content,int link_id)
        {
            BLL.BaseBLL<Entity.CusUser> bll_user = new BaseBLL<Entity.CusUser>();
            if (bll_user.Exists(p => p.ID == user_id))
                return false;
            BLL.BaseBLL<Entity.CusUserMessage> bll_msg = new BaseBLL<Entity.CusUserMessage>();
            Entity.CusUserMessage entity = new Entity.CusUserMessage();
            entity.Content = content;
            entity.CusUserID = user_id;
            entity.Type = type;
            entity.LinkID = link_id.ToString();
            bll_msg.Add(entity);
            return entity.ID > 0;
        }
    }
}
