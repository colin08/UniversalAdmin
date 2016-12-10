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
        /// 获取项目分页列表
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name="rowCount"></param>
        /// <param name="user_id">0为所有，否则为某个用户的项目</param>
        /// <param name="search_title">搜索标题</param>
        /// <param name="category_id">所属分类</param>
        /// <returns></returns>
        public static List<Entity.Project> GetPageData(int page_index, int page_size, ref int rowCount, int user_id, string search_title)
        {
            //TODO 项目列表
            rowCount = 0;
            List<Entity.Project> response_entity = new List<Entity.Project>();
            if (user_id == 0)
                return response_entity;

            var db = new DataCore.EFDBContext();
            string sql = "";
            string sql_total = "";
            int begin_index = (page_index - 1) * page_size + 1;
            int end_index = page_index * page_size;

            if (!string.IsNullOrWhiteSpace(search_title))
            {
                sql = "select * from (select ROW_NUMBER() OVER(ORDER BY FavTime DESC) as row, *from( select W.*, U.NickName, u.Telphone from(select pro.*, fav.AddTime as FavTime from CusUserProjectFavorites as fav left join Project as pro on fav.ProjectID = pro.ID where fav.CusUserID = " + user_id + ") as W LEFT JOIN CusUser as U on W.CusUserID = u.ID) as T where T.NickName like '%" + search_title + "%' or T.Title like '%" + search_title + "%') AS Z where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
                sql_total = "select count(1) from (select W.*,U.NickName,u.Telphone from (select pro.Title,pro.CusUserID,pro.ApproveUserID,See,TOID,pro.AddTime,LastUpdateTime,fav.AddTime as FavTime from CusUserProjectFavorites as fav left join Project as pro on fav.ProjectID = pro.ID where fav.CusUserID = " + user_id + ") as W LEFT JOIN CusUser as U on W.CusUserID =u.ID) as T where T.NickName like '%" + search_title + "%' or T.Title like '%" + search_title + "%'";
            }
            else
            {
                sql = "select * from (select ROW_NUMBER() OVER(ORDER BY FavTime DESC) as row,* from (select W.*,U.NickName,u.Telphone from (select pro.*,fav.AddTime as FavTime from CusUserProjectFavorites as fav left join Project as pro on fav.ProjectID = pro.ID where fav.CusUserID =" + user_id + ") as W LEFT JOIN CusUser as U on W.CusUserID =u.ID) as T) as Z where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
                sql_total = "select count(1) from (select W.*,U.NickName,u.Telphone from (select pro.Title,pro.CusUserID,pro.ApproveUserID,See,TOID,pro.AddTime,LastUpdateTime,fav.AddTime as FavTime from CusUserProjectFavorites as fav left join Project as pro on fav.ProjectID = pro.ID where fav.CusUserID =" + user_id + ") as W LEFT JOIN CusUser as U on W.CusUserID =u.ID) as T";
            }
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
