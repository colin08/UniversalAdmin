using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 工作计划
    /// </summary>
    public class BLLWorkPlan
    {
        public static Entity.WorkPlan GetModel(int id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                return db.WorkPlans.AsNoTracking().Include(p => p.ApproveUser).Include(p=>p.CusUser).Include(p => p.WorkPlanItemList).Where(p => p.ID == id).FirstOrDefault();
            }
        }

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

        /// <summary>
        /// app获取计划列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static List<Entity.WorkPlan> GetPagedList(int pageIndex, int pageSize, ref int rowCount,int user_id)
        {
            using (var db =new DataCore.EFDBContext())
            {
                rowCount = db.WorkPlans.Where(p=>p.CusUserID == user_id).Count();
                return db.WorkPlans.OrderByDescending(p=>p.AddTime).Where(p => p.CusUserID == user_id).Include(p => p.ApproveUser).Include(p => p.WorkPlanItemList).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
            }

        }

        /// <summary>
        /// 计划审批
        /// </summary>
        /// <returns></returns>
        public static bool Approve(int user_id,int plan_id,out string msg)
        {
            msg = "";
            var entity_user = new BLL.BaseBLL<Entity.CusUser>().GetModel(p => p.ID == user_id);
            if(entity_user == null)
            {
                msg = "审核的用户不存在";
                return false;
            }
            BLL.BaseBLL<Entity.WorkPlan> bll = new BLL.BaseBLL<Entity.WorkPlan>();
            var entity = bll.GetModel(p => p.ID == plan_id);
            if (entity == null)
            {
                msg = "要审批的计划不存在";
                return false;
            }

            if(entity.ApproveUserID != user_id)
            {
                msg = "该计划不是由您来审核";
                return false;
            }

            if (entity.IsApprove)
            {
                msg = "已审批";
                return false;
            }

            entity.IsApprove = true;
            entity.ApproveTime = DateTime.Now;
            if (bll.Modify(entity, "IsApprove", "ApproveTime") > 0)
            {
                msg = "审批成功";
                BLL.BLLMsg.PushMsg(entity.CusUserID, Entity.CusUserMessageType.planapproveok, string.Format(BLL.BLLMsgTemplate.PlanApproveOK, entity_user.NickName), entity.ID);
                return true;
            }
            else
            {
                msg = "审批失败";
                return false;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Modify(Entity.WorkPlan entity)
        {
            var db = new DataCore.EFDBContext();
            db.WorkPlanItems.Where(p => p.WorkPlanID == entity.ID).ToList().ForEach(p => db.WorkPlanItems.Remove(p));

            if(entity.WorkPlanItemList != null)
            {
                foreach (var item in entity.WorkPlanItemList)
                {
                    item.WorkPlanID = entity.ID;
                    db.WorkPlanItems.Add(item);
                }
            }
            entity.WorkPlanItemList.Clear();
            var ss = db.Entry<Entity.WorkPlan>(entity);
            ss.State = System.Data.Entity.EntityState.Modified;
            int row = db.SaveChanges();
            db.Dispose();
            return row > 0;
        }
        
    }
}
