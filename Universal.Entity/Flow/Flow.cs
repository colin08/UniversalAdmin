﻿using System;
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
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
            FlowNodes = new List<FlowNode>();
        }

        public int ID { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        [MaxLength(50)]
        public string Title { get; set; }
        
        /// <summary>
        /// 是否默认流程
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 添加的用户ID
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 添加的用户信息
        /// </summary>
        public virtual CusUser CusUser { get; set; }

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
        public virtual ICollection<FlowNode> FlowNodes { get; set; }

    }
}
