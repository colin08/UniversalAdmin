using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Web.Framework
{
    /// <summary>
    /// 前台视图页面基类型
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class HBLMobileViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public WebWorkContext WorkContext;

        /// <summary>
        /// 初始化 System.Web.Mvc.AjaxHelper、System.Web.Mvc.HtmlHelper 和 System.Web.Mvc.UrlHelper
        /// </summary>
        public override void InitHelpers()
        {
            base.InitHelpers();
            Html.EnableClientValidation(true);//启用客户端验证
            Html.EnableUnobtrusiveJavaScript(true);//启用非侵入式脚本
            WorkContext = ((BaseHBLMobileController)(this.ViewContext.Controller)).WorkContext;
        }
    }


    /// <summary>
    /// 后台视图页面基类型
    /// </summary>
    public abstract class HBLMobileViewPage: HBLMobileViewPage<dynamic>
    {

    }
}
