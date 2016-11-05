using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 自定义表单基类字段
    /// </summary>
    public class ViewModelCustomFormBase
    {
        /// <summary>
        /// 操作状态(0：默认情况,1：处理成功，2：数据不存在，3：数据验证有误)
        /// </summary>
        public int Msg { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgBox { get; set; }

    }
}