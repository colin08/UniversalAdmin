using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.DataCore.Entity
{
    /// <summary>
    /// 后台公共字段基类
    /// </summary>
    public class BaseAdminEntity
    {
        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 添加信息的用户信息
        /// </summary>
        public virtual SysUser AddUser { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Required]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 最后修改的用户的信息
        /// </summary>
        public virtual SysUser LastUpdateUser { get; set; }

    }
}
