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
            var entity = db.SysRoles.Find(2);
            string pwd = SecureHelper.MD5("admin");
            for (int i = 0; i < 30; i++)
            {
                var user = new Entity.SysUser()
                {
                    Avatar = "",
                    Gender = Entity.UserGender.男,
                    LastLoginTime = DateTime.Now,
                    NickName = "编辑用户" + (i + 1).ToString(),
                    Password = pwd,
                    RegTime = DateTime.Now,
                    Status = true,
                    SysRole = entity,
                    UserName = "edit" + (i + 1).ToString()
                };
                db.SysUsers.Add(user);
            }

            db.SaveChanges();

            //var group = db.SysRoutes.GroupBy(p => p.Tag).ToList();

            db.Dispose();
            Assert.AreEqual(1, 1);
        }
    }
}