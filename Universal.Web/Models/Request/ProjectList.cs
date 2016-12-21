using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 获取可见项目列表所需参数
    /// </summary>
    public class ProjectList : BasePage
    {
        /// <summary>
        /// 是否只获取我的项目
        /// </summary>
        public bool only_mine { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string keyword { get; set; }

        /// <summary>
        /// 状态；0：全部；1：已完结；2：未完结
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 节点ID，不筛选传0
        /// </summary>
        public int node_id { get; set; }

        /// <summary>
        /// 节点状态,0:所有；1：未开始；2：进行中；3：已结束
        /// </summary>
        public int node_status { get; set; }

        /// <summary>
        /// 开始时间，没有时间则传入null
        /// </summary>
        public DateTime? begin_time { get; set; }

        /// <summary>
        /// 结束时间，没有时间则传入null
        /// </summary>
        public DateTime? end_time { get; set; }

    }
}