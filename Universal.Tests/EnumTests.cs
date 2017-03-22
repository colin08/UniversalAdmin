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
            string msg = "";
            int total = 0;

            Tools.SMSHelper.SendVer("17688700150", "测试");

            //BLL.BLLCusUser.GetJobTaskPageList(5, 1, 26, out total);

            //BLL.BLLProjectFlowNode.SetEnd(189);

            //var ss = BLL.BLLProjectFlowNode.GetProjectFlow(33);

            //BLL.BLLProject.CopyFlowNodeFromCompact(null, 33, true, 68);

            //int[] tes = BLL.BLLFlow.SetNodeCids(1, 59, "正经的标题", 2, "17", out msg);

            //var isOK=  BLL.BLLFlow.GenerateFlowNodeCompact(59);

            //var ss = BLL.BLLFlow.GetUIFlowNode(59);

            //var ss = BLL.BLLFlow.GetFlowData(59);

            var a = 1;
            //复制节点
            //BLL.BLLProject.ExecFlowNodeProcessTo(null, 4, true, 59);

            //1：插入节点，2：添加子节点，3：添加合并节点   
            //string msg = "";
            ////int [] test = BLL.BLLFlow.AddFlowNode(1, 59, "", 18, 123, 3, out msg);
            ////bool is_ok = BLL.BLLFlow.DelWebFlowNode(123, out msg);
            //int a = 1;
            //string ids = ",1,2,3,4,5,";
            ////开头有逗号
            //if (ids.StartsWith(","))
            //{
            //    ids = ids.Substring(1, ids.Length - 1);
            //}
            //if (ids.EndsWith(","))
            //{
            //    ids = ids.Substring(0,ids.Length - 1);
            //}


            //string s = SecureHelper.MD5("hbl123");

            //string s = EnumHelper.GetEnumShowName(typeof(Tools.ConfigFileEnum), 1);
            //string s2=  Enum.GetName(typeof(ConfigFileEnum), 1);
            //Trace.WriteLine(s);
            Assert.AreEqual(1, 1);
        }
    }
}
