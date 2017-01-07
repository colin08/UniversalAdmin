using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Universal.Entity
{
    /// <summary>
    /// 会与参会人员
    /// </summary>
    public class WorkJobUser
    {
        public int ID { get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        public int WorkJobID { get; set; }

        /// <summary>
        /// 任务信息
        /// </summary>
        public virtual WorkJob WorkJob { get; set; }
        
        /// <summary>
        /// 参与人员ID
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 参与人员信息
        /// </summary>
        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsConfirm { get; set; }        
    }
}
