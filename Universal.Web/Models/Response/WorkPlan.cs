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
        public WorkPlan()
        {
            this.plan_item = new List<WorkPlanItem>();
        }

        public int id { get; set; }

        /// <summary>
        /// 计划名称
        /// </summary>
        public string week_text { get; set; }
        
        /// <summary>
        /// 审批人用户id
        /// </summary>
        public int approve_user_id { get; set; }

        /// <summary>
        /// 审批人用户昵称
        /// </summary>
        public string approve_user_name { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public Entity.ApproveStatusType approve_status { get; set; }
        
        /// <summary>
        /// 审批状态文本
        /// </summary>
        public string approve_status_text { get; set; }

        /// <summary>
        /// 审核不通过时的说明
        /// </summary>
        public string approve_no_text { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime? approve_time { get; set; }

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
        public List<WorkPlanItem> plan_item { get; set; }

    }

    /// <summary>
    /// 计划条米
    /// </summary>
    public class WorkPlanItem
    {
        /// <summary>
        /// 项ID
        /// </summary>
        public int item_id { get; set; }

        /// <summary>
        ///  标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 工作内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 预期目标 选填
        /// </summary>
        public string want_taget { get; set; }

        /// <summary>
        /// 完成时限 选填
        /// </summary>
        public string done_time { get; set; }

        /// <summary>
        /// 完成情况 0:进行中；1：已完成；2：取消，未审核通过时不展示/修改
        /// </summary>
        public Entity.PlanStatus status { get; set; }

        /// <summary>
        /// 状态文本，修改时传入空字符串
        /// </summary>
        public string status_text{get;set;}

        /// <summary>
        /// 备注，未审核通过时不展示/修改
        /// </summary>
        public string remark { get; set; }
    }

}