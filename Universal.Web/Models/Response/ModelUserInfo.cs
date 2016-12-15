using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class ModelUserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int id { get; set; }
        
        /// <summary>
        /// 手机号
        /// </summary>
        public string telphone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string nick_name { get; set; }

        /// <summary>
        /// 所属部门id
        /// </summary>
        public int department_id { get; set; }

        /// <summary>
        /// 所属部门名称
        /// </summary>
        public string department_name { get; set; }

        /// <summary>
        /// 该用户是否是所属部门的主管
        /// </summary>
        public bool is_department_manager { get; set; }

        /// <summary>
        /// 所属职位id
        /// </summary>
        public int job_id { get; set; }

        /// <summary>
        /// 所属职位名称
        /// </summary>
        public string job_name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Entity.CusUserGender gender { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 生日，可为空
        /// </summary>
        public DateTime? brithday { get; set; }

        /// <summary>
        /// 短号
        /// </summary>
        public string shor_num { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string about_me { get; set; }
        
    }
}