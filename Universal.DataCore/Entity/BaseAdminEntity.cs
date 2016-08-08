using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.DataCore.Entity
{
    /// <summary>
    /// 后台公共字段基类
    /// </summary>
    public class BaseAdminEntity
    {

        /// <summary>
        /// 状态 -1:(数据不存在等)，-2：数据验证出错，1：成功并跳转
        /// </summary>
        [NotMapped]
        public int Msg { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [NotMapped]
        public string MsgBox { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>
        [NotMapped]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Required,Display(Name ="添加时间")]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 添加信息的用户信息
        /// </summary>
        [Display(Name ="添加者")]
        public virtual SysUser AddUser { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Required,Display(Name ="最后修改时间")]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 最后修改的用户的信息
        /// </summary>
        [Display(Name ="最后修改者")]
        public virtual SysUser LastUpdateUser { get; set; }

    }
}
