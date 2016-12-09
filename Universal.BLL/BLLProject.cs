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

        //TODO 获取列表

        //TODO 获取项目拆迁列表

        //TODO 添加项目拆迁

        //TODO 删除项目拆迁

    }
}
