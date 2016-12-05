using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 选择用户
    /// </summary>
    public class SelectUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string telphone { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }

    }
}