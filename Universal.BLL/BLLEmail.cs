using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.Tools;

namespace Universal.BLL
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    public class BLLEmail
    {
        /// <summary>
        /// 配置文集那
        /// </summary>
        private static WebSiteModel WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);

        /// <summary>
        /// 发送测试邮件
        /// </summary>
        /// <returns></returns>
        public static bool Send_Demo(string title, string to_user, NameValueCollection col)
        {
            /** col示例
            NameValueCollection myCol = new NameValueCollection();
            myCol.Add("ename", "litdev");
            myCol.Add("link", "http://www.google.com");
            */
            string server_path = "~/App_Data/mailtemplate/demo.html";
            if (!IOHelper.FileExists(server_path))
                return false;
            string templetpath = IOHelper.GetMapPath(server_path);
            EmailHelper email = new EmailHelper();
            email.enableSsl = WebSite.EmailEnableSsl;
            email.host = WebSite.EmailHost;
            email.isbodyHtml = true;
            email.mailFrom = WebSite.EmailFrom;
            email.mailPwd = WebSite.EmailPwd;
            email.mailSubject = title;
            email.mailToArray = to_user.Split(',');
            email.port = WebSite.EmailPort;
            email.mailBody = EmailTemplateHelper.BulidByFile(templetpath, col);
            return email.Send();
        }



    }
}