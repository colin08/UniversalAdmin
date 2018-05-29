using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Quartz;
using System.Configuration;

namespace TimerQuartzService
{
    /// <summary>
    /// 在线咨询退款计划任务
    /// </summary>
    public class Job_AdvisoryRefund : BaseJobObj
    {
        public override void JobExcute(IJobExecutionContext context)
        {
            string site_host = ConfigurationManager.AppSettings["HDMP"].ToString();
            JobKey key = context.JobDetail.Key;
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string id = dataMap.GetString("id");
            string site_url = site_host + "/Intranet/TaskSetAdvisoryRefund?id=" + id;
            Tools.WebHelper.HttpGet(site_url);
            jobStatus.Info("咨询自动结束：" + site_url);
            base.JobExcute(context);
        }
    }
}
