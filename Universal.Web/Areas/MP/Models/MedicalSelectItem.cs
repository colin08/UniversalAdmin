using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.MP.Models
{
    /// <summary>
    /// 体检加项页Model
    /// </summary>
    public class MedicalSelectItem
    {
        /// <summary>
        /// 项目类别0:未选择，1：套餐内(不可选)，2:已选择
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        public string SZM { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public int MedicalID { get; set; }

        /// <summary>
        /// 项目唯一ID
        /// </summary>
        public string OnlyID { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public string price { get; set; }

    }

    public class SelectMedicalCategory
    {
        public SelectMedicalCategory()
        {
            this.items = new List<SelectMedicalCategoryItem>();
        }

        /// <summary>
        /// 分类ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string title { get; set; }

        public List<SelectMedicalCategoryItem> items { get; set; }
    }

    public class SelectMedicalCategoryItem
    {
        /// <summary>
        /// 项目类别0:未选择，1：套餐内(不可选)，2:已选择
        /// </summary>
        public int type { get; set; }

        public int id { get; set; }

        public string title { get; set; }

        public string price { get; set; }
    }

}