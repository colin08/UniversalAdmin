using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Universal.Tools;

namespace Universal.Entity
{
    /// <summary>
    /// 操作日志类别
    /// </summary>
    public enum SysLogMethodType : byte
    {
        [Description("添加")]
        Add = 1,
        [Description("修改")]
        Update = 2,
        [Description("删除")]
        Delete = 3,
        [Description("登录")]
        Login = 4,
        [Description("还原")]
        Resotre
    }

    /// <summary>
    /// 操作日志
    /// </summary>
    public class SysLogMethod
    {
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public int SysUserID { get; set; }

        /// <summary>
        /// 操作类别
        /// </summary>
        [Required]
        public SysLogMethodType Type { get; set; }

        [NotMapped]
        public string TypeStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<SysLogMethodType>(this.Type);
            }
        }

        /// <summary>
        /// 详细内容
        /// </summary>
        [StringLength(500)]
        [Required]
        public string Detail { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 操作的用户
        /// </summary>
        public virtual SysUser SysUser { get; set; }

    }
}
