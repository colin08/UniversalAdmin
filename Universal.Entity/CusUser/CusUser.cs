using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 用户性别
    /// </summary>
    public enum CusUserGender : byte
    {
        male=1,
        female
    }

    /// <summary>
    /// 前端用户
    /// </summary>
    public class CusUser
    {

        public CusUser()
        {
            this.Status = true;
            this.RegTime = DateTime.Now;
            this.LastLoginTime = DateTime.Now;
        }

        public int ID { get; set; }
        
        [Display(Name = "手机号"), Index(IsUnique = true), StringLength(20),Required(ErrorMessage = "手机号不能为空")]
        public string Telphone { get; set; }
        
        [Display(Name = "姓名"),StringLength(20,ErrorMessage ="姓名不能超过20个字符")]
        public string NickName { get; set; }
        
        [Required(ErrorMessage = "密码不能为空"),StringLength(255), Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name ="是否管理员")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 所属部门ID
        /// </summary>
        public int CusDepartmentID { get; set; }

        /// <summary>
        /// 部门信息
        /// </summary>
        public virtual CusDepartment CusDepartment { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public int CusUserJobID { get; set; }

        /// <summary>
        /// 职位信息
        /// </summary>
        public virtual CusUserJob CusUserJob { get; set; }

        [Display(Name = "性别"),Required]
        public CusUserGender Gender { get; set; }
        
        [Display(Name = "状态"),Required]
        public bool Status { get; set; }
        
        [Display(Name = "头像")]
        public string Avatar { get; set; }

        [Display(Name ="生日")]
        public DateTime? Brithday { get; set; }

        [Display(Name = "短号"),MaxLength(20,ErrorMessage ="不能超过20位")]
        public string ShorNum { get; set; }

        [Display(Name ="邮箱"),RegularExpression(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }

        [Display(Name ="简介"),MaxLength(500,ErrorMessage ="不能超过500个字符")]
        public string AboutMe { get; set; }

        [Display(Name ="注册时间"),Required]
        public DateTime RegTime { get; set; }

        [Display(Name ="最后登录时间"),Required]
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 用户的权限
        /// </summary>
        public virtual ICollection<CusUserRoute> CusUserRoute { get; set; }
        
    }
}
