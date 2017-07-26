using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 修改工作计划项所需参数
    /// </summary>
    public class EditWorkPlanItem
    {
        /// <summary>
        /// 工作计划ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 当前登录的用户id
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 修改的项
        /// </summary>
        public List<EditWorkPlanItemModel> item { get; set; }
    }
    
    /// <summary>
    /// 具体项
    /// </summary>
    public class EditWorkPlanItemModel
    {
        /// <summary>
        /// 项id
        /// </summary>
        public int item_id { get; set; }

        /// <summary>
        /// 完成状态，0：未完成，1：已完成
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 完成备注
        /// </summary>
        public string remark { get; set; }
    }


}