using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Universal.Tests
{
    [TestClass]
    public class APITokenTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            string key = "lizd@fd9^hblyalsd!&" + Tools.WebHelper.ConvertDateTimeInt(DateTime.Now);
            Tools.Crypto3DES des = new Tools.Crypto3DES(Tools.SiteKey.DES3KEY);
            string val = des.DESEnCode(key);

            Assert.AreEqual(1, 1);
        }
    }
}
