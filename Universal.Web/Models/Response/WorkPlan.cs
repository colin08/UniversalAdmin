using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 我的工作计划信息
    /// </summary>
    public class WorkPlan
    {
        public int id { get; set; }

        /// <summary>
        /// 周期
        /// </summary>
        public string week_text { get; set; }

        /// <summary>
        /// 本周工作记录
        /// </summary>
        public string now_job { get; set; }


        /// <summary>
        /// 下周工作计划
        /// </summary>
        public string next_plan { get; set; }

        /// <summary>
        /// 是否审批
        /// </summary>
        public bool is_approve { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime? approve_time { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime done_time { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; }
    }
}