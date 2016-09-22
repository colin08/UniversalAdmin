using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Entity
{
    /// <summary>
    /// 意见反馈
    /// </summary>
    public class Feedback
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 联系名称
        /// </summary>
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(20)]
        public string Telphone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(500)]
        public string Content { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        public DateTime AddTime { get; set; }

    }
}
