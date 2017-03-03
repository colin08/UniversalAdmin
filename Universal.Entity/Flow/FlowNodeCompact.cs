using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Universal.Entity
{

    /// <summary>
    /// 流程节点完善
    /// </summary>
    public class FlowNodeCompact
    {
        public int ID { get; set; }

        /// <summary>
        /// 所属流程
        /// </summary>
        public int FlowID { get; set; }

        /// <summary>
        /// 所属流程信息
        /// </summary>
        public virtual Flow Flow { get; set; }

        /// <summary>
        /// 节点ID
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        public virtual Node Node { get; set; }

        /// <summary>
        /// 是否是流程里的第一个
        /// </summary>
        public bool IsFrist { get; set; }

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
