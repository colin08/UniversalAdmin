using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Universal.Entity
{
    /// <summary>
    /// 项目流程节点附件
    /// </summary>
    public class ProjectFlowNodeFile
    {
        public int ID { get; set; }

        /// <summary>
        /// 节点ID
        /// </summary>
        public int ProjectFlowNodeID { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        public virtual ProjectFlowNode ProjectFlowNode { get; set; }

        /// <summary>
        /// 附件名称，使用原名称
        /// </summary>
        [MaxLength(200)]
        public string FileName { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        [MaxLength(500)]
        public string FilePath { get; set; }

        /// <summary>
        /// 附件大小,KB或MB显示
        /// </summary>
        [MaxLength(30)]
        public string FileSize { get; set; }
    }
}
