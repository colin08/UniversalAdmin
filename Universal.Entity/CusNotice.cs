using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 公告
    /// </summary>
    public class CusNotice
    {
        public CusNotice()
        {
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required, MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required,Column(TypeName ="text")]
        public string Content { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        public virtual CusUser CusUser { get; set; }

        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 通知用户
        /// </summary>
        public virtual ICollection<CusNoticeUser> CusNoticeUser { get; set; }

    }
}
