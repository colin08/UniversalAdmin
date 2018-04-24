using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;


namespace Universal.BLL
{
    public class BLLClinicDepartment
    {

        /// <summary>
        /// 根据诊所ID获取所有的部门
        /// </summary>
        /// <param name="clinic_id">clinic_id</param>
        /// <param name="SZMList"></param>
        /// <returns></returns>
        public static List<Entity.ClinicDepartment> LoadAllSelectList(int clinic_id,out List<string> SZMList)
        {
            SZMList = new List<string>();
            using (var db = new DataCore.EFDBContext())
            {
                var db_list = db.ClinicDepartments.Where(p=>p.Status && p.ClinicID == clinic_id).OrderByDescending(p => p.Weight).AsNoTracking().ToList();
                SZMList = db.Database.SqlQuery<string>("select SZM from  [dbo].[ClinicDepartment] group by SZM").ToList();
                return db_list;
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
                string strSql = "update ClinicDepartment set Status=0 where id in(" + ids + ")";
                db.Database.ExecuteSqlCommand(strSql);
                return true;
            }
        }
    }
}
