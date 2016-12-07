using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Universal.Entity
{
    /// <summary>
    /// 节点联系人
    /// </summary>
    public class ProjectNodeUser
    {
        public int ID { get; set; }

        /// <summary>
        /// 节点ID
        /// </summary>
        public int ProjectNodeID { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        public virtual ProjectNode ProjectNode { get; set; }

        /// <summary>
        /// 联系人员ID
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 联系人员信息
        /// </summary>
        public virtual CusUser CusUser { get; set; }
    }
}
