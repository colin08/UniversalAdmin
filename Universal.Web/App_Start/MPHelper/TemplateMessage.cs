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
        public static void SendDoctorsAndUserAdvisoryIsPay(string order_num)
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
            string link_url = WebSite.SiteUrl + "/MP/Doctors/AdvisoryInfo?id=" + entity.ID;
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
            string link_url_user = WebSite.SiteUrl + "/MP/Advisory/AdvisoryInfo?id=" + entity.ID;
            var template_data_user = new
            {
                first = new TemplateDataItem(first_user),
                keyword1 = new TemplateDataItem(keyword1_user),
                keyword2 = new TemplateDataItem(keyword2_user),
                remark = new TemplateDataItem(remark_user)
            };
            var result_user = TemplateApi.SendTemplateMessage(WebSite.WeChatAppID, entity.MPUserInfo.OpenID, templateId_user, link_url_user, template_data_user);
            if (result_user.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("在线咨询-已支付-给用户发消息出错：{0},订单号：{1}", result.errmsg, order_num));
            }
            //设置医生超时未回复自动退款
            TaskJobHelper.AddAdvisoryRefund(entity.ID, TypeHelper.ObjectToDateTime(entity.PayTime,DateTime.Now), WebSite.AdvisoryNoReplyTimeOut);
        }


        /// <summary>
        /// 在线咨询超时后设置结束-给用户和医生发送消息
        /// </summary>
        /// <param name="order_num"></param>
        /// <returns></returns>
        public static void SendDoctorsAndUserAdvisoryIsDone(int id,string timeStr)
        {
            var entity = BLL.BLLConsultation.GetModel(id);
            if (entity == null) { System.Diagnostics.Trace.WriteLine("在线咨询-设置超时自动结束-给医生发消息出错：咨询不存在" + id.ToString()); return; }
            if (entity.Status != Entity.ConsultationStatus.已完成) { System.Diagnostics.Trace.WriteLine("在线咨询-设置超时自动结束-给医生发消息出错：订单号不是已完成状态" + id.ToString()); return; }
            if (entity.MPDoctorInfo == null) { System.Diagnostics.Trace.WriteLine("在线咨询-设置超时自动结束-给医生发消息出错：医生不存在" + id.ToString()); return; }
            if (entity.MPUserInfo == null) { System.Diagnostics.Trace.WriteLine("在线咨询-设置超时自动结束-给医生发消息出错：用户不存在" + id.ToString()); return; }
            if (entity.ConsultationDisease == null) { System.Diagnostics.Trace.WriteLine("在线咨询-设置超时自动结束-给医生发消息出错：咨询类型为空" + id.ToString()); return; }

            //给医生发
            string first = string.Format("{0},您好，患者咨询已结束。", entity.MPDoctorInfo.RealName);
            string keyword1 = string.Format("超过{0},自动结束", timeStr);
            string keyword2 = entity.MPUserInfo.RealName;
            string keyword3 = WebHelper.CutString(entity.Content, 10);
            string keyword4 = TypeHelper.ObjectToDateTime(entity.PayTime, DateTime.Now).ToString("yyyy年MM月dd日 HH点mm分");
            string remark = "该咨询已可以进行结算，点击前往结算";
            string link_url = WebSite.SiteUrl + "/MP/Doctors/AdvisoryClear";
            var templateId = "UJYpt7rtGd6AodLWeZYQ4bwvBpXbSjTqWjc3xHgLOBo";
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
                System.Diagnostics.Trace.WriteLine(string.Format("在线咨询-设置超时自动结束-给医生发消息出错：{0},咨询ID：{1}", result.errmsg, id.ToString()));
            }
            //给用户发消息
            string templateId_user = "fmqOskT9csvy9ez1JmBC4NsrcTFUl3XUPMqgrnDBx94";
            string first_user = string.Format("{0},您好，您的咨询已结束。", entity.MPUserInfo.RealName);
            string keyword1_user = WebHelper.CutString(entity.Content, 10);
            string keyword2_user = string.Format("超过{0},自动结束", timeStr);
            string remark_user = "点击查看我的咨询";
            string link_url_user = WebSite.SiteUrl + "/MP/Advisory/AdvisoryList";
            var template_data_user = new
            {
                first = new TemplateDataItem(first_user),
                keyword1 = new TemplateDataItem(keyword1_user),
                keyword2 = new TemplateDataItem(keyword2_user),
                remark = new TemplateDataItem(remark_user)
            };
            var result_user = TemplateApi.SendTemplateMessage(WebSite.WeChatAppID, entity.MPUserInfo.OpenID, templateId_user, link_url_user, template_data_user);
            if (result_user.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("在线咨询-设置超时自动结束-给用户发消息出错：{0},订单号：{1}", result.errmsg, id.ToString()));
            }
        }

        /// <summary>
        /// 在线咨询设置结束-给用户和医生发送消息--医生手动关闭咨询
        /// </summary>
        /// <param name="order_num"></param>
        /// <returns></returns>
        public static void SendDoctorsAndUserAdvisoryIsDone(int id)
        {
            var entity = BLL.BLLConsultation.GetModel(id);
            if (entity == null) { System.Diagnostics.Trace.WriteLine("在线咨询-医生手动关闭咨询-给医生发消息出错：咨询不存在" + id.ToString()); return; }
            if (entity.Status != Entity.ConsultationStatus.已完成) { System.Diagnostics.Trace.WriteLine("在线咨询-医生手动关闭咨询-给医生发消息出错：订单号不是已完成状态" + id.ToString()); return; }
            if (entity.MPDoctorInfo == null) { System.Diagnostics.Trace.WriteLine("在线咨询-医生手动关闭咨询-给医生发消息出错：医生不存在" + id.ToString()); return; }
            if (entity.MPUserInfo == null) { System.Diagnostics.Trace.WriteLine("在线咨询-医生手动关闭咨询-给医生发消息出错：用户不存在" + id.ToString()); return; }
            if (entity.ConsultationDisease == null) { System.Diagnostics.Trace.WriteLine("在线咨询-医生手动关闭咨询-给医生发消息出错：咨询类型为空" + id.ToString()); return; }

            //给医生发
            string first = string.Format("{0},您好，患者咨询已结束。", entity.MPDoctorInfo.RealName);
            string keyword1 = string.Format("您提前关闭了该咨询");
            string keyword2 = entity.MPUserInfo.RealName;
            string keyword3 = WebHelper.CutString(entity.Content, 10);
            string keyword4 = TypeHelper.ObjectToDateTime(entity.PayTime, DateTime.Now).ToString("yyyy年MM月dd日 HH点mm分");
            string remark = "该咨询已可以进行结算，点击前往结算";
            string link_url = WebSite.SiteUrl + "/MP/Doctors/AdvisoryClear";
            var templateId = "UJYpt7rtGd6AodLWeZYQ4bwvBpXbSjTqWjc3xHgLOBo";
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
                System.Diagnostics.Trace.WriteLine(string.Format("在线咨询-医生手动关闭咨询-给医生发消息出错：{0},咨询ID：{1}", result.errmsg, id.ToString()));
            }
            //给用户发消息
            string templateId_user = "fmqOskT9csvy9ez1JmBC4NsrcTFUl3XUPMqgrnDBx94";
            string first_user = string.Format("{0},您好，您的咨询已结束。", entity.MPUserInfo.RealName);
            string keyword1_user = WebHelper.CutString(entity.Content, 10);
            string keyword2_user = string.Format("{0}医生手动关闭了该咨询", entity.MPDoctorInfo.RealName);
            string remark_user = "点击查看我的咨询";
            string link_url_user = WebSite.SiteUrl + "/MP/Advisory/AdvisoryList";
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
                System.Diagnostics.Trace.WriteLine(string.Format("在线咨询-医生手动关闭咨询-给用户发消息出错：{0},订单号：{1}", result.errmsg, id.ToString()));
            }
        }


        /// <summary>
        /// 在线咨询超时超时未回复，退款操作-给用户和医生发送消息
        /// </summary>
        /// <param name="order_num"></param>
        /// <returns></returns>
        public static void SendDoctorsAndUserAdvisoryIsRefund(int id, string timeStr)
        {
            var entity = BLL.BLLConsultation.GetModel(id);
            if (entity == null) { System.Diagnostics.Trace.WriteLine("在线咨询-设置超时未回复退款-给医生发消息出错：咨询不存在" + id.ToString()); return; }
            if (entity.Status != Entity.ConsultationStatus.已完成) { System.Diagnostics.Trace.WriteLine("在线咨询-设置超时未回复退款-给医生发消息出错：订单号不是已完成状态" + id.ToString()); return; }
            if (entity.MPDoctorInfo == null) { System.Diagnostics.Trace.WriteLine("在线咨询-设置超时未回复退款-给医生发消息出错：医生不存在" + id.ToString()); return; }
            if (entity.MPUserInfo == null) { System.Diagnostics.Trace.WriteLine("在线咨询-设置超时未回复退款-给医生发消息出错：用户不存在" + id.ToString()); return; }
            if (entity.ConsultationDisease == null) { System.Diagnostics.Trace.WriteLine("在线咨询-设置超时未回复退款-给医生发消息出错：咨询类型为空" + id.ToString()); return; }

            //给医生发
            string first = string.Format("{0},您好，患者咨询已结束。", entity.MPDoctorInfo.RealName);
            string keyword1 = string.Format("超过{0}未回复,自动结束", timeStr);
            string keyword2 = entity.MPUserInfo.RealName;
            string keyword3 = WebHelper.CutString(entity.Content, 10);
            string keyword4 = TypeHelper.ObjectToDateTime(entity.PayTime, DateTime.Now).ToString("yyyy年MM月dd日 HH点mm分");
            string remark = "后期咨询请注意及时回复。";
            string link_url = WebSite.SiteUrl + "/MP/Doctors/Advisory";
            var templateId = "UJYpt7rtGd6AodLWeZYQ4bwvBpXbSjTqWjc3xHgLOBo";
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
                System.Diagnostics.Trace.WriteLine(string.Format("在线咨询-设置超时未回复退款-给医生发消息出错：{0},咨询ID：{1}", result.errmsg, id.ToString()));
            }
            //给用户发消息
            string templateId_user = "fmqOskT9csvy9ez1JmBC4NsrcTFUl3XUPMqgrnDBx94";
            string first_user = string.Format("{0},您好，您的咨询已结束。", entity.MPUserInfo.RealName);
            string keyword1_user = WebHelper.CutString(entity.Content, 10);
            string keyword2_user = string.Format("医生超过{0}未回复,自动结束", timeStr);
            string remark_user = "您的咨询费用已返回到您的支付账户，请及时查看。";
            string link_url_user = WebSite.SiteUrl + "/MP/Advisory/AdvisoryList";
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
                System.Diagnostics.Trace.WriteLine(string.Format("在线咨询-设置超时未回复退款-给用户发消息出错：{0},订单号：{1}", result.errmsg, id.ToString()));
            }
        }


        /// <summary>
        /// 咨询回复提醒
        /// </summary>
        public static void SendReplyNotify(Entity.ReplayUserType type,int id,string content)
        {
            var entity = BLL.BLLConsultation.GetModel(id);
            if (entity == null) { System.Diagnostics.Trace.WriteLine("在线咨询-咨询回复提醒：咨询不存在" + id.ToString()); return; }
            if (entity.MPDoctorInfo == null) { System.Diagnostics.Trace.WriteLine("在线咨询-咨询回复提醒：医生不存在" + id.ToString()); return; }
            if (entity.MPUserInfo == null) { System.Diagnostics.Trace.WriteLine("在线咨询-咨询回复提醒：用户不存在" + id.ToString()); return; }
            
            if (type == Entity.ReplayUserType.User)
            {
                //如果是用户发的-则给医生发消息
                string first = string.Format("{0},您好，患者回复了您。", entity.MPDoctorInfo.RealName);
                string keyword1 = string.Format(""+ WebHelper.CutString(content, 10));
                string keyword2 = DateTime.Now.ToString("yyyy年MM月dd日 HH点mm分");
                string remark = "点击查看详情。";
                string link_url = WebSite.SiteUrl + "/MP/Doctors/AdvisoryInfo?id=" + id;
                var templateId = "_TfWjSUpBVZiLgxQOA3-dyniZ0w3xjROb2LPj-S1Oq4";
                var template_data = new
                {
                    first = new TemplateDataItem(first),
                    keyword1 = new TemplateDataItem(keyword1),
                    keyword2 = new TemplateDataItem(keyword2),
                    remark = new TemplateDataItem(remark)
                };
                var result = TemplateApi.SendTemplateMessage(WebSite.WeChatAppID, entity.MPDoctorInfo.OpenID, templateId, link_url, template_data);
                if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("在线咨询-咨询回复提醒-给医生发消息出错：{0},咨询ID：{1}", result.errmsg, id.ToString()));
                }
            }
            else {
                //如果是医生发的，则给用户发消息
                string first = string.Format("{0}，您好，医生回复了您的咨询。", entity.MPUserInfo.RealName);
                string keyword1 = WebHelper.CutString(entity.Content, 10);
                string keyword2 = WebHelper.CutString(content, 10);//DateTime.Now.ToString("yyyy年MM月dd日 HH点mm分");
                string keyword3 = entity.MPDoctorInfo.RealName;
                string remark = "点击查看详情。";
                string link_url = WebSite.SiteUrl + "/MP/Advisory/AdvisoryInfo?id=" + id;
                var templateId = "ph3gsfikUr1HVn-bL_yHpmeZ5TOwjl85T_KlZX7WkYI";
                var template_data = new
                {
                    first = new TemplateDataItem(first),
                    keyword1 = new TemplateDataItem(keyword1),
                    keyword2 = new TemplateDataItem(keyword2),
                    keyword3 = new TemplateDataItem(keyword3),
                    remark = new TemplateDataItem(remark)
                };
                var result = TemplateApi.SendTemplateMessage(WebSite.WeChatAppID, entity.MPUserInfo.OpenID, templateId, link_url, template_data);
                if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("在线咨询-咨询回复提醒-给用户发消息出错：{0},咨询ID：{1}", result.errmsg, id.ToString()));
                }
            }
        }

    }
}