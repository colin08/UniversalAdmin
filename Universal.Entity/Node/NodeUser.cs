using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Universal.Entity
{
    /// <summary>
    /// 节点联系人
    /// </summary>
    public class NodeUser
    {
        public int ID { get; set; }

        /// <summary>
        /// 节点ID
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        public virtual Node Node { get; set; }

        /// <summary>
        /// 参与人员ID
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 参与人员信息
        /// </summary>
        public virtual CusUser CusUser { get; set; }
    }
}
