using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 权限
    /// </summary>
    public class BLLCusRoute
    {
        /// <summary>
        /// 获取权限，同时判断用户是否有该权限
        /// </summary>
        /// <returns></returns>
        public static List<Model.AdminUserRoute> GetRouteExists(int user_id)
        {
            var db = new DataCore.EFDBContext();
            SqlParameter[] param = { new SqlParameter("@user_id", user_id) };
            List<Model.AdminUserRoute> list = new List<Model.AdminUserRoute>();
            list = db.Database.SqlQuery<Model.AdminUserRoute>("dbo.sp_GetCusRouteExists @user_id", param).ToList();
            db.Dispose();
            return list;
        }

        /// <summary>
        /// 获取用户第一个权限
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static string GetFristRoute(int user_id)
        {
            string str = "";
            var db = new DataCore.EFDBContext();
            var entity = db.CusUserRoutes.Include(p => p.CusRoute).Where(p => p.CusUserID == user_id).FirstOrDefault();
            if (entity != null)
                str = entity.CusRoute.ControllerName;
            db.Dispose();
            return str;
        }

    }
}
