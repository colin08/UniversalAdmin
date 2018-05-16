using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Universal.Tools;

namespace Universal.Web.MPHelper
{
    /// <summary>
    /// 支付类别-回调时对应不同订单进行支付
    /// </summary>
    public enum PayAttach
    {
        /// <summary>
        /// 
        /// </summary>
        账户充值 =1,
        /// <summary>
        /// 
        /// </summary>
        咨询支付=2,
        /// <summary>
        /// 
        /// </summary>
        体检套餐
    }

    /// <summary>
    /// 支付工具
    /// </summary>
    public class WXPay
    {
        /// <summary>
        /// 站点配置文件
        /// </summary>
        private static WebSiteModel WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="order_num">自己生成的订单号</param>
        /// <param name="total_fee">订单总金额</param>
        /// <param name="refund_fee">要退款的金额</param>
        public static void Refund(string order_num, decimal total_fee, decimal refund_fee)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            string outRefundNo = "OutRefunNo-" + DateTime.Now.Ticks;
            int totalFee = (int)(total_fee * 100);
            int refundFee = (int)(refund_fee * 100);
            var dataInfo = new TenPayV3RefundRequestData(WebSite.WeChatAppID, WebSite.WeChatPayMchid, WebSite.WeChatPayPayKey,
                null, nonceStr, null, order_num, outRefundNo, totalFee, refundFee, WebSite.WeChatPayMchid, null);
            //TODO 退款证书位置设置
            var cert = IOHelper.GetMapPath("/App_Data/wxPayCert/apiclient_cert.p12");// @"D:\cert\apiclient_cert_SenparcRobot.p12";//根据自己的证书位置修改
            var result = TenPayV3.Refund(dataInfo, cert, WebSite.WeChatPayMchid);
            System.Diagnostics.Trace.WriteLine(string.Format("订单：{0} 退款状态：{1},详细信息：{2}", order_num, result.result_code, result.err_code_des));
        }

        /// <summary>
        /// 企业支付给个人(用于向医生结算咨询款)
        /// </summary>
        /// <param name="order_num">本系统订单号</param>
        /// <param name="open_id"></param>
        /// <param name="amount">退款金额，单位：分</param>
        /// <param name="desc"></param>
        public static bool Transfers(string order_num,string open_id,decimal amount,string desc,out string msg)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            var data = new TenPayV3TransfersRequestData(WebSite.WeChatPayMchAppID,
                WebSite.WeChatPayMchid, null, nonceStr, 
                order_num, open_id, WebSite.WeChatPayPayKey, "NO_CHECK", "", amount, desc, "192.168.1.201");

            //TODO 企业付款给个人  设置证书位置和密码
            string cert_io = IOHelper.GetMapPath("/App_Data/wxPayCert/apiclient_cert.p12");
            var result = TenPayV3.Transfers(data, cert_io, WebSite.WeChatPayMchid);
            msg = result.return_msg;
            System.Diagnostics.Trace.WriteLine("企业支付给个人，返回结果："+result.ToJson());
            return result.return_code.Equals("SUCCESS");
        }

    }
}