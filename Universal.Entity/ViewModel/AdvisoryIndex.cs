using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 咨询列表首页数据
    /// </summary>
    public class AdvisoryIndex
    {
        public AdvisoryIndex()
        {
            this.area_list = new List<AdvisoryIndexDep>();
            //this.area_list.Add(new AdvisoryIndexDep(0, "全国"));
            this.hospital_list = new List<AdvisoryIndexDep>();
            //this.hospital_list.Add(new AdvisoryIndexDep(0, "全部医院"));
            this.dep_list = new List<AdvisoryIndexDep>();
            //this.dep_list.Add(new AdvisoryIndexDep(0,"全部科室"));
        }

        /// <summary>
        /// 医生列表
        /// </summary>
        public List<DoctorSearch> doctors_list { get; set; }

        /// <summary>
        /// 医生筛选后总数
        /// </summary>
        public int doctors_total { get; set; }

        /// <summary>
        /// 地区列表
        /// </summary>
        public List<AdvisoryIndexDep> area_list { get; set; }

        /// <summary>
        /// 医院列表-如果筛选了地区
        /// </summary>
        public List<AdvisoryIndexDep> hospital_list { get; set; }

        /// <summary>
        /// 科室数据-如果筛选了医院
        /// </summary>
        public List<AdvisoryIndexDep> dep_list { get; set; }
        
    }

    /// <summary>
    /// 科室数据
    /// </summary>
    public class AdvisoryIndexDep
    {
        public AdvisoryIndexDep() { }
        public AdvisoryIndexDep(int id,string title)
        {
            this.id = id;
            this.title = title;
        }

        public int id { get; set; }

        public string title { get; set; }
    }
}
