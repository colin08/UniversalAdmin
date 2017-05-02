using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 用户推送消息设置开关需要的参数
    /// </summary>
    public class UserPushTrun
    {
        /// <summary>
        /// 登录的用户ID
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 设置开启的类别id，1公告通知；2待办事项；3项目提醒，逗号分割，示例:1,2,3
        /// </summary>
        public string data { get; set; }
    }
}