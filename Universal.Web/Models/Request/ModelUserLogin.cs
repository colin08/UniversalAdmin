using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class ModelUserLogin
    {
        /// <summary>
        /// 手机号或邮箱
        /// </summary>
        public string phone_email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string pwd { get; set; }
        
    }
}