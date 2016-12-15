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

        /// <summary>
        /// 部门里对应的人数
        /// </summary>
        public int user_total { get; set; }

    }
}