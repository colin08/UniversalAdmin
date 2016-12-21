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

            //string msg = "";
            //string json = " {\"project_id\":4,\"reference_pieces\":\"\",\"list\":[{\"id\":\"9\",\"node_id\":9,\"node_title\":\"这个时间\",\"left\":600,\"top\":408,\"index\":\"1\",\"status\":\"true\",\"icon\":\"fa-check\",\"process_to\":\"\"}]}";
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //BLL.Model.ProjectFlow data = js.Deserialize<BLL.Model.ProjectFlow>(json);
            //BLL.BLLProjectFlowNode.SaveAllNode(data, out msg);


            //var s = BLL.BLLProjectFlowNode.GetProjectFlow(4);

            //string msg = "";
            //string json = "{\"stage_id\":4,\"title\":\"拆迁5\",\"begin_time\":\"2016-12-09\",\"ZongHuShu\":\"总户数\",\"YiQYHuShu\":\"已签约户数\",\"WeiQYHuShu\":\"未签约户数\",\"ZhanDiMianJi\":\"占地面积\",\"JiDiMianJi\":\"基底面积\",\"KongDiMianJi\":\"空地面积\",\"YiQYMianJi\":\"已签约面积\",\"WeiQYMianJi\":\"未签约面积\",\"ChaiZhanDiMianJi\":\"房屋拆除(占地面积)\",\"ChaiJianZhuMianJi\":\"房屋拆除(建筑面积)\",\"ChaiBuChangMianJi\":\"房屋拆除(补偿面积)\",\"ChaiBuChangjinE\":\"房屋拆除(补偿金额)\",\"file_list\":[{\"file_name\":\"a.doc\",\"file_path\":\"/uploads/avatar.jpg\",\"file_size\":\"60KB\"}]}";
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //BLL.Model.ProjectStage data = js.Deserialize<BLL.Model.ProjectStage>(json);
            //int a = BLL.BLLProjectStage.Add(4,data, out msg);
            //BLL.BLLProjectStage.Modfify(4, data, out id);

            string source_file_name = "06爱叮叮-第三方合作商接口文档说明(服务端).docx";
            string source_file_ext = source_file_name.Substring(source_file_name.LastIndexOf('.') + 1);

            Assert.AreEqual(1, 1);
        }
    }
}
