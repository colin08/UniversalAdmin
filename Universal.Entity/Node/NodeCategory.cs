using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Universal.Entity
{
    /// <summary>
    /// 节点分类
    /// </summary>
    public class NodeCategory
    {
        public NodeCategory()
        {
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime AddTime { get; set; }

    }
}
