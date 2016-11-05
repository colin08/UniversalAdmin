using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL.Model
{
    public class AdminUserRoute
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 权限说明
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 是否选中,大于0为选中
        /// </summary>
        public int is_check { get; set; }
    }
}
