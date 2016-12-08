using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 流程节点
    /// </summary>
    public class WebFlow
    {
        /// <summary>
        /// 流程ID
        /// </summary>
        public int flow_id { get; set; }

        /// <summary>
        /// 流程对应节点ID
        /// </summary>
        public int node_id { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string node_title { get; set; }

        /// <summary>
        /// 父级流程ID
        /// </summary>
        public int? parent_id { get; set; }

        /// <summary>
        /// 箭头指向节点流程ID
        /// </summary>
        public string toid { get; set; }
    }

    /// <summary>
    /// 获取所有节点
    /// </summary>
    public class WebNode
    {
        public int node_id { get; set; }


        public string node_title { get; set; }
    }


}