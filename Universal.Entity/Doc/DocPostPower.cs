using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 秘籍可看的分类或用户
    /// </summary>
    public class DocPostPower
    {

        public int ID { get; set; }

        /// <summary>
        /// 秘籍ID
        /// </summary>
        public int DocPostID { get; set; }

        /// <summary>
        /// 所属秘籍
        /// </summary>
        public virtual DocPost DocPost { get; set; }

        /// <summary>
        /// 部门ID或用户ID
        /// </summary>
        public int TOID { get; set; }

    }
}
