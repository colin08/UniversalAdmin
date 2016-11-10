using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    /// <summary>
    /// 无限级分类操作
    /// </summary>
    public class BLLCusCategory
    {
        /// <summary>
        /// 添加分类数据
        /// </summary>
        /// <returns></returns>
        public static int Add(int pid,string title)
        {
            if (pid <= 0)
                return -1;

            var db = new DataCore.EFDBContext();
            var p_entity = db.CusCategorys.Find(pid);
            if (p_entity == null)
                return -1;

            var entity = new Entity.CusCategory();
            if (pid == 0)
                entity.PID = null;
            else
                entity.PID = pid;
            entity.Title = title;
            entity.Depth = p_entity.PID == null ? 1 : p_entity.Depth + 1;

            db.CusCategorys.Add(entity);
            db.SaveChanges();
            db.Dispose();
            return entity.ID;
        }

        /// <summary>
        /// 修改分类数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pid">父级ID,为0标识不修改</param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool Modify(int id,int pid,string title)
        {
            if (id <= 0)
                return false;
            if (pid < 0)
                return false;

            var db = new DataCore.EFDBContext();
            var entity = db.CusCategorys.Find(id);
            if (entity == null)
                return false;
            if (pid >0)
            {
                var p_entity = db.CusCategorys.Find(pid);
                if(p_entity != null)
                {
                    entity.Depth = p_entity.PID == null ? 1 : p_entity.Depth + 1;
                    entity.PID = pid;
                }
            }
            entity.Title = title;
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// 根据ID获取子或父数据
        /// </summary>
        /// <param name="up">查找父级，否则为查找子级</param>
        /// <param name="id">当前分类ID</param>
        /// <returns></returns>
        public static List<Entity.CusCategory> GetList(bool up, int id)
        {
            List<Entity.CusCategory> list = new List<Entity.CusCategory>();
            var db = new DataCore.EFDBContext();
            SqlParameter[] param = { new SqlParameter("@Id", id) };
            string proc_name = "dbo.sp_GetParentCusCategory @Id";
            if (!up)
                proc_name = "dbo.sp_GetChildCusCategory @Id";
            list = db.CusCategorys.SqlQuery(proc_name, param).ToList();
            db.Dispose();
            return list;
        }

        /// <summary>
        /// 获取所属某个分类的数据集数量，包含子类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public static int GetChildDataTotal(DataCore.EFDBContext db, int id)
        //{
        //    string Sql = "select count(1) as total from CusUser where charindex(rtrim(CusDepartmentID),(select dbo.fn_GetChildDepartmentStr(" + id.ToString() + ")))>0";
        //    return db.Database.SqlQuery<int>(Sql).ToList()[0];
        //}

    }
}
