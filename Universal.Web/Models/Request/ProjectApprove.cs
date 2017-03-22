using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 项目审批接口所需参数
    /// </summary>
    public class ProjectApprove
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 当前登录的用户ID
        /// </summary>
        public int user_id { get; set; }


        /// <summary>
        /// 审批状态
        /// </summary>
        public Entity.ApproveStatusType status { get; set; }

        /// <summary>
        /// 审批备注
        /// </summary>
        public string remark { get; set; }

    }
}