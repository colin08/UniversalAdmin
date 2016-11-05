using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    public class DocCategory
    {
        public DocCategory()
        {
            this.Status = true;
            this.Depth = 1;
            this.AddTime = DateTime.Now;
            this.Priority = 0;
        }

        public int ID { get; set; }

        [Display(Name = "标题"), StringLength(30), Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int? PID { get; set; }

        /// <summary>
        /// 父级部门信息
        /// </summary>
        [ForeignKey("PID")]
        public DocCategory PDocCategory { get; set; }

        /// <summary>
        /// 深度，从1递增
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 优先级，越大，同级显示的时候越靠前
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
