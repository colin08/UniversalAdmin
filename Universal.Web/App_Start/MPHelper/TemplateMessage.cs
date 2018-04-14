using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.AdvancedAPIs;
using Universal.Tools;

namespace Universal.Web.MPHelper
{
    /// <summary>
    /// 微信模板消息帮助类
    /// </summary>
    public class TemplateMessage
    {
        /// <summary>
        /// 站点配置文件
        /// </summary>
        private static WebSiteModel WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);

        public static bool SendTestMsg(string open_id,string frist,string message,string remark)
        {
            var templateId = "u56qAtUTVDCHnqa19Ag_YN_pPMO_uXo1uVp8Ca-AZ_g";
            var test_data = new
            {
                frist=new TemplateDataItem(frist),
                keyword1 = new TemplateDataItem(message, "#457939"),
                keyword5=new TemplateDataItem(DateTime.Now.ToString()),
                remark=new TemplateDataItem(remark)
            };
            var result = TemplateApi.SendTemplateMessage(WebSite.WXAppID, open_id, templateId, "http://www.baidu.com", test_data);
            return result.errcode == Senparc.Weixin.ReturnCode.请求成功;
        }
    }
}