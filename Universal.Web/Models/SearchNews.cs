using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchNews
    {
        public SearchNews() { }
        public SearchNews(string img_url,string open_url,string title,string summary)
        {
            this.img_url = img_url;
            this.open_url = open_url;
            this.title = title;
            this.summary = summary;
        }

        /// <summary>
        /// /
        /// </summary>
        public string img_url { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string open_url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string summary { get; set; }
    }
}