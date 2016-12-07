using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Universal.Entity
{
    public enum ProjectFileType:byte
    {
        //附件
        file=1,
        //相册
        album
    }

    /// <summary>
    /// 项目附件
    /// </summary>
    public class ProjectFile
    {
        public ProjectFile()
        {
            this.Type = ProjectFileType.file;
            this.AddTime = DateTime.Now;
        }


        public int ID { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public ProjectFileType Type { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public virtual Project Project { get; set; }

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
