namespace Universal.DataCore.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using Tools;

    internal sealed class Configuration : DbMigrationsConfiguration<Universal.DataCore.EFDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Universal.DataCore.EFDBContext context)
        {
            //添加默认商户
            var entity_mch = new Entity.SysMerchant();
            entity_mch.AddTime = DateTime.Now;
            entity_mch.Title = "试镜";
            entity_mch.IsSuperMch = true;
            entity_mch.Remark = "我们的自营店";
            context.SysMerchants.AddOrUpdate(x => x.Title, entity_mch);

            //用户组
            var role_list = new List<Entity.SysRole>() {
                new Entity.SysRole() {
                    AddTime = DateTime.Now,
                    RoleName = "管理员",
                    RoleDesc = "管理员组",
                    SysMerchant =entity_mch,
                    IsAdmin = true
                },
                new Entity.SysRole() {
                    AddTime = DateTime.Now,
                    RoleName = "员工",
                    RoleDesc = "员工组",
                    SysMerchant =entity_mch,
                    IsAdmin = false
                }
            };

            role_list.ForEach(p => context.SysRoles.AddOrUpdate(x => x.RoleName,p));
            context.SaveChanges();

            var role_root = context.SysRoles.Where(p => p.RoleName == "管理员").FirstOrDefault();
            string pwd = SecureHelper.MD5("sj2015");
            var user_root = new Entity.SysUser() {
                LastLoginTime = DateTime.Now,
                RegTime = DateTime.Now,
                NickName = "店主",
                Password = pwd,
                Status = true,
                SysRole = role_root,
                UserName = "chief",
                Gender = Entity.UserGender.男,
                SysMerchant = entity_mch,
                Avatar = ""
            };
            context.SysUsers.AddOrUpdate(p => p.UserName, user_root);
            
            context.SaveChanges();
        }
    }
}
