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
    [TestClass]
    public class JFData
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var db = new EFDBContext();
            //var de_list = db.CusDepartments.ToList();
            //var user = db.CusUsers.Find(1);

            //foreach (var item in de_list)
            //{
            //    var de_admin = new Entity.CusDepartmentAdmin();
            //    de_admin.CusDepartment = item;
            //    de_admin.CusUser = user;
            //    db.CusDepartmentAdmins.Add(de_admin);
            //}

            //db.SaveChanges();


            //db.Dispose();
            //string zip_path ="";
            //BLL.BLLProjectFlowNode.ImportProject(89,out zip_path);
            
            Assert.AreEqual(true, true);
        }
    }
}
