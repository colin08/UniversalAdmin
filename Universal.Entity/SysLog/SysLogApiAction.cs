using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Entity
{
    public class SysLogApiAction
    {
        public int ID { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        [MaxLength(500)]
        public string Uri { get; set; }

        /// <summary>
        /// 控制器名字
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        [MaxLength(10)]
        public string ActionName { get; set; }

        /// <summary>
        /// 请求开始时间
        /// </summary>
        public DateTime ExecuteStartTime { get; set; }

        /// <summary>
        /// 请求结束时间
        /// </summary>
        public DateTime ExecuteEndTime { get; set; }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public double ExecuteTime { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [MaxLength()]
        public string ActionParams { get; set; }

        /// <summary>
        /// Http请求报文头
        /// </summary>
        [MaxLength()]
        public string HttpRequestHeaders { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [MaxLength(20)]
        public string IP { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        [MaxLength(5)]
        public string HttpMethod { get; set; }

    }
}
