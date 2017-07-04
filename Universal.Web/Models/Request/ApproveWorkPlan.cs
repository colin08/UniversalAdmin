using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 审核工作计划所需参数
    /// </summary>
    public class ApproveWorkPlan
    {
        public int user_id { get; set; }

        /// <summary>
        /// 工作计划id
        /// </summary>
        public int work_plan_id { get; set; }

        /// <summary>
        /// 审核状态，1：通过，2：不通过
        /// </summary>
        public Entity.ApproveStatusType approve_status { get; set; }

        /// <summary>
        /// 审核不通过时的说明
        /// </summary>
        public string approve_no_text { get; set; }
    }
}