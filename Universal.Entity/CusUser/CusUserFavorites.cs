using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{

    /// <summary>
    /// 收藏类别
    /// </summary>
    public enum CusUserFavoritesType
    {
        project=1,
        docment
    }

    /// <summary>
    /// 用户收藏
    /// </summary>
    public class CusUserFavorites
    {
        public CusUserFavorites()
        {
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        public int CusUserID { get; set; }

        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 收藏类别
        /// </summary>
        public CusUserFavoritesType Type { get; set; }

        /// <summary>
        /// 收藏的ID
        /// </summary>
        public int TOID { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }


    }
}
