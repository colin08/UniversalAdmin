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
        protected bool CheckAdminPower(string PageKey)
        {
            //BLL.ManagerRole bll = new BLL.ManagerRole();
            //return bll.CheckAdminPower(WorkContext.AdminInfo.Manager_RoleId, WorkContext.AdminInfo.Manager_RoleType, PageKey);
            return true;
        }

        /// <summary>
        /// 验证管理员权限
        /// </summary>
        /// <returns></returns>
        protected bool CheckPower(int role_id, int role_type, int pa_id)
        {
            //BLL.ManagerRole bll = new BLL.ManagerRole();
            //return bll.CheckAdminPower(role_id, role_type, pa_id);
            return true;
        }
    }


    /// <summary>
    /// 后台视图页面基类型
    /// </summary>
    public abstract class AdminViewPage : AdminViewPage<dynamic>
    {

    }
}
