using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.DataCore.Entity
{
    /// <summary>
    /// 系统错误日志
    /// </summary>
    public class SysLogException
    {
        public int ID { get; set; }


        /// <summary>
        /// 错误消息
        /// </summary>
        [Required,MaxLength(255)]
        public string Message { get; set; }

        /// <summary>
        /// 错误位置
        /// </summary>
        [Required,MaxLength(50)]
        public string Source { get; set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        [Required]
        public string StackTrace { get; set; }
        /// <summary>
        /// 发生时间
        /// </summary>
        [Required]
        public DateTime AddTime { get; set; }

    }
}
