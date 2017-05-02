using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Web.Models
{
    /// <summary>
    /// 编辑管理员用户
    /// </summary>
    public class ViewModelAdminUser: ViewModelCustomFormBase
    {

        public ViewModelAdminUser()
        {
            this.password = "";
            this.status = true;
            this.gender = Entity.CusUserGender.male;
            this.avatar = "/uploads/avatar.jpg";
            this.user_route = new List<BLL.Model.AdminUserRoute>();
            this.MsgBox = "success";
        }

        /// <summary>
        /// 用户id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "姓名"),Required(ErrorMessage ="姓名不能为空"), StringLength(20, ErrorMessage = "姓名不能超过20个字符")]
        public string nick_name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号"), Index(IsUnique = true), StringLength(20), Required(ErrorMessage = "手机号不能为空")]
        public string telphone { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [Required(ErrorMessage ="部门必须选择")]
        public int department_id { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public bool status { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string department_title { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [Required(ErrorMessage ="职位必须选择")]
        public int job_id { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string job_title { get; set; }
        
        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name = "邮箱"),RegularExpression(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?", ErrorMessage = "邮箱格式不正确")]
        public string email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        public string password { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        public string avatar { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别"), Required]
        public Entity.CusUserGender gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Display(Name ="生日")]
        public DateTime? brithday { get; set; }

        [Display(Name = "短号"), MaxLength(20, ErrorMessage = "不能超过20位")]
        public string short_num { get; set; }

        /// <summary>
        /// 个人介绍
        /// </summary>
        [Display(Name = "简介"), MaxLength(500, ErrorMessage = "不能超过500个字符")]
        public string about_me { get; set; }

        /// <summary>
        /// 所有权限
        /// </summary>
        public List<BLL.Model.AdminUserRoute> user_route { get; set; }

        /// <summary>
        /// 权限ID,逗号分割
        /// </summary>
        public string user_route_str { get; set; }

    }

}