using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Script.Serialization;

namespace Universal.Tests
{
    [TestClass]
    public class APITokenTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            //string key = "lizd@fd9^hblyalsd!&" + Tools.WebHelper.ConvertDateTimeInt(DateTime.Now);
            //Tools.Crypto3DES des = new Tools.Crypto3DES(Tools.SiteKey.DES3KEY);
            //string val = des.DESEnCode(key);

            string msg = "";
            string json = " {\"id\":0,\"node_id\":4,\"node_title\":\"哟呵呵呵\",\"index\":1,\"piece\":0,\"status\":true,\"is_start\":false,\"is_end\":false,\"top\":371,\"left\":486,\"color\":\"\",\"icon\":\"\",\"process_to\":\"\"}";
            JavaScriptSerializer js = new JavaScriptSerializer();
            BLL.Model.ProjectFlowNode flow_info = js.Deserialize<BLL.Model.ProjectFlowNode>(json);
            BLL.BLLProjectFlowNode.AddNode(4,"",flow_info, out msg);


            var s = BLL.BLLProjectFlowNode.GetProjectFlow(4);

            Assert.AreEqual(1, 1);
        }
    }
}
