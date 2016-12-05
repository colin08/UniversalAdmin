using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 下载记录
    /// </summary>
    public class DownloadLog
    {
        public DownloadLog()
        {
            this.AddTime = DateTime.Now;
        }
        public int ID { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        [MaxLength(500)]
        public string Title { get; set; }

        public DateTime AddTime { get; set; }

    }
}
