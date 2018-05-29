using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.TaskModel
{
    /// <summary>
    /// 重启后获取需要添加到超过3天设置咨询为结束状态的实体
    /// </summary>
    public class AdvisoryTimeOut
    {
        /// <summary>
        /// 咨询ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime pay_time { get; set; }
    }

    public class AdvisoryTimeOutAPI
    {
        public int time_out { get; set; }

        public List<AdvisoryTimeOut> data_list { get; set; }

    }

}