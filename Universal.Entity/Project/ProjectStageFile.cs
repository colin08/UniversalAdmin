using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Universal.Entity
{
    /// <summary>
    /// 项目分期附件
    /// </summary>
    public class ProjectStageFile
    {
        public int ID { get; set; }

        /// <summary>
        /// 分期ID
        /// </summary>
        public int ProjectStageID { get; set; }

        /// <summary>
        /// 分期信息
        /// </summary>
        public virtual ProjectStage ProjectStage { get; set; }

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
