using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 在线咨询医生筛选
    /// </summary>
    public class DoctorSearch
    {

        public DoctorSearch()
        {
            this.specialt_list = new List<string>();
        }

        public int id { get; set; }

        public int clinic_id { get; set; }

        public string clinic_name { get; set; }

        /// <summary>
        /// 所属区域ID
        /// </summary>
        public int? area_id { get; set; }
        
        /// <summary>
        /// 所属区域名称
        /// </summary>
        public string area_name { get; set; }

        /// <summary>
        /// 科室ID，逗号分隔
        /// </summary>
        public string dep_ids { get; set; }

        /// <summary>
        /// 科室名称，逗号分隔
        /// </summary>
        public string dep_names { get; set; }

        /// <summary>
        /// 擅长名称
        /// </summary>
        public string specialty_names { get; set; }

        public List<string> specialt_list { get; set; }

        /// <summary>
        /// 医生名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 头衔
        /// </summary>
        public string touxian { get; set; }

        /// <summary>
        /// 是否可以咨询
        /// </summary>
        public bool can_adv { get; set; }

        /// <summary>
        /// 咨询价格
        /// </summary>
        public decimal adv_price { get; set; }

        /// <summary>
        /// 医师介绍
        /// </summary>
        public string show_me { get; set; }

        /// <summary>
        /// 获取价格，如果小数大于0，则返回小数，否则返回整形
        /// </summary>
        public string GetPrice
        {
            get
            {
                if ((adv_price - (int)adv_price) == 0) return ((int)adv_price).ToString();
                else return adv_price.ToString("F2");
            }
        }

    }

}
