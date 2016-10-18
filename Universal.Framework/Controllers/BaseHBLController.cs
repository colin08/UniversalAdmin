using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using Universal.Tools;
using System.Data.Entity;
using Universal.BLL;
using System.Collections.Generic;

namespace Universal.Web.Framework
{
    public class BaseHBLController : Controller
    {
        /// <summary>
        /// 工作上下文
        /// </summary>
        public WebWorkContext WorkContext = new WebWorkContext();
        /// <summary>
        /// 站点配置文件
        /// </summary>
        public WebSiteModel WebSite = null;

        /// <summary>
        /// 初始化调用构造函数后可能不可用的数据
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);
            WorkContext.SessionId = Session.SessionID;
            WorkContext.IsHttpAjax = WebHelper.IsAjax();
            WorkContext.IsHttpPost = WebHelper.IsPost();
            WorkContext.IP = WebHelper.GetIP();
            WorkContext.Url = WebHelper.GetUrl();
            WorkContext.UrlReferrer = WebHelper.GetUrlReferrer();
            WorkContext.AjaxStringEntity = new WebAjaxEntity<string>();
            WorkContext.AjaxStringEntity.msg = 0; //默认错误
            WorkContext.AjaxStringEntity.total = 0;
            //用户
            WorkContext.UserInfo = BLLCusUser.GetUserInfo();
        }
    }
}
