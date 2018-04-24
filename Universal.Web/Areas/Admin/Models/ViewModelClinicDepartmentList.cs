using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    /// <summary>
    /// 分页列表
    /// </summary>
    public class ViewModelClinicDepartmentList : BasePageModel
    {
        /// <summary>
        /// 当前筛选的关键字
        /// </summary>
        public string word { get; set; }

        public int cli { get; set; }

        /// <summary>
        /// 列表
        /// </summary>
        public List<Entity.ClinicDepartment> DataList { get; set; }
        
    }
}