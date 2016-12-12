using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    public class ViewModelContactsUser
    {
        /// <summary>
        /// 当前第几页
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 每页多少条数据
        /// </summary>
        public int page_size { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int total_page { get; set; }

        public string department_title { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public int d { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string w { get; set; }

        public List<Entity.CusUser> DataList { get; set; }
    }
}