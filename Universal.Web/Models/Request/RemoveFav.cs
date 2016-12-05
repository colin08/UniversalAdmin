using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 移除秘籍收藏所需参数
    /// </summary>
    public class RemoveFav
    {
        /// <summary>
        /// 收藏的ID
        /// </summary>
        public string ids { get; set; }
    }
}