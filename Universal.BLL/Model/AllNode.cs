using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL.Model
{
    public class AllNode
    {
        public int category_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string category_name { get; set; }

        public List<AllNodeList> node_list { get; set; }
    }

    public class AllNodeList
    {
        public int node_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string node_name { get; set; }

    }
}
