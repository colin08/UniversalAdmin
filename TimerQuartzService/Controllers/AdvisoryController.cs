using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TimerQuartzService.Controllers
{
    public class AdvisoryController : ApiController
    {
        static readonly ILog log = LogManager.GetLogger("JobLogAppender"); //日志信息

        [Route("api/advisory/add/done")]
        [HttpGet]
        public APIResponseEntity<string> AddDone(int id,DateTime pay_time,int time_out)
        {
            APIResponseEntity<string> response_entity = new APIResponseEntity<string>();
            response_entity.msg = 1;
            response_entity.msgbox = "success";
            Job.CreateJobHelper.AddAdvisoryDone(id, pay_time, time_out);
            //同时移除超时未回复退款接口
            QuartzManager<Job_AdvisoryRefund>.RemoveJob("task_advisory_refund_" + id.ToString());
            return response_entity;
        }

        [Route("api/advisory/add/refund")]
        [HttpGet]
        public APIResponseEntity<string> AddRefund(int id, DateTime pay_time, int time_out)
        {
            APIResponseEntity<string> response_entity = new APIResponseEntity<string>();
            response_entity.msg = 1;
            response_entity.msgbox = "success";
            Job.CreateJobHelper.AddAdvisoryRefund(id, pay_time, time_out);
            return response_entity;
        }
    }
}
