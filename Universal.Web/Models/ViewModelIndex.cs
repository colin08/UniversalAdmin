using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 首页数据实体
    /// </summary>
    public class ViewModelIndex
    {
        public ViewModelIndex()
        {
            this.DocumentList = new List<Entity.DocPost>();
            JobTask = new List<Entity.CusUserMessage>();
        }

        public Entity.CusNotice TopNotice { get; set; }

        /// <summary>
        /// 消息待办
        /// </summary>
        public List<Entity.CusUserMessage> JobTask { get; set; }

        /// <summary>
        /// 最新秘籍
        /// </summary>
        public List<Entity.DocPost> DocumentList { get; set; }
    }
}