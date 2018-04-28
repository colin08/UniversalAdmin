using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    /// <summary>
    /// 标签云
    /// </summary>
    public class ViewModelCloudTags
    {
        public ViewModelCloudTags(string text,int id)
        {
            this.text = text;
            this.weight = 4;
            this.link = "javascript:modify(" + id + ",'" + text + "')";
        }

        public string text { get; set; }

        public int weight { get; set; }

        public string link { get; set; }
    }

    /// <summary>
    /// 医学通识标签云
    /// </summary>
    public class ViewModelNewsTags
    {
        public ViewModelNewsTags(string text, int id)
        {
            this.text = text;
            this.weight = 4;            
            this.link = "javascript:modify(" + id + ")";
        }

        public string text { get; set; }

        public int weight { get; set; }

        public string link { get; set; }
    }
}