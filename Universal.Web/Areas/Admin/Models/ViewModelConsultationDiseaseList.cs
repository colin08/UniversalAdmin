using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    /// <summary>
    /// 列表
    /// </summary>
    public class ViewModelConsultationDiseaseList : BasePageModel
    {
        /// <summary>
        /// 当前筛选的关键字
        /// </summary>
        public string word { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<Entity.ConsultationDisease> DataList { get; set; }
        
    }
}