using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Universal.Entity
{
    /// <summary>
    /// 会议召集
    /// </summary>
    public class WorkMeeting
    {
        public WorkMeeting()
        {
            this.AddTime = DateTime.Now;
            this.BeginTime = DateTime.Now;
            this.Status = WorkStatus.cancel;
            this.WorkMeetingUsers = new List<WorkMeetingUser>();
            this.FileList = new List<WorkMeetingFile>();
        }

        public int ID { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public WorkStatus Status { get; set; }

        /// <summary>
        /// 状态文本
        /// </summary>
        [NotMapped]
        public string StatusText
        {
            get
            {
                if (DateTime.Now < BeginTime)
                    return "未开始";
                else if (DateTime.Now >= BeginTime && DateTime.Now <= EndTime)
                    return "进行中";
                else if (DateTime.Now > EndTime)
                    return "已结束";
                else
                    return "未知";
            }
        }

        /// <summary>
        /// 主题
        /// </summary>
        [MaxLength(500)]
        public string Title { get; set; }

        /// <summary>
        /// 议程
        /// </summary>
        [MaxLength(1000)]
        public string Content { get; set; }

        /// <summary>
        /// 开会时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结会时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 会议地点
        /// </summary>
        [MaxLength(100)]
        public string Location { get; set; }

        /// <summary>
        /// 时间显示
        /// </summary>
        [NotMapped]
        public string TimeTxt
        {
            get
            {
                double day_cha = Tools.WebHelper.DateTimeDiff(BeginTime, EndTime, "ad");
                if (day_cha < 1)
                {
                    //当天
                    return BeginTime.ToString("yyyy-MM-dd HH:mm") + " ~ " + EndTime.ToString("HH:mm");
                }
                else
                {
                    return BeginTime.ToString("yyyy-MM-dd HH:mm") + " ~ " + EndTime.ToString("yyyy-MM-dd HH:mm");
                }
            }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 与会人员
        /// </summary>
        public virtual ICollection<WorkMeetingUser> WorkMeetingUsers { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public virtual ICollection<WorkMeetingFile> FileList { get; set; }
    }
}
