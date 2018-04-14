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

            //db.SaveChanges();

            //var group = db.SysRoutes.GroupBy(p => p.Tag).ToList();

            ////诊所
            //var entity_clinic = new Entity.Clinic();
            //entity_clinic.Address = "深圳市";
            //entity_clinic.ChengCheLuXian = "这里是乘车路线";
            //entity_clinic.City = Entity.ClinicCity.Futian;
            //entity_clinic.FuWuXiangMu = "服务项目";
            //entity_clinic.FuWuYuYan = "服务语言";
            //entity_clinic.ICON = "";
            //entity_clinic.Telphone = "07553232";
            //entity_clinic.Title = "福田区厚德总部";
            //entity_clinic.WorkHours = "工作时间";
            //db.Clinics.Add(entity_clinic);

            ////诊所科室
            //for (int i = 0; i < 10; i++)
            //{
            //    var entity_dep = new Entity.ClinicDepartment();
            //    entity_dep.ClinicID = 1;
            //    entity_dep.Desc = "科室说明";
            //    entity_dep.Title = "科室名称-" + (i + 1);
            //    db.ClinicDepartments.Add(entity_dep);
            //}
            ////医生特长
            //for (int i = 0; i < 10; i++)
            //{
            //    var entity_sss = new Entity.DoctorsSpecialty();
            //    entity_sss.Title = "特长-" + (i + 1);
            //    db.DoctorsSpecialtys.Add(entity_sss);
            //}

            //var entity_mpuser = new Entity.MPUser();
            //entity_mpuser.OpenID = "abcd";
            //db.MPUsers.Add(entity_mpuser);

            //var techang = db.DoctorsSpecialtys.Where(p => p.ID > 0).Take(2).ToList();
            //var keshi = db.ClinicDepartments.Where(p => p.ID > 0).Take(5).ToList();
            //var entity_mpuser_doc = new Entity.MPUserDoctors();
            //entity_mpuser_doc.ClinicID = 1;
            //entity_mpuser_doc.ID = 1;
            //entity_mpuser_doc.TouXian = "主治医师";
            //entity_mpuser_doc.DoctorsSpecialtyList = techang;
            //entity_mpuser_doc.ClinicDepartmentList = keshi;
            //db.MPUserDoctors.Add(entity_mpuser_doc);
            //var entity = db.MPUsers.Where(p => p.ID == 1).Include(p => p.DoctorsInfo).FirstOrDefault();
            //entity.AccountBalance = 20;
            //entity.DoctorsInfo.ShowMe = "医生个人介绍";
            //entity.DoctorsInfo.DoctorsSpecialtyList = techang;

            //for (int i = 0; i < 20; i++)
            //{
            //    var entity_medicalreport = new Entity.MpUserMedicalReport();
            //    entity_medicalreport.FilePath = "/Assets/mui/img/华汉门诊体检报告.pdf";
            //    entity_medicalreport.MPUserID = 1;
            //    entity_medicalreport.Title = "2018-04-12 体检报告-" + (i + 1);
            //    db.MpUserMedicalReports.Add(entity_medicalreport);
            //}

            var entity_user = db.MPUsers.Find(1);
            entity_user.AccountBalance = 1499;
            for (int i = 0; i < 30; i++)
            {
                var entity_details = new Entity.MPUserAmountDetails();
                entity_details.Amount = 155 + i;
                entity_details.MPUserID = 1;
                entity_details.Title = "这里是交易的文字说明-"+i.ToString();
                if(i % 2 ==0)
                {
                    entity_details.Type = Entity.MPUserAmountDetailsType.Add;
                }
                else
                {
                    entity_details.Type = Entity.MPUserAmountDetailsType.Less;
                }
                db.MPUserAmountDetails.Add(entity_details);
            }

            //for (int i = 1; i <= 4; i++)
            //{
            //    var entity = new Entity.Medical();
            //    entity.Desc = "这里是套餐的介绍内容，应该是富文本";
            //    entity.ImgUrl = "/Assets/mui/img/demo/" + i.ToString() + ".png";
            //    entity.YPrice = 1999;
            //    entity.Price = 1499;
            //    entity.Title = "体检套餐-" + i.ToString();
            //    int skie = i + 3;
            //    int take = i + 5;
            //    var item_list = db.MedicalItems.Where(p => p.ID > 0).OrderBy(p => p.ID).Skip(skie).Take(take).ToList();
            //    entity.MedicalItems = item_list;
            //    db.Medicals.Add(entity);
            //}

            //for (int i = 1; i < 5; i++)
            //{
            //    var entity = new Entity.MedicalBanner();
            //    entity.LinkType = Entity.MedicalBannerLinkType.Medical;
            //    entity.LinkVal = "4";
            //    entity.Status = true;
            //    entity.ImgUrl = "/Assets/mui/img/demo/2.png";
            //    entity.Title = "常规体检套餐大优惠" + i.ToString() + "！";
            //    db.MedicalBanners.Add(entity);
            //}

            db.SaveChanges();
            db.Dispose();
            Assert.AreEqual(1, 1);
        }
    }
}