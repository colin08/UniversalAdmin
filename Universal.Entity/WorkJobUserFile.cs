using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity
{
    /// <summary>
    /// 任务指派完成反馈附件
    /// </summary>
    public class WorkJobUserFile
    {
        public int ID { get; set; }

        public int WorkJobUserID { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual WorkJobUser WorkJobUser { get; set; }

        /// <summary>
        /// 附件名称，使用原名称
        /// </summary>
        [MaxLength(200)]
        public string FileName { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        [MaxLength(500)]
        public string FilePath { get; set; }

        /// <summary>
        /// 附件大小,KB或MB显示
        /// </summary>
        [MaxLength(30)]
        public string FileSize { get; set; }

    }
}
