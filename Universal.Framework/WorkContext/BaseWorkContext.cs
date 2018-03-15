using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Web.Framework
{
    public class BaseWorkContext
    {
        /// <summary>
        /// 当前请求是否为ajax请求
        /// </summary>
        public bool IsHttpAjax;

        /// <summary>
        /// 当前请求是否为Post请求
        /// </summary>
        public bool IsHttpPost;

        /// <summary>
        /// 当前的SessionId
        /// </summary>
        public string SessionId;

        /// <summary>
        /// 用户ip
        /// </summary>
        public string IP;

        /// <summary>
        /// 当前url
        /// </summary>
        public string Url;

        /// <summary>
        /// 上一次访问的url
        /// </summary>
        public string UrlReferrer;

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller;

        /// <summary>
        /// 动作方法
        /// </summary>
        public string Action;

        /// <summary>
        /// 页面标示符 ，area/controller/action   已转换为小写
        /// </summary>
        public string PageKey;

        /// <summary>
        /// 页面标识符，controleraction 
        /// </summary>
        public string PageKeyCookie;

        /// <summary>
        /// Ajax请求一般操作返回json实体对象，一般不需要data字段
        /// </summary>
        public UnifiedResultEntity<string> AjaxStringEntity;

    }
}
