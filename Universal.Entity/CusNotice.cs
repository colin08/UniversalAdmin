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
        /// 权限类别
        /// </summary>
        public DocPostSee See { get; set; }

        /// <summary>
        /// 通知范围
        /// </summary>
        [NotMapped]
        public string NoticeStr
        {
            get
            {
                switch (this.See)
                {
                    case DocPostSee.everyone:
                        return "所有人";
                    case DocPostSee.department:
                        return "特定部门";
                    case DocPostSee.user:
                        return "特定用户";
                    default:
                        return "未知";
                }
            }
        }

        /// <summary>
        /// 部门ID或用户ID，逗号分割，前后要加逗号:,1,2,3,4,
        /// </summary>
        [MaxLength(1000)]
        public string TOID { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
        
    }
}
