using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.Tools;

namespace Universal.Web.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public class WebWorkContext : BaseWorkContext
    {
        public WebSiteModel WebSiteConfig = null;

        /// <summary>
        /// 当前页面所属大分类标识
        /// </summary>
        public string CategoryMark = string.Empty;

        /// <summary>
        /// 当前页面二级分类标识
        /// </summary>
        public string CategoryErMark = string.Empty;

    }
}
