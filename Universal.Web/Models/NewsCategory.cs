using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{

    public class NewsIndex
    {
        public NewsIndex()
        {
            this.category_list = new List<NewsCategory>();
            this.banner_list = new List<Entity.NewsBanner>();
        }

        public List<NewsCategory> category_list { get; set; }

        public List<Entity.NewsBanner> banner_list { get; set; }
    }

    /// <summary>
    /// 医学通识分类
    /// </summary>
    public class NewsCategory
    {
        public NewsCategory()
        {
            this.tag_list = new List<NewsTag>();
        }

        public int category_id { get; set; }

        public string category_title { get; set; }

        public List<NewsTag> tag_list { get; set; }

    }

    public class NewsTag
    {
        public int tag_id { get; set; }

        public string tag_title { get; set; }
    }

}