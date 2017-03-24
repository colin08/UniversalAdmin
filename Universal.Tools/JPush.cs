using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using cn.jpush.api.common.resp;

namespace Universal.Tools
{
    /// <summary>
    /// 极光推送
    /// </summary>
    public class JPush
    {
        private static JPushClient client = new JPushClient("6da23cfd2274b04081ac005e", "428ecddd26b07c895c53c191");

        /// <summary>
        /// 推送所有平台并且指定Alias
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="content"></param>
        /// <param name="link_id"></param>
        /// <param name="ext">额外参数，如果是bbs，则需要指定是求大神还是晒大神</param>
        /// <returns></returns>
        public static void PushALl(string alias, string content, int type, string link_data, string ext = "")
        {
            string[] alias_arr = alias.Split(',');
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = Audience.s_alias(alias_arr);
            var notification = new Notification().setAlert(content);
            notification.AndroidNotification = new AndroidNotification().setTitle(content);
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);
            pushPayload.notification = notification.Check();
            pushPayload.message = Message.content(content);
            pushPayload.message.AddExtras("type", type);
            pushPayload.message.AddExtras("data", link_data);
            pushPayload.message.AddExtras("ext", ext);
            try
            {
                var result = client.SendPush(pushPayload);

            }
            catch (APIRequestException ex)
            {
                Tools.IOHelper.WriteLogs(string.Format("推送失败APIRequestException: {0},{1},{2}", ex.Status, ex.ErrorCode, ex.ErrorMessage));
            }
            catch (APIConnectionException ex)
            {
                Tools.IOHelper.WriteLogs(string.Format("推送失败APIConnectionException: {0}", ex.Message));
            }
        }
    }
}
