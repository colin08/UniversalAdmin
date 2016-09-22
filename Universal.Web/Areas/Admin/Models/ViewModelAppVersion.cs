using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    public class ViewModelAppVersion :BasePageModel
    {
        public int platform { get; set; }

        public List<Entity.AppVersion> DataList { get; set; }
    }
}