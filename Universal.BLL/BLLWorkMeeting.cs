using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 会议召集
    /// </summary>
    public class BLLWorkMeeting
    {
        public static Entity.WorkMeeting GetModel(int id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                return db.WorkMeetings.AsNoTracking().Include(p => p.FileList).Include(p => p.WorkMeetingUsers.Select(s => s.CusUser)).Include(p=>p.CusUser).Where(p => p.ID == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// 用户点击确认参会
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user_id"></param>
        public static void Join(int id,int user_id)
        {
            using (var db =new DataCore.EFDBContext())
            {
                var entity_mee = db.WorkMeetings.Where(p => p.ID == id).AsNoTracking().FirstOrDefault();
                if (entity_mee == null)
                    return;
                var entity_user = db.CusUsers.Where(p => p.ID == user_id).AsNoTracking().FirstOrDefault();
                if (entity_user == null)
                    return;
                var entity = db.WorkMeetingUsers.Where(p => p.WorkMeetingID == id && p.CusUserID == user_id).FirstOrDefault();
                if(entity != null)
                {
                    entity.IsConfirm = true;
                    db.SaveChanges();
                    //向发起会议者发送通知
                    BLLMsg.PushMsg(entity_mee.CusUserID, Entity.CusUserMessageType.confrimjoinmeeting, string.Format(BLLMsgTemplate.ConfrimJoinMeeting, entity_user.NickName, entity_mee.Title), id);
                }
            }
        }
        
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static int Add(Entity.WorkMeeting entity,string ids)
        {
            var db = new DataCore.EFDBContext();
            var entity_cus_user = db.CusUsers.Find(entity.CusUserID);
            if (entity_cus_user == null)
                return 0;
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
            BLL.BLLMsg.PushSomeUser(ids, Entity.CusUserMessageType.waitmeeting, string.Format(BLLMsgTemplate.WaitMeeting, entity.Title), entity.ID);
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
            var entity_cus_user = db.CusUsers.Find(entity.CusUserID);
            if (entity_cus_user == null)
                return false;
            var old_entity = db.WorkMeetings.Where(p => p.ID == entity.ID).AsNoTracking().FirstOrDefault();
            db.WorkMeetingUsers.Where(p => p.WorkMeetingID == entity.ID).ToList().ForEach(p => db.WorkMeetingUsers.Remove(p));
            db.WorkMeetingFiles.Where(p => p.WorkMeetingID == entity.ID).ToList().ForEach(p => db.WorkMeetingFiles.Remove(p));
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

            List<Entity.WorkMeetingFile> file_list = entity.FileList.ToList();
            foreach (var item in file_list)
            {
                item.WorkMeetingID = entity.ID;
                db.WorkMeetingFiles.Add(item);
            }
            entity.FileList.Clear();

            var ss = db.Entry<Entity.WorkMeeting>(entity);
            ss.State = System.Data.Entity.EntityState.Modified;
            int row = db.SaveChanges();
            db.Dispose();
            if (old_entity.TimeTxt != entity.TimeTxt)
            {
                //会议改期
                BLL.BLLMsg.PushSomeUser(ids, Entity.CusUserMessageType.waitmeeting, string.Format(BLLMsgTemplate.MeetingChangeDate, entity_cus_user.NickName, entity.Title, old_entity.TimeTxt, entity.TimeTxt), entity.ID);
            }
            return row > 0;
        }
        
    }
}
