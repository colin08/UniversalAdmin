using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 用户推送消息开关
    /// </summary>
    public class CusUserPushTurn
    {
        public int ID { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public int CusUserID { get; set; }

        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 开启通知的类别，逗号分割 1公告通知   2待办事项   3项目提醒
        /// </summary>
        public string OnStr { get; set; }

    }
}
