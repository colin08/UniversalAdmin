using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Web.Framework
{
    public class AdminWorkContext:BaseWorkContext
    {
        /// <summary>
        /// 当前登录的用户
        /// </summary>
        public DataCore.Entity.SysUser UserInfo;
    }
}
