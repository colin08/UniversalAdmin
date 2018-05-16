using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;

namespace Universal.Web.Areas.MP.Controllers
{
    /// <summary>
    /// 支付入口
    /// </summary>
    public class PayController : BaseMPController
    {

        #region 测试代码
        //    /// <summary>
        //    /// 测试支付
        //    /// </summary>
        //    /// <returns></returns>
        //    public ActionResult Test()
        //    {
        //        string sp_billno = string.Format("{0}{1}{2}", WorkContext.WebSite.WeChatPayMchid/*10位*/, DateTime.Now.ToString("yyyyMMddHHmmss"),
        //                    TenPayV3Util.BuildRandomStr(6));
        //        var timeStamp = TenPayV3Util.GetTimestamp();
        //        var nonceStr = TenPayV3Util.GetNoncestr();
        //        var body = "测试-商品名称";
        //        var price = 1;
        //        string notify_url = WorkContext.WebSite.SiteUrl + "/PayNotify/WeChatPay";
        //        string attach = ((int)MPHelper.PayAttach.账户充值).ToString();//额外自定义参数
        //        var xmlDataInfo = new TenPayV3UnifiedorderRequestData(WorkContext.WebSite.WeChatAppID, WorkContext.WebSite.WeChatPayMchid, body, sp_billno, price, Request.UserHostAddress, notify_url, TenPayV3Type.JSAPI, WorkContext.open_id, WorkContext.WebSite.WeChatPayPayKey, nonceStr, null, null, null, null, attach);
        //        var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口
        //        var package = string.Format("prepay_id={0}", result.prepay_id);
        //        ViewData["product"] = "这里是商品名称";
        //        ViewData["price"] = price;
        //        ViewData["appId"] = WorkContext.WebSite.WeChatAppID;
        //        ViewData["timeStamp"] = timeStamp;
        //        ViewData["nonceStr"] = nonceStr;
        //        ViewData["package"] = package;
        //        ViewData["paySign"] = TenPayV3.GetJsPaySign(WorkContext.WebSite.WeChatAppID, timeStamp, nonceStr, package, WorkContext.WebSite.WeChatPayPayKey);
        //        //订单号放到Session里
        //        Session["TEMPORDERNUM"] = sp_billno;

        //        return View();
        //    }

        //    /// <summary>
        //    /// 支付成功
        //    /// </summary>
        //    /// <returns></returns>
        //    public ActionResult TestOK()
        //    {
        //        string order_num = Session["TEMPORDERNUM"] as string;

        //        return View();
        //    } 
        #endregion


        /// <summary>
        /// 体检套餐支付
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderMedical(string o, string back)
        {
            if (string.IsNullOrWhiteSpace(o)) return PromptView("/MP/Medical/Index", "非法参数");
            BLL.BaseBLL<Entity.OrderMedical> bll_order = new BLL.BaseBLL<Entity.OrderMedical>();
            var entity_order = bll_order.GetModel(p => p.OrderNum == o, "ID DESC");
            if (entity_order == null) return PromptView("/MP/Medical/Index", "订单不存在");
            if (entity_order.MPUserID != WorkContext.UserInfo.ID) return PromptView("/MP/Medical/Index", "该订单不属于您");
            if (entity_order.Status != Entity.OrderStatus.等待支付) return PromptView("/MP/Medical/Index", "该订单已不是待支付状态");
            string back_url = "";
            if (!string.IsNullOrWhiteSpace(back))
            {
                back_url = back;
            }
            else
            {
                back_url = "/MP/Medical/AddMInfo?mid=" + entity_order.MedicalID + "&o=" + o;
            }
            ViewData["BackUrl"] = back_url;
            ViewData["NextUrl"] = "MP/Pay/StatusOrderMedical";
            ViewData["OrderNum"] = o;

            var timeStamp = TenPayV3Util.GetTimestamp();
            var nonceStr = TenPayV3Util.GetNoncestr();
            var body = entity_order.Title;
            var price = (int)(entity_order.RelAmount * 100);
            string notify_url = WorkContext.WebSite.SiteUrl + "/PayNotify/WeChatPay";
            string attach = ((int)MPHelper.PayAttach.体检套餐).ToString();//额外自定义参数
            string ip = Request.UserHostAddress;
            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(WorkContext.WebSite.WeChatAppID,
                WorkContext.WebSite.WeChatPayMchid,
                body, o, price, ip, notify_url, TenPayV3Type.JSAPI, WorkContext.open_id, WorkContext.WebSite.WeChatPayPayKey, nonceStr,
                null, null, null, null, attach);
            //System.Diagnostics.Trace.WriteLine("APPID:" + WorkContext.WebSite.WeChatAppID);
            //System.Diagnostics.Trace.WriteLine("Mchid:" + WorkContext.WebSite.WeChatPayMchid);
            //System.Diagnostics.Trace.WriteLine("Body:" + body);
            //System.Diagnostics.Trace.WriteLine("OrderNum:" + o);
            //System.Diagnostics.Trace.WriteLine("Price:" + price);
            //System.Diagnostics.Trace.WriteLine("IP:" + ip);
            //System.Diagnostics.Trace.WriteLine("notify_url:" + notify_url);
            //System.Diagnostics.Trace.WriteLine("trade_type:" + TenPayV3Type.JSAPI);
            //System.Diagnostics.Trace.WriteLine("OPEN_ID:" + WorkContext.open_id);
            //System.Diagnostics.Trace.WriteLine("KEY:" + WorkContext.WebSite.WeChatPayPayKey);
            //System.Diagnostics.Trace.WriteLine("nonceStr:" + nonceStr);
            //System.Diagnostics.Trace.WriteLine("attach:" + attach);

            //System.Diagnostics.Trace.WriteLine("SIGN:" + xmlDataInfo.Sign);

            var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口
            //System.Diagnostics.Trace.WriteLine(result.return_code + "--" + result.return_msg);
            var package = string.Format("prepay_id={0}", result.prepay_id);
            ViewData["product"] = entity_order.Title;
            ViewData["price"] = entity_order.RelAmount.ToString("F2");
            ViewData["appId"] = WorkContext.WebSite.WeChatAppID;
            ViewData["timeStamp"] = timeStamp;
            ViewData["nonceStr"] = nonceStr;
            ViewData["package"] = package;
            ViewData["paySign"] = TenPayV3.GetJsPaySign(WorkContext.WebSite.WeChatAppID, timeStamp, nonceStr, package, WorkContext.WebSite.WeChatPayPayKey);
            //订单号放到Session里
            Session["TEMPORDERNUM"] = o;
            return View();
        }

        /// <summary>
        /// 检查支付订单是否支付成功
        /// </summary>
        /// <returns></returns>
        public ActionResult StatusOrderMedical()
        {
            string order_num = Session["TEMPORDERNUM"] as string;
            BLL.BaseBLL<Entity.OrderMedical> bll_order = new BLL.BaseBLL<Entity.OrderMedical>();
            var entity_order = bll_order.GetModel(p => p.OrderNum == order_num, "ID DESC");
            if (entity_order == null) return PromptView("/MP/Medical/Index", "订单不存在");
            if (entity_order.MPUserID != WorkContext.UserInfo.ID) return PromptView("/MP/Medical/Index", "该订单不属于您");
            ViewData["Price"] = entity_order.RelAmount.ToString("F2");
            if (entity_order.Status == Entity.OrderStatus.已支付)
            {
                ViewData["Status"] = 1;
            }
            else
                ViewData["Status"] = 0;
            return View();
        }


        /// <summary>
        /// 账户充值
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public ActionResult Recharge(decimal amount)
        {
            if (amount <=0) return PromptView("/MP/BasicUser/Recharge", "请输入正确的充值金额");
            string sp_billno = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"),TenPayV3Util.BuildRandomStr(6));
            string msg = "";
            var add_status = BLL.BLLMPUserAmountOrder.AddOrder(sp_billno, amount, WorkContext.UserInfo.ID, "账户充值", out msg);
            if(!add_status) return PromptView("/MP/BasicUser/Recharge", "创建支付订单失败");

            ViewData["NextUrl"] = "/MP/Pay/StatusRecharge";
            ViewData["OrderNum"] = sp_billno;

            var timeStamp = TenPayV3Util.GetTimestamp();
            var nonceStr = TenPayV3Util.GetNoncestr();
            var body = "账户充值";
            var price = (int)(amount * 100);
            string notify_url = WorkContext.WebSite.SiteUrl + "/PayNotify/WeChatPay";
            string attach = ((int)MPHelper.PayAttach.账户充值).ToString();//额外自定义参数
            string ip = Request.UserHostAddress;
            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(WorkContext.WebSite.WeChatAppID,
                WorkContext.WebSite.WeChatPayMchid,
                body, sp_billno, price, ip, notify_url, TenPayV3Type.JSAPI, WorkContext.open_id, WorkContext.WebSite.WeChatPayPayKey, nonceStr,
                null, null, null, null, attach);
            var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口
            var package = string.Format("prepay_id={0}", result.prepay_id);
            ViewData["product"] = body;
            ViewData["price"] = amount.ToString("F2");
            ViewData["appId"] = WorkContext.WebSite.WeChatAppID;
            ViewData["timeStamp"] = timeStamp;
            ViewData["nonceStr"] = nonceStr;
            ViewData["package"] = package;
            ViewData["paySign"] = TenPayV3.GetJsPaySign(WorkContext.WebSite.WeChatAppID, timeStamp, nonceStr, package, WorkContext.WebSite.WeChatPayPayKey);
            //订单号放到Session里
            Session["TEMPORDERNUMRecharge"] = sp_billno;
            return View();
        }

        /// <summary>
        /// 检查支付订单是否支付成功
        /// </summary>
        /// <returns></returns>
        public ActionResult StatusRecharge()
        {
            string order_num = Session["TEMPORDERNUMRecharge"] as string;
            BLL.BaseBLL<Entity.MPUserAmountOrder> bll_order = new BLL.BaseBLL<Entity.MPUserAmountOrder>();
            var entity_order = bll_order.GetModel(p => p.OrderNum == order_num, "ID DESC");
            if (entity_order == null) return PromptView("/MP/BasicUser/Recharge", "订单不存在");
            if (entity_order.MPUserID != WorkContext.UserInfo.ID) return PromptView("/MP/BasicUser/Recharge", "该订单不属于您");
            ViewData["Price"] = entity_order.Amount.ToString("F2");
            if (entity_order.Status)
            {
                ViewData["Status"] = 1;
            }
            else
                ViewData["Status"] = 0;
            return View();
        }


    }
}