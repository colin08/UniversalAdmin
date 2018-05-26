using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 医生端用户咨询列表
    /// </summary>
    public class ConsultationDoctor
    {
        public int id { get; set; }

        /// <summary>
        /// 1：进行中，2:已关闭
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string gender { get; set; }

        /// <summary>
        /// 疾病类型
        /// </summary>
        public string disease { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 最后回复时间
        /// </summary>
        public DateTime last_reply_time { get; set; }

        /// <summary>
        /// 最后回复时间，多少分钟前
        /// </summary>
        public string last_reply_time_str { get; set; }

        /// <summary>
        /// 最后回复用户类别
        /// </summary>
        public ReplayUserType last_replay_user { get; set; }

        /// <summary>
        /// 最后回复内容
        /// </summary>
        public string last_reply_content { get; set; }
        
        /// <summary>
        /// 关闭原因
        /// </summary>
        public string close_desc { get; set; }
                
        /// <summary>
        /// 咨询价格
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 结算状态说明
        /// </summary>
        public string sett_desc { get; set; }

    }

    /// <summary>
    /// 用户端我的咨询列表
    /// </summary>
    public class ConsultationUser
    {
        public int id { get; set; }


        /// <summary>
        /// 订单号
        /// </summary>
        public string order_num { get; set; }

        /// <summary>
        /// 1：进行中，2:已关闭
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 头衔
        /// </summary>
        public string touxian { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 最后回复时间
        /// </summary>
        public DateTime last_reply_time { get; set; }

        /// <summary>
        /// 最后回复时间，多少分钟前
        /// </summary>
        public string last_reply_time_str { get; set; }

        /// <summary>
        /// 最后回复用户类别
        /// </summary>
        public ReplayUserType last_replay_user { get; set; }

        /// <summary>
        /// 最后回复内容
        /// </summary>
        public string last_reply_content { get; set; }

        /// <summary>
        /// 关闭原因
        /// </summary>
        public string close_desc { get; set; }
    }
}
