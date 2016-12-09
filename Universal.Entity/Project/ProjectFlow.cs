using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;


namespace Universal.Entity
{
    /// <summary>
    /// 项目流程
    /// </summary>
    public class ProjectFlow
    {
        public ProjectFlow()
        {
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
            this.ProjectFlowNodes = new List<ProjectFlowNode>();
        }

        public int ID { get; set; }

        
        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 该流程的节点信息
        /// </summary>
        public virtual ICollection<ProjectFlowNode> ProjectFlowNodes { get; set; }

    }
}
