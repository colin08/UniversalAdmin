using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Universal.Entity
{
    /// <summary>
    /// 节点
    /// </summary>
    public class Node
    {
        public Node()
        {
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
            this.NodeUsers = new List<NodeUser>();
            this.NodeFiles = new List<NodeFile>();
        }

        public int ID { get; set; }

        /// <summary>
        /// 所属分类
        /// </summary>
        public int NodeCategoryID { get; set; }

        /// <summary>
        /// 分类信息
        /// </summary>
        public virtual NodeCategory NodeCategory { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        [MaxLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// 是否是条件节点
        /// </summary>
        public bool IsFactor { get; set; }

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
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 联系人员
        /// </summary>
        public virtual ICollection<NodeUser> NodeUsers { get; set; }

        /// <summary>
        /// 节点附件
        /// </summary>
        public virtual ICollection<NodeFile> NodeFiles { get; set; }

    }
}
