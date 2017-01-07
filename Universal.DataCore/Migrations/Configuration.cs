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
            //var role_list = new List<Entity.SysRole>() {
            //    new Entity.SysRole() {
            //        AddTime = DateTime.Now,
            //        RoleName = "����Ա",
            //        RoleDesc = "����Ա��",
            //        IsAdmin = true
            //    },
            //    new Entity.SysRole() {
            //        AddTime = DateTime.Now,
            //        RoleName = "�༭�û�",
            //        RoleDesc = "�༭�û���",
            //        IsAdmin = false
            //    }
            //};

            //role_list.ForEach(p => context.SysRoles.AddOrUpdate(x => x.RoleName,p));
            //context.SaveChanges();

            //var role_root = context.SysRoles.Where(p => p.RoleName == "����Ա").FirstOrDefault();
            //string pwd = SecureHelper.MD5("admin");
            //var user_root = new Entity.SysUser() {
            //    LastLoginTime = DateTime.Now,
            //    RegTime = DateTime.Now,
            //    NickName = "��������Ա",
            //    Password = pwd,
            //    Status = true,
            //    SysRoleID = role_root.ID,
            //    UserName = "admin",
            //    Gender = Entity.UserGender.��,
            //    Avatar = ""
            //};
            //context.SysUsers.AddOrUpdate(p => p.UserName, user_root);


            //var cus_user_job = new Entity.CusUserJob() {
            //     Title="����ְλ"
            //};
            //context.CusUserJobs.AddOrUpdate(cus_user_job);

            //var cus_user_department = new Entity.CusDepartment();
            //cus_user_department.PID = null;
            //cus_user_department.Title = "���Բ���";
            //context.CusDepartments.Add(cus_user_department);

            //var cus_user = new Entity.CusUser();
            //cus_user.AboutMe = "���˺����ڿ�������";
            //cus_user.CusDepartmentID = cus_user_department.ID;
            //cus_user.CusUserJobID = cus_user_job.ID;
            //cus_user.Gender = Entity.CusUserGender.male;
            //cus_user.IsAdmin = true;
            //cus_user.Email = "litdev@outlook.com";
            //cus_user.NickName = "��������";
            //cus_user.Password = SecureHelper.MD5("hbl123");
            //cus_user.Status = true;
            //cus_user.Avatar = "/uploads/avatar.jpg";
            //cus_user.Telphone = "18681559829";
            //context.CusUsers.AddOrUpdate(cus_user);


            //var route_1 = new Entity.CusRoute();
            //route_1.ControllerName = "adminuser";
            //route_1.Title = "Ա������";
            //context.CusRoutes.AddOrUpdate(route_1);

            //var route_2 = new Entity.CusRoute();
            //route_2.ControllerName = "department";
            //route_2.Title = "���Ź���";
            //context.CusRoutes.AddOrUpdate(route_2);

            //var route_3 = new Entity.CusRoute();
            //route_3.ControllerName = "document";
            //route_3.Title = "�ؼ��б�";
            //context.CusRoutes.AddOrUpdate(route_3);

            //var route_4 = new Entity.CusRoute();
            //route_4.ControllerName = "doccategory";
            //route_4.Title = "��������";
            //context.CusRoutes.AddOrUpdate(route_4);

            //var route_5 = new Entity.CusRoute();
            //route_5.ControllerName = "flow";
            //route_5.Title = "��������";
            //context.CusRoutes.AddOrUpdate(route_5);

            //var route_6 = new Entity.CusRoute();
            //route_6.ControllerName = "node";
            //route_6.Title = "�ڵ�����";
            //context.CusRoutes.AddOrUpdate(route_6);

            //var route_7 = new Entity.CusRoute();
            //route_7.ControllerName = "adminproject";
            //route_7.Title = "��Ŀ�б�";
            //context.CusRoutes.AddOrUpdate(route_7);


            //var route_8 = new Entity.CusRoute();
            //route_8.ControllerName = "notice";
            //route_8.Title = "�������";
            //context.CusRoutes.AddOrUpdate(route_8);

            //var route_9 = new Entity.CusRoute();
            //route_9.ControllerName = "appversion";
            //route_9.Title = "�汾�б�";
            //context.CusRoutes.AddOrUpdate(route_9);

            //var route_10 = new Entity.CusRoute();
            //route_10.ControllerName = "nodecategory";
            //route_10.Title = "�ڵ����";
            //context.CusRoutes.AddOrUpdate(route_10);

            //var user_route_1 = new Entity.CusUserRoute();
            //user_route_1.CusRoute = route_1;
            //user_route_1.CusUser = cus_user;
            //context.CusUserRoutes.AddOrUpdate(user_route_1);

            //var user_route_2 = new Entity.CusUserRoute();
            //user_route_2.CusRoute = route_2;
            //user_route_2.CusUser = cus_user;
            //context.CusUserRoutes.AddOrUpdate(user_route_2);

            //var user_route_3 = new Entity.CusUserRoute();
            //user_route_3.CusRoute = route_3;
            //user_route_3.CusUser = cus_user;
            //context.CusUserRoutes.AddOrUpdate(user_route_3);

            //var user_route_4 = new Entity.CusUserRoute();
            //user_route_4.CusRoute = route_4;
            //user_route_4.CusUser = cus_user;
            //context.CusUserRoutes.AddOrUpdate(user_route_4);

            //var user_route_5 = new Entity.CusUserRoute();
            //user_route_5.CusRoute = route_5;
            //user_route_5.CusUser = cus_user;
            //context.CusUserRoutes.AddOrUpdate(user_route_5);

            //var user_route_6 = new Entity.CusUserRoute();
            //user_route_6.CusRoute = route_6;
            //user_route_6.CusUser = cus_user;
            //context.CusUserRoutes.AddOrUpdate(user_route_6);

            //var user_route_7 = new Entity.CusUserRoute();
            //user_route_7.CusRoute = route_7;
            //user_route_7.CusUser = cus_user;
            //context.CusUserRoutes.AddOrUpdate(user_route_7);

            //var user_route_8 = new Entity.CusUserRoute();
            //user_route_8.CusRoute = route_8;
            //user_route_8.CusUser = cus_user;
            //context.CusUserRoutes.AddOrUpdate(user_route_8);

            //var user_route_9 = new Entity.CusUserRoute();
            //user_route_9.CusRoute = route_9;
            //user_route_9.CusUser = cus_user;
            //context.CusUserRoutes.AddOrUpdate(user_route_9);

            //var user_route_10 = new Entity.CusUserRoute();
            //user_route_10.CusRoute = route_10;
            //user_route_10.CusUser = cus_user;
            //context.CusUserRoutes.AddOrUpdate(user_route_10);



            ////�ؼ�����
            //var doc = new Entity.DocCategory();
            //doc.Depth = 1;
            //doc.PID = null;
            //doc.Title = "����";
            //context.DocCategorys.AddOrUpdate(doc);

            context.SaveChanges();
        }
    }
}
