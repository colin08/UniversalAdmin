using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    /// <summary>
    /// 系统注册的路由列表
    /// </summary>
    public class ModelRoute
    {
        public string Desc { get; set; }

        public bool IsPost { get; set; }

        public string Route { get; set; }

        public string Tag { get; set; }
    }
}