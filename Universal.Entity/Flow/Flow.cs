using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Universal.Entity
{
    /// <summary>
    /// 流程类别
    /// </summary>
    public enum FlowType
    {
        [Description("普通的")]
        basic,
        [Description("计划立项")]
        JHLX,
        [Description("单元规划")]
        DYGH,
        [Description("信息核查")]
        XXHC,
        [Description("主体确认")]
        ZTQR,
        [Description("用地审批")]
        YDSP
    }

    /// <summary>
    /// 流程信息
    /// </summary>
    public class Flow
    {
        public Flow()
        {
            this.Pieces = "";
            this.FlowType = FlowType.basic;
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
        /// 流程类别
        /// </summary>
        public FlowType FlowType { get; set; }

        /// <summary>
        /// 类别icon
        /// </summary>
        public string FlowTypeICON
        {
            get
            {
                switch (FlowType)
                {
                    case Entity.FlowType.basic:
                        return "ico-nav2";
                    case Entity.FlowType.JHLX:
                        return "ico-nav2";
                    case Entity.FlowType.DYGH:
                        return "ico-nav3";
                    case Entity.FlowType.XXHC:
                        return "ico-nav4";
                    case Entity.FlowType.ZTQR:
                        return "ico-nav5";
                    case Entity.FlowType.YDSP:
                        return "ico-nav6";
                    default:
                        return "ico-nav2";
                }
            }
        }

        /// <summary>
        /// 引用的块，逗号分割:,1,2,3,4,
        /// </summary>
        public string Pieces { get; set; }

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
