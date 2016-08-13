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
            var SysRoles_1 = context.SysRoles.Where(p=>p.RoleName == "管理员").FirstOrDefault();
            if(SysRoles_1 == null)
            {
                SysRoles_1 = new Entity.SysRole()
                {
                    AddTime = DateTime.Now,
                    RoleName = "管理员",
                    RoleDesc = "管理员组",
                    IsAdmin = true
                };
                context.SysRoles.Add(SysRoles_1);
            }

            var SysRoles_2 = context.SysRoles.Where(p => p.RoleName == "编辑用户").FirstOrDefault();
            if (SysRoles_2 == null)
            {
                SysRoles_2 = new Entity.SysRole()
                {
                    AddTime = DateTime.Now,
                    RoleName = "编辑用户",
                    RoleDesc = "编辑用户组",
                    IsAdmin = false
                };
                context.SysRoles.Add(SysRoles_2);
            }
            
            var SysUser_Root = context.SysUsers.Where(p => p.UserName == "admin").FirstOrDefault();
            if(SysUser_Root == null)
            {
                string pwd = SecureHelper.MD5("admin");
                SysUser_Root = new Entity.SysUser() {
                    LastLoginTime = DateTime.Now,
                    RegTime = DateTime.Now,
                    NickName = "超级管理员",
                    Password = pwd,
                    Status = true,
                    SysRole = SysRoles_1,
                    UserName = "admin",
                    Gender = Entity.UserGender.男,
                    Avatar= ""
                };
                context.SysUsers.Add(SysUser_Root);
            }



            context.SaveChanges();
        }
    }
}
