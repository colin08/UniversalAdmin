using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Entity
{
    /// <summary>
    /// 会议附件
    /// </summary>
    public class WorkMeetingFile
    {
        public int ID { get; set; }

        /// <summary>
        /// 会议ID
        /// </summary>
        public int WorkMeetingID { get; set; }

        /// <summary>
        /// 会议
        /// </summary>
        public virtual WorkMeeting WorkMeeting { get; set; }

        /// <summary>
        /// 附件名称，使用原名称
        /// </summary>
        [MaxLength(200)]
        public string FileName { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        [MaxLength(500)]
        public string FilePath { get; set; }

        /// <summary>
        /// 附件大小,KB或MB显示
        /// </summary>
        [MaxLength(30)]
        public string FileSize { get; set; }
        
    }
}
