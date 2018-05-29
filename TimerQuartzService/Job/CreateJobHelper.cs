using Common.Logging;
using System;
using System.Collections.Generic;

namespace TimerQuartzService.Job
{
    /// <summary>
    /// 创建任务帮助
    /// </summary>
    public class CreateJobHelper
    {
        /// <summary>  
        /// JOB状态日志  
        /// </summary>  
        protected internal static readonly ILog jobStatus = LogManager.GetLogger("JobLogAppender");

        /// <summary>
        /// 添加咨询超时自动结束任务
        /// </summary>
        /// <param name="id">咨询ID</param>
        /// <param name="pay_time">支付时间</param>
        /// <param name="time_out">超时时间-小时</param>
        public static void AddAdvisoryDone(int id, DateTime pay_time, int time_out)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("id", id);

            DateTime dt = pay_time.AddHours(time_out);

            if (dt < DateTime.Now)
                dt = DateTime.Now.AddSeconds(20);

            string cron = dt.Second.ToString() + " " +
                    dt.Minute.ToString() + " " +
                    dt.Hour.ToString() + " " +
                    dt.Day.ToString() + " " +
                    dt.Month.ToString() + " ? " +
                    dt.Year.ToString();
            QuartzManager<Job_AdvisoryDone>.addJob("task_advisory_done_" + id, cron, dic);
            jobStatus.Info("【咨询超时自动结束】添加ID为：" + id + "的任务，执行时间：" + dt.ToString());
        }


        /// <summary>
        /// 添加咨询超时自动退款任务
        /// </summary>
        /// <param name="id">咨询ID</param>
        /// <param name="pay_time">支付时间</param>
        /// <param name="time_out">超时时间-小时</param>
        public static void AddAdvisoryRefund(int id, DateTime pay_time, int time_out)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("id", id);

            DateTime dt = pay_time.AddHours(time_out);

            if (dt < DateTime.Now)
                dt = DateTime.Now.AddSeconds(20);

            string cron = dt.Second.ToString() + " " +
                    dt.Minute.ToString() + " " +
                    dt.Hour.ToString() + " " +
                    dt.Day.ToString() + " " +
                    dt.Month.ToString() + " ? " +
                    dt.Year.ToString();
            QuartzManager<Job_AdvisoryRefund>.addJob("task_advisory_refund_" + id, cron, dic);
            jobStatus.Info("【咨询超时自动退款】添加ID为：" + id + "的任务，执行时间：" + dt.ToString());
        }

    }

}
