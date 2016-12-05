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
            this.WorkMeetingUsers = new List<WorkMeetingUser>();
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
                switch (this.Status)
                {
                    case WorkStatus.ing:
                        return "进行中";
                    case WorkStatus.done:
                        return "已完成";
                    case WorkStatus.cancel:
                        return "已取消";
                    default:
                        return "";
                }
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
        /// 会议地点
        /// </summary>
        [MaxLength(100)]
        public string Location { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 与会人员
        /// </summary>
        public virtual ICollection<WorkMeetingUser> WorkMeetingUsers { get; set; }
    }
}
