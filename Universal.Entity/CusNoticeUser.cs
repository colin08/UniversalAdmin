using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity
{
    /// <summary>
    /// 公告通知用户
    /// </summary>
    public class CusNoticeUser
    {
        public int ID { get; set; }

        public int CusNoticeID { get; set; }

        public virtual CusNotice CusNotice { get; set; }

        public int CusUserID { get; set; }

        public virtual CusUser CusUser { get; set; }
    }
}
