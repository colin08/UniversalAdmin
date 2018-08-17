using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    public class CompanyProfile
    {
        public Tools.CompanyProfileModel SiteConfig { get; set; }

        public List<Entity.Banner> banner_list { get; set; }
    }
}