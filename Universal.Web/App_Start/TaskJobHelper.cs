using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Universal.Tools;

namespace Universal.Web
{
    /// <summary>
    /// 跟计划任务数据交互
    /// </summary>
    public class TaskJobHelper
    {
        /// <summary>
        /// 添加一个任务-咨询超时自动完成
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pay_time"></param>
        /// <param name="time_out"></param>
        /// <returns></returns>
        public static bool AddAdvisoryDone(int id,DateTime pay_time,int time_out)
        {
            string url = string.Format("http://127.0.0.1:9876/api/advisory/add/done?id={0}&pay_time={1}&time_out={2}", id.ToString(), pay_time.ToString("yyyy-MM-dd HH:mm:ss"), time_out.ToString());
            WebHelper.HttpGet(url);
            return true;
        }

        /// <summary>
        /// 添加一个任务-咨询超时自动退款
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pay_time"></param>
        /// <param name="time_out"></param>
        /// <returns></returns>
        public static bool AddAdvisoryRefund(int id, DateTime pay_time, int time_out)
        {
            string url = string.Format("http://127.0.0.1:9876/api/advisory/add/refund?id={0}&pay_time={1}&time_out={2}", id.ToString(), pay_time.ToString("yyyy-MM-dd HH:mm:ss"), time_out.ToString());
            WebHelper.HttpGet(url);
            return true;
        }
    }
}