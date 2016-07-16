using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Universal.DataCore.Entity
{
    /// <summary>
    /// 用户组
    /// </summary>
    public class SysRole
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        [StringLength(30), Required]
        public string RoleName { get; set; }

        /// <summary>
        /// 是否是管理员组(管理组拥有所有权限)
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 用户组说明
        /// </summary>
        [StringLength(255), Required]
        public string RoleDesc { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Required]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 组所拥有的权限
        /// </summary>
        public ICollection<SysRoleRoute> SysRoleRoutes { get; set; }
    }
}
