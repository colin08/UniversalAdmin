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
            //string json = "{\"flow_id\":13,\"flow_node_list\":[{\"top\":1,\"left\":1,\"process_to\":\"4\",\"flow_node_id\":2,\"color\":\"1\",\"icon\":\"1\"},{\"top\":2,\"left\":2,\"process_to\":\"2\",\"flow_node_id\":4,\"color\":\"2\",\"icon\":\"2\"}]}";
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //BLL.Model.WebSaveFlow flow_info = js.Deserialize<BLL.Model.WebSaveFlow>(json);
            //flow_info.flow_node_list[0].left = 555;
            //BLL.BLLFlow.WebSaveFlowData(flow_info, out msg);
            //var ids = BLL.BLLFlow.AddFlowNode(1, 13, 2, 100, 100, "", "", "", out msg);


            var s = BLL.BLLFlow.GetWebFlowData(13);

            Assert.AreEqual(1, 1);
        }
    }
}
