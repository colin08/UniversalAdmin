using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 案例展示
    /// </summary>
    public class BLLCaseShow
    {

        /// <summary>
        /// 获取一个实体，返回所拥有的企业合作，id逗号分隔
        /// </summary>
        /// <param name="id"></param>
        /// <param name="team_work_ids"></param>
        /// <returns></returns>
        public static Entity.CaseShow GetModel(int id, out string team_work_ids)
        {
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.CaseShows.Where(p => p.ID == id).AsNoTracking().FirstOrDefault();
                string strSql = "select Cast(TeamWork_ID as nvarchar(20))+',' from(SELECT TeamWork_ID FROM[dbo].[TeamWorkCaseShow] where CaseShow_ID = " + id.ToString() + ") as T for xml path('')";
                team_work_ids = db.Database.SqlQuery<string>(strSql).FirstOrDefault();
                return entity;
            }
        }

        /// <summary>
        /// 添加案例
        /// </summary>
        /// <returns></returns>
        public static bool Add(Entity.CaseShow model, string team_work_ids)
        {
            if (model == null) return false;
            using (var db = new DataCore.EFDBContext())
            {
                var entity = new Entity.CaseShow();
                entity.Address = model.Address;
                entity.AddUserID = model.AddUserID;
                entity.CategoryID = model.CategoryID;
                entity.Content = model.Content;
                entity.ImgUrl = model.ImgUrl;
                entity.LastUpdateUserID = model.LastUpdateUserID;
                entity.Status = model.Status;
                entity.Summary = model.Summary;
                entity.Time = model.Time;
                entity.Title = model.Title;
                entity.Type = model.Type;
                entity.Weight = model.Weight;
                db.CaseShows.Add(entity);
                db.SaveChanges();
                StringBuilder sb_sql = new StringBuilder();
                foreach (var item in team_work_ids.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                    sb_sql.Append(string.Format("INSERT INTO dbo.TeamWorkCaseShow values({0},{1});", item, entity.ID));
                }
                if (sb_sql.Length > 0)
                {
                    db.Database.ExecuteSqlCommand(sb_sql.ToString());
                }
            }
            return true;

        }
        
        /// <summary>
        /// 修改案例
        /// </summary>
        /// <param name="model"></param>
        /// <param name="team_work_ids"></param>
        /// <returns></returns>
        public static bool Modify(Entity.CaseShow model, string team_work_ids)
        {
            if (model == null) return false;
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.CaseShows.Find(model.ID);
                if (entity == null) return false;

                entity.Address = model.Address;
                entity.AddUserID = model.AddUserID;
                entity.CategoryID = model.CategoryID;
                entity.Content = model.Content;
                entity.ImgUrl = model.ImgUrl;
                entity.LastUpdateUserID = model.LastUpdateUserID;
                entity.Status = model.Status;
                entity.Summary = model.Summary;
                entity.Time = model.Time;
                entity.Title = model.Title;
                entity.Type = model.Type;
                entity.Weight = model.Weight;
                db.SaveChanges();
                StringBuilder sb_sql = new StringBuilder();
                //先清空
                sb_sql.Append(string.Format("DELETE dbo.TeamWorkCaseShow where CaseShow_ID = {0};", entity.ID));
                foreach (var item in team_work_ids.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                    sb_sql.Append(string.Format("INSERT INTO dbo.TeamWorkCaseShow values({0},{1});", item, entity.ID));
                }
                db.Database.ExecuteSqlCommand(sb_sql.ToString());
            }
            return true;
        }

    }
}
