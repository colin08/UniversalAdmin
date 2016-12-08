using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Universal.Entity
{
    /// <summary>
    /// 流程信息
    /// </summary>
    public class Flow
    {
        public Flow()
        {
            this.Depth = 0;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 指向节点ID
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        public virtual Node Node { get; set; }

        /// <summary>
        /// 流程名称,为顶级时使用
        /// </summary>
        [MaxLength(50)]
        public string FlowName { get; set; }

        /// <summary>
        /// 某个流程名称，如果为顶级，则为流程标题
        /// </summary>
        [MaxLength(30)]
        public string Title { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int? PID { get; set; }

        /// <summary>
        /// 父级信息
        /// </summary>
        [ForeignKey("PID")]
        public Flow PFlow { get; set; }

        /// <summary>
        /// 顶级父ID
        /// </summary>
        public int? TopPID { get; set; }

        /// <summary>
        /// 指向流程ID
        /// </summary>
        public string TOID { get; set; }

        /// <summary>
        /// 深度，从1递增
        /// </summary>
        public int Depth { get; set; }
        
        /// <summary>
        /// 优先级，越大，同级显示的时候越靠前
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 是否默认流程
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

    }
}
