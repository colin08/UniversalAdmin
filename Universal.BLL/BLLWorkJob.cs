using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    /// <summary>
    /// 任务指派
    /// </summary>
    public class BLLWorkJob
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static int Add(Entity.WorkJob entity,string ids)
        {
            var db = new DataCore.EFDBContext();
            if(!string.IsNullOrWhiteSpace(ids))
            {
                List<Entity.WorkJobUser> users = new List<Entity.WorkJobUser>();
                foreach (var item in ids.Split(','))
                {
                    int id = Tools.TypeHelper.ObjectToInt(item);
                    var entity_user = db.CusUsers.Find(id);
                    if(entity_user != null)
                    {
                        var model = new Entity.WorkJobUser();
                        model.CusUserID = id;
                        model.IsConfirm = false;
                        users.Add(model);
                    }
                }
                entity.WorkJobUsers = users;
            }
            db.WorkJobs.Add(entity);
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
        public static bool Modify(Entity.WorkJob entity, string ids)
        {
            var db = new DataCore.EFDBContext();
            db.WorkJobUsers.Where(p => p.WorkJobID == entity.ID).ToList().ForEach(p => db.WorkJobUsers.Remove(p));
            if (!string.IsNullOrWhiteSpace(ids))
            {
                foreach (var item in ids.Split(','))
                {
                    int id = Tools.TypeHelper.ObjectToInt(item);
                    var entity_user = db.CusUsers.Find(id);
                    if (entity_user != null)
                    {
                        var model = new Entity.WorkJobUser();
                        model.CusUserID = id;
                        model.IsConfirm = false;
                        model.WorkJobID = entity.ID;
                        db.WorkJobUsers.Add(model);
                    }
                }
            }
            var ss = db.Entry<Entity.WorkJob>(entity);
            ss.State = System.Data.Entity.EntityState.Modified;
            int row = db.SaveChanges();
            db.Dispose();
            return row > 0;
        }
        
    }
}
