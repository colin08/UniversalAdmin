using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL.Model
{
    /// <summary>
    /// 项目拆迁情况
    /// </summary>
    public class ProjectStage
    {
        public ProjectStage()
        {
            this.file_list = new List<ProjectStageFile>();
        }

        /// <summary>
        /// 期ID
        /// </summary>
        public int stage_id { get; set; }
        
        public string title { get; set; }

        /// <summary>
        /// 拆迁开始时间
        /// </summary>
        public string begin_time { get; set; }

        /// <summary>
        /// 总户数
        /// </summary>
        public string ZongHuShu { get; set; }

        /// <summary>
        /// 已签约户数
        /// </summary>
        public string YiQYHuShu { get; set; }

        // <summary>
        /// 未签约户数
        /// </summary>
        public string WeiQYHuShu { get; set; }

        // <summary>
        /// 占地面积
        /// </summary>
        public string ZhanDiMianJi { get; set; }

        // <summary>
        /// 基底面积
        /// </summary>
        public string JiDiMianJi { get; set; }

        // <summary>
        /// 空地面积
        /// </summary>
        public string KongDiMianJi { get; set; }

        // <summary>
        /// 已签约面积
        /// </summary>
        public string YiQYMianJi { get; set; }

        // <summary>
        /// 未签约面积
        /// </summary>
        public string WeiQYMianJi { get; set; }

        // <summary>
        /// 房屋拆除(占地面积)
        /// </summary>
        public string ChaiZhanDiMianJi { get; set; }

        // <summary>
        /// 房屋拆除(建筑面积)
        /// </summary>
        public string ChaiJianZhuMianJi { get; set; }

        // <summary>
        /// 房屋拆除(补偿面积)
        /// </summary>
        public string ChaiBuChangMianJi { get; set; }

        // <summary>
        /// 房屋拆除(补偿金额)
        /// </summary>
        public string ChaiBuChangjinE { get; set; }


        /// <summary>
        /// 文件数量
        /// </summary>
        public List<ProjectStageFile> file_list { get; set; }

    }

    /// <summary>
    /// 项目拆迁附件
    /// </summary>
    public class ProjectStageFile
    {

        public string file_name { get; set; }

        /// <summary>
        /// 附件路径
        /// </summary>
        public string file_path { get; set; }

        /// <summary>
        /// 附件大小
        /// </summary>
        public string file_size { get; set; }

    }

}
