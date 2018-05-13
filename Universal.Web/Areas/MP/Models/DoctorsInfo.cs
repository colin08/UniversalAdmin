using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.MP.Models
{
    /// <summary>
    /// 医生信息
    /// </summary>
    public class DoctorsInfo
    {
        public string name { get; set; }

        public string gender { get; set; }

        public string brithday { get; set; }

        public string idnumber { get; set; }

        public string telphone { get; set; }

        public int zhensuo_id { get; set; }

        public string zhensuo { get; set; }

        public string keshi { get; set; }
        public string keshi_ids { get; set; }

        public string touxian { get; set; }

        public string shanchang { get; set; }

        public string shanchang_ids { get; set; }

        public string jianjie { get; set; }

        public string code { get; set; }

    }

}