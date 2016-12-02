using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{

    /// <summary>
    /// 用户秘籍收藏
    /// </summary>
    public class CusUserDocFavorites
    {
        public CusUserDocFavorites()
        {
            this.AddTime = DateTime.Now;
        }

        public int ID { get; set; }

        public int CusUserID { get; set; }

        public virtual CusUser CusUser { get; set; }
        
        /// <summary>
        /// 秘籍的ID
        /// </summary>
        public int DocPostID { get; set; }

        /// <summary>
        /// 秘籍信息
        /// </summary>
        public virtual DocPost DocPost { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }


    }
}
