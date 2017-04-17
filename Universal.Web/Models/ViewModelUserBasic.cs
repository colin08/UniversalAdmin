﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 用户基本资料
    /// </summary>
    public class ViewModelUserBasic
    {
        public ViewModelUserBasic()
        {
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "姓名"), StringLength(20, ErrorMessage = "姓名不能超过20个字符")]
        public string nick_name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Entity.CusUserGender gender { get; set; }

        public DateTime? brithday { get; set; }

        /// <summary>
        /// 短号
        /// </summary>
        [Display(Name = "短号"), MaxLength(20, ErrorMessage = "不能超过20位")]
        public string short_num { get; set; }


        /// <summary>
        /// 关于我
        /// </summary>
        [Display(Name = "简介"), MaxLength(500, ErrorMessage = "不能超过150个字符")]
        public string about_me { get; set; }


        public bool State { get; set; }

    }
}