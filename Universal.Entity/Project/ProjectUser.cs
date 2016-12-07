using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity
{
    /// <summary>
    /// 项目联系人
    /// </summary>
    public class ProjectUser
    {
        public int ID { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// 项目信息
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public int CusUserID { get; set; }

        /// <summary>
        /// 人员信息
        /// </summary>
        public virtual CusUser CusUser { get; set; }
    }
}
