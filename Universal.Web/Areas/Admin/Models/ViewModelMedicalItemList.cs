﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewModelMedicalItemList : BasePageModel
    {
        /// <summary>
        /// 当前筛选的关键字
        /// </summary>
        public string word { get; set; }

        /// <summary>
        /// 用户列表
        /// </summary>
        public List<Entity.MedicalItem> DataList { get; set; }
        
    }
}