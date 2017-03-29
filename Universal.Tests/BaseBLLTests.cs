using Microsoft.VisualStudio.TestTools.UnitTesting;
using Universal.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL.Tests
{
    [TestClass()]
    public class BaseBLLTests
    {
        [TestMethod()]
        public void BaseBLLTest()
        {
            //BaseBLL<Entity.Demo> bll = new BaseBLL<Entity.Demo>();
            //List<BLL.FilterSearch> filter = new List<FilterSearch>();
            //filter.Add(new FilterSearch("ID", "1", FilterSearchContract.等于));
            //var entity = bll.GetModel(filter, null, "AddUser.SysRole");

            BaseBLL<Entity.SysUser> bll = new BaseBLL<Entity.SysUser>();
            List<FilterSearch> filters = new List<FilterSearch>();
            filters.Add(new FilterSearch("ID","1", FilterSearchContract.等于));
            //var entity = bll.GetModel(filters, null, "SysRole.SysRoleRoutes.SysRoute");
            var total = 0;
            //var list = bll.GetPagedList(1, 10, ref total, filters, "RegTime desc", "SysRole");
            var list = bll.GetPagedList(1, 10, ref total, filters, "RegTime desc", "SysRole");

            Assert.AreEqual(total, 1);
        }
        
    }
}