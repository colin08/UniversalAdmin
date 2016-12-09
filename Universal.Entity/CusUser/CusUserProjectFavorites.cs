using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 用户项目收藏
    /// </summary>
    public class CusUserProjectFavorites
    {
        public int ID { get; set; }

        public int CusUserID { get; set; }

        public virtual CusUser CusUser { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectID { get; set; }

        public virtual Project Project { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

    }
}
