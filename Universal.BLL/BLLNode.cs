using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    /// <summary>
    /// 节点
    /// </summary>
    public class BLLNode
    {
        /// <summary>
        /// 添加节点数据
        /// </summary>
        /// <returns></returns>
        public static bool Add(Entity.Node model)
        {
            if (model == null)
                return false;
            using (var db = new DataCore.EFDBContext())
            {
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
        public static bool Modify(Entity.Node model)
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
