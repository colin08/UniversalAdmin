using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 合作企业的案例列表
    /// </summary>
    public class CaseShowTeamWork
    {
        /// <summary>
        /// 合作企业详情
        /// </summary>
        public Entity.TeamWork team_work_info { get; set; }

        /// <summary>
        /// 该企业下的合作案例
        /// </summary>
        public List<Entity.CaseShow> case_show_list { get; set; }

    }
}
