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
        /// 开启/关闭节点
        /// </summary>
        public bool Status { get; set; }

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
    }
}
