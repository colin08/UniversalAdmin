using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    /// <summary>
    /// 统计图数据
    /// </summary>
    public class HighCharts
    {
        /// <summary>
        /// x轴数据
        /// </summary>
        public string x_data { get; set; }
        /// <summary>
        /// y轴数据
        /// </summary>
        public string y_data { get; set; }
    }
}