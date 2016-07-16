using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.DataCore.Entity
{
    /// <summary>
    /// app的平台分类枚举
    /// </summary>
    public enum APPVersionPlatforms
    {
        /// <summary>
        /// 安卓
        /// </summary>
        Android = 1,

        /// <summary>
        /// 苹果
        /// </summary>
        IOS = 2
    }

    /// <summary>
    /// app版本枚举
    /// </summary>
    public enum APPVersionType
    {
        标准版 = 1,
        企业版 = 2
    }

    public class AppVersion
    {
        public int ID { get; set; }

        /// <summary>
        /// 所属平台
        /// </summary>
        public APPVersionPlatforms Platforms { get; set; }

        /// <summary>
        /// 所属版本
        /// </summary>
        public APPVersionType APPType { get; set; }

        /// <summary>
        /// 文件MD5
        /// </summary>
        [Required, MaxLength(100)]
        public string MD5 { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        [Required]
        public long Size { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [Required, MaxLength(20)]
        public string Version { get; set; }

        /// <summary>
        /// 升级号
        /// </summary>
        [Required]
        public int VersionCode { get; set; }

        /// <summary>
        /// logo地址
        /// </summary>
        [Required, MaxLength(255)]
        public string LogoImg { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        [Required, MaxLength(255)]
        public string DownUrl { get; set; }

        /// <summary>
        /// 链接地址，IOS使用
        /// </summary>
        [MaxLength(500)]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 更新介绍
        /// </summary>
        [Required, MaxLength(500)]
        public string Content { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [Required]
        public DateTime AddTime { get; set; }
    }
}
