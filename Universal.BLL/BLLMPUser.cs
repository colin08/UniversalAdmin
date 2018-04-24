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
