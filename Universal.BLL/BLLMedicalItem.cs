using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace Universal.BLL
{
    public class BLLMedicalItem
    {

        /// <summary>
        /// 根据套餐ID获取该套餐引用的项
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public static List<Entity.MedicalItem> GetItemByMedicalID(int mid)
        {
            using (var db=new DataCore.EFDBContext())
            {
                var entity = db.Medicals.Where(p => p.ID == mid).Include(p => p.MedicalItems).AsNoTracking().FirstOrDefault();
                if (entity == null) return new List<Entity.MedicalItem>();
                return entity.MedicalItems.ToList();
            }
        }

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
                string strSql = "update MedicalItem set Status=0 where id in(" + ids + ")";
                db.Database.ExecuteSqlCommand(strSql);
                return true;
            }
        }
    }
}
