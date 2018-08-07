using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 用户体检报告
    /// </summary>
    public class MpUserMedicalReport
    {
        public MpUserMedicalReport()
        {
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }


        [MaxLength(30)]
        public string IDCardNumber { get; set; }

        /// <summary>
        /// 报告标题
        /// </summary>
        [MaxLength(255)]
        public string Title { get; set; }

        [NotMapped]
        public string GetTitle
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Title)) return "";
                return Tools.StringHelper.SubString(Title, 10);
            }
        }

        /// <summary>
        /// 报告pdf附件地址
        /// </summary>
        public string FilePath { get; set; }


        public DateTime AddTime { get; set; }

        [NotMapped]
        public string GetTime
        {
            get
            {
                return AddTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

    }
}
