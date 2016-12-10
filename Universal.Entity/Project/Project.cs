using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace Universal.Entity
{
    /// <summary>
    /// 项目区域
    /// </summary>
    public enum ProjectArea
    {
        [Description("其他")]
        QiTa,
        [Description("宝安区")]
        BaoAn,
        [Description("龙岗区")]
        LongGang,
        [Description("南山区")]
        NanShan,
        [Description("福田区")]
        FuTian,
        [Description("罗湖区")]
        LuoHu,
        [Description("盐田区")]
        YanTian,
        [Description("龙华新区")]
        LongHua,
        [Description("光明新区")]
        GuangMing,
        [Description("坪山新区")]
        PingShan,
        [Description("大鹏新区")]
        DaPing
    }

    /// <summary>
    /// 项目改造性质
    /// </summary>
    public enum ProjectGaiZao
    {
        [Description("其他")]
        QiTa,
        [Description("工改工")]
        GongGaiGong,
        [Description("工改商")]
        GongGaiShang,
        [Description("工改居")]
        GongGaiJu,
        [Description("商改居")]
        ShangGaiJu,
        [Description("居改居")]
        JuGaiJu
    }

    /// <summary>
    /// 项目信息
    /// </summary>
    public class Project
    {
        public Project()
        {
            this.ProjectUsers = new List<ProjectUser>();
            this.ProjectFiles = new List<ProjectFile>();
            this.See = DocPostSee.everyone;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
        }

        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// 项目所属人员(责任人)
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 项目所属人员信息(责任人)
        /// </summary>
        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 审核人员ID
        /// </summary>
        public int ApproveUserID { get; set; }

        /// <summary>
        /// 审核人员信息
        /// </summary>
        [ForeignKey("ApproveUserID")]
        public virtual CusUser ApproveUser { get; set; }
        
        /// <summary>
        /// 当前项目节点信息
        /// </summary>
        [NotMapped]
        public virtual ProjectFlowNode NowNode { get; set; }

        /// <summary>
        /// 引用的流程ID
        /// </summary>
        public int FlowID { get; set; }

        /// <summary>
        /// 权限类别
        /// </summary>
        public DocPostSee See { get; set; }

        /// <summary>
        /// 可见部门ID或用户ID，逗号分割，前后要加逗号:,1,2,3,4,
        /// </summary>
        [MaxLength(1000)]
        public string TOID { get; set; }

        /// <summary>
        /// 此项目引用到的流程块,逗号分割，前后要加逗号:,1,2,3,4,
        /// </summary>
        [MaxLength(100)]
        public string Pieces { get; set; }

        /// <summary>
        /// 项目区域
        /// </summary>
        public ProjectArea Area { get; set; }

        /// <summary>
        /// 改造性质
        /// </summary>
        public ProjectGaiZao GaiZaoXingZhi { get; set; }

        /// <summary>
        /// 用地性质、宗地号
        /// </summary>
        [MaxLength(200)]
        public string ZhongDiHao { get; set; }

        /// <summary>
        /// 申报主体
        /// </summary>
        [MaxLength(200)]
        public string ShenBaoZhuTi { get; set; }

        /// <summary>
        /// 总建筑面积
        /// </summary>        
        public decimal ZongMianJi { get; set; }

        /// <summary>
        /// 总面积其他信息
        /// </summary>
        [MaxLength(200)]
        public string ZongMianJiOther { get; set; }

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
        [MaxLength(200)]
        public string TuDiShiYongQuan { get; set; }

        /// <summary>
        /// 建设规划许可证
        /// </summary>
        [MaxLength(200)]
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
        public DateTime LiXiangTime { get; set; }

        /// <summary>
        /// 统计使用，年度,根据立项时间来获取
        /// </summary>
        public int TJYear { get; set; }

        /// <summary>
        /// 统计使用，季度,根据立项时间来获取
        /// </summary>
        public int TJQuarter { get; set; }

        /// <summary>
        /// 设置年份
        /// </summary>
        public void SetYear()
        {
            TJYear = LiXiangTime.Year;
        }

        /// <summary>
        /// 设置季度
        /// </summary>
        public void SetQuarter()
        {
            double f = Convert.ToDouble(LiXiangTime.Month) / 3f;
            if (f > Convert.ToInt32(f))
            {
                TJQuarter = Convert.ToInt32(f) + 1;
            }
            TJQuarter = Convert.ToInt32(f);
        }
        
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
        [MaxLength(20)]
        public string FenChengBiLi { get; set; }

        /// <summary>
        /// 均价（单位：千）
        /// </summary>
        public decimal JunJia { get; set; }

        public DateTime AddTime { get; set; }

        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 是否收藏
        /// </summary>
        [NotMapped]
        public bool IsFavorites { get; set; }

        /// <summary>
        /// 项目联系人
        /// </summary>
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        
        /// <summary>
        /// 项目附件
        /// </summary>
        public ICollection<ProjectFile> ProjectFiles { get; set; }
    }
}
