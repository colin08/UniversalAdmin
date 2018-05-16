using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 用户资金变动记录表
    /// </summary>
    public class BLLMPUserAmountDetails
    {
        /// <summary>
        /// 获取信息实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.MPUserAmountDetails GetModel(int id)
        {
            using (var db=new DataCore.EFDBContext())
            {
                return db.MPUserAmountDetails.Where(p => p.ID == id).AsNoTracking().FirstOrDefault();
            }
        }
    }
}
