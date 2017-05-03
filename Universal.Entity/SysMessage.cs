using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public class SysMessage
    {
        public int ID { get; set; }

        [Display(Name ="消息内容"),Required,MaxLength(255)]
        public string Content { get; set; }

        [Display(Name ="是否已读")]
        public bool IsRead { get; set; }

        [Display(Name ="新标签页打开",Description = "是否在新标签页打开，否则就是在框架内打开")]
        public bool OpenNewTab { get; set; }
        
        [Display(Name ="链接地址"),Required(ErrorMessage ="链接地址不能为空"),MaxLength(500)]
        public string LinkUrl { get; set; }

        [Display(Name ="添加时间")]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 时间字符串
        /// </summary>
        [NotMapped]
        public string TimeBefore
        {
            get
            {
                return Tools.WebHelper.DateStringFromNow(this.AddTime);
            }
        }

    }
}
