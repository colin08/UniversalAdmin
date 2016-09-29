using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    /// <summary>
    /// 接口日志
    /// </summary>
    public class ViewModelLogAPIAction: BasePageModel
    {
        /// <summary>
        /// 当前筛选的关键字
        /// </summary>
        public string word { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public List<Entity.SysLogApiAction> DataList { get; set; }
    }
}