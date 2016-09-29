using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Web.Framework
{
    public class WebApiMonitorLog
    {
        public string ControllerName
        {
            get;
            set;
        }
        public string ActionName
        {
            get;
            set;
        }

        public DateTime ExecuteStartTime
        {
            get;
            set;
        }
        public DateTime ExecuteEndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 请求的Action 参数
        /// </summary>
        public Dictionary<string, object> ActionParams
        {
            get;
            set;
        }
        /// <summary>
        /// Http请求头
        /// </summary>
        public string HttpRequestHeaders
        {
            get;
            set;
        }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string HttpMethod
        {
            get;
            set;
        }
        /// <summary>
        /// 请求的IP地址
        /// </summary>
        public string IP
        {
            get;
            set;
        }

        /// <summary>
        /// 请求的地址
        /// </summary>
        public string Uri
        {
            get;
            set;
        }

        /// <summary>
        /// 获取监控指标日志
        /// </summary>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public string GetLoginfo()
        {
            string Msg = @"
            Action执行时间监控：
            请求URI：{0}
            ControllerName：{1}Controller
            ActionName:{2}
            开始时间：{3}
            结束时间：{4}
            总 时 间：{5}秒
            Action参数：{6}
            Http请求头:{7}
            客户端IP：{8},
            HttpMethod:{9}
                    ";
            return string.Format(Msg,
                Uri,
                ControllerName,
                ActionName,
                ExecuteStartTime,
                ExecuteEndTime,
                (ExecuteEndTime - ExecuteStartTime).TotalMilliseconds,
                GetCollections(ActionParams),
                 HttpRequestHeaders,
                Tools.WebHelper.GetIP(),
                HttpMethod);

        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        public Entity.SysLogApiAction GetLogEntity()
        {
            double TotalMilliseconds = (ExecuteEndTime - ExecuteStartTime).TotalMilliseconds;
            string actionParams = GetCollections(ActionParams);
            string ip = Tools.WebHelper.GetIP();
            return new Entity.SysLogApiAction()
            {
                ActionName = ActionName,
                ControllerName = ControllerName,
                Uri = Uri,
                ExecuteStartTime = ExecuteStartTime,
                ExecuteEndTime = ExecuteEndTime,
                ActionParams = actionParams,
                IP = ip,
                HttpMethod = HttpMethod,
                ExecuteTime = TotalMilliseconds,
                HttpRequestHeaders = HttpRequestHeaders
            };
        }

        /// <summary>
        /// 获取Action 参数
        /// </summary>
        /// <param name="Collections"></param>
        /// <returns></returns>
        public string GetCollections(Dictionary<string, object> Collections)
        {
            string Parameters = string.Empty;
            if (Collections == null || Collections.Count == 0)
            {
                return Parameters;
            }
            foreach (string key in Collections.Keys)
            {
                Parameters += string.Format("{0}={1}&", key, Collections[key]);
            }
            if (!string.IsNullOrWhiteSpace(Parameters) && Parameters.EndsWith("&"))
            {
                Parameters = Parameters.Substring(0, Parameters.Length - 1);
            }
            return Parameters;
        }
    }
}
