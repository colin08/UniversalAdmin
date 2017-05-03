using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.DataCore;
using Universal.Entity;
using Universal.Tools;

namespace Universal.BLL
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public class BLLSysMessage
    {
        /// <summary>
        /// 设置消息已读
        /// </summary>
        /// <param name="id">-1时全部，否则单挑</param>
        /// <returns></returns>
        public static bool SetMsgRead(int id,out string msg)
        {
            msg = "ok";
            using (var db = new DataCore.EFDBContext())
            {
                if(id == -1) db.Database.ExecuteSqlCommand("Update SysMessage set IsRead = 1 where IsRead = 0");
                else
                {
                    var entity = db.SysMessages.Find(id);
                    if(entity != null)
                    {
                        if(!entity.IsRead)
                        {
                            entity.IsRead = true;
                            db.SaveChanges();
                        }
                    }
                }
            }
            return true;

        }
    }
}
