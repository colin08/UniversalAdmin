using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.DataCore.Entity
{
    /// <summary>
    /// 表单
    /// </summary>
    public class BaseAdminFrom
    {

        /// <summary>
        /// 状态 -1:(数据不存在等)，1：成功并跳转，2：数据验证出错
        /// </summary>
        [NotMapped]
        public int Msg { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [NotMapped]
        public string MsgBox { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>
        [NotMapped]
        public string RedirectUrl { get; set; }

    }
}
