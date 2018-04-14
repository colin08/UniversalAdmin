using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.MP.Models
{
    /// <summary>
    /// 体检套餐订单完善用户信息
    /// </summary>
    public class OrderMedicalInfo
    {
        public OrderMedicalInfo()
        {
            Gender = 1;
        }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNumber { get; set; }

        public string Telphone { get; set; }

        public int Gender { get; set; }

        public DateTime Brithday { get; set; }

        public DateTime YuYueDate { get; set; }

        public string YuYueStr { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

    }
}