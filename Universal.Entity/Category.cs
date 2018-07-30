using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 栏目分类
    /// </summary>
    public class Category:BaseAdminEntity
    {
        public Category()
        {
            this.Status = true;
            this.Depth = 1;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
            this.Remark = "";
            this.Summary = "";
            this.TitleEr = "";
            //this.AddUserID = 1;
            //this.LastUpdateUserID = 1;
        }


        public int ID { get; set; }

        [Display(Name = "栏目名称"), MaxLength(30,ErrorMessage ="不能超过30个字符"), Required(ErrorMessage = "分类名称不能为空")]
        public string Title { get; set; }

        [Display(Name = "栏目二级名称"), MaxLength(50, ErrorMessage = "不能超过50个字符")]
        public string TitleEr { get; set; }

        [Display(Name ="栏目标识"),MaxLength(100,ErrorMessage ="不能超过100个字符"),Required(ErrorMessage ="标识不能为空")]
        public string CallName { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int? PID { get; set; }

        /// <summary>
        /// 父级信息
        /// </summary>
        [ForeignKey("PID"), Display(Name = "父类")]
        public Category PCategory { get; set; }

        /// <summary>
        /// 深度，从1递增
        /// </summary>
        public int Depth { get; set; }


        [Display(Name = "状态")]
        public bool Status { get; set; }

        /// <summary>
        /// 优先级，越大，同级显示的时候越靠前
        /// </summary>
        [Display(Name = "权重")]
        public int Weight { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        [Display(Name ="封面图"),MaxLength(300)]
        public string ImgUrl { get; set; }

        [Display(Name ="一句话简介"),MaxLength(300,ErrorMessage ="不能超过300个字符")]
        public string Summary { get; set; }

        [Display(Name = "介绍"), MaxLength(500, ErrorMessage = "不能超过500个字符")]
        public string Remark { get; set; }


    }
}
