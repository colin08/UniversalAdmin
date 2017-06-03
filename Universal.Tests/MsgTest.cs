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

            ///var str = Tools.TypeHelper.Baodate2ChineseSimple(DateTime.Now);

            string msg = "";
            //BLL.BLLProjectFlowNode.SetSelect(304, out msg);

            //Tools.JPush.PushALl("15876594020", "手动测试", 1, "2");

            BLL.BLLMsg.PushMsg(8, Entity.CusUserMessageType.approveproject, string.Format(BLL.BLLMsgTemplate.ApproveProject, "推送测试"), 161);

            //var ss = Entity.APPVersionPlatforms.Android;
            //string app_name = Tools.EnumHelper.GetBEnumShowName(typeof(Entity.APPVersionPlatforms), (byte)ss);
            //BLL.BLLMsg.PushAllUser(Entity.CusUserMessageType.appupdate, string.Format(BLL.BLLMsgTemplate.AppUpdate, app_name), 1);
            Assert.AreEqual(1,1);
        }
    }
}
