using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 秘籍权限
    /// </summary>
    public enum DocPostSee
    {
        /// <summary>
        /// 所有人
        /// </summary>
        everyone =0,
        /// <summary>
        /// 某些部门下
        /// </summary>
        department =1,
        /// <summary>
        /// 特定用户
        /// </summary>
        user=2
    }

    /// <summary>
    /// 秘籍文章
    /// </summary>
    public class DocPost
    {
        public DocPost()
        {
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
            this.FileSize = "0";
        }

        public int ID { get; set; }

        [Display(Name = "标题"), StringLength(255), Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }

        /// <summary>
        /// 权限类别
        /// </summary>
        public DocPostSee See { get; set; }
        
        /// <summary>
        /// 所属分类
        /// </summary>
        public int DocCategoryID { get; set; }

        /// <summary>
        /// 分类信息
        /// </summary>
        public virtual DocCategory DocCategory { get; set; }

        /// <summary>
        /// 上传者
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 上传者信息
        /// </summary>
        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        [MaxLength(500)]
        public string FilePath { get; set; }

        /// <summary>
        /// 附件大小,KB或MB显示
        /// </summary>
        [MaxLength(30)]
        public string FileSize { get; set; }

        /// <summary>
        /// 部门ID或用户ID，逗号分割，前后要加逗号:,1,2,3,4,
        /// </summary>
        [MaxLength(1000)]
        public string TOID { get; set; }


        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        
    }
}
