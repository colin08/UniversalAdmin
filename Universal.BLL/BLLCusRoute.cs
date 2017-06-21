using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity;
using System.ComponentModel;

namespace Universal.BLL
{
    /// <summary>
    /// 权限类别
    /// </summary>
    public enum CusRouteType
    {
        [Description("adminuser")]
        员工管理,
        [Description("department")]
        部门管理,
        [Description("document")]
        秘籍操作,
        [Description("doccategory")]
        秘籍分类,
        [Description("flow")]
        流程设置,
        [Description("node")]
        节点设置,
        [Description("adminproject")]
        项目操作,
        [Description("notice")]
        公告管理,
        [Description("appversion")]
        版本列表,
        [Description("nodecategory")]
        节点分类,
        [Description("contacts")]
        通讯录
    }

    /// <summary>
    /// 权限
    /// </summary>
    public class BLLCusRoute
    {
        /// <summary>
        /// 判断用户是否有权限
        /// </summary>
        /// <returns></returns>
        public static bool CheckUserAuthority(CusRouteType type,int user_id)
        {
            string controller_name = Tools.EnumHelper.GetDescription<CusRouteType>(type);
            using (var db =new DataCore.EFDBContext())
            {
                var route_entity = db.CusRoutes.AsNoTracking().Where(p => p.ControllerName == controller_name).FirstOrDefault();
                if(route_entity == null) return false;

                if (db.CusUserRoutes.Any(p => p.CusUserID == user_id && p.CusRouteID == route_entity.ID)) return true;

                return false;
            }

        }

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
