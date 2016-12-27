using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models.Response
{
    public class SeeModel
    {
        public SeeModel(int id,string name)
        {
            this.id = id;
            this.name = name;
        }
        public int id { get; set; }

        public string name { get; set; }
    }
}