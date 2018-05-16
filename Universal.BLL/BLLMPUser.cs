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
    /// 微信用户操作
    /// </summary>
    public class BLLMPUser
    {

        /// <summary>
        /// 获取账户余额
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static decimal GetAccountBalance(int id)
        {
            using (var db=new DataCore.EFDBContext())
            {
                var entity = db.MPUsers.Where(p => p.ID == id).AsNoTracking().FirstOrDefault();
                if (entity != null) return entity.AccountBalance;
                else return 0M;
            }
        }

        /// <summary>
        /// 根据openid获取用户实体,如果用户不存在，则添加一个用户信息
        /// </summary>
        /// <param name="open_id"></param>
        /// <returns></returns>
        public static Entity.MPUser GetUserInfoOrAdd(string open_id)
        {
            if (string.IsNullOrWhiteSpace(open_id)) return null;
            using (var db=new DataCore.EFDBContext())
            {
                if(!db.MPUsers.Any(p=>p.OpenID == open_id))
                {
                    //添加用户
                    Entity.MPUser entity_add = new Entity.MPUser();
                    entity_add.OpenID = open_id;
                    db.MPUsers.Add(entity_add);
                    db.SaveChanges();
                }
                return db.MPUsers.Where(p => p.OpenID == open_id).Include(p => p.DoctorsInfo).AsNoTracking().FirstOrDefault();                
            }
        }

        /// <summary>
        /// 添加一个用户(不存在才添加)，带有详细信息
        /// </summary>
        /// <param name="open_id"></param>
        /// <param name="nick_name"></param>
        /// <param name="avatar"></param>
        /// <param name="sex"></param>
        /// <returns></returns>
        public static Entity.MPUser AddUserInfo(string open_id,string nick_name,string avatar,int sex)
        {
            if (string.IsNullOrWhiteSpace(open_id)) return null;
            using (var db=new DataCore.EFDBContext())
            {
                var entity_user = db.MPUsers.Where(p => p.OpenID == open_id).FirstOrDefault();
                if (entity_user != null) return entity_user;
                entity_user = new Entity.MPUser();
                entity_user.OpenID = open_id;
                entity_user.NickName = nick_name;
                entity_user.Avatar = avatar;
                entity_user.Gender = (Entity.MPUserGenderType)sex;
                db.MPUsers.Add(entity_user);
                db.SaveChanges();
                return entity_user;
            }
        }

        
        /// <summary>
        /// 判断用户是否完善了资料
        /// </summary>
        /// <param name="open_id"></param>
        /// <returns></returns>
        public static bool IsFullInfo(string open_id)
        {
            if (string.IsNullOrWhiteSpace(open_id)) return false;
            using (var db=new DataCore.EFDBContext())
            {
                var entity = db.MPUsers.Where(p => p.OpenID == open_id).AsNoTracking().FirstOrDefault();
                if (entity == null) return false;
                return entity.IsFullInfo;
            }
        }


        /// <summary>
        /// 使用账户余额支付体检订单
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="order_num"></param>
        /// <param name="mad_id">返回账户资金变动记录ID</param>
        /// <returns></returns>
        public static bool PayMedicalUseBalance(int user_id,string order_num,out int mad_id)
        {
            mad_id = 0;
            using (var db=new DataCore.EFDBContext())
            {
                var entity_user = db.MPUsers.Find(user_id);
                if (entity_user == null) return false;
                if (!entity_user.Status) return false;
                var entity_order = db.OrderMedicals.Where(p => p.OrderNum == order_num).FirstOrDefault();
                if (entity_order == null) return false;
                if (entity_order.Status != Entity.OrderStatus.等待支付) return false;

                if (entity_user.AccountBalance < entity_order.RelAmount) return false;

                entity_user.AccountBalance = entity_user.AccountBalance - entity_order.RelAmount;
                entity_order.Status = Entity.OrderStatus.已支付;
                entity_order.PayTime = DateTime.Now;
                entity_order.PayType = Entity.OrderPayType.账户余额;

                var entity_detail = new Entity.MPUserAmountDetails();
                entity_detail.Amount = entity_order.RelAmount;
                entity_detail.MPUserID = user_id;
                entity_detail.Title = "支付体检订单";
                entity_detail.Type = Entity.MPUserAmountDetailsType.Less;
                db.MPUserAmountDetails.Add(entity_detail);
                try
                {
                    db.SaveChanges();
                    mad_id = entity_detail.ID;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("使用账户余额支付体检订单出错：" + ex.Message);
                    return false;
                }

            }
            return true;
        }

        /// <summary>
        /// 批量禁用用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool DisEnbleUser(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids)) return false;
            using (var db=new DataCore.EFDBContext())
            {
                string strSql = "update MPUser set Status=0 where id in(" + ids + ")";
                db.Database.ExecuteSqlCommand(strSql);
                return true;
            }
        }

    }
}
