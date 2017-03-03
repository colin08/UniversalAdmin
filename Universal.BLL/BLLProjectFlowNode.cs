using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 项目流程处理
    /// </summary>
    public class BLLProjectFlowNode
    {
        /// <summary>
        /// 获取当前项目进行的的流程信息
        /// </summary>
        /// <returns></returns>
        public static List<Model.ProjectFlowNode> GetProjectFlow(int project_id)
        {
            List<Model.ProjectFlowNode> response_entity = new List<Model.ProjectFlowNode>();
            if (project_id <= 0)
                return response_entity;
            var db = new DataCore.EFDBContext();
            var entity_project = db.Projects.Find(project_id);
            if (entity_project == null)
                return response_entity;

            //先获取is_end为true的节点
            var is_end_list = db.ProjectFlowNodes.AsNoTracking().Include(p => p.Node).Where(p => p.ProjectID == project_id && p.IsEnd == true).ToList();
            if (is_end_list.Count == 0)
            {
                //如果没有为true的，则查找frist节点
                is_end_list = db.ProjectFlowNodes.AsNoTracking().Include(p => p.Node).Where(p => p.ProjectID == project_id && p.IsFrist).ToList();
            }
            foreach (var item in is_end_list)
            {
                Model.ProjectFlowNode model = new Model.ProjectFlowNode();
                model.icon = item.ICON;
                model.piece = item.Piece;
                model.process_to = item.ProcessTo;
                model.node_id = item.NodeID;
                model.node_title = item.Node.Title;
                model.node_is_fator = item.Node.IsFactor;
                model.color = item.Color;
                model.left = item.Left;
                model.top = item.Top;
                model.is_end = item.IsEnd;
                model.project_flow_node_id = item.ID;
                response_entity.Add(model);
            }
            db.Dispose();
            return response_entity;
        }

        /// <summary>
        /// 获取下一级节点
        /// </summary>
        /// <param name="project_id"></param>
        /// <param name="project_flow_node_id"></param>
        /// <returns></returns>
        public static List<Model.ProjectFlowNode> GetNextFlowNode(int project_id, int project_flow_node_id)
        {
            List<Model.ProjectFlowNode> response_entity = new List<Model.ProjectFlowNode>();
            if (project_id <= 0)
                return response_entity;
            var db = new DataCore.EFDBContext();
            var entity_project = db.Projects.Find(project_id);
            if (entity_project == null)
                return response_entity;

            var entity_project_node = db.ProjectFlowNodes.Find(project_flow_node_id);
            if (entity_project_node == null)
                return response_entity;
            if (string.IsNullOrWhiteSpace(entity_project_node.ProcessTo))
                return response_entity;

            var next_list = db.ProjectFlowNodes.SqlQuery("SELECT * FROM [dbo].[ProjectFlowNode] where charindex(','+ltrim(ID)+',','," + entity_project_node.ProcessTo + ",') > 0").AsNoTracking().ToList();
            foreach (var item in next_list)
            {
                var entity_node = db.Nodes.Where(p => p.ID == item.NodeID).AsNoTracking().FirstOrDefault();
                if (entity_node == null)
                    continue;
                Model.ProjectFlowNode model = new Model.ProjectFlowNode();
                model.icon = item.ICON;
                model.piece = item.Piece;
                model.process_to = item.ProcessTo;
                model.node_id = item.NodeID;
                model.node_title = entity_node.Title;
                model.node_is_fator = entity_node.IsFactor;
                model.color = item.Color;
                model.left = item.Left;
                model.top = item.Top;
                model.is_end = item.IsEnd;
                model.project_flow_node_id = item.ID;
                response_entity.Add(model);
            }
            return response_entity;
        }

        /// <summary>
        /// 获取项目的单个流程信息
        /// </summary>
        /// <returns></returns>
        public static Model.ProjectFlowNode GetProjectFlowNodeInfo(int project_flow_node_id)
        {
            Model.ProjectFlowNode response_entity = new Model.ProjectFlowNode();
            if (project_flow_node_id <= 0)
                return response_entity;
            var db = new DataCore.EFDBContext();
            var entity_flow_node = db.ProjectFlowNodes.Include(p => p.Node).Where(p => p.ID == project_flow_node_id).AsNoTracking().FirstOrDefault();
            if (entity_flow_node == null)
                return response_entity;
            response_entity.icon = entity_flow_node.ICON;
            response_entity.piece = entity_flow_node.Piece;
            response_entity.process_to = entity_flow_node.ProcessTo;
            response_entity.node_id = entity_flow_node.NodeID;
            response_entity.node_title = entity_flow_node.Node.Title;
            response_entity.node_is_fator = entity_flow_node.Node.IsFactor;
            response_entity.color = entity_flow_node.Color;
            response_entity.left = entity_flow_node.Left;
            response_entity.top = entity_flow_node.Top;
            response_entity.is_end = entity_flow_node.IsEnd;
            response_entity.project_flow_node_id = entity_flow_node.ID;
            db.Dispose();
            return response_entity;
        }

        /// <summary>
        /// 保存节点位置信息
        /// </summary>
        /// <returns></returns>
        public static bool SaveLocation(List<Model.ProjectFlowNode> list_model)
        {
            using (var db = new DataCore.EFDBContext())
            {
                foreach (var model in list_model)
                {
                    var entity = db.ProjectFlowNodes.Find(model.project_flow_node_id);
                    if (entity == null)
                        continue;

                    entity.Color = model.color;
                    entity.ICON = model.icon;
                    entity.Left = model.left;
                    entity.Top = model.top;
                    db.SaveChanges();
                }
                return true;
            }
        }

        /// <summary>
        /// 设置节点完成
        /// </summary>
        /// <param name="project_flow_node_id"></param>
        /// <returns></returns>
        public static bool SetEnd(int project_flow_node_id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.ProjectFlowNodes.Find(project_flow_node_id);
                if (entity == null)
                    return false;
                entity.IsEnd = true;
                entity.EndTime = DateTime.Now;
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 设置条件节点选中
        /// </summary>
        /// <param name="project_flow_node_id"></param>
        /// <returns></returns>
        public static bool SetSelect(int project_flow_node_id,out string msg)
        {
            msg = "ok";
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.ProjectFlowNodes.Include(p => p.Node).Where(p => p.ID == project_flow_node_id).FirstOrDefault();
                if (entity == null)
                {
                    msg = "项目流程节点不存在";
                    return false;
                }
                if(!entity.Node.IsFactor)
                {
                    msg = "该节点不是条件节点";
                    return false;
                }
                //获取同级条件节点
                var tj_list =  db.ProjectFlowNodes.SqlQuery("SELECT * FROM [dbo].[ProjectFlowNode] where charindex(','+ltrim(ProcessTo)+',','," + entity.ProcessTo + ",') > 0").AsNoTracking().ToList();
                foreach (var item in tj_list)
                {
                    if(item.ID != project_flow_node_id && item.IsSelect)
                    {
                        msg = "已有其他条件节点设置为选中状态";
                        return false;
                    }
                }
                //条件都满足
                entity.IsSelect = true;
                db.SaveChanges();
                return true;
            }
        }
        

        ///// <summary>
        ///// 前端：删除某个流程节点信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public static bool DelProjectFlowNode(int id,string project_pieces,out string msg)
        //{
        //    msg = "";
        //    var db =new DataCore.EFDBContext();
        //    var entity_node = db.ProjectFlowNodes.Find(id);
        //    if(entity_node == null)
        //    {
        //        msg = "项目流程节点不存在";
        //        db.Dispose();
        //        return false;
        //    }
        //    if(entity_node.Piece !=0 && string.IsNullOrWhiteSpace(project_pieces))
        //    {
        //        msg = "该节点有所属块，请传入新的块id组，格式：,1,2,3,4,";
        //        db.Dispose();
        //        return false;
        //    }
        //    var entity_project = db.Projects.Find(entity_node.ProjectID);
        //    if(entity_project == null)
        //    {
        //        msg = "所属项目不存在";
        //        db.Dispose();
        //        return false;
        //    }

        //    if(entity_project.Pieces!= project_pieces)
        //        entity_project.Pieces = project_pieces;
        //    db.ProjectFlowNodes.Remove(entity_node);
        //    db.SaveChanges();
        //    db.Dispose();
        //    return true;
        //}

        ///// <summary>
        ///// 修改某个节点
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //public static bool SaveNode(Model.ProjectFlowNode model, out string msg)
        //{
        //    msg = "";
        //    var db = new DataCore.EFDBContext();

        //    var entity = db.ProjectFlowNodes.Find(model.id);
        //    if (entity == null)
        //    {
        //        msg = "项目流程节点不存在";
        //        db.Dispose();
        //        return false;
        //    }
        //    var entity_node = db.Nodes.Find(model.node_id);
        //    if (entity_node == null)
        //    {
        //        msg = "要修改的节点不存在";
        //        db.Dispose();
        //        return false;
        //    }

        //    if (model.status)
        //    {
        //        if (entity.IsStart != model.is_start)
        //        {
        //            if (model.is_start)
        //                entity.BeginTime = DateTime.Now;
        //            else
        //                entity.BeginTime = null;
        //            entity.IsStart = model.is_start;
        //        }

        //        if (entity.IsEnd != model.is_end)
        //        {
        //            if (model.is_end)
        //                entity.EndTime = DateTime.Now;
        //            else
        //                entity.EndTime = null;
        //            entity.IsEnd = model.is_end;
        //        }
        //    }
        //    else
        //        msg = "status为假时不能修改is_start、is_end";

        //    entity.Status = model.status;
        //    entity.Color = model.color;
        //    entity.ICON = model.icon;
        //    entity.Left = model.left;
        //    entity.Top = model.top;
        //    entity.NodeID = model.node_id;
        //    entity.Piece = model.piece;
        //    entity.ProcessTo = model.process_to;

        //    db.SaveChanges();
        //    return true;
        //}

        ///// <summary>
        ///// 修改所有节点信息
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //public static bool SaveAllNode(Model.ProjectFlow model, out string msg)
        //{
        //    msg = "";
        //    var db = new DataCore.EFDBContext();
        //    var entity_project = db.Projects.Find(model.project_id);
        //    if (entity_project == null)
        //    {
        //        db.Dispose();
        //        msg = "项目不存在";
        //        return false;
        //    }
        //    foreach (var item in model.list)
        //    {
        //        var entity_flow_node = db.ProjectFlowNodes.Find(item.id);
        //        if (entity_flow_node == null)
        //        {
        //            msg = "项目流程节点不存在";
        //            db.Dispose();
        //            return false;
        //        }
        //        var entity_node = db.Nodes.Find(item.node_id);
        //        if(entity_node == null)
        //        {
        //            msg = "要修改的节点不存在";
        //            db.Dispose();
        //            return false;
        //        }

        //        if (item.status)
        //        {
        //            if (entity_flow_node.IsStart != item.is_start)
        //            {
        //                if (item.is_start)
        //                    entity_flow_node.BeginTime = DateTime.Now;
        //                else
        //                    entity_flow_node.BeginTime = null;
        //                entity_flow_node.IsStart = item.is_start;
        //            }

        //            if (entity_flow_node.IsEnd != item.is_end)
        //            {
        //                if (item.is_end)
        //                    entity_flow_node.EndTime = DateTime.Now;
        //                else
        //                    entity_flow_node.EndTime = null;
        //                entity_flow_node.IsEnd = item.is_end;
        //            }
        //        }
        //        else
        //            msg = "status为假时不能修改is_start、is_end";

        //        entity_flow_node.Status = item.status;
        //        entity_flow_node.Color = item.color;
        //        entity_flow_node.ICON = item.icon;
        //        entity_flow_node.Left = item.left;
        //        entity_flow_node.Top = item.top;
        //        entity_flow_node.NodeID = item.node_id;
        //        entity_flow_node.Piece = item.piece;
        //        entity_flow_node.ProcessTo = item.process_to;
        //    }

        //    entity_project.Pieces = model.reference_pieces;
        //    db.SaveChanges();
        //    return true;
        //}

        ///// <summary>
        ///// 添加流程节点
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //public static int AddNode(int project_id, string project_pieces, Model.ProjectFlowNode model, out string msg)
        //{
        //    msg = "";
        //    var db = new DataCore.EFDBContext();
        //    var entity_project = db.Projects.Find(project_id);
        //    if (entity_project == null)
        //    {
        //        msg = "项目不存在";
        //        return 0;
        //    }
        //    entity_project.Pieces = project_pieces;

        //    var entity = new Entity.ProjectFlowNode();
        //    if (model.status)
        //    {
        //        if (entity.IsStart != model.is_start)
        //        {
        //            if (model.is_start)
        //                entity.BeginTime = DateTime.Now;
        //            else
        //                entity.BeginTime = null;
        //            entity.IsStart = model.is_start;
        //        }

        //        if (entity.IsEnd != model.is_end)
        //        {
        //            if (model.is_end)
        //                entity.EndTime = DateTime.Now;
        //            else
        //                entity.EndTime = null;
        //            entity.IsEnd = model.is_end;
        //        }
        //    }
        //    else
        //        msg = "status为假时不能修改is_start、is_end";

        //    entity.Status = model.status;
        //    entity.Color = model.color;
        //    entity.ICON = model.icon;
        //    entity.Left = model.left;
        //    entity.Top = model.top;
        //    entity.NodeID = model.node_id;
        //    entity.Piece = model.piece;
        //    entity.ProcessTo = model.process_to;
        //    entity.ProjectID = project_id;
        //    db.ProjectFlowNodes.Add(entity);
        //    db.SaveChanges();
        //    return entity.ID;
        //}
    }
}
