using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 部门管理首页数据
    /// </summary>
    public class ViewModelDepartment
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public int department_id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int? parent_id { get; set; }

        /// <summary>
        /// 部门主管信息
        /// </summary>
        public List<ViewModelDepartmentAdmin> admin_list { get; set; }
    }

    /// <summary>
    /// 部门管理员信息
    /// </summary>
    public class ViewModelDepartmentAdmin
    {
        /// <summary>
        /// 主管ID
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 主管名称
        /// </summary>
        public string name { get; set; }

    }

}