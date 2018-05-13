using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Tools
{
    public class SMSHelper
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phopneNo">发送目的号码，多号码以空格格开</param>
        /// <param name="text">发送的内容</param>
        /// <returns></returns>
        public static bool SendVer(string phopneNo, string text)
        {
            string requestUrl = string.Format("http://211.147.239.62:9050/cgi-bin/sendsms?username=langx@langx&password=Netbit44@&to={0}&text={1}&subid=&msgtype=4", phopneNo, WebHelper.UrlEncode(text, "GB2312"));
            try
            {
                string html = WebHelper.GetRequestData(requestUrl, "GET", "");
                //IOHelper.WriteLogs("发送短信返回的内容："+html);
                switch (TypeHelper.ObjectToInt(html))
                {
                    case 0:
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("短信发送失败：" + ex.Message);
                return false;
            }
        }
    }
}
