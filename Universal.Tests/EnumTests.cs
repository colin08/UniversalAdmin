using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.Tools;

namespace Universal.Tests
{
    [TestClass()]
    public class EnumTests
    {
        [TestMethod]
        public void Testxzz()
        {
            //string s = EnumHelper.GetEnumShowName(typeof(Tools.ConfigFileEnum), 1);
            //string s2=  Enum.GetName(typeof(ConfigFileEnum), 1);
            //Trace.WriteLine(s);

            var ss = BLL.BLLCategory.GetCaseShowCategory();

            Assert.AreEqual(1, 1);
        }
    }
}
