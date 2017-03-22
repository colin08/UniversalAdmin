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
        /// 当前节点在此流程里是否存在
        /// </summary>
        public bool exists { get; set; }

        /// <summary>
        /// 子级ID
        /// </summary>
        public List<FlowCNodeList> c_node_list { get; set; }
    }

    public class FlowCNodeList
    {
        public int c_node_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string c_node_name { get; set; }
    }
}
