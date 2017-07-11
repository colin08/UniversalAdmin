using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    /// <summary>
    /// 商户分页列表
    /// </summary>
    public class ViewModelSysMerchantList : BasePageModel
    {
        /// <summary>
        /// 当前筛选的关键字
        /// </summary>
        public string word { get; set; }

        /// <summary>
        /// 商户列表
        /// </summary>
        public List<Entity.SysMerchant> DataList { get; set; }
        
    }
}