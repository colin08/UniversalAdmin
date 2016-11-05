using Microsoft.VisualStudio.TestTools.UnitTesting;
using Universal.DataCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Universal.Tools;

namespace Universal.Tests
{
    [TestClass()]
    public class EFDBContextTests
    {
        [TestMethod()]
        public void EFDBContextTest()
        {
            var db = new EFDBContext();
            //编辑用户组
            //var role = db.SysRoles.Where(p => p.ID == 2).Include(s => s.SysRoleRoutes.Select(y => y.SysRoute)).FirstOrDefault();
            //添加测试路由
            //int[] power = { (int)DataCore.Entity.SysRouteType.add, (int)DataCore.Entity.SysRouteType.delete, (int)DataCore.Entity.SysRouteType.select };
            //int[] power2 = { (int)DataCore.Entity.SysRouteType.update, (int)DataCore.Entity.SysRouteType.upload };
            //var route1 = new DataCore.Entity.SysRoute() { AddTime = DateTime.Now, Desc = "测试路由", Route = "/home/index", Tag = "其他", RouteType = power };
            //var route2 = new DataCore.Entity.SysRoute() { AddTime = DateTime.Now, Desc = "测试路由2", Route = "/home/test", Tag = "其他2", RouteType = power2 };
            //db.SysRoutes.Add(route1);
            //db.SysRoutes.Add(route2);

            //添加组对应权限
            //var roleroute = new DataCore.Entity.SysRoleRoute() { SysRoleID = role.ID, SysRoute = route1 };
            //var roleroute2 = new DataCore.Entity.SysRoleRoute() { SysRoleID = role.ID, SysRoute = route2 };
            //db.SysRoleRoutes.Add(roleroute);
            //db.SysRoleRoutes.Add(roleroute2);

            //添加用户
            //var entity = db.SysRoles.Find(2);
            //string pwd = SecureHelper.MD5("admin");
            //for (int i = 0; i < 30; i++)
            //{
            //    var user = new Entity.SysUser()
            //    {
            //        Avatar = "",
            //        Gender = Entity.UserGender.男,
            //        LastLoginTime = DateTime.Now,
            //        NickName = "编辑用户" + (i + 1).ToString(),
            //        Password = pwd,
            //        RegTime = DateTime.Now,
            //        Status = true,
            //        SysRole = entity,
            //        UserName = "edit" + (i + 1).ToString()
            //    };
            //    db.SysUsers.Add(user);
            //}

            //var cus_user_job = new Entity.CusUserJob();
            //cus_user_job.Title = "工程师3";
            //db.CusUserJobs.Add(cus_user_job);
            //var cus_user_job = db.CusUserJobs.Find(1);

            //var cus_department = db.CusDepartments.Find(1);

            //var cus_user = new Entity.CusUser();
            //cus_user.AboutMe = "abd";
            //cus_user.CusDepartment = cus_department;
            //cus_user.CusUserJob = cus_user_job;
            //cus_user.Gender = Entity.CusUserGender.female;
            //cus_user.IsAdmin = false;
            //cus_user.NickName = "张三";
            //cus_user.Password = Tools.SecureHelper.MD5("123");
            //cus_user.Status = true;
            //cus_user.Telphone = "13058084692";
            //db.CusUsers.Add(cus_user);

            //var cus_user1 = db.CusUsers.Find(1);
            //var cus_user = db.CusUsers.Find(2);
            //List<Entity.CusUser> list = new List<Entity.CusUser>();
            //list.Add(cus_user);
            //list.Add(cus_user1);
            //var cus_department = db.CusDepartments.Where(p => p.ID == 1).Include(p => p.DepartmentAdminUsers).FirstOrDefault();
            //cus_department.DepartmentAdminUsers = list;
            var depart_p = db.CusDepartments.Find(1);

            var depart_1 = new Entity.CusDepartment();
            depart_1.Title = "营销部";
            depart_1.Depth = 2;
            depart_1.PDepartment = depart_p;
            db.CusDepartments.Add(depart_1);

            var depart_2 = new Entity.CusDepartment();
            depart_2.Title = "营销部1";
            depart_2.Depth = 3;
            depart_2.PDepartment = depart_1;
            db.CusDepartments.Add(depart_2);

            var depart_3 = new Entity.CusDepartment();
            depart_3.Title = "营销部2";
            depart_3.Depth = 3;
            depart_3.PDepartment = depart_1;
            db.CusDepartments.Add(depart_3);

            var depart_4 = new Entity.CusDepartment();
            depart_4.Title = "财务部";
            depart_4.Depth = 2;
            depart_4.PDepartment = depart_p;
            db.CusDepartments.Add(depart_4);

            var depart_5 = new Entity.CusDepartment();
            depart_5.Title = "财务部1";
            depart_5.Depth = 3;
            depart_5.PDepartment = depart_4;
            db.CusDepartments.Add(depart_5);

            var depart_6 = new Entity.CusDepartment();
            depart_6.Title = "财务部2";
            depart_6.Depth = 3;
            depart_6.PDepartment = depart_4;
            db.CusDepartments.Add(depart_6);

            var depart_7 = new Entity.CusDepartment();
            depart_7.Title = "产品部";
            depart_7.Depth = 2;
            depart_7.PID = depart_p.ID;
            db.CusDepartments.Add(depart_7);

            var depart_8 = new Entity.CusDepartment();
            depart_8.Title = "产品部1";
            depart_8.Depth = 3;
            depart_8.PDepartment = depart_7;
            db.CusDepartments.Add(depart_8);

            var depart_9 = new Entity.CusDepartment();
            depart_9.Title = "产品部2";
            depart_9.Depth = 3;
            depart_9.PDepartment = depart_7;
            db.CusDepartments.Add(depart_9);

            var cus_user = db.CusUsers.Find(1);
            if(cus_user != null)
            {
                cus_user.CusDepartment = depart_2;
                db.SaveChanges();
            }

            db.SaveChanges();
            
            //var cus_ddd = db.CusDepartments.Where(p => p.ID == 1).Include(p => p.DepartmentAdminUsers).FirstOrDefault();

            //var group = db.SysRoutes.GroupBy(p => p.Tag).ToList();

            db.Dispose();
            Assert.AreEqual(1, 1);
        }
    }
}