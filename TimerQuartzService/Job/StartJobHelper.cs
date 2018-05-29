using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace TimerQuartzService
{
    /// <summary>
    /// 服务启动任务帮助类
    /// </summary>
    public class StartJobHelper
    {
        /// <summary>  
        /// JOB状态日志  
        /// </summary>  
        protected internal static readonly ILog jobStatus = LogManager.GetLogger("JobLogAppender");

        /// <summary>
        /// 当服务启动时调用
        /// </summary>
        public static void Start()
        {
            //从数据库里获取设置咨询超时完成
            SetAdvisoryDone();

            //从数据库里获取设置咨询超时退款
            SetAdvisoryRefund();
        }

        /// <summary>
        /// 从数据库里获取设置咨询超时完成
        /// </summary>
        static void SetAdvisoryDone()
        {
            string site_host = System.Configuration.ConfigurationManager.AppSettings["HDMP"].ToString();
            string site_url = site_host + "/Intranet/TaskGetAdvisoryDoneIds";
            string result_json = Tools.WebHelper.HttpGet(site_url);
            Model.AdvisoryTimeOutAPI result_model;
            try
            {
                result_model = Tools.JsonHelper.ToObject<Model.AdvisoryTimeOutAPI>(result_json);
            }
            catch (Exception ex)
            {
                jobStatus.Error("获取咨询超时完成列表出错：" + ex.Message);
                return;
            }
            if (result_model == null) return;
            if (result_model.data_list == null) return;
            foreach (var item in result_model.data_list)
            {
                Job.CreateJobHelper.AddAdvisoryDone(item.id, item.pay_time, result_model.time_out);
            }
            jobStatus.Info("启动添加咨询超时完成，添加数据数量：" + result_model.data_list.Count.ToString());
        }

        /// <summary>
        /// 从数据库里获取设置咨询超时退款
        /// </summary>
        static void SetAdvisoryRefund()
        {
            string site_host = System.Configuration.ConfigurationManager.AppSettings["HDMP"].ToString();
            string site_url = site_host + "/Intranet/TaskGetAdvisoryRefundIds";
            string result_json = Tools.WebHelper.HttpGet(site_url);
            Model.AdvisoryTimeOutAPI result_model;
            try
            {
                result_model = Tools.JsonHelper.ToObject<Model.AdvisoryTimeOutAPI>(result_json);
            }
            catch (Exception ex)
            {
                jobStatus.Error("获取咨询超时退款列表出错：" + ex.Message);
                return;
            }
            if (result_model == null) return;
            if (result_model.data_list == null) return;
            foreach (var item in result_model.data_list)
            {
                Job.CreateJobHelper.AddAdvisoryRefund(item.id, item.pay_time, result_model.time_out);
            }
            jobStatus.Info("启动添加咨询超时退款，添加数据数量：" + result_model.data_list.Count.ToString());
        }
    }
}
