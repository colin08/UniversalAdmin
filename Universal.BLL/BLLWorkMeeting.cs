using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    /// <summary>
    /// 会议召集
    /// </summary>
    public class BLLWorkMeeting
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static int Add(Entity.WorkMeeting entity,string ids)
        {
            var db = new DataCore.EFDBContext();
            if(!string.IsNullOrWhiteSpace(ids))
            {
                List<Entity.WorkMeetingUser> users = new List<Entity.WorkMeetingUser>();
                foreach (var item in ids.Split(','))
                {
                    int id = Tools.TypeHelper.ObjectToInt(item);
                    var entity_user = db.CusUsers.Find(id);
                    if(entity_user != null)
                    {
                        var model = new Entity.WorkMeetingUser();
                        model.CusUserID = id;
                        model.IsConfirm = false;
                        users.Add(model);
                    }
                }
                entity.WorkMeetingUsers = users;
            }
            db.WorkMeetings.Add(entity);
            db.SaveChanges();
            db.Dispose();
            return entity.ID;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool Modify(Entity.WorkMeeting entity, string ids)
        {
            var db = new DataCore.EFDBContext();
            db.WorkMeetingUsers.Where(p => p.WorkMeetingID == entity.ID).ToList().ForEach(p => db.WorkMeetingUsers.Remove(p));
            if (!string.IsNullOrWhiteSpace(ids))
            {
                foreach (var item in ids.Split(','))
                {
                    int id = Tools.TypeHelper.ObjectToInt(item);
                    var entity_user = db.CusUsers.Find(id);
                    if (entity_user != null)
                    {
                        var model = new Entity.WorkMeetingUser();
                        model.CusUserID = id;
                        model.IsConfirm = false;
                        model.WorkMeetingID = entity.ID;
                        db.WorkMeetingUsers.Add(model);
                    }
                }
            }
            var ss = db.Entry<Entity.WorkMeeting>(entity);
            ss.State = System.Data.Entity.EntityState.Modified;
            int row = db.SaveChanges();
            db.Dispose();
            return row > 0;
        }
        
    }
}
