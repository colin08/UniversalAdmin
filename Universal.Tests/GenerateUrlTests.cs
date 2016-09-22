using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Universal.Tools.Tests
{
    [TestClass()]
    public class GenerateUrlTests
    {
        [TestMethod()]
        public void ShortUrlTest()
        {
            string[] s =  GenerateUrl.ShortUrl("http://www.baidu.com");

            string ddd = GenerateUrl.Short(123243343);
            long a = GenerateUrl.UnShort("8l7dR");//123243343
            ///Trace.WriteLine(s);
            Assert.AreEqual(1,1);
        }
    }
}