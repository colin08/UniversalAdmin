using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL.Model
{
    /// <summary>
    /// 项目流程
    /// </summary>
    [Serializable]
    public class ProjectFlow
    {
        public int total { get; set; }

        public int project_id { get; set; }

        /// <summary>
        /// 块内容
        /// </summary>
        public string reference_pieces { get; set; }

        public List<ProjectFlowNode> list { get; set; }
    }

    /// <summary>
    /// 项目流程的节点信息
    /// </summary>
    [Serializable]
    public class ProjectFlowNode
    {
        /// <summary>
        /// 当前流程节点在数据库保存的值
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 流程节点ID(那几个定死的节点)
        /// </summary>
        public int project_flow_node_id { get; set; }

        /// <summary>
        /// 流程节点标题
        /// </summary>
        public string project_flow_node_title { get; set; }

        public string process_to { get; set; }

        /// <summary>
        /// 所属块
        /// </summary>
        public int piece { get; set; }

        public int left { get; set; }

        public int top { get; set; }
        
        public string color { get; set; }

        public string icon { get; set; }

        /// <summary>
        /// 开启/关闭节点
        /// </summary>
        public bool status { get; set; }

        /// <summary>
        /// 是否开始
        /// </summary>
        public bool is_start { get; set; }

        /// <summary>
        /// 是否结束
        /// </summary>
        public bool is_end { get; set; }

    }
}
