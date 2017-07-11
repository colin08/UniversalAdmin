using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universal.Entity;
using Universal.DataCore;

namespace Universal.BLL
{
    public class BLLSysRoute
    {
        EFDBContext db = new EFDBContext();
        /// <summary>
        /// 获取分组数据
        /// </summary>
        /// <param name="is_super_mch"></param>
        /// <returns></returns>
        public List<IGrouping<string, SysRoute>> GetListGroupByTag(bool is_super_mch =false)
        {
            if(!is_super_mch)
                return db.SysRoutes.AsNoTracking().Where(p=>p.IsSuperMch == is_super_mch).GroupBy(p => p.Tag).ToList();
            else
                return db.SysRoutes.AsNoTracking().GroupBy(p => p.Tag).ToList();
        }
        
    }
}
