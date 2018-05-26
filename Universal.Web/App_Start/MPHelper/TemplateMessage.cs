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
            string keyword1 = string.Format("{0}", time);
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

        /// <summary>
        /// 在线咨询已支付后给医生和用户发送消息
        /// </summary>
        /// <param name="order_num"></param>
        /// <returns></returns>
        public static void SendDoctorsAndUserAdvisoryIsOK(string order_num)
        {
            var entity = BLL.BLLConsultation.GetModel(order_num);
            if (entity == null) { System.Diagnostics.Trace.WriteLine("在线咨询-已支付-给医生发消息出错：订单号不存在"+order_num); return; }
            if(entity.Status != Entity.ConsultationStatus.已支付) { System.Diagnostics.Trace.WriteLine("在线咨询-已支付-给医生发消息出错：订单号不是刚支付状态" + order_num); return; }
            if(entity.MPDoctorInfo == null) { System.Diagnostics.Trace.WriteLine("在线咨询-已支付-给医生发消息出错：医生不存在" + order_num); return; }
            if (entity.MPUserInfo == null) { System.Diagnostics.Trace.WriteLine("在线咨询-已支付-给医生发消息出错：用户不存在" + order_num); return; }
            if (entity.ConsultationDisease == null) { System.Diagnostics.Trace.WriteLine("在线咨询-已支付-给医生发消息出错：咨询类型为空" + order_num); return; }

            //给医生发
            string first = string.Format("{0},您好，有患者向你提出咨询", entity.MPDoctorInfo.RealName);
            string keyword1 = entity.MPUserInfo.RealName;
            string keyword2 = entity.ConsultationDisease.Title;
            string keyword3 = WebHelper.CutString(entity.Content, 10);
            string keyword4= TypeHelper.ObjectToDateTime(entity.PayTime, DateTime.Now).ToString("yyyy年MM月dd日 HH点mm分");
            string remark = "请您及时回复，点击查看咨询详情";
            string link_url = WebSite + "/MP/Doctors/AdvisoryInfo?id=" + entity.ID;
            var templateId = "H8523M9weflJrcbuWKqKmxHjvZscYr7HRYi_MSjgUZw";
            var template_data = new
            {
                first = new TemplateDataItem(first),
                keyword1 = new TemplateDataItem(keyword1),
                keyword2 = new TemplateDataItem(keyword2),
                keyword3 = new TemplateDataItem(keyword3),
                keyword4 = new TemplateDataItem(keyword4),
                remark = new TemplateDataItem(remark)
            };
            var result = TemplateApi.SendTemplateMessage(WebSite.WeChatAppID, entity.MPDoctorInfo.OpenID, templateId, link_url, template_data);
            if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("在线咨询-已支付-给医生发消息出错：{0},订单号：{1}", result.errmsg, order_num));
            }
            //给用户发消息
            string templateId_user = "KVlf01nndKVMzTCvEOtgAaEEdr1JO8IaEFoOeZRzyzY";
            string first_user = "支付咨询成功";
            string keyword1_user = entity.MPDoctorInfo.RealName;
            string keyword2_user = entity.PayMoney.ToString("F2") +"元";
            string remark_user = "点击查看咨询详情";
            string link_url_user = WebSite + "/MP/Advisory/AdvisoryInfo?id=" + entity.ID;
            var template_data_user = new
            {
                first = new TemplateDataItem(first),
                keyword1 = new TemplateDataItem(keyword1),
                keyword2 = new TemplateDataItem(keyword2),
                remark = new TemplateDataItem(remark)
            };
            var result_user = TemplateApi.SendTemplateMessage(WebSite.WeChatAppID, entity.MPUserInfo.OpenID, templateId_user, link_url_user, template_data_user);
            if (result_user.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("在线咨询-已支付-给用户发消息出错：{0},订单号：{1}", result.errmsg, order_num));
            }
        }

    }
}