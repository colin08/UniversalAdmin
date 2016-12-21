using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 添加我的工作计划所需参数
    /// </summary>
    public class WorkPlan
    {

        public int user_id { get; set; }

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
        /// 计划完成时间
        /// </summary>
        public DateTime done_time { get; set; }
    }
}