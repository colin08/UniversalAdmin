using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 项目
    /// </summary>
    public class BLLProject
    {

        public static Entity.Project GetModel(int id)
        {
            if (id <= 0)
                return null;

            var db = new DataCore.EFDBContext();
            var entity = db.Projects.AsNoTracking().Include(p => p.ProjectUsers.Select(s => s.CusUser)).Include(p => p.ProjectFiles).Include(p => p.ApproveUser).Where(p => p.ID == id).FirstOrDefault();

            db.Dispose();
            return entity;
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user_ids">项目联系人</param>
        /// <returns></returns>
        public static int Add(Entity.Project entity, string user_ids, out string msg)
        {
            msg = "";
            var db = new DataCore.EFDBContext();
            db.Set<Entity.Project>().Add(entity);

            var flow_entity = db.Flows.Find(entity.FlowID);
            if (flow_entity == null)
            {
                msg = "所选流程不存在";
                return 0;
            }
            entity.Pieces = flow_entity.Pieces;
            //处理项目联系人
            foreach (var item in user_ids.Split(','))
            {
                var project_user = new Entity.ProjectUser();
                var user_id = Tools.TypeHelper.ObjectToInt(item);
                var user = db.CusUsers.Find(user_id);
                if (user != null)
                {
                    project_user.CusUserID = user_id;
                    project_user.Project = entity;
                    db.ProjectUsers.Add(project_user);
                }
            }

            //处理节点
            var db_flow_node_list = db.FlowNodes.Where(p => p.FlowID == flow_entity.ID).ToList();
            foreach (var item in db_flow_node_list)
            {
                Entity.ProjectFlowNode entity_node = new Entity.ProjectFlowNode();
                entity_node.Color = item.Color;
                entity_node.ICON = item.ICON;
                entity_node.Left = item.Left;
                entity_node.NodeID = item.NodeID;
                entity_node.ProcessTo = item.ProcessTo;
                entity_node.Project = entity;
                entity_node.Status = true;
                entity_node.Piece = item.Piece;
                entity_node.Top = item.Top;
                db.ProjectFlowNodes.Add(entity_node);
            }

            db.SaveChanges();
            db.Dispose();
            return entity.ID;
        }

        /// <summary>
        /// 修改项目(流程ID不可修改)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user_ids">项目联系人</param>
        /// <returns></returns>
        public static int Modify(Entity.Project entity, string user_ids, out string msg)
        {
            msg = "";
            var db = new DataCore.EFDBContext();

            db.ProjectFiles.Where(p => p.ProjectID == entity.ID).ToList().ForEach(p => db.ProjectFiles.Remove(p));
            db.ProjectUsers.Where(p => p.ProjectID == entity.ID).ToList().ForEach(p => db.ProjectUsers.Remove(p));

            //处理项目联系人
            foreach (var item in user_ids.Split(','))
            {
                var project_user = new Entity.ProjectUser();
                var user_id = Tools.TypeHelper.ObjectToInt(item);
                var user = db.CusUsers.Find(user_id);
                if (user != null)
                {
                    project_user.CusUserID = user_id;
                    project_user.ProjectID = entity.ID;
                    db.ProjectUsers.Add(project_user);
                }
            }

            List<Entity.ProjectFile> file_list = entity.ProjectFiles.ToList();
            foreach (var item in file_list)
            {
                item.ProjectID = entity.ID;
                db.ProjectFiles.Add(item);
            }
            entity.ProjectFiles.Clear();

            var db_entity = db.Entry<Entity.Project>(entity);
            db_entity.State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            db.Dispose();
            return entity.ID;
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Del(int id)
        {
            if (id <= 0)
                return false;
            var db = new DataCore.EFDBContext();
            db.Projects.Remove(db.Projects.Find(id));
            db.Dispose();
            return true;
        }

        /// <summary>
        /// 获取用户可见的项目分页列表
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name="rowCount"></param>
        /// <param name="user_id">当前登录的用户</param>
        /// <param name="search_title">搜索标题</param>
        /// <param name="only_mine">是否只获取我的</param>
        /// <returns></returns>
        public static List<Entity.Project> GetPageData(int page_index, int page_size, ref int rowCount, int user_id, string search_title, bool only_mine)
        {
            //TODO 项目列表
            rowCount = 0;
            List<Entity.Project> response_entity = new List<Entity.Project>();
            if (user_id == 0)
                return response_entity;

            int begin_index = (page_index - 1) * page_size + 1;
            int end_index = page_index * page_size;

            var db = new DataCore.EFDBContext();
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

            string strWhere = " Where ID>0 ";
            if (!string.IsNullOrWhiteSpace(search_title))
            {
                strWhere += " and CHARINDEX(N'" + search_title + "', Title) > 0 ";
            }

            sql = "select * from (SELECT ROW_NUMBER() OVER(ORDER BY LastUpdateTime DESC) as row, *FROM(select * from[dbo].[Project] " + strWhere + ") as S where See = 0 or CHARINDEX((case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end),(case CusUserID when " + user_id + " then(case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end) end)+TOID)> 0) as T  where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
            sql_total = "select count(1) FROM (select * from  [dbo].[Project] " + strWhere + ") as S where See = 0 or CHARINDEX((case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end),(case CusUserID when " + user_id + " then(case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end) end) + TOID)> 0";


            rowCount = db.Database.SqlQuery<int>(sql_total).ToList()[0];
            response_entity = db.Database.SqlQuery<Entity.Project>(sql).ToList();
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


        //TODO 获取项目拆迁列表

        //TODO 添加项目拆迁

        //TODO 删除项目拆迁

    }
}
