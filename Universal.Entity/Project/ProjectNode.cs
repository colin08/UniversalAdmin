using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Universal.Entity
{
    /// <summary>
    /// 项目流程信息，和节点合并在一起
    /// </summary>
    public class ProjectNode
    {
        public ProjectNode()
        {
            this.Status = true;
            this.Depth = 0;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
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
        /// 父级ID
        /// </summary>
        public int? PID { get; set; }

        /// <summary>
        /// 父级信息
        /// </summary>
        [ForeignKey("PID")]
        public ProjectNode PProjectNode { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        [MaxLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// 办事地址
        /// </summary>
        [MaxLength(500)]
        public string Location { get; set; }

        /// <summary>
        /// 办事流程说明，富文本内容
        /// </summary>
        [MaxLength(4010)]
        public string Content { get; set; }

        /// <summary>
        /// 指向流程ID
        /// </summary>
        public string TOID { get; set; }

        /// <summary>
        /// 深度，从1递增
        /// </summary>
        public int Depth { get; set; }

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
        /// 优先级，越大，同级显示的时候越靠前
        /// </summary>
        public int Priority { get; set; }

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
