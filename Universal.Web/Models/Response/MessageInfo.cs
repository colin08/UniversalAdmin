using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 我的消息信息
    /// </summary>
    public class MessageInfo
    {
        public int id { get; set; }

        // <summary>
        /// 消息类别
        /// </summary>
        public Entity.CusUserMessageType type { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string type_name { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 链接ID
        /// </summary>
        public string link_id { get; set; }
                
        /// <summary>
        /// 发布人名字，包含部门信息
        /// </summary>
        public string add_user_name { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime add_time { get; set; }

    }
}