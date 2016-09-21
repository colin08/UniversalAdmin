using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Universal.Web
{
    /// <summary>
    /// 过滤配置器
    /// </summary>
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute(),1);
            filters.Add(new Universal.Web.Framework.CustomWebExceptionAttribute(),2);
            filters.Add(new Framework.CustomActionAttribute());
        }
    }
}