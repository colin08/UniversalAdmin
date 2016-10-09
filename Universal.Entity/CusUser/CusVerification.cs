using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Entity
{
    /// <summary>
    /// 验证码类别
    /// </summary>
    public enum CusVerificationType:byte
    {
        //重置密码
        RestPwd = 1,
        //修改密码或手机
        Modify
    }

    /// <summary>
    /// 验证码
    /// </summary>
    public class CusVerification
    {
        public int ID { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        [Required]
        public Guid Guid { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required, MaxLength(10)]
        public string Code { get; set; }

        /// <summary>
        /// 验证码类别
        /// </summary>
        public CusVerificationType Type { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        public DateTime AddTime { get; set; }
    }
}
