using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 职位
    /// </summary>
    public class CusUserJob
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public CusUserJob()
        {
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        [Display(Name = "职位名称"),MaxLength(100),Required(ErrorMessage ="职位名称不能为空")]
        public string Title { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
