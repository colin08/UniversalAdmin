using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Universal.Entity
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum WorkStatus : byte
    {
        [Description("进行中")]
        ing = 0,
        [Description("已完成")]
        done = 1,
        [Description("取消")]
        cancel = 2
    }

    /// <summary>
    /// 会议召集
    /// </summary>
    public class WorkJob
    {
        public WorkJob()
        {
            this.AddTime = DateTime.Now;
            this.DoneTime = DateTime.Now;
            this.Status = WorkStatus.ing;
            this.WorkJobUsers = new List<WorkJobUser>();
            this.FileList = new List<WorkJobFile>();
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
        public string StatusText { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [MaxLength(500)]
        public string Title { get; set; }

        /// <summary>
        /// 任务内容
        /// </summary>
        [MaxLength(1000)]
        public string Content { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime DoneTime { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 执行人
        /// </summary>
        public virtual ICollection<WorkJobUser> WorkJobUsers { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public virtual ICollection<WorkJobFile> FileList { get; set; }
    }
}
