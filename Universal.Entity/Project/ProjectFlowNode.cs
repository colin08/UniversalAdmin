using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Universal.Entity
{
    /// <summary>
    /// 项目流程的节点
    /// </summary>
    public class ProjectFlowNode
    {
        public ProjectFlowNode()
        {
            this.LastUpdateTime = DateTime.Now;
            this.ProjectFlowNodeFiles = new List<ProjectFlowNodeFile>();
        }

        public int ID { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        public virtual Project Project { get; set; }
        
        /// <summary>
        /// 节点ID
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        public virtual Node Node { get; set; }

        /// <summary>
        /// 如果是当前的节点是条件节点，是否走当前节点
        /// </summary>
        public bool IsSelect { get; set; }

        /// <summary>
        /// 是否是流程里的第一个
        /// </summary>
        public bool IsFrist { get; set; }

        /// <summary>
        /// 当前节点备注信息
        /// </summary>
        [MaxLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 开启/关闭节点
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 所属块ID
        /// </summary>
        public int Piece { get; set; }

        /// <summary>
        /// 是否开始
        /// </summary>
        public bool IsStart { get; set; }

        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsEnd { get; set; }

        /// <summary>
        /// 元素距离顶部位置
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// 元素距离左边位置
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// 箭头指向方向流程节点，逗号分割
        /// </summary>
        public string ProcessTo { get; set; }

        /// <summary>
        /// 节点的颜色
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 节点的图标
        /// </summary>
        public string ICON { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int EditUserId { get; set; }

        [ForeignKey("EditUserId")]
        public virtual CusUser EditUser { get; set; }

        /// <summary>
        /// 最后操作时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        public virtual ICollection<ProjectFlowNodeFile> ProjectFlowNodeFiles { get; set; }

    }
}
