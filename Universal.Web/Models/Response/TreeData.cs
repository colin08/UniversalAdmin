using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 通用树数据
    /// </summary>
    public class TreeData
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int pid { get; set; }

        public string title { get; set; }
    }
}