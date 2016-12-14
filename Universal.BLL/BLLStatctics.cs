using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    /// <summary>
    /// 统计
    /// </summary>
    public class BLLStatctics
    {
        /// <summary>
        /// 面积统计
        /// </summary>
        /// <param name="project_id_str"></param>
        /// <returns></returns>
        public static Model.Statctics Domain(string project_id_str)
        {
            if (string.IsNullOrWhiteSpace(project_id_str))
                project_id_str = "";
            
            var db = new DataCore.EFDBContext();
            SqlParameter[] param = {
                                               new SqlParameter("@project_id_str",project_id_str)
                                    };
            var chart_data = db.Database.SqlQuery<Model.Statctics>("dbo.sp_StatcticsDomain @project_id_str", param).FirstOrDefault();
            if (chart_data == null)
                chart_data = new Model.Statctics();

            db.Dispose();
            return chart_data;
        }
    }
}
