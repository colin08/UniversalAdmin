using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity
{
    /// <summary>
    /// 项目流程节点正在进行的信息
    /// </summary>
    public class ProjectFlowNodeDoing
    {
        public ProjectFlowNodeDoing()
        {
            this.flow_node_id = -1;
        }
        /// <summary>
        /// 流程节点ID
        /// </summary>
        public int flow_node_id { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string node_title { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string flow_node_remark { get; set; }

    }
}
