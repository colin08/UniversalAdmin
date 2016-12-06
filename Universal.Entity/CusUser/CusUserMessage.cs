using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 用户消息类别
    /// </summary>
    public enum CusUserMessageType
    {
        /// <summary>
        /// APP更新
        /// </summary>
        [Description("App更新")]
        appupdate = 1,

        /// <summary>
        /// 企业公告
        /// </summary>
        [Description("企业公告")]
        notice,

        /// <summary>
        /// 文件分享
        /// </summary>
        [Description("文件分享")]
        fileshare,

        /// <summary>
        /// 待审核项目
        /// </summary>
        [Description("待审核项目")]
        approveproject,


        /// <summary>
        /// 项目审核通过
        /// </summary>
        [Description("项目审核通过")]
        appproveok,

        /// <summary>
        /// 待参加会议
        /// </summary>
        [Description("待参加会议")]
        waitmeeting,

        /// <summary>
        /// 确认参加会议
        /// </summary>
        [Description("确认参加会议")]
        confrimjoinmeeting,

        /// <summary>
        /// 会议取消提醒
        /// </summary>
        [Description("会议取消提醒")]
        meetingcancel,

        /// <summary>
        /// 会议改期提醒
        /// </summary>
        [Description("会议改期提醒")]
        meetingchangedate,

        /// <summary>
        /// 待完成任务
        /// </summary>
        [Description("待完成任务")]
        waitjobdone,

        /// <summary>
        /// 任务到期提示
        /// </summary>
        [Description("任务到期提示")]
        jobtimeout,

        /// <summary>
        /// 确认完成任务
        /// </summary>
        [Description("确认完成任务")]
        confrimdonejob,

        /// <summary>
        /// 待审核计划
        /// </summary>
        [Description("待审核计划")]
        waitapproveplan,

        /// <summary>
        /// 计划审核通过
        /// </summary>
        [Description("计划审核通过")]
        planapproveok,

        /// <summary>
        /// 收藏的项目更新提醒
        /// </summary>
        [Description("收藏的项目更新提醒")]
        favprojectupdate,

        /// <summary>
        /// 收藏的秘籍更新提醒
        /// </summary>
        [Description("收藏的秘籍更新提醒")]
        favdocupdate,

        /// <summary>
        /// 流程更新提醒
        /// </summary>
        [Description("流程更新提醒")]
        flowupdate,
    }

    /// <summary>
    /// 用户消息
    /// </summary>
    public class CusUserMessage
    {
        public CusUserMessage()
        {
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public int CusUserID { get; set; }

        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 消息类别
        /// </summary>
        public CusUserMessageType Type { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [MaxLength(500)]
        public string Content { get; set; }

        /// <summary>
        /// 链接ID
        /// </summary>
        [MaxLength(10)]
        public string LinkID { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 消息是否读取文本显示
        /// </summary>
        [NotMapped]
        public string IsReadText
        {
            get
            {
                return IsRead ? "已读" : "未读";
            }
        }

        /// <summary>
        /// 类型名称
        /// </summary>
        [NotMapped]
        public string TypeName
        {
            get
            {
                return Tools.EnumHelper.GetDescription<CusUserMessageType>(this.Type);
            }
        }

        /// <summary>
        /// 类别图标
        /// </summary>
        [NotMapped]
        public string TypeLogo
        {
            get
            {
                switch (this.Type)
                {
                    
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 链接地址，网站使用例如 /Info/Job?id=1&msg=2
        /// </summary>
        [NotMapped]
        public string LinkUrl
        {
            get
            {
                switch (Type)
                {
                    case CusUserMessageType.appupdate:
                        return "/Info/AppUpdate?id="+this.LinkID+"&msg="+this.ID.ToString()+"";
                    case CusUserMessageType.notice:
                        return "/Info/Notice?id=" + this.LinkID + "&msg=" + this.ID.ToString() + "";
                    case CusUserMessageType.fileshare:
                        return "";
                    case CusUserMessageType.approveproject:
                        return "";
                    case CusUserMessageType.appproveok:
                        return "";
                    case CusUserMessageType.waitmeeting:
                        return "/Info/Meeting?id=" + this.LinkID + "&msg=" + this.ID.ToString() + "";
                    case CusUserMessageType.confrimjoinmeeting:
                        return "/Info/Meeting?id=" + this.LinkID + "&msg=" + this.ID.ToString() + "";
                    case CusUserMessageType.meetingcancel:
                        return "/Info/Meeting?id=" + this.LinkID + "&msg=" + this.ID.ToString() + "";
                    case CusUserMessageType.meetingchangedate:
                        return "/Info/Meeting?id=" + this.LinkID + "&msg=" + this.ID.ToString() + "";
                    case CusUserMessageType.waitjobdone:
                        return "/Info/Job?id=" + this.LinkID + "&msg=" + this.ID.ToString() + "";
                    case CusUserMessageType.jobtimeout:
                        return "/Info/Job?id=" + this.LinkID + "&msg=" + this.ID.ToString() + "";
                    case CusUserMessageType.confrimdonejob:
                        return "/Info/Job?id=" + this.LinkID + "&msg=" + this.ID.ToString() + "";
                    case CusUserMessageType.waitapproveplan:
                        return "";
                    case CusUserMessageType.planapproveok:
                        return "";
                    case CusUserMessageType.favprojectupdate:
                        return "";
                    case CusUserMessageType.favdocupdate:
                        return "";
                    case CusUserMessageType.flowupdate:
                        return "";
                    default:
                        return "";
                }
            }
        }


        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
