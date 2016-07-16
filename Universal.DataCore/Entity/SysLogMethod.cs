using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.DataCore.Entity
{
    /// <summary>
    /// 操作日志类别
    /// </summary>
    public enum SysLogMethodType
    {
        Add = 1,
        Update = 2,
        Delete = 3,
        Login = 4
    }

    /// <summary>
    /// 操作日志
    /// </summary>
    public class SysLogMethod
    {
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public int SysUserID { get; set; }

        /// <summary>
        /// 操作类别
        /// </summary>
        [Required]
        public SysLogMethodType Type { get; set; }

        /// <summary>
        /// 详细内容
        /// </summary>
        [StringLength(500)]
        [Required]
        public string Detail { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 操作的用户
        /// </summary>
        public virtual SysUser SysUser { get; set; }

    }
}
