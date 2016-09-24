using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 后台公共字段基类
    /// </summary>
    public class BaseAdminEntity
    {
        /// <summary>
        /// 添加时间
        /// </summary>
        [Required,Display(Name ="添加时间")]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 添加者ID
        /// </summary>
        public int AddUserID { get; set; }

        /// <summary>
        /// 添加信息的用户信息
        /// </summary>
        [Display(Name ="添加者")]
        [ForeignKey("AddUserID")]
        public virtual SysUser AddUser { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [Required,Display(Name ="最后修改时间")]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 最后修改者ID
        /// </summary>
        public int LastUpdateUserID { get; set; }

        /// <summary>
        /// 最后修改的用户的信息
        /// </summary>
        [Display(Name ="最后修改者")]
        [ForeignKey("LastUpdateUserID")]
        public virtual SysUser LastUpdateUser { get; set; }

    }
}
