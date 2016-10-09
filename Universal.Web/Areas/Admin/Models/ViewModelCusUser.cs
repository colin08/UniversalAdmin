using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    public class ViewModelCusUser : BasePageModel
    {
        public string word { get; set; }

        public List<Entity.CusUser> DataList { get; set; }
    }
}