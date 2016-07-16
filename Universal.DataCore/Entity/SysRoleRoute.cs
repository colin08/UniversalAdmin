using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.DataCore.Entity
{
    
    /// <summary>
    /// 用户组权限信息
    /// </summary>
    public class SysRoleRoute
    {
        public int ID { get; set; }

        /// <summary>
        /// 关联的组ID
        /// </summary>
        public int SysRoleID { get; set; }
        
        /// <summary>
        /// 对象的操作路由
        /// </summary>
        public int SysRouteID { get; set; }

        /// <summary>
        /// 权限信息
        /// </summary>
        public virtual SysRoute SysRoute { get; set; }

    }
}
