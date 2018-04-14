using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.Tools;

namespace Universal.Web.Framework
{
    public class MPWorkContext:BaseWorkContext
    {
        /// <summary>
        /// 当前登录的用户
        /// </summary>
        public Entity.MPUser UserInfo;                

        /// <summary>
        /// 用户OPENID
        /// </summary>
        public string open_id { get; set; }

        /// <summary>
        /// 站点配置文件
        /// </summary>
        public WebSiteModel WebSite { get; set; }
    }
}
