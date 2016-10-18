using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 发送验证码
    /// </summary>
    public class ViewModelSendPwd
    {
        /// <summary>
        /// 标识
        /// </summary>
        public Guid guid { get; set; }

        public string telphone { get; set; }

        public string code { get; set; }

        public ViewModelSendPwd()
        {
            this.guid = Guid.NewGuid();
        }

    }
}