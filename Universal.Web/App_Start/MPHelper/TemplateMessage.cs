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

        public static bool SendTestMsg(string open_id, string frist, string message, string remark)
        {
            var templateId = "u56qAtUTVDCHnqa19Ag_YN_pPMO_uXo1uVp8Ca-AZ_g";
            var test_data = new
            {
                frist = new TemplateDataItem(frist),
                keyword1 = new TemplateDataItem(message, "#457939"),
                keyword5 = new TemplateDataItem(DateTime.Now.ToString()),
                remark = new TemplateDataItem(remark)
            };
            var result = TemplateApi.SendTemplateMessage(WebSite.WeChatAppID, open_id, templateId, "http://www.baidu.com", test_data);
            return result.errcode == Senparc.Weixin.ReturnCode.请求成功;
        }

        /// <summary>
        /// 账户余额变动提醒
        /// </summary>
        /// <param name="mad_id">用户账户记录表历史</param>
        /// <param name="open_id"></param>
        /// <param name="order_num"></param>
        /// <param name="link_url"></param>
        /// <returns></returns>
        public static bool SendUserAmountMsg(int mad_id, string open_id,string order_num,string link_url)
        {            
            var entity = BLL.BLLMPUserAmountDetails.GetModel(mad_id);
            if(entity == null)
            {
                System.Diagnostics.Trace.WriteLine("模板消息:未找到要发送的资金变动历史-" + mad_id.ToString());
                return false;
            }
            string t_type = "支出";
            if(entity.Type == Entity.MPUserAmountDetailsType.Add)
            {
                t_type = "支入";
            }
            string amount = entity.Amount.ToString("F2");
            string time = entity.AddTime.ToString("yyyy-MM-dd HH:mm");
            string first = string.Format("您的帐户于{0} {1}{2}元", time, t_type, amount);
            string keyword1 = string.Format("操作时间:{0}", time);
            string keyword2 = entity.Title;
            string remark = "流水号:" + order_num;

            var templateId = "wl1PgPOX3wLN9BBkIhaLeyGj6e4O_Z9icIS9OVnsX2s";
            var template_data = new
            {
                first = new TemplateDataItem(first),
                keyword1 = new TemplateDataItem(keyword1),
                keyword2 = new TemplateDataItem(keyword2),
                remark = new TemplateDataItem(remark)
            };
            var result = TemplateApi.SendTemplateMessage(WebSite.WeChatAppID, open_id, templateId, link_url, template_data);
            return result.errcode == Senparc.Weixin.ReturnCode.请求成功;
        }
    }
}