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
            var role_list = new List<Entity.SysRole>() {
                new Entity.SysRole() {
                    AddTime = DateTime.Now,
                    RoleName = "管理员",
                    RoleDesc = "管理员组",
                    IsAdmin = true
                },
                new Entity.SysRole() {
                    AddTime = DateTime.Now,
                    RoleName = "编辑用户",
                    RoleDesc = "编辑用户组",
                    IsAdmin = false
                }
            };

            role_list.ForEach(p => context.SysRoles.AddOrUpdate(x => x.RoleName,p));
            context.SaveChanges();

            var role_root = context.SysRoles.Where(p => p.RoleName == "管理员").FirstOrDefault();
            string pwd = SecureHelper.MD5("admin");
            var user_root = new Entity.SysUser() {
                LastLoginTime = DateTime.Now,
                RegTime = DateTime.Now,
                NickName = "超级管理员",
                Password = pwd,
                Status = true,
                SysRole = role_root,
                UserName = "admin",
                Gender = Entity.UserGender.男,
                Avatar = ""
            };
            context.SysUsers.AddOrUpdate(p => p.UserName, user_root);
            

            context.SaveChanges();
        }
    }
}
