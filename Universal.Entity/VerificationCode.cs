using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{

    /// <summary>
    /// 验证码类别
    /// </summary>
    public enum VerificationCodeType : byte
    {
        [Description("完善资料")]
        FullInfo =1
    }

    /// <summary>
    /// 验证码
    /// </summary>
    public class VerificationCode
    {
        public VerificationCode()
        {
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required(),MaxLength(30)]
        public string Telphone { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [MaxLength(20),Required()]
        public string Code { get; set; }

        /// <summary>
        /// 验证码类别
        /// </summary>
        public VerificationCodeType Type { get; set; }


        public DateTime AddTime { get; set; }
    }
}
