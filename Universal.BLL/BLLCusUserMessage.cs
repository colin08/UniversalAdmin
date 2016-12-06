using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;

namespace Universal.BLL
{
    /// <summary>
    /// 用户消息
    /// </summary>
    public class BLLCusUserMessage
    {
        /// <summary>
        /// 设置所有已读
        /// </summary>
        public static void SetAllRead(int user_id)
        {
            using (var db =new DataCore.EFDBContext())
            {
                db.CusUserMessages.Where(p => p.CusUserID == user_id && p.IsRead == false).Update(p => new Entity.CusUserMessage { IsRead = true });
            }
        }
    }
}
