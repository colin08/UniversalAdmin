using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Universal.DataCore.Entity
{
    /// <summary>
    /// app的平台分类枚举
    /// </summary>
    public enum APPVersionPlatforms
    {
        [Description("安卓")]
        Android = 1,

        [Description("苹果")]
        IOS = 2
    }

    /// <summary>
    /// app版本枚举
    /// </summary>
    public enum APPVersionType
    {
        [Description("标准版")]
        Standard = 1,
        [Description("企业版")]
        Enterprise = 2
    }

    public class AppVersion
    {
        public int ID { get; set; }
        
        [Display(Name = "所属平台")]
        public APPVersionPlatforms Platforms { get; set; }

        [NotMapped]
        public string PlatformsStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<APPVersionPlatforms>(this.Platforms);
            }
        }

        [NotMapped]
        public string PlatformsLog
        {
            get
            {
                switch (Platforms)
                {
                    case APPVersionPlatforms.Android:
                        return "fa fa-android";
                    case APPVersionPlatforms.IOS:
                        return "fa fa-apple";
                    default:
                        return "fa fa-question";
                }
            }
        }

        /// <summary>
        /// 所属版本
        /// </summary>
        [Display(Name = "版本类别")]
        public APPVersionType APPType { get; set; }

        [NotMapped]
        public string APPTypeStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<APPVersionType>(this.APPType);
            }
        }
        
        [Required, MaxLength(100),Display(Name ="文件MD5值")]
        public string MD5 { get; set; }
        
        [Required,Display(Name ="文件大小")]
        public long Size { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [Required, MaxLength(20),Display(Name ="版本号")]
        public string Version { get; set; }
        
        [Required,Display(Name ="升级号")]
        public int VersionCode { get; set; }
        
        [Required, MaxLength(255),Display(Name ="Logo地址")]
        public string LogoImg { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        [Required, MaxLength(255),Display(Name ="下载地址")]
        public string DownUrl { get; set; }
        
        [MaxLength(500),Display(Name ="链接地址(IOS)")]
        public string LinkUrl { get; set; }
        
        [Required, MaxLength(500),Display(Name ="更新介绍")]
        public string Content { get; set; }
        
        [Required,Display(Name ="发布时间")]
        public DateTime AddTime { get; set; }
    }
}
