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

            var category_a = new Entity.CusCategory();
            category_a.PID = null;
            category_a.Title = "国内";
            context.CusCategorys.Add(category_a);

            var category_b = new Entity.CusCategory();
            category_b.PID = null;
            category_b.Title = "世界";
            context.CusCategorys.Add(category_b);

            var category_1 = new Entity.CusCategory();
            category_1.PCategory = category_a;
            category_1.Title = "社会";
            category_1.Depth = 2;
            context.CusCategorys.Add(category_1);

            var category_2 = new Entity.CusCategory();
            category_2.PCategory = category_a;
            category_2.Title = "经济";
            category_2.Depth = 2;
            context.CusCategorys.Add(category_2);

            var category_3 = new Entity.CusCategory();
            category_3.PCategory = category_a;
            category_3.Title = "文化";
            category_3.Depth = 2;
            context.CusCategorys.Add(category_3);

            var category_4 = new Entity.CusCategory();
            category_4.PCategory = category_b;
            category_4.Title = "格局";
            category_4.Depth = 2;
            context.CusCategorys.Add(category_4);

            var category_5 = new Entity.CusCategory();
            category_5.PCategory = category_b;
            category_5.Title = "要闻";
            category_5.Depth = 2;
            context.CusCategorys.Add(category_5);

            var category_6 = new Entity.CusCategory();
            category_6.PCategory = category_b;
            category_6.Title = "趋势";
            category_6.Depth = 2;
            context.CusCategorys.Add(category_6);

            context.SaveChanges();
        }
    }
}
