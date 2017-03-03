using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL.Model
{
    [Serializable]
    public class WebFlow
    {
        public int total { get; set; }

        public int flow_id { get; set; }
        
        public List<WebFlowNode> list { get; set; }
    }

    /// <summary>
    /// 流程的节点信息
    /// </summary>
    [Serializable]
    public class WebFlowNode
    {
        /// <summary>
        /// 流程节点ID
        /// </summary>
        public int flow_node_id { get; set; }

        /// <summary>
        /// 节点ID
        /// </summary>
        public int node_id { get; set; }

        /// <summary>
        /// 流程节点标题
        /// </summary>
        public string flow_node_title { get; set; }
        
        /// <summary>
        /// 父级ID，逗号分割
        /// </summary>
        public string pids { get; set; }

    }
}
