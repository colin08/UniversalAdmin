using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{

    /// <summary>
    /// 项目基本信息
    /// </summary>
    public class ProjectBasicInfo
    {
        public int project_id { get; set; }

        /// <summary>
        /// 是否收藏
        /// </summary>
        public bool is_fav { get; set; }

        public int create_user_id { get; set; }

        /// <summary>
        /// 项目收藏id
        /// </summary>
        public int favorites_id { get; set; }

    }

    /// <summary>
    /// 项目动态中显示的数据
    /// </summary>
    public class ProjectListInfo:ProjectBasicInfo
    {
        public ProjectListInfo()
        {
            this.file_list = new List<ProjectFile>();
        }

        /// <summary>
        /// 项目标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 项目责任人名称
        /// </summary>
        public string user_name { get; set; }
        

        /// <summary>
        /// 当前节点
        /// </summary>
        public string node_title { get; set; }

        /// <summary>
        /// 当前节点描述
        /// </summary>
        public string node_content { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime last_update_time { get; set; }

        /// <summary>
        /// 相册
        /// </summary>
        public List<ProjectFile> file_list { get; set; }

    }

    /// <summary>
    /// 项目基本信息
    /// </summary>
    public class ProjectInfo: ProjectBasicInfo
    {
        public ProjectInfo()
        {
            this.see_model = new List<SeeModel>();
            this.file_list = new List<ProjectFile>();
            this.contact_users = new List<SelectUser>();
        }
        
        /// <summary>
        /// 项目责任人（添加者）
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// 项目责任人名称
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 项目责任人电话
        /// </summary>
        public string user_telphone { get; set; }

        /// <summary>
        /// 审批人id
        /// </summary>
        public int approve_id { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public string approve_name { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public Entity.ApproveStatusType approve_status { get; set; }

        /// <summary>
        /// 审批备注
        /// </summary>
        public string approve_remark { get; set; }

        /// <summary>
        /// 项目标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 流程iD
        /// </summary>
        public int flow_id { get; set; }

        /// <summary>
        /// 项目流程名称
        /// </summary>
        public string flow_name { get; set; }

        /// <summary>
        /// 查看权限
        /// </summary>
        public Entity.DocPostSee post_see { get; set; }

        /// <summary>
        /// 拥有权限的部门id或用户id
        /// </summary>
        public string see_ids { get; set; }

        /// <summary>
        /// 拥有权限的部门或用户
        /// </summary>
        public List<SeeModel> see_model { get; set; }

        /// <summary>
        /// 联系人ID
        /// </summary>
        public string user_ids { get; set; }

        /// <summary>
        /// 项目联系人
        /// </summary>
        public List<SelectUser> contact_users { get; set; }

        /// <summary>
        /// 附件/相册
        /// </summary>
        public List<ProjectFile> file_list { get; set; }

    }

    /// <summary>
    /// 项目的项目信息
    /// </summary>
    public class ProjectInfoPro:ProjectBasicInfo
    {
        /// <summary>
        /// 项目区域
        /// </summary>
        public Entity.ProjectArea Area { get; set; }

        /// <summary>
        /// 改造性质
        /// </summary>
        public string GaiZaoXingZhi { get; set; }

        /// <summary>
        /// 用地性质、宗地号
        /// </summary>
        public string ZhongDiHao { get; set; }

        /// <summary>
        /// 申报主体
        /// </summary>
        public string ShenBaoZhuTi { get; set; }

        /// <summary>
        /// 总建筑面积
        /// </summary>        
        public decimal ZongMianJi { get; set; }

        /// <summary>
        /// 总面积其他信息
        /// </summary>
        public string ZongMianJiOther { get; set; }

        /// <summary>
        /// 更新单元用地面积
        /// </summary>
        public decimal GengXinDanYuanYongDiMianJi { get; set; }

        /// <summary>
        /// 五类权属用地面积
        /// </summary>
        public decimal WuLeiQuanMianJi { get; set; }

        /// <summary>
        /// 老屋村用地面积
        /// </summary>
        public decimal LaoWuCunMianJi { get; set; }

        /// <summary>
        /// 非农建设用地面积
        /// </summary>
        public decimal FeiNongMianJi { get; set; }

        /// <summary>
        /// 开发建设用地面积
        /// </summary>
        public decimal KaiFaMianJi { get; set; }

        /// <summary>
        /// 容积率
        /// </summary>
        public decimal RongJiLv { get; set; }

        /// <summary>
        /// 土地使用权出让合作书
        /// </summary>
        public string TuDiShiYongQuan { get; set; }

        /// <summary>
        /// 建设规划许可证
        /// </summary>
        public string JianSheGuiHuaZheng { get; set; }

        /// <summary>
        /// 拆迁建设用地面积
        /// </summary>
        public decimal ChaiQianYongDiMianJi { get; set; }

        /// <summary>
        /// 拆迁建筑面积
        /// </summary>
        public decimal ChaiQianJianZhuMianJi { get; set; }

        /// <summary>
        /// 立项时间
        /// </summary>
        public DateTime? LiXiangTime { get; set; }

        /// <summary>
        /// 专项规划时间
        /// </summary>
        public DateTime? ZhuanXiangTime { get; set; }

        /// <summary>
        /// 主体确认时间
        /// </summary>
        public DateTime? ZhuTiTime { get; set; }


        /// <summary>
        /// 用地审批时间
        /// </summary>
        public DateTime? YongDiTime { get; set; }

        /// <summary>
        /// 开盘时间
        /// </summary>
        public DateTime? KaiPanTime { get; set; }

        /// <summary>
        /// 分成比例
        /// </summary>
        public string FenChengBiLi { get; set; }

        /// <summary>
        /// 均价（单位：千）
        /// </summary>
        public decimal JunJia { get; set; }
    }

    /// <summary>
    /// 项目分期信息
    /// </summary>
    public class ProjectInfoStage : ProjectBasicInfo
    {
        public ProjectInfoStage()
        {
            this.file_list = new List<ProjectFile>();
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

        /// <summary>
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
        public List<ProjectFile> file_list { get; set; }
    }

    /// <summary>
    /// 项目附件，包含附件和相册
    /// </summary>
    public class ProjectFile
    {
        /// <summary>
        /// 类别,1:附件；2：相册
        /// </summary>
        public Entity.ProjectFileType type { get; set; }

        /// <summary>
        /// 附件名称，使用原名称
        /// </summary>
        public string file_name { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        public string file_path { get; set; }

        /// <summary>
        /// 附件大小,KB或MB显示
        /// </summary>
        public string file_size { get; set; }

    }

}