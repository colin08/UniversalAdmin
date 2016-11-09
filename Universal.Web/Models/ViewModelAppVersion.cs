using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Universal.Entity;

namespace Universal.Web.Models
{
    public class ViewModelAppVersion:ViewModelCustomFormBase
    {
        public int ID { get; set; }

        [Display(Name = "所属平台"),Required(ErrorMessage ="平台不能为空")]
        public APPVersionPlatforms Platforms { get; set; }
        
        /// <summary>
        /// 所属版本
        /// </summary>
        [Display(Name = "版本类别"), Required(ErrorMessage = "版本类别不能为空")]
        public APPVersionType APPType { get; set; }
        
        [MaxLength(100)]
        public string MD5 { get; set; }
        
        public long Size { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [MaxLength(20), Display(Name = "版本号")]
        public string Version { get; set; }

        [Required, Display(Name = "升级号")]
        public int VersionCode { get; set; }

        [MaxLength(255), Display(Name = "Logo地址")]
        public string LogoImg { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        [MaxLength(255), Display(Name = "下载地址")]
        public string DownUrl { get; set; }

        [MaxLength(500), Display(Name = "链接地址(IOS)")]
        public string LinkUrl { get; set; }

        [Required, MaxLength(500), Display(Name = "更新介绍")]
        public string Content { get; set; }
        
    }
}