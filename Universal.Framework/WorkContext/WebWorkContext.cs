using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Web.Framework
{
    public class WebWorkContext:BaseWorkContext
    {
        /// <summary>
        /// 当前登录的用户
        /// </summary>
        public Entity.CusUser UserInfo;

        /// <summary>
        /// 管理首页
        /// </summary>
        public string ManagerHome { get; set; }

    }
}
