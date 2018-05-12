using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 咨询内容列表
    /// </summary>
    public class ConsultationList
    {
        public ConsultationList()
        {
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 主题ID
        /// </summary>
        public int ConsultationID { get; set; }

        /// <summary>
        /// 咨询主题信息
        /// </summary>
        public virtual Consultation Consultation { get; set; }

        /// <summary>
        /// 回复的用户类别
        /// </summary>
        public ReplayUserType UserType { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string Content { get; set; }


        public DateTime AddTime { get; set; }

        /// <summary>
        /// 咨询附件
        /// </summary>
        public virtual ICollection<ConsultationListFile> ConsultationListFiles { get; set; }

    }
}
