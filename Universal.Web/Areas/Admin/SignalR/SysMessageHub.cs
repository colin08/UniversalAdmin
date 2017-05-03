using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Universal.Web.Areas.Admin.SignalR
{
    /// <summary>
    /// 系统消息集线器
    /// </summary>
    [HubName("sysMessageService")]
    public class SysMessageHub : Hub
    {
        [HubMethodName("show")]
        public static void Show()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SysMessageHub>();
            context.Clients.All.displayDatas();
        }

    }
}