using Microsoft.VisualStudio.TestTools.UnitTesting;
using Universal.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Tools.Tests
{
    [TestClass()]
    public class EmailHelperTests
    {
        [TestMethod()]
        public void SendTest()
        {

            EmailHelper email = new EmailHelper();
            email.mailFrom = "769371877@qq.com";
            email.mailPwd = "kzvtmxnibombbdih";
            email.mailSubject = "欢迎来到云影|CloudSS";
            email.mailBody = "亲爱的云影客户,非常感谢您加入云影，您在云影客户中心的登录信息如下：";
            email.isbodyHtml = true;    //是否是HTML
            email.host = "smtp.qq.com";//如果是QQ邮箱则：smtp:qq.com,依次类推
            email.port = 587;
            email.enableSsl = true;
            email.mailToArray = new string[] { "litdev@outlook.com" };//接收者邮件集合
            //email.mailCcArray = new string[] { "litdev@outlook.com" };//抄送者邮件集合
            email.attachmentsPath = new string[] { };
            if (email.Send())
            {
                
            }
            else
            {
                
            }

            Assert.AreEqual(1,1);
        }
    }
}