using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 医生特长
    /// </summary>
    public class BLLDoctorsSpecialty
    {

        /// <summary>
        /// 获取所有的特长
        /// </summary>
        /// <param name="SZMList"></param>
        /// <returns></returns>
        public static List<Entity.DoctorsSpecialty> LoadAllSelectList(out List<string> SZMList)
        {
            SZMList = new List<string>();
            using (var db = new DataCore.EFDBContext())
            {
                var db_list = db.DoctorsSpecialtys.OrderBy(p => p.AddTime).AsNoTracking().ToList();
                SZMList = db.Database.SqlQuery<string>("select SZM from  [dbo].[DoctorsSpecialty] group by SZM").ToList();
                return db_list;
            }
        }

        /// <summary>
        /// 根据ID列表获取
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<Entity.DoctorsSpecialty> GetListByIDS(string ids)
        {
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            if (id_list.Length == 0) return null;
            using (var db=new DataCore.EFDBContext())
            {
                return db.DoctorsSpecialtys.Where(p => id_list.Contains(p.ID)).ToList();
            }
        }

        /// <summary>
        /// 根据ID列表获取
        /// </summary>
        /// <param name="titles"></param>
        /// <returns></returns>
        public static List<Entity.DoctorsSpecialty> GetListByTitles(string titles)
        {
            var title_list = titles.Split(',');
            if (title_list.Length == 0) return null;
            using (var db = new DataCore.EFDBContext())
            {
                return db.DoctorsSpecialtys.Where(p => title_list.Contains(p.Title)).ToList();
            }
        }
    }
}
