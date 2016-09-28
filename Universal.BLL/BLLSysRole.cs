using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.DataCore;
using Universal.Entity;
using Universal.Tools;

namespace Universal.BLL
{
    public class BLLSysRole
    {
        EFDBContext db = new EFDBContext();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="qx"></param>
        /// <returns></returns>

        public int Add(SysRole entity,string qx)
        {
            entity.AddTime = DateTime.Now;
            db.SysRoles.Add(entity);
            if (!string.IsNullOrWhiteSpace(qx))
            {
                foreach (var item in qx.Split(','))
                {
                    int route_id = TypeHelper.ObjectToInt(item);
                    db.SysRoleRoutes.Add(
                        new SysRoleRoute() { SysRole = entity, SysRouteID = route_id }
                    );
                }
            }
            return db.SaveChanges();
        }

        public bool Modify(SysRole entity,string qx)
        {
            if (entity == null)
                return false;
            if (entity.ID == 0)
                return false;

            var old_entity = db.SysRoles.Find(entity.ID);
            db.Entry(old_entity).CurrentValues.SetValues(entity);
            
            //修改权限数据
            if(string.IsNullOrWhiteSpace(qx))
            {
                db.SysRoleRoutes.Where(p => p.SysRoleID == entity.ID).ToList().ForEach(p => db.Entry(p).State = System.Data.Entity.EntityState.Deleted);
            }
            List<int> new_id_list = qx.Split(',').Select(Int32.Parse).ToList();
            var route_list = db.SysRoleRoutes.Where(p => p.SysRoleID == entity.ID).ToList();
            List<int> route_id_list = new List<int>();
            foreach (var item in route_list)
                route_id_list.Add(item.SysRouteID);
            //判断存在的差
            var route_del_list = route_id_list.Except(new_id_list).ToList();
            foreach (var item in route_del_list)
            {
                //删除
                var del_entity = db.SysRoleRoutes.Where(p => p.SysRouteID == item && p.SysRoleID == entity.ID).FirstOrDefault();
                db.SysRoleRoutes.Remove(del_entity);

            }

            var route_add_list = new_id_list.Except(route_id_list).ToList();
            foreach (var item in route_add_list)
            {
                //做增加
                db.SysRoleRoutes.Add(new SysRoleRoute() { SysRoleID = entity.ID, SysRouteID = item });
            }

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                throw new Exception("保存测试信息失败", ex);
            }
            return true;
        }

    }
}
