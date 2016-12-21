using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 获取消息列表所需参数
    /// </summary>
    public class MessageList :BasePage
    {
        /// <summary>
        /// 消息类别，1：未读；2：已读；0：所有
        /// </summary>
        public int msg_type { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string searh_word { get; set; }

    }
}