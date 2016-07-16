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
            //��
            var SysRoles = new List<Entity.SysRole>
            {
                new Entity.SysRole(){ AddTime=DateTime.Now, RoleName="����Ա", RoleDesc="����Ա��"},
                new Entity.SysRole(){ AddTime=DateTime.Now,RoleName="�༭�û�",RoleDesc="�༭�û���" }
            };
            SysRoles.ForEach(s => context.SysRoles.AddOrUpdate(p => p.RoleName, s));
            context.SaveChanges();

            //�û�
            var SysUser = new Entity.SysUser();
            SysUser.IsAdmin = true;
            SysUser.LastLoginTime = DateTime.Now;
            SysUser.RegTime = DateTime.Now;
            SysUser.NickName = "��������Ա";
            SysUser.Password = SecureHelper.MD5("admin");
            SysUser.Status = true;
            SysUser.SysRole = context.SysRoles.Where(s => s.RoleName == "����Ա").First();
            SysUser.SysRoleID = SysUser.SysRole.ID;
            SysUser.UserName = "admin";
            SysUser.Gender = Entity.UserGender.��;
            SysUser.Avatar = "";
            try
            {
                context.SysUsers.AddOrUpdate(s => s.UserName, SysUser);
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
