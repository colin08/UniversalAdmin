using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 部门管理员
    /// </summary>
    public class CusDepartmentAdmin
    {
        public int ID { get; set; }

        public int CusDepartmentID { get; set; }

        public virtual CusDepartment CusDepartment { get; set; }

        public int CusUserID { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public virtual CusUser CusUser { get; set; }

    }
}
