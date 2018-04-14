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

        /// <summary>
        /// 测试支付
        /// </summary>
        /// <returns></returns>
        public ActionResult Test()
        {
            string sp_billno = string.Format("{0}{1}{2}", WorkContext.WebSite.WeChatPayMchid/*10位*/, DateTime.Now.ToString("yyyyMMddHHmmss"),
                        TenPayV3Util.BuildRandomStr(6));
            var timeStamp = TenPayV3Util.GetTimestamp();
            var nonceStr = TenPayV3Util.GetNoncestr();
            var body = "测试-商品名称";
            var price = 1;
            string notify_url = WorkContext.WebSite.SiteUrl + "/PayNotify/WeChatPay";
            string attach = ((int)MPHelper.PayAttach.账户充值).ToString();//额外自定义参数
            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(WorkContext.WebSite.WeChatPayAppID, WorkContext.WebSite.WeChatPayMchid, body, sp_billno, price, Request.UserHostAddress, notify_url, TenPayV3Type.JSAPI, WorkContext.open_id, WorkContext.WebSite.WeChatPayPayKey, nonceStr, null, null, null, null, attach);
            var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口
            var package = string.Format("prepay_id={0}", result.prepay_id);
            ViewData["product"] = "这里是商品名称";
            ViewData["price"] = price;
            ViewData["appId"] = WorkContext.WebSite.WeChatPayAppID;
            ViewData["timeStamp"] = timeStamp;
            ViewData["nonceStr"] = nonceStr;
            ViewData["package"] = package;
            ViewData["paySign"] = TenPayV3.GetJsPaySign(WorkContext.WebSite.WeChatPayAppID, timeStamp, nonceStr, package, WorkContext.WebSite.WeChatPayPayKey);
            //订单号放到Session里
            Session["TEMPORDERNUM"] = sp_billno;

            return View();
        }

        /// <summary>
        /// 支付成功
        /// </summary>
        /// <returns></returns>
        public ActionResult TestOK()
        {
            string order_num = Session["TEMPORDERNUM"] as string;

            return View();
        }


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
            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(WorkContext.WebSite.WeChatPayAppID, WorkContext.WebSite.WeChatPayMchid, body, o, price, Request.UserHostAddress, notify_url, TenPayV3Type.JSAPI, WorkContext.open_id, WorkContext.WebSite.WeChatPayPayKey, nonceStr, null, null, null, null, attach);
            var result = TenPayV3.Unifiedorder(xmlDataInfo);//调用统一订单接口
            var package = string.Format("prepay_id={0}", result.prepay_id);
            ViewData["product"] = entity_order.Title;
            ViewData["price"] = entity_order.RelAmount.ToString("F2");
            ViewData["appId"] = WorkContext.WebSite.WeChatPayAppID;
            ViewData["timeStamp"] = timeStamp;
            ViewData["nonceStr"] = nonceStr;
            ViewData["package"] = package;
            ViewData["paySign"] = TenPayV3.GetJsPaySign(WorkContext.WebSite.WeChatPayAppID, timeStamp, nonceStr, package, WorkContext.WebSite.WeChatPayPayKey);
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
    }
}