using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchCase
    {
        public SearchCase() { }

        public SearchCase(string img_url,string open_url,string title,string time,string address)
        {
            this.img_url = img_url;
            this.open_url = open_url;
            this.title = title;
            this.time = time;
            this.address = address;
        }

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
        public string time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string address { get; set; }


    }
}