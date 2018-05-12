using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 附件类别
    /// </summary>
    public enum ConsultationFileType : byte
    {
        Image = 1,
        Voice = 2
    }

    /// <summary>
    /// 咨询附件
    /// </summary>
    public class ConsultationFile
    {
        public ConsultationFile()
        {
            this.FilePath = "";
        }

        public ConsultationFile(ConsultationFileType type,string file_path)
        {
            this.Type = type;
            this.FilePath = file_path;
        }

        public int ID { get; set; }

        /// <summary>
        /// 咨询ID
        /// </summary>
        public int ConsultationID { get; set; }

        /// <summary>
        /// 帖子信息
        /// </summary>
        public virtual Consultation Consultation { get; set; }

        /// <summary>
        /// 附件类别
        /// </summary>
        public ConsultationFileType Type { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        [MaxLength(500)]
        public string FilePath { get; set; }
    }

    /// <summary>
    /// 咨询互相回复内容附件
    /// </summary>
    public class ConsultationListFile
    {
        public ConsultationListFile()
        {
            this.FilePath = "";
        }

        public ConsultationListFile(ConsultationFileType type, string file_path)
        {
            this.Type = type;
            this.FilePath = file_path;
        }

        public int ID { get; set; }

        /// <summary>
        /// 回复ID
        /// </summary>
        public int ConsultationListID { get; set; }

        /// <summary>
        /// 帖子信息
        /// </summary>
        public virtual ConsultationList ConsultationInfo { get; set; }

        /// <summary>
        /// 附件类别
        /// </summary>
        public ConsultationFileType Type { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        [MaxLength(500)]
        public string FilePath { get; set; }


    }
}
