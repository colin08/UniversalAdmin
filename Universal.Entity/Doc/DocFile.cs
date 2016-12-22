using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Universal.Entity
{
    /// <summary>
    /// 秘籍附件
    /// </summary>
    public class DocFile
    {
        public DocFile()
        {
            this.AddTime = DateTime.Now;
        }


        public int ID { get; set; }
        
        /// <summary>
        /// 秘籍ID
        /// </summary>
        public int DocPostID { get; set; }

        /// <summary>
        /// 秘籍
        /// </summary>
        public virtual DocPost DocPost { get; set; }

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
        /// 上传时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
