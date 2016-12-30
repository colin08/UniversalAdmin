using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 添加/修改我的工作计划所需参数
    /// </summary>
    public class WorkPlan
    {
        /// <summary>
        /// 添加传入0，修改传入id
        /// </summary>
        public int id { get; set; }

        public int user_id { get; set; }

        /// <summary>
        /// 计划名称
        /// </summary>
        public string week_text { get; set; }

        /// <summary>
        /// 审批人用户id
        /// </summary>
        public int approve_user_id { get; set; }
        
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime begin_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }

        /// <summary>
        /// 计划条目
        /// </summary>
        public List<Models.Response.WorkPlanItem> plan_item { get; set; }
    }
}