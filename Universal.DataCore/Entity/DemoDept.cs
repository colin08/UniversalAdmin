using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Universal.DataCore.Entity
{
    /// <summary>
    /// 成员
    /// </summary>
     public class DemoDept
    {
        public int ID { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public int DemoID { get; set; }

        [Display(Name ="标题"),MaxLength(255,ErrorMessage ="最大长度255")]
        public string Title { get; set; }

        [Display(Name ="图片"),MaxLength(255)]
        public string ImgUrl { get; set; }

        [Display(Name ="其他数据")]
        public int Num { get; set; }
        
    }
}
