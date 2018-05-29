﻿using System;
using System.ServiceProcess;
using Common.Logging;
using Microsoft.Owin.Hosting;
using System.Configuration;

namespace TimerQuartzService
{
    public partial class TimerService : ServiceBase
    {
        static readonly ILog log = LogManager.GetLogger("JobLogAppender"); //日志信息

        private IDisposable apiserver = null;

        public TimerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string serveruri = ConfigurationManager.AppSettings["WebAPIServerURI"].ToString();
            // Start OWIN host
            apiserver = WebApp.Start<Startup>(url: serveruri);

            log.Info("服务启动");

            //启动所有的定时任务
            QuartzManager<BaseJobObj>.startJobs();
            //重新添加未完成的任务
            StartJobHelper.Start();

        }

        protected override void OnStop()
        {
            if (apiserver != null)
                apiserver.Dispose();

            log.Info("服务停止");
            QuartzManager<BaseJobObj>.ShutdownJobs();
            log.Info("定时任务已全部停止");
        }
    }
}
