using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    public class ViewModelNodeCategory:ViewModelCustomFormBase
    {
        public int id { get; set; }

        public string title { get; set; }

        public string remark { get; set; }

    }
}