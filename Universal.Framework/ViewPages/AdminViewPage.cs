using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Web.Framework
{
    /// <summary>
    /// 后台视图页面基类型
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class AdminViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public AdminWorkContext WorkContext;

        /// <summary>
        /// 初始化 System.Web.Mvc.AjaxHelper、System.Web.Mvc.HtmlHelper 和 System.Web.Mvc.UrlHelper
        /// </summary>
        public override void InitHelpers()
        {
            base.InitHelpers();
            Html.EnableClientValidation(true);//启用客户端验证
            Html.EnableUnobtrusiveJavaScript(true);//启用非侵入式脚本
            WorkContext = ((BaseAdminController)(this.ViewContext.Controller)).WorkContext;
        }

        /// <summary>
        /// 验证管理员权限
        /// </summary>
        /// <param name="PageKey">页面标示符 /controller/action  小写</param>
        /// <returns></returns>
        protected bool CheckAdminPower(string PageKey, bool isPost)
        {
            if (string.IsNullOrWhiteSpace(PageKey))
            {
                isPost = WorkContext.IsHttpPost;
                PageKey = WorkContext.PageKey;
            }
            if (WorkContext.UserInfo.SysRole.IsAdmin)
                return true;
            var result = true;
            var db = new DataCore.EFDBContext();
            if (db.SysRoutes.Count(p => p.IsPost == isPost && p.Route == PageKey) > 0)
            {
                var entity = WorkContext.UserInfo.SysRole.SysRoleRoutes.Where(p => p.SysRoute.Route == PageKey && p.SysRoute.IsPost == isPost).FirstOrDefault();
                result = entity == null ? false : true;
            }
            db.Dispose();
            return result;
        }
    }


    /// <summary>
    /// 后台视图页面基类型
    /// </summary>
    public abstract class AdminViewPage : AdminViewPage<dynamic>
    {

    }
}
