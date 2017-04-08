using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using Universal.Tools;
using System.Data.Entity;
using Universal.BLL;
using System.Collections.Generic;
using System.Data.Entity;

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
            //设置当前控制器类名
            WorkContext.Controller = RouteData.Values["controller"].ToString().ToLower();
            //设置当前动作方法名
            WorkContext.Action = RouteData.Values["action"].ToString().ToLower();
            WorkContext.PageKey = string.Format("/{0}/{1}", WorkContext.Controller, WorkContext.Action).ToLower();

            //用户
            WorkContext.UserInfo = BLLCusUser.GetUserInfo();

            if (WorkContext.UserInfo == null)
            {
                WorkContext.ManagerHome = "/Account/Login";
            }
            else
            {
                if (!WorkContext.UserInfo.IsAdmin)
                    WorkContext.ManagerHome = "/User/Basic";
                else
                {
                    //获取有权限的第一个
                    string controll = BLL.BLLCusRoute.GetFristRoute(WorkContext.UserInfo.ID);
                    if (string.IsNullOrWhiteSpace(controll))
                        WorkContext.ManagerHome = "";
                    else
                        WorkContext.ManagerHome = "/" + controll + "/index";
                }
            }
            GetMessage();

            GetFavorites();
        }

        /// <summary>
        /// 分页 计算总页数
        /// </summary>
        /// <param name="total"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        protected int CalculatePage(int total, int page_size)
        {
            if (page_size <= 0)
            {
                return 0;
            }
            return TypeHelper.ObjectToInt(Math.Ceiling(Convert.ToDouble(total) / Convert.ToDouble(page_size)));
        }


        /// <summary>
        /// 更新消息信息
        /// </summary>
        protected void GetMessage()
        {
            if(WorkContext.UserInfo != null)
            {
                //获取消息
                BLL.BaseBLL<Entity.CusUserMessage> bll = new BaseBLL<Entity.CusUserMessage>();
                WorkContext.UnReadMessageTopList = bll.GetListBy(5, p => p.CusUserID == WorkContext.UserInfo.ID && p.IsRead == false, "IsRead DESC,AddTime desc").ToList();
                WorkContext.MessageTotal = bll.GetCount(p => p.CusUserID == WorkContext.UserInfo.ID && p.IsRead == false);
            }
        }
        
        /// <summary>
        /// 获取收藏列表信息
        /// </summary>
        protected void GetFavorites()
        {
            if(WorkContext.UserInfo != null)
            {
                List<BLL.Model.LayoutFavorites> result = new List<BLL.Model.LayoutFavorites>();
                BLL.BaseBLL<Entity.CusUserProjectFavorites> bll = new BaseBLL<Entity.CusUserProjectFavorites>();
                var project_list = bll.GetListBy(3, p => p.CusUserID == WorkContext.UserInfo.ID, "AddTime DESC", p => p.Project);
                foreach (var item in project_list)
                {
                    var model = new BLL.Model.LayoutFavorites();
                    model.icon = "/Assets/theme/ico/ico_35.png";
                    model.id = item.ID;
                    model.link_url = "/Project/BasicInfo?id=" + item.ProjectID.ToString();
                    model.title = item.Project.Title;
                    model.er_title = "预留";
                    result.Add(model);
                }

                BLL.BaseBLL<Entity.CusUserDocFavorites> bll_doc = new BaseBLL<Entity.CusUserDocFavorites>();
                var doc_list = bll_doc.GetListBy(3, p => p.CusUserID == WorkContext.UserInfo.ID, "AddTime DESC", p => p.DocPost);
                foreach (var item in doc_list)
                {
                    var model = new BLL.Model.LayoutFavorites();
                    model.icon = "/Assets/theme/ico/ico_36.png";
                    model.id = item.ID;
                    model.link_url = "/Info/Document?id=" + item.DocPostID.ToString();
                    model.title = item.DocPost.Title;
                    model.er_title = "";
                    result.Add(model);
                }

                WorkContext.UserFavoritesList = result;
            }
        }


        /// <summary>
        /// 在进行授权时调用
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;

            //判断是否登陆
            if (WorkContext.UserInfo == null)
            {
                List<string> path_list = new List<string>();
                path_list.Add("/account/login");
                path_list.Add("/account/resetpwd");
                path_list.Add("/account/resetsuc");
                path_list.Add("/account/sendcode");
                path_list.Add("/share/doc");
                if (!path_list.Contains(WorkContext.PageKey.ToLower()))
                {
                    if (WebHelper.IsAjax())
                    {
                        WorkContext.AjaxStringEntity.msg = 0;
                        WorkContext.AjaxStringEntity.msgbox = "登录超时";
                        filterContext.Result = Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        filterContext.Result = RedirectToAction("Login", "Account");
                    }
                }
            }

        }


    }
}
