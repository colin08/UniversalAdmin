using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 节点
    /// </summary>
    public class BLLNode
    {
        /// <summary>
        /// 获取视图，包含incloud
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.Node GetMode(int id)
        {
            Entity.Node entity = null;
            using (var db =new DataCore.EFDBContext())
            {
                entity = db.Nodes.Include(p => p.NodeUsers.Select(s=>s.CusUser)).Include(p => p.NodeFiles).Where(p => p.ID == id).FirstOrDefault();
            }

            return entity;
        }
 
        /// <summary>
        /// 添加节点数据
        /// </summary>
        /// <returns></returns>
        public static bool Add(Entity.Node model, string ids)
        {
            if (model == null)
                return false;
            using (var db = new DataCore.EFDBContext())
            {
                if (!string.IsNullOrWhiteSpace(ids))
                {
                    List<Entity.NodeUser> users = new List<Entity.NodeUser>();
                    foreach (var item in ids.Split(','))
                    {
                        int id = Tools.TypeHelper.ObjectToInt(item);
                        var entity_user = db.CusUsers.Find(id);
                        if (entity_user != null)
                        {
                            var user = new Entity.NodeUser();
                            user.CusUserID = id;
                            users.Add(user);
                        }
                    }
                    model.NodeUsers = users;
                }

                db.Nodes.Add(model);
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 修改节点数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Modify(Entity.Node model, string ids)
        {
            if(model == null)
                return false;
            if (model.ID <= 0)
                return false;

            var db = new DataCore.EFDBContext();
            if (!db.Nodes.Any(p => p.ID == model.ID))
                return false;
            //删除之前的附件
            db.NodeFiles.Where(p => p.NodeID == model.ID).ToList().ForEach(p => db.NodeFiles.Remove(p));
            //删除之前的联系人员
            db.NodeUsers.Where(p => p.NodeID == model.ID).ToList().ForEach(p => db.NodeUsers.Remove(p));

            if (!string.IsNullOrWhiteSpace(ids))
            {
                foreach (var item in ids.Split(','))
                {
                    int id = Tools.TypeHelper.ObjectToInt(item);
                    var entity_user = db.CusUsers.Find(id);
                    if (entity_user != null)
                    {
                        var user = new Entity.NodeUser();
                        user.CusUserID = id;
                        user.NodeID = model.ID;
                        db.NodeUsers.Add(user);
                    }
                }
            }

            //附件到DbContext上下文
            var entity = db.Entry<Entity.Node>(model);
            //标识状态为修改
            entity.State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return true;

        }
        
    }
}
