﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            //BaseBLL<Entity.SysUser> bll = new BaseBLL<Entity.SysUser>();
            //List<FilterSearch> filters = new List<FilterSearch>();
            //filters.Add(new FilterSearch("ID","1", FilterSearchContract.等于));
            ////var entity = bll.GetModel(filters, null, "SysRole.SysRoleRoutes.SysRoute");
            //var total = 0;
            ////var list = bll.GetPagedList(1, 10, ref total, filters, "RegTime desc", "SysRole");
            //var list = bll.GetPagedList(1, 10, ref total, filters, "RegTime desc", "SysRole");

            //Assert.AreEqual(total, 1);

            //List<int> new_list = new List<int>();
            //new_list.Add(1);
            //new_list.Add(2);
            //new_list.Add(3);

            //List<int> old_list = new List<int>();
            //old_list.Add(3);
            //old_list.Add(4);
            //old_list.Add(5);

            ////新增的
            //var ss = new_list.Except(old_list).ToList();
            ////删除的
            //var sss = old_list.Except(new_list).ToList();

            //string msg = "";
            //var sss = BLL.BLLOrderMedical.ModifyOrderItem("668801", "1,2,3,4,5,6,7,8,9,10", out msg);

            DateTime dt_1 = Tools.TypeHelper.ObjectToDateTime("1993-05-10");
            var ss = DateTime.Now.Year - dt_1.Year;
            var sss = DateTime.Now - dt_1;
            Assert.AreEqual(1,1);
            //Assert.AreEqual(msg, "ok");
        }

    }
}