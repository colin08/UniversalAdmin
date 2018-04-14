//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Universal.BLL
//{
//    /// <summary>
//    /// 无限级分类操作
//    /// </summary>
//    public class BLLCusCategory
//    {

//        /// <summary>
//        /// 添加分类数据
//        /// </summary>
//        /// <returns></returns>
//        public static int Add(Entity.CusCategory entity)
//        {
//            if (entity == null)
//                return -1;

//            var db = new DataCore.EFDBContext();
//            Entity.CusCategory p_entity = null;
//            if (entity.PID != null)
//            {
//                p_entity = db.CusCategorys.Find(entity.PID);
//                if (p_entity == null)
//                {
//                    entity.PID = null;
//                    entity.Depth = 1;
//                }
//                else
//                {
//                    entity.Depth = p_entity.Depth + 1;
//                }
//            }
            
//            db.CusCategorys.Add(entity);
//            db.SaveChanges();
//            db.Dispose();
//            return entity.ID;
//        }
        
//        /// <summary>
//        /// 修改分类数据
//        /// </summary>
//        /// <returns></returns>
//        public static bool Modify(Entity.CusCategory entity)
//        {
//            if (entity == null)
//                return false;

//            var db = new DataCore.EFDBContext();
//            Entity.CusCategory p_entity = null;
//            if (entity.PID != null)
//            {
//                p_entity = db.CusCategorys.Find(entity.PID);
//                if (p_entity == null)
//                {
//                    entity.PID = null;
//                    entity.Depth = 1;
//                }
//                else
//                {
//                    entity.Depth = p_entity.Depth + 1;
//                }
//            }

//            db.Entry<Entity.CusCategory>(entity).State = System.Data.Entity.EntityState.Modified;
//            db.SaveChanges();
//            db.Dispose();
//            return true;
//        }

//        /// <summary>
//        /// 根据ID获取子或父数据
//        /// </summary>
//        /// <param name="up">查找父级，否则为查找子级</param>
//        /// <param name="id">当前分类ID</param>
//        /// <returns></returns>
//        public static List<Entity.CusCategory> GetList(bool up, int id)
//        {
//            List<Entity.CusCategory> list = new List<Entity.CusCategory>();
//            var db = new DataCore.EFDBContext();
//            SqlParameter[] param = { new SqlParameter("@Id", id) };
//            string proc_name = "dbo.sp_GetParentCusCategory @Id";
//            if (!up)
//                proc_name = "dbo.sp_GetChildCusCategory @Id";
//            list = db.CusCategorys.SqlQuery(proc_name, param).ToList();
//            db.Dispose();
//            return list;
//        }

//        /// <summary>
//        /// 获取某个分类下所有的子类
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public static string GetChildIDStr(int id)
//        {
//            using (var db = new DataCore.EFDBContext())
//            {
//                string Sql = "select dbo.fn_GetChildCusCategoryStr(" + id.ToString() + ") as idstr";
//                return db.Database.SqlQuery<string>(Sql).ToList()[0];
//            }
//        }

//        /// <summary>
//        /// 获取所属某个分类的数据集数量，包含子类
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        //public static int GetChildDataTotal(DataCore.EFDBContext db, int id)
//        //{
//        //    string Sql = "select count(1) as total from CusUser where charindex(rtrim(CusDepartmentID),(select dbo.fn_GetChildDepartmentStr(" + id.ToString() + ")))>0";
//        //    return db.Database.SqlQuery<int>(Sql).ToList()[0];
//        //}

//    }
//}
