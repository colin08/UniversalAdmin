using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Universal.Tests
{
    [TestClass]
    public class MsgTest
    {
        [TestMethod]
        public void TestMethod1()
        {

            string msg = "";
            BLL.BLLProjectFlowNode.SetSelect(304, out msg);

            //Tools.JPush.PushALl("13714673949", "手动测试", 1, "2");

            //var ss = Entity.APPVersionPlatforms.Android;
            //string app_name = Tools.EnumHelper.GetBEnumShowName(typeof(Entity.APPVersionPlatforms), (byte)ss);
            //BLL.BLLMsg.PushAllUser(Entity.CusUserMessageType.appupdate, string.Format(BLL.BLLMsgTemplate.AppUpdate, app_name), 1);
            //Assert.AreEqual(1,1);
        }
    }
}
