using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Universal.Entity
{
    /// <summary>
    /// 节点附件
    /// </summary>
    public class NodeFile
    {
        public NodeFile()
        {
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 节点ID
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        public virtual Node Node { get; set; }

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

        /// <summary>
        /// 上传人员ID
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 上传人员信息
        /// </summary>
        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime AddTime { get; set; }

    }
}
