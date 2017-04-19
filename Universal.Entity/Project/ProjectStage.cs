using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Universal.Entity
{
    /// <summary>
    /// 项目分期情况
    /// </summary>
    public class ProjectStage
    {
        public ProjectStage()
        {
            this.FileList = new List<Entity.ProjectStageFile>();
            this.BeginTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// 分期标题
        /// </summary>
        [MaxLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// 拆迁开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 总户数
        /// </summary>
        [MaxLength(50)]
        public string ZongHuShu { get; set; }

        /// <summary>
        /// 已签约户数
        /// </summary>
        [MaxLength(50)]
        public string YiQYHuShu { get; set; }

        // <summary>
        /// 未签约户数
        /// </summary>
        [MaxLength(50)]
        public string WeiQYHuShu { get; set; }

        // <summary>
        /// 占地面积
        /// </summary>
        [MaxLength(50)]
        public string ZhanDiMianJi { get; set; }

        // <summary>
        /// 基底面积
        /// </summary>
        [MaxLength(50)]
        public string JiDiMianJi { get; set; }

        // <summary>
        /// 空地面积
        /// </summary>
        [MaxLength(50)]
        public string KongDiMianJi { get; set; }

        // <summary>
        /// 已签约面积
        /// </summary>
        [MaxLength(50)]
        public string YiQYMianJi { get; set; }

        // <summary>
        /// 未签约面积
        /// </summary>
        [MaxLength(50)]
        public string WeiQYMianJi { get; set; }

        // <summary>
        /// 房屋拆除(占地面积)
        /// </summary>
        [MaxLength(50)]
        public string ChaiZhanDiMianJi { get; set; }

        // <summary>
        /// 房屋拆除(建筑面积)
        /// </summary>
        [MaxLength(50)]
        public string ChaiJianZhuMianJi { get; set; }

        // <summary>
        /// 房屋拆除(补偿面积)
        /// </summary>
        [MaxLength(50)]
        public string ChaiBuChangMianJi { get; set; }

        // <summary>
        /// 房屋拆除(补偿金额)
        /// </summary>
        [MaxLength(50)]
        public string ChaiBuChangjinE { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public ICollection<ProjectStageFile> FileList { get; set; }

    }
}
