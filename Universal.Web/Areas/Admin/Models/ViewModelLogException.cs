using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    public class ViewModelLogException:BasePageModel
    {
        /// <summary>
        /// 当前筛选的关键字
        /// </summary>
        public string word { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Entity.SysLogException> DataList { get; set; }
    }
}