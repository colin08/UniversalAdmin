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
            string ids = ",1,2,3,4,5,";
            //开头有逗号
            if (ids.StartsWith(","))
            {
                ids = ids.Substring(1, ids.Length - 1);
            }
            if (ids.EndsWith(","))
            {
                ids = ids.Substring(0,ids.Length - 1);
            }


            //string s = SecureHelper.MD5("hbl123");

            //string s = EnumHelper.GetEnumShowName(typeof(Tools.ConfigFileEnum), 1);
            //string s2=  Enum.GetName(typeof(ConfigFileEnum), 1);
            //Trace.WriteLine(s);
            Assert.AreEqual(1, 1);
        }
    }
}
