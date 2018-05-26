using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Universal.Tools;

namespace Universal.Web.Framework
{
    /// <summary>
    /// 只允许普通用户访问的Action
    /// </summary>
    public class OnlyBasicUserAttribute : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// Action执行前，第一步
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var entity_user = BLL.BLLMPUserState.GetUserInfo();
            if (entity_user != null)
            {
                if (entity_user.Identity == Entity.MPUserIdentity.Doctors)
                {
                    if (WebHelper.IsAjax())
                    {
                        UnifiedResultEntity<string> res = new UnifiedResultEntity<string>();
                        res.msg = 0;
                        res.msgbox = "此功能不允许医生使用";
                        JsonResult jr = new JsonResult();
                        jr.Data = res;
                        filterContext.Result = jr;
                    }
                    else
                    {
                        //当前访问的URL
                        string request_url = filterContext.RequestContext.HttpContext.Request.Url.AbsolutePath.ToLower();
                        UserDocRouteMap map = new UserDocRouteMap();
                        //跳到对应医生页面
                        filterContext.Result = new RedirectResult(map.GetMapUrl(true, request_url));

                    }
                }
            }

            //base.OnActionExecuting(filterContext);
        }
        /// <summary>
        /// Action执行后，第二步
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Action执行之后
            //base.OnActionExecuted(filterContext);            
        }
    }

    internal class UserDocRouteMap
    {
        List<UserDocRouteMapEntity> route_list = new List<UserDocRouteMapEntity>();

        public UserDocRouteMap()
        {
            //个人中心首页
            route_list.Add(new UserDocRouteMapEntity("/mp/basicuser/index", "/mp/doctors/index"));
            //TODO 设置用户-医生的映射地址 我要咨询--向我咨询列表
            route_list.Add(new UserDocRouteMapEntity("/mp/advisory/index", "/mp/doctors/advisory"));
            //TODO 设置用户-医生的映射地址 预约挂号--我的预约挂号列表
        }

        /// <summary>
        /// 获取对应的地址
        /// </summary>
        /// <returns></returns>
        internal string GetMapUrl(bool is_user, string request_url)
        {
            UserDocRouteMapEntity search = null;
            if (is_user)
            {
                search = route_list.Where(p => p.user == request_url).FirstOrDefault();
                if (search != null) return search.doc; else return "/mp/doctors/index";
            }
            else
            {
                search = route_list.Where(p => p.doc == request_url).FirstOrDefault();
                if (search != null) return search.user; else return "/mp/basicuser/index";
            }
        }
    }

    internal class UserDocRouteMapEntity
    {
        internal UserDocRouteMapEntity(string user, string doc)
        {
            this.user = user;
            this.doc = doc;
        }

        /// <summary>
        /// 普通用户访问的路径
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// 对应的医生路径
        /// </summary>
        public string doc { get; set; }
    }

}
