using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{

    /// <summary>
    /// 案例类别
    /// </summary>
    public enum CaseShowType : byte
    {
        [Description("最新案例")]
        New = 1,
        [Description("经典案例")]
        Classic
    }

    /// <summary>
    /// 案例展示-创意视觉
    /// </summary>
    public class CaseShow : BaseAdminEntity
    {

        public CaseShow()
        {
            this.Status = true;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Display(Name = "案例名称"), MaxLength(50, ErrorMessage = "不能超过50个字符"), Required(ErrorMessage = "项目名称不能为空")]
        public string Title { get; set; }

        [Display(Name = "案例类别")]
        public CaseShowType Type { get; set; }

        [Display(Name = "所属栏目")]
        public int CategoryID { get; set; }

        /// <summary>
        /// 栏目详情
        /// </summary>
        public virtual Category Category { get; set; }
        
        /// <summary>
        /// 获取案例类别文本
        /// </summary>
        public string GetTypeStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription(Type);
            }
        }

        [Display(Name = "图片"), MaxLength(300)]
        public string ImgUrl { get; set; }

        [Display(Name = "时间"), MaxLength(20, ErrorMessage = "不能超过20个字符"), Required(ErrorMessage = "时间不能为空")]
        public string Time { get; set; }

        [Display(Name = "地点"), MaxLength(20, ErrorMessage = "不能超过20个字符"), Required(ErrorMessage = "地点不能为空")]
        public string Address { get; set; }

        [Display(Name = "简单描述"), MaxLength(500, ErrorMessage = "不能超过500个字符")]
        public string Summary { get; set; }
        
        [Display(Name = "状态")]
        public bool Status { get; set; }

        /// <summary>
        /// 优先级，越大，同级显示的时候越靠前
        /// </summary>
        [Display(Name = "权重")]
        public int Weight { get; set; }


        [Display(Name ="内容")]
        public string Content { get; set; }

        /// <summary>
        /// 合作企业，多对多关系
        /// </summary>
        public virtual ICollection<TeamWork> TeamWorks { get; set; }

    }
}
