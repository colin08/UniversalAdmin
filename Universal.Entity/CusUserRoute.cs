using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Entity
{
    /// <summary>
    /// 用户对应的权限
    /// </summary>
    public class CusUserRoute
    {
        public int ID { get; set; }


        /// <summary>
        /// 用户信息
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public int CusRouteID { get; set; }

        /// <summary>
        /// 权限信息
        /// </summary>
        public virtual CusRoute CusRoute { get; set; }
        

    }
}
