using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    public class ViewModelTree
    {

        public int id { get; set; }

        public int pId { get; set; }

        public string name { get; set; }

        public bool open { get; set; }

        //指定序列化成员名称
        [DataMember(Name = "checked")] 
        public bool is_checked { get; set; }
    }
}