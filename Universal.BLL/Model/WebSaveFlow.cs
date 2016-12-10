using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.BLL.Model
{
    /// <summary>
    /// 保存流程节点所要传输的数据
    /// </summary>
    [Serializable]
    public class WebSaveFlow
    {
        /// <summary>
        /// 流程ID
        /// </summary>
        public int flow_id { get; set; }

        /// <summary>
        /// 引用的块ID，逗号分割，前后都要加逗号
        /// </summary>
        public string reference_pieces { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        public List<WebSaveFlowNode> flow_node_list { get; set; }
        
    }

    [Serializable]
    public class WebSaveFlowNode
    {
        public int flow_node_id { get; set; }

        public int top { get; set; }

        public int left { get; set; }

        public int piece { get; set; }

        public string color { get; set; }

        public string icon { get; set; }

        public string process_to { get; set; }

    }
}
