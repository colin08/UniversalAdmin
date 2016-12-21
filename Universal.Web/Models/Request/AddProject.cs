using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Request
{
    /// <summary>
    /// 新建项目所需参数
    /// </summary>
    public class AddProject
    {
        public AddProject()
        {
            this.file_list = new List<Response.ProjectFile>();
        }

        /// <summary>
        /// 当前登录用户ID
        /// </summary>
        public int user_id { get; set; }

        public string title { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public int approve_user_id { get; set; }

        /// <summary>
        /// 引用的流程ID
        /// </summary>
        public int flow_id { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public Entity.DocPostSee post_see { get; set; }

        /// <summary>
        /// 可以看的用户或部门id，逗号分割
        /// </summary>
        public string see_ids { get; set; }
        

        /// <summary>
        /// 项目联系人
        /// </summary>
        public string user_ids { get; set; }
        
        /// <summary>
        /// 项目附件
        /// </summary>
        public List<Models.Response.ProjectFile> file_list { get; set; }

        /// <summary>
        /// 项目区域
        /// </summary>
        public Entity.ProjectArea area { get; set; }

        /// <summary>
        /// 改造性质
        /// </summary>
        public Entity.ProjectGaiZao GaiZaoXingZhi { get; set; }

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
        public DateTime LiXiangTime { get; set; }

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
}