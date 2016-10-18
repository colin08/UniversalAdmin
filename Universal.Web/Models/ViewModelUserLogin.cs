using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 用户登录所需参数
    /// </summary>
    public class ViewModelUserLogin
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        [MaxLength(30, ErrorMessage = "用户名至多30个字符"), MinLength(3, ErrorMessage = "用户名至少3个字符")]
        public string user_name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [MaxLength(30, ErrorMessage = "密码至多30个字符"),MinLength(3,ErrorMessage ="密码至少3个字符")]
        public string password { get; set; }

        /// <summary>
        /// 是否记住登陆
        /// </summary>
        public bool is_rember { get; set; }
        
    }
}