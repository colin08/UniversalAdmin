using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL.Model
{
    /// <summary>
    /// 流程节点
    /// </summary>
    public class FlowNode
    {
        public int category_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string category_name { get; set; }

        public List<FlowNodeList> node_list { get; set; }
    }

    public class FlowNodeList
    {
        public int node_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string node_name { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public List<FlowPNodeList> p_node_list { get; set; }
    }

    public class FlowPNodeList
    {
        public int p_node_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string p_node_name { get; set; }
    }
}
