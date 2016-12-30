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
            return row > 0;
        }
        
    }
}
