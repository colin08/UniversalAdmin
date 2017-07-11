using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.Entity;
using Universal.DataCore;
using System.Data;

namespace Universal.BLL
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class BLLSysUser
    {
        /// <summary>
        /// 验证是否有权限
        /// </summary>
        /// <param name="page_key"></param>
        /// <param name="is_post"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool CheckAdminPower(string page_key,bool is_post,Entity.SysUser user)
        {
            using (var db = new EFDBContext())
            {
                var entity_route = db.SysRoutes.AsNoTracking().Where(p => p.IsPost == is_post && p.Route == page_key).FirstOrDefault();
                if (entity_route == null) return true;
                var entity_mach = db.SysMerchants.AsNoTracking().Where(p => p.ID == user.SysMerchantID).FirstOrDefault();
                if (entity_mach == null) return false;

                //如果该路由指定必须是我们可以访问
                if(entity_route.IsSuperMch)
                {
                    //如果该用户是我们自己人，则判断他有没有权限
                    if(entity_mach.IsSuperMch)
                    {
                        //如果该用户是管理员组,则放行
                        if(user.SysRole.IsAdmin) return true;
                        else
                        {
                            //否则判断是否有权限
                            return db.SysRoleRoutes.Any(p => p.SysRoleID == user.SysRoleID && p.SysRouteID == entity_route.ID);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }else
                {
                    //如果该用户是管理员组,则放行
                    if (user.SysRole.IsAdmin) return true;
                    else
                    {
                        //否则判断是否有权限
                        return db.SysRoleRoutes.Any(p => p.SysRoleID == user.SysRoleID && p.SysRouteID == entity_route.ID);
                    }
                }
            }
        }

        /// <summary>
        /// 判断用户组是否是管理员
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static bool CheckUserRoleIsAdmin(int user_id)
        {
            BLL.BaseBLL<Entity.SysUser> bll = new BaseBLL<SysUser>();
            var model = bll.GetModel(p => p.ID == user_id, "ID ASC", "SysRole");
            if (model == null) return false;
            return model.SysRole.IsAdmin;
        }

    }
}
