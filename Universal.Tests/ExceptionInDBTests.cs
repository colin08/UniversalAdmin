using Microsoft.VisualStudio.TestTools.UnitTesting;
using Universal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Tests
{
    [TestClass()]
    public class ExceptionInDBTests
    {
        [TestMethod()]
        public void ToInDBTest()
        {
            try
            {
                throw new Exception("异步添加异常信息测试"+DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                ExceptionInDB.ToInDB(ex);
            }
            Assert.AreEqual(1, 1);
        }
    }
}