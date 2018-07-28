using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Web.Framework
{
    public class WebErrorModel
    {
        public WebErrorModel()
        {

        }
        public WebErrorModel(string back_url,string msg)
        {
            this.back_url = back_url;
            this.message = msg;
        }

        /// <summary>
        /// 返回地址
        /// </summary>
        public string back_url { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string message { get; set; }
    }
}
