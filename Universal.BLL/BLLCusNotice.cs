using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Universal.BLL
{
    /// <summary>
    /// 系统通知
    /// </summary>
    public class BLLCusNotice
    {
        /// <summary>
        /// 添加系统通知
        /// </summary>
        /// <returns></returns>
        public static bool Add(Entity.CusNotice entity, string user_id_str)
        {
            if (entity == null)
                return false;

            using (var db = new DataCore.EFDBContext())
            {
                db.CusNotices.Add(entity);
                if(!string.IsNullOrWhiteSpace(user_id_str))
                {
                    foreach (var item in user_id_str.Split(','))
                    {
                        int u_id = Tools.TypeHelper.ObjectToInt(item);
                        var notice_user = new Entity.CusNoticeUser();
                        notice_user.CusNotice = entity;
                        notice_user.CusUserID = u_id;
                        db.CusNoticeUsers.Add(notice_user);
                    }
                }

                db.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 修改系统通知
        /// </summary>
        /// <returns></returns>
        public static bool Edit(Entity.CusNotice entity, string user_id_str)
        {
            if (entity == null)
                return false;

            using (var db = new DataCore.EFDBContext())
            {
                DbEntityEntry entry = db.Entry<Entity.CusNotice>(entity);
                entry.State = EntityState.Modified;
                db.CusNoticeUsers.Where(p => p.CusNoticeID == entity.ID).ToList().ForEach(p => db.CusNoticeUsers.Remove(p));
                if(!string.IsNullOrWhiteSpace(user_id_str))
                {
                    foreach (var item in user_id_str.Split(','))
                    {
                        int u_id = Tools.TypeHelper.ObjectToInt(item);
                        var notice_user = new Entity.CusNoticeUser();
                        notice_user.CusNoticeID = entity.ID;
                        notice_user.CusUserID = u_id;
                        db.CusNoticeUsers.Add(notice_user);
                    }
                }

                db.SaveChanges();
            }
            return true;
        }
    }
}
