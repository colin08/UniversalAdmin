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
        }

        /// <summary>
        /// 最新秘籍
        /// </summary>
        public List<Entity.DocPost> DocumentList { get; set; }
    }
}