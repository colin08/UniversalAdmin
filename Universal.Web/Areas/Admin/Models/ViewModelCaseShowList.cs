using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    public class ViewModelCaseShowList : BasePageModel
    {
        /// <summary>
        /// 案例类别
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 所属分类
        /// </summary>
        public int cid { get; set; }

        /// <summary>
        /// 当前筛选的关键字
        /// </summary>
        public string word { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<Entity.CaseShow> DataList { get; set; }
    }
}