﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewModelMedicalItemCategoryList : BasePageModel
    {
        /// <summary>
        /// 当前筛选的关键字
        /// </summary>
        public string word { get; set; }

        /// <summary>
        /// 列表
        /// </summary>
        public List<Entity.MedicalItemCategory> DataList { get; set; }
        
    }
}