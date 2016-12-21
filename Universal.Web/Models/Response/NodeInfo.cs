using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 节点/流程信息
    /// </summary>
    public class NodeInfo
    {
        public int node_id { get; set; }

        public string title { get; set; }

        /// <summary>
        /// 是否默认（流程里的）
        /// </summary>
        public bool is_def { get; set; }

    }
}