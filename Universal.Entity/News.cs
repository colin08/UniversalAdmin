using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 新闻类别
    /// </summary>
    public enum NewsType : byte
    {
        [Description("朗形动态")]
        LX =1,
        [Description("行业新闻")]
        HY
    }

    /// <summary>
    /// 新闻咨询
    /// </summary>
    public class News: BaseAdminEntity
    {
        public News()
        {
            this.Status = true;
            this.AddTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
            this.Source = "朗形";
            this.Author = "朗形";
            this.SourceLinkUrl = "";
        }

        public int ID { get; set; }

        /// <summary>
        /// 获取Base64位编码ID
        /// </summary>
        public string GetBase64ID
        {
            get
            {
                return Tools.Base64.EncodeBase64(ID.ToString());
            }
        }

        [Display(Name = "新闻标题"), MaxLength(30, ErrorMessage = "不能超过100个字符"), Required(ErrorMessage = "新闻标题不能为空")]
        public string Title { get; set; }

        [Display(Name ="新闻分类")]
        public NewsType Type { get; set; }

        /// <summary>
        /// 获取新闻类别字符串
        /// </summary>
        [NotMapped]
        public string GetTypeStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<NewsType>(Type);
            }
        }

        [Display(Name = "状态")]
        public bool Status { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        [Display(Name = "封面图"), MaxLength(300)]
        public string ImgUrl { get; set; }

        [Display(Name = "详情顶部大图"), MaxLength(300)]
        public string ImgUrlBig { get; set; }

        /// <summary>
        /// 优先级，越大，同级显示的时候越靠前
        /// </summary>
        [Display(Name = "权重")]
        public int Weight { get; set; }

        [Display(Name = "新闻来源"), MaxLength(30, ErrorMessage = "不能超过30个字符")]
        public string Source { get; set; }

        [Display(Name = "新闻来源网址"), MaxLength(500, ErrorMessage = "不能超过500个字符")]
        public string SourceLinkUrl { get; set; }


        [Display(Name = "作者"), MaxLength(30, ErrorMessage = "不能超过30个字符")]
        public string Author { get; set; }

        [Display(Name = "简介"), MaxLength(300, ErrorMessage = "不能超过300个字符")]
        public string Summary { get; set; }

        [Display(Name = "新闻内容")]
        public string Content { get; set; }


    }
}
