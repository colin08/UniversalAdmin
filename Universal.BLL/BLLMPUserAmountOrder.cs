using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 用户充值订单
    /// </summary>
    public class BLLMPUserAmountOrder
    {
        /// <summary>
        /// 添加一个订单
        /// </summary>
        /// <param name="order_num"></param>
        /// <param name="amount"></param>
        /// <param name="user_id"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static bool AddOrder(string order_num, decimal amount, int user_id, string desc, out string msg)
        {
            msg = "ok";
            if (string.IsNullOrWhiteSpace(order_num)) { msg = "订单号不能为空"; return false; }
            if (amount <= 0) { msg = "充值金额非法"; return false; }
            if (user_id <= 0) { msg = "非法用户"; return false; }
            using (var db = new DataCore.EFDBContext())
            {
                var entity_user = db.MPUsers.Where(p => p.ID == user_id).FirstOrDefault();
                if (entity_user == null) { msg = "用户不存在"; return false; }
                if (entity_user.Identity == Entity.MPUserIdentity.Doctors) { msg = "医生不能充值"; return false; }
                var entity = new Entity.MPUserAmountOrder(order_num, amount, user_id, desc);
                db.MPUserAmountOrders.Add(entity);
                db.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 设置订单支付成功
        /// </summary>
        /// <param name="order_num"></param>
        /// <param name="open_id"></param>
        /// <param name="wx_order">微信订单号</param>
        /// <param name="vip_amount">达到这个金额即升为VIP</param>
        /// <returns></returns>
        public static bool SetPayOK(string order_num, string wx_order, string open_id, decimal vip_amount,out int mad_id)
        {
            mad_id = 0;
            using (var db = new DataCore.EFDBContext())
            {
                var entity_order = db.MPUserAmountOrders.Where(p => p.OrderNum == order_num).FirstOrDefault();
                if (entity_order == null)
                {
                    System.Diagnostics.Trace.WriteLine("用户充值订单置为已支付出错：订单号" + order_num + "不存在");
                    return false;
                }
                if (entity_order.Status)
                {
                    //System.Diagnostics.Trace.WriteLine("用户充值订单置为已支付出错：订单号" + order_num + "已经是支付状态了");
                    return false;
                }
                entity_order.Status = true;
                entity_order.PayTime = DateTime.Now;
                entity_order.OpenID = open_id;
                entity_order.OrderNumWX = wx_order;
                entity_order.OpenID = open_id;

                var entity_user = db.MPUsers.Where(p => p.ID == entity_order.MPUserID).FirstOrDefault();
                //TODO 符合条件-升级为VIP 是不是应该发个通知
                if (entity_user.Identity == Entity.MPUserIdentity.Normal && entity_order.Amount >= vip_amount) entity_user.Identity = Entity.MPUserIdentity.VIP;
                //修改用户账户余额
                entity_user.AccountBalance = entity_user.AccountBalance + entity_order.Amount;

                //增加用户资金变动表
                var entity_details = new Entity.MPUserAmountDetails();
                entity_details.Amount = entity_order.Amount;
                entity_details.Title = "微信充值";
                entity_details.MPUserID = entity_order.MPUserID;
                entity_details.Type = Entity.MPUserAmountDetailsType.Add;
                db.MPUserAmountDetails.Add(entity_details);

                try
                {
                    db.SaveChanges();
                    mad_id = entity_details.ID;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("用户充值订单置为已支付出错：" + ex.Message);
                    return false;
                }
            }
            return true;
        }

    }
}
