using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 咨询结算显示
    /// </summary>
    public class Settlement
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public string amount { get; set; }
        
        /// <summary>
        /// 结算价
        /// </summary>
        public string rel_amount { get; set; }

        /// <summary>
        /// 打款状态文本
        /// </summary>
        public string pay_s_str { get; set; }

        /// <summary>
        /// 打款状态说明
        /// </summary>
        public string pay_desc { get; set; }
        
        /// <summary>
        /// 审核状态文本
        /// </summary>
        public string s_str { get; set; }

        /// <summary>
        /// 审核状态说明
        /// </summary>
        public string s_desc { get; set; }

    }
}
