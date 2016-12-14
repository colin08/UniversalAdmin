using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL.Model
{
    public class Statctics
    {
        public Statctics()
        {
            this.x_data = "";
            this.y_data = "";
        }
        public string x_data { get; set; }

        public string y_data { get; set; }
    }
}
