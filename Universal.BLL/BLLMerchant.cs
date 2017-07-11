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
    /// <summary>
    /// 商户
    /// </summary>
    public class BLLMerchant
    {
        /// <summary>
        /// 判断商户id是否存在
        /// </summary>
        /// <param name="mch_id"></param>
        /// <returns></returns>
        public static bool Exists(int mch_id)
        {
            if (mch_id <= 0) return false;
            using (var db =new EFDBContext())
            {
                var entity = db.SysMerchants.AsNoTracking().Where(p => p.ID == mch_id).FirstOrDefault();
                if (entity == null) return false;
                if (!entity.Status) return false;
                return true;
            }
        }

        /// <summary>
        /// 判断商户id是否存在
        /// </summary>
        /// <param name="mch_id"></param>
        /// <returns></returns>
        public static bool Exists(string mch_id)
        {
            int id = Tools.TypeHelper.ObjectToInt(mch_id);
            if (id <= 0) return false;
            using (var db = new EFDBContext())
            {
                var entity = db.SysMerchants.AsNoTracking().Where(p => p.ID == id).FirstOrDefault();
                if (entity == null) return false;
                if (!entity.Status) return false;
                return true;
            }
        }

        /// <summary>
        /// 判断商户名称是否存在
        /// </summary>
        /// <param name="mch_name"></param>
        /// <returns></returns>
        public static bool ExistsName(string mch_name)
        {
            if (string.IsNullOrWhiteSpace(mch_name)) return true;
            using (var db = new EFDBContext())
            {
                return db.SysMerchants.Any(p => p.Title == mch_name);
            }
        }

        /// <summary>
        /// 更改最后登录时间
        /// </summary>
        /// <param name="mch_id"></param>
        public static void ModifyLastLoginTime(int mch_id)
        {
            BLL.BaseBLL<Entity.SysMerchant> bll = new BaseBLL<SysMerchant>();
            var entity = bll.GetModel(p => p.ID == mch_id, "ID ASC");
            if (entity == null) return;
            entity.LastLoginTime = DateTime.Now;
            bll.Modify(entity, "LastLoginTime");
        }

        /// <summary>
        /// 判断是否是超级商户
        /// </summary>
        /// <param name="mch_id"></param>
        /// <returns></returns>
        public static bool CheckIsSuper(int mch_id)
        {
            if (mch_id <= 0) return false;
            using (var db =new EFDBContext())
            {
                var entity = db.SysMerchants.AsNoTracking().Where(p => p.ID == mch_id).FirstOrDefault();
                if (entity == null) return false;
                return entity.IsSuperMch;
            }
        }


        /// <summary>
        /// 添加商户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static int Add(Entity.SysMerchant entity)
        {
            if (entity == null) return 0;
            using(var db =new EFDBContext())
	        {
                db.SysMerchants.Add(entity);

                //添加用户和用户组
                var entity_role_admin = new Entity.SysRole()
                {
                    AddTime = DateTime.Now,
                    RoleName = "管理员",
                    RoleDesc = "管理员组",
                    SysMerchant = entity,
                    IsAdmin = true
                };
                var entity_role_basic = new Entity.SysRole()
                {
                    AddTime = DateTime.Now,
                    RoleName = "员工",
                    RoleDesc = "员工组",
                    SysMerchant = entity,
                    IsAdmin = false
                };

                db.SysRoles.Add(entity_role_basic);
                db.SysRoles.Add(entity_role_admin);

                string pwd = SecureHelper.MD5("sj2015");
                var user_root = new Entity.SysUser()
                {
                    LastLoginTime = DateTime.Now,
                    RegTime = DateTime.Now,
                    NickName = "店长",
                    Password = pwd,
                    Status = true,
                    SysRole = entity_role_admin,
                    UserName = "chief",
                    Gender = Entity.UserGender.男,
                    SysMerchant = entity,
                    Avatar = ""
                };
                db.SysUsers.Add(user_root);
                
                db.SaveChanges();
                return entity.ID;
	        }

        }

    }
}
