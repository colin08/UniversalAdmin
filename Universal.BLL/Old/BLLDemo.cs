//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Universal.DataCore;
//using Universal.Entity;
//using EntityFramework.Extensions;

//namespace Universal.BLL
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class BLLDemo
//    {
//        /// <summary>
//        /// 上下文
//        /// </summary>
//        EFDBContext db = new EFDBContext();

//        /// <summary>
//        /// 得到一个实体，多个incloud
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public Entity.Demo GetModel(int id)
//        {
//            if (id <= 0)
//                return null;
            
//            return db.Demo.Where(p => p.ID == id).Include(p => p.LastUpdateUser).Include(p => p.AddUser).Include(p => p.Albums).Include(p => p.Depts).FirstOrDefault();
//        }

//        /// <summary>
//        /// 修改
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        public bool Modify(Demo entity)
//        {
//            if (entity == null)
//                return false;
//            if (entity.ID == 0)
//                return false;

//            //删除旧数据
//            //db.DemoAlbums.Where(p => p.DemoID == entity.ID).ToList().ForEach(p => db.Entry(p).State = EntityState.Deleted);
//            //db.DemoDepts.Where(p => p.DemoID == entity.ID).ToList().ForEach(p => db.Entry(p).State = EntityState.Deleted);
//            db.DemoAlbums.Where(p => p.DemoID == entity.ID).Delete();
//            db.DemoDepts.Where(p => p.DemoID == entity.ID).Delete();

//            var old_entity = db.Demo.Find(entity.ID);
//            db.Entry(old_entity).CurrentValues.SetValues(entity);
//            ((List<DemoAlbum>)entity.Albums).ForEach(p => db.Entry(p).State = EntityState.Added);
//            foreach (var item in entity.Depts)
//            {
//                item.DemoID = entity.ID;
//                db.Entry(item).State = EntityState.Added;
//            }
//            try
//            {
//                db.SaveChanges();
//            }
//            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
//            {
//                throw new Exception("保存测试信息失败", ex);
//            }
//            return true;
//        }

//    }
//}
