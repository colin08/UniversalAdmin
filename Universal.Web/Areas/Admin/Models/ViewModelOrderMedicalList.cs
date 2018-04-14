using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    /// <summary>
    /// 列表
    /// </summary>
    public class ViewModelOrderMedicalList : BasePageModel
    {
        /// <summary>
        /// 当前筛选的组
        /// </summary>
        public int role { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int role2 { get; set; }

        /// <summary>
        /// 当前筛选的关键字
        /// </summary>
        public string word { get; set; }

        /// <summary>
        /// 列表
        /// </summary>
        public List<Entity.OrderMedical> DataList { get; set; }
        
    }
}