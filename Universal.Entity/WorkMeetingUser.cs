using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Universal.Entity
{
    /// <summary>
    /// 会与参会人员
    /// </summary>
    public class WorkMeetingUser
    {
        public int ID { get; set; }

        /// <summary>
        /// 会议ID
        /// </summary>
        public int WorkMeetingID { get; set; }

        /// <summary>
        /// 会议信息
        /// </summary>
        public virtual WorkMeeting WorkMeeting { get; set; }

        /// <summary>
        /// 参与人员ID
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 参与人员信息
        /// </summary>
        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 是否确认
        /// </summary>
        public bool IsConfirm { get; set; }        
    }
}
