﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 收藏所需参数
    /// </summary>
    public class AddFavorites
    {
        /// <summary>
        /// 当前登录的用户ID
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 秘籍ID，逗号分割例如：1,2,3,4,5
        /// </summary>
        public string doc_ids { get; set; }

    }
}