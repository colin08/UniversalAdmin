using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP.TenPayLibV3;
using Universal.Tools;
using System.Data.Entity;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 支付结果通知回调接口
    /// </summary>
    public class PayNotifyController : Controller
    {
        /// <summary>
        /// 站点配置文件
        /// </summary>
        public static WebSiteModel WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);
        
        /// <summary>
        /// 微信支付回调
        /// </summary>
        /// <returns></returns>
        public ActionResult WeChatPay()
        {
            ResponseHandler resHandler = new ResponseHandler(null);
            string return_code = resHandler.GetParameter("return_code");
            string return_msg = resHandler.GetParameter("return_msg");
            string res = null;
            resHandler.SetKey(WebSite.WeChatPayPayKey);
            if (resHandler.IsTenpaySign() && return_code.ToUpper() == "SUCCESS")
            {
                res = "success";

                string out_trade_no = resHandler.GetParameter("out_trade_no");
                string transaction_id = resHandler.GetParameter("transaction_id");

                //支付的用户open_id
                string open_id = resHandler.GetParameter("openid");

                //自定义数据包
                string attach = resHandler.GetParameter("attach");

                MPHelper.PayAttach type = (MPHelper.PayAttach)TypeHelper.ObjectToInt(attach);

                switch (type)
                {
                    case MPHelper.PayAttach.账户充值:
                        //对订单执行操作
                        ExcRecharge(out_trade_no, transaction_id, open_id);
                        break;
                    case MPHelper.PayAttach.咨询支付:
                        //对订单执行操作
                        break;
                    case MPHelper.PayAttach.体检套餐:
                        ExcMedicalOrder(out_trade_no, transaction_id, open_id);
                        break;
                    default:
                        break;
                }

            }
            else res = "wrong";
            string xml = string.Format(@"<xml>
   <return_code><![CDATA[{0}]]></return_code>
   <return_msg><![CDATA[{1}]]></return_msg>
</xml>", return_code, return_msg);
            return Content(xml, "text/xml");
        }


        #region 支付对应订单的处理

        /// <summary>
        /// 处理充值订单
        /// <param name="order_num">订单号</param>
        /// </summary>
        private void ExcRecharge(string order_num,string wx_order,string open_id)
        {
            BLL.BLLMPUserAmountOrder.SetPayOK(order_num, wx_order, open_id, WebSite.VIPAmount);
        }

        /// <summary>
        /// 处理体检套餐订单
        /// <param name="order_num">订单号</param>
        /// </summary>
        private void ExcMedicalOrder(string order_num,string wx_order,string open_id)
        {
            BLL.BaseBLL<Entity.OrderMedical> bll_order = new BLL.BaseBLL<Entity.OrderMedical>();
            var entity_order = bll_order.GetModel(p => p.OrderNum == order_num, "ID ASC");
            if (entity_order == null) return;
            if (entity_order.Status != Entity.OrderStatus.等待支付) return;
            entity_order.Status = Entity.OrderStatus.已支付;
            entity_order.PayTime = DateTime.Now;
            entity_order.OrderNumWX = wx_order;
            entity_order.OpenID = open_id;
            bll_order.Modify(entity_order, "Status", "PayTime", "OrderNumWX", "OpenID");
        }

        #endregion

    }
}