using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;


namespace Universal.BLL
{
    public class BLLClinic
    {
        /// <summary>
        /// 批量禁用
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool DisEnble(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids)) return false;
            using (var db = new DataCore.EFDBContext())
            {
                string strSql = "update Clinic set Status=0 where id in(" + ids + ")";
                db.Database.ExecuteSqlCommand(strSql);
                return true;
            }
        }
    }
}
