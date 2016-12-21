using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 编辑项目
    /// </summary>
    public class ViewModelProject: ViewModelCustomFormBase
    {

        public ViewModelProject()
        {
            this.user_ids = "";
            this.users_entity = new List<ViewModelDocumentCategory>();
            this.see_entity = new List<ViewModelDocumentCategory>();
            this.file_list = new List<ViewModelListFile>();
            this.LiXiangTime = DateTime.Now;
        }

        public int id { get; set; }

        [Required(ErrorMessage ="请填写项目名称"),MaxLength(200,ErrorMessage ="不能超过200个字符")]
        public string title { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [Required(ErrorMessage = "请选择审核人员")]
        public int approve_user_id { get; set; }

        public string approve_user_name { get; set; }

        /// <summary>
        /// 引用的流程ID
        /// </summary>
        [Required(ErrorMessage ="请选择流程")]
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
        /// 可以看的用户或部门信息，用于还原数据
        /// </summary>
        public List<ViewModelDocumentCategory> see_entity { get; set; }

        /// <summary>
        /// 项目联系人
        /// </summary>
        public string user_ids { get; set; }

        /// <summary>
        /// 项目联系人
        /// </summary>
        public List<ViewModelDocumentCategory> users_entity { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public string files { get; set; }

        /// <summary>
        /// 附件信息
        /// </summary>
        public List<ViewModelListFile> file_list { get; set; }

        /// <summary>
        /// 处理前端拼接的数据，并返回数据库所需数据
        /// </summary>
        public List<Entity.ProjectFile> BuildFileList()
        {
            List<Entity.ProjectFile> db_list = new List<Entity.ProjectFile>();
            if (this.files == null)
                return db_list;
            if (this.files.Length == 0)
                return db_list;
            if (this.files.EndsWith("|"))
                this.files = this.files.Substring(0, this.files.Length - 1);
            this.file_list.Clear();

            foreach (var item in files.Split('|'))
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                ViewModelListFile model = new ViewModelListFile();
                Entity.ProjectFile entity = new Entity.ProjectFile();
                string[] f_len = item.Split(',');
                if (f_len.Length == 3)
                {
                    model.file_path = f_len[0];
                    model.file_name = f_len[1];
                    model.file_size = f_len[2];

                    entity.FilePath = f_len[0];
                    entity.FileName = f_len[1];
                    entity.FileSize = f_len[2];
                    entity.Type = Entity.ProjectFileType.file;
                    db_list.Add(entity);
                }
                this.file_list.Add(model);
            }

            return db_list;
        }

        /// <summary>
        /// 构造前端展示所需数据
        /// </summary>
        /// <param name="entity"></param>
        public void BuildViewModelListFile(List<Entity.ProjectFile> entity)
        {
            if (entity == null)
                return;
            System.Text.StringBuilder files = new System.Text.StringBuilder();
            foreach (var item in entity)
            {
                if (this.file_list == null)
                    this.file_list = new List<ViewModelListFile>();

                if(item.Type == Entity.ProjectFileType.file)
                {
                    file_list.Add(new ViewModelListFile(item.FilePath, item.FileName, item.FileSize));
                    files.Append(item.FilePath + "," + item.FileName + "," + item.FileSize + "|");
                }
            }
            this.files = files.ToString();
        }


        /// <summary>
        /// 项目区域
        /// </summary>
        [Required(ErrorMessage ="请选择区域")]
        public Entity.ProjectArea area { get; set; }

        /// <summary>
        /// 改造性质
        /// </summary>
        [Required(ErrorMessage ="请选择改造性质")]
        public Entity.ProjectGaiZao GaiZaoXingZhi { get; set; }

        /// <summary>
        /// 用地性质、宗地号
        /// </summary>
        [MaxLength(200,ErrorMessage ="不能超过200个字符")]
        public string ZhongDiHao { get; set; }

        /// <summary>
        /// 申报主体
        /// </summary>
        [MaxLength(200, ErrorMessage = "不能超过200个字符")]
        public string ShenBaoZhuTi { get; set; }

        /// <summary>
        /// 总建筑面积
        /// </summary>  
        public decimal ZongMianJi { get; set; }
        
        /// <summary>
        /// 总面积其他信息
        /// </summary>
        [MaxLength(200, ErrorMessage = "不能超过200个字符")]
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
        [MaxLength(200, ErrorMessage = "不能超过200个字符")]
        public string TuDiShiYongQuan { get; set; }

        /// <summary>
        /// 建设规划许可证
        /// </summary>
        [MaxLength(200, ErrorMessage = "不能超过200个字符")]
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
        [Required(ErrorMessage ="必填")]
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
        [Required(ErrorMessage = "必填"),MaxLength(20,ErrorMessage ="不能超过20个字符")]
        public string FenChengBiLi { get; set; }

        /// <summary>
        /// 均价（单位：千）
        /// </summary>
        [Required(ErrorMessage ="必填")]
        public decimal JunJia { get; set; }

    }
}