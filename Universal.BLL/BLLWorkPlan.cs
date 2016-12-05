using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    /// <summary>
    /// 工作计划
    /// </summary>
    public class BLLWorkPlan
    {
        /// <summary>
        /// 获取主管待审批的计划
        /// </summary>
        /// <param name="page_index"></param>
        /// <param name="page_size"></param>
        /// <param name="rowCount"></param>
        /// <param name="user_id">主管id</param>
        /// <returns></returns>
        public static List<Entity.WorkPlan> GetAdminApprovePageData(int page_index, int page_size, ref int rowCount, int user_id)
        {
            rowCount = 0;
            List<Entity.WorkPlan> response_entity = new List<Entity.WorkPlan>();
            if (user_id == 0)
                return response_entity;


            int begin_index = (page_index - 1) * page_size + 1;
            int end_index = page_index * page_size;

            var db = new DataCore.EFDBContext();

            //非主管
            if (!db.CusDepartmentAdmins.Any(p => p.CusUserID == user_id))
            {
                return response_entity;
            }

            string sql = "";
            string sql_total = "";
            int user_department_id = 0;
            string user_department_str = "";
            string user_id_str = "";
            var entity_user = db.CusUsers.Find(user_id);

            if (entity_user == null)
                return response_entity;
            else
            {
                user_department_id = entity_user.CusDepartmentID;
                user_department_str = "," + user_department_id + ",";
                user_id_str = "," + user_id + ",";
            }

            sql = @"select * from (
                    SELECT ROW_NUMBER() OVER(ORDER BY IsApprove DESC) as row, *FROM(
                    select * from(
                    SELECT A.*, ',' + dbo.fn_GetWorkPlanApproveUserIds(B.CusDepartmentID) + ',' as ids from WorkPlan as A left join CusUser as B on A.CusUserID = B.ID
                    ) AS Z where CHARINDEX('," + user_id.ToString()+",', ids) > 0) as T) as X where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";

            sql_total = @"select count(1) from (
                    SELECT A.*,',' + dbo.fn_GetWorkPlanApproveUserIds(B.CusDepartmentID) + ',' as ids from WorkPlan as A left join CusUser as B on A.CusUserID = B.ID
                    ) AS Z
                    where CHARINDEX(',"+user_id.ToString()+",', ids)> 0";
            

            rowCount = db.Database.SqlQuery<int>(sql_total).ToList()[0];
            response_entity = db.Database.SqlQuery<Entity.WorkPlan>(sql).ToList();
            foreach (var item in response_entity)
            {
                var entity = db.CusUsers.Find(item.CusUserID);
                if (entity == null)
                {
                    entity = new Entity.CusUser();
                }
                item.CusUser = entity;                
            }
            db.Dispose();
            return response_entity;

        }

    }
}
