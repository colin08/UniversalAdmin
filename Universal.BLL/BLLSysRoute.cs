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
        /// <returns></returns>
        public List<IGrouping<string, SysRoute>> GetListGroupByTag()
        {
            return db.SysRoutes.GroupBy(p => p.Tag).ToList();
        }
        
    }
}
