using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 用户性别枚举
    /// </summary>
    public enum UserGender : byte
    {
        男 = 1,
        女 = 2
    }

    /// <summary>
    /// 系统用户
    /// </summary>
    public class SysUser
    {
        public SysUser()
        {
            this.Gender = UserGender.男;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 所属商户ID
        /// </summary>
        public int SysMerchantID { get; set; }

        /// <summary>
        /// 商户信息
        /// </summary>
        public virtual SysMerchant SysMerchant { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(20), Display(Name = "用户名")]
        [Required(ErrorMessage = "用户名不能为空")]
        [RegularExpression(@"^[a-zA-Z0-9]{3,10}$", ErrorMessage = "用户名只能由3-10位字母或数字组成")]
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "昵称")]
        [StringLength(30), Required(ErrorMessage = "昵称不能为空")]
        public string NickName { get; set; }

        /// <summary>
        /// 用户性别
        /// </summary>
        [Required, Display(Name = "性别")]
        public UserGender Gender { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [StringLength(255), Display(Name = "密码"), Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 用户状态，是否正常
        /// </summary>
        [Required, Display(Name = "状态")]
        public bool Status { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        [Display(Name = "头像")]
        public string Avatar { get; set; }

        /// <summary>
        /// 头像或者默认头像
        /// </summary>
        [NotMapped]
        public string AvatarOrDefault
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Avatar))
                    return "/Assets/img/default_avatar.jpg";
                else
                    return this.Avatar;
            }
        }

        /// <summary>
        /// 所属用户组ID
        /// </summary>
        [Required(ErrorMessage = "请选择所属用户组"), Display(Name = "用户组")]
        public int SysRoleID { get; set; }

        /// <summary>
        /// 用户组信息
        /// </summary>
        public virtual SysRole SysRole { get; set; }


        /// <summary>
        /// 注册时间
        /// </summary>
        [Required, Display(Name = "注册时间")]
        public DateTime RegTime { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Required, Display(Name = "最后登录时间")]
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 用户日志信息
        /// </summary>
        //public virtual ICollection<SysLog> SysLog { get; set;}
    }
}
