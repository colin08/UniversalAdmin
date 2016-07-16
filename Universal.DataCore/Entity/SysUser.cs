using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.DataCore.Entity
{
    /// <summary>
    /// 用户性别枚举
    /// </summary>
    public enum UserGender
    {
        男 = 1,
        女 = 2
    }

    /// <summary>
    /// 系统用户
    /// </summary>
    public class SysUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(20)]
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(30), Required]
        public string NickName { get; set; }

        /// <summary>
        /// 用户性别
        /// </summary>
        [Required]
        public UserGender Gender { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [StringLength(255), Required]
        public string Password { get; set; }
        
        /// <summary>
        /// 用户状态，是否正常
        /// </summary>
        [Required]
        public bool Status { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 是否可以管理后台
        /// </summary>
        [Required]
        public bool IsAdmin { get; set; }


        /// <summary>
        /// 所属用户组ID
        /// </summary>
        [Required]
        public int SysRoleID { get; set; }

        /// <summary>
        /// 用户组信息
        /// </summary>
        public virtual SysRole SysRole { get; set; }


        /// <summary>
        /// 注册时间
        /// </summary>
        [Required]
        public DateTime RegTime { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Required]
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 用户日志信息
        /// </summary>
        //public virtual ICollection<SysLog> SysLog { get; set;}
    }
}
