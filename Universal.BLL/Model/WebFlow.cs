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
        /// 流程节点标题
        /// </summary>
        public string flow_node_title { get; set; }

        public string process_to { get; set; }

        /// <summary>
        /// 拼接的样式
        /// </summary>
        public string style { get; set; }

        public string icon { get; set; }

    }
}
