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
        /// 获取项目的流程信息
        /// </summary>
        /// <returns></returns>
        public static Model.ProjectFlow GetProjectFlow(int project_id)
        {
            Model.ProjectFlow response_entity = new Model.ProjectFlow();
            if (project_id <= 0)
                return response_entity;
            var db = new DataCore.EFDBContext();
            var entity_project = db.Projects.Find(project_id);
            if (entity_project == null)
                return response_entity;
            var flow_node_list = db.ProjectFlowNodes.AsNoTracking().Include(p => p.Node).Where(p => p.ProjectID == project_id).ToList();
            List<Model.ProjectFlowNode> response_list = new List<Model.ProjectFlowNode>();
            foreach (var item in flow_node_list)
            {
                Model.ProjectFlowNode model = new Model.ProjectFlowNode();
                model.icon = item.ICON;
                model.piece = item.Piece;
                model.process_to = item.ProcessTo;
                model.node_id = item.ID;
                model.node_title = item.Node.Title;
                model.color = item.Color;
                model.left = item.Left;
                model.top = item.Top;
                model.index = item.Index;
                model.status = item.Status;
                model.is_end = item.IsEnd;
                model.id = item.ID;
                model.is_start = item.IsStart;
                response_list.Add(model);
            }
            db.Dispose();
            response_entity.total = response_list.Count;
            response_entity.project_id = project_id;
            response_entity.reference_pieces = entity_project.Pieces;
            return response_entity;
        }

        /// <summary>
        /// 前端：删除某个流程节点信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DelProjectFlowNode(int id,string project_pieces,out string msg)
        {
            msg = "";
            var db =new DataCore.EFDBContext();
            var entity_node = db.ProjectFlowNodes.Find(id);
            if(entity_node == null)
            {
                msg = "项目流程节点不存在";
                db.Dispose();
                return false;
            }
            if(entity_node.Piece !=0 && string.IsNullOrWhiteSpace(project_pieces))
            {
                msg = "该节点有所属块，请传入新的块id组，格式：,1,2,3,4,";
                db.Dispose();
                return false;
            }
            var entity_project = db.Projects.Find(entity_node.ProjectID);
            if(entity_project == null)
            {
                msg = "所属项目不存在";
                db.Dispose();
                return false;
            }

            if(entity_project.Pieces!= project_pieces)
                entity_project.Pieces = project_pieces;
            db.ProjectFlowNodes.Remove(entity_node);
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// 修改某个节点
        /// </summary>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SaveNode(DataCore.EFDBContext db, Model.ProjectFlowNode model, out string msg)
        {
            msg = "";
            bool auto_save = db == null;
            if (db == null)
                db = new DataCore.EFDBContext();

            var entity = db.ProjectFlowNodes.Find(model.id);
            if (entity == null)
            {
                msg = "项目流程节点不存在";
                return false;
            }
            if (model.status)
            {
                if (entity.IsStart != model.is_start)
                {
                    if (model.is_start)
                        entity.BeginTime = DateTime.Now;
                    else
                        entity.BeginTime = null;
                    entity.IsStart = model.is_start;
                }

                if (entity.IsEnd != model.is_end)
                {
                    if (model.is_end)
                        entity.EndTime = DateTime.Now;
                    else
                        entity.EndTime = null;
                    entity.IsEnd = model.is_end;
                }
            }
            else
                msg = "status为假时不能修改is_start、is_end";

            entity.Status = model.status;
            entity.Color = model.color;
            entity.ICON = model.icon;
            entity.Left = model.left;
            entity.Top = model.top;
            entity.Index = model.index;
            entity.NodeID = model.node_id;
            entity.Piece = model.piece;
            entity.ProcessTo = model.process_to;
            if (auto_save)
                db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 修改某个节点
        /// </summary>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SaveNode(Model.ProjectFlowNode model, out string msg)
        {
            msg = "";
            var db = new DataCore.EFDBContext();

            var entity = db.ProjectFlowNodes.Find(model.id);
            if (entity == null)
            {
                msg = "项目流程节点不存在";
                return false;
            }
            if (model.status)
            {
                if (entity.IsStart != model.is_start)
                {
                    if (model.is_start)
                        entity.BeginTime = DateTime.Now;
                    else
                        entity.BeginTime = null;
                    entity.IsStart = model.is_start;
                }

                if (entity.IsEnd != model.is_end)
                {
                    if (model.is_end)
                        entity.EndTime = DateTime.Now;
                    else
                        entity.EndTime = null;
                    entity.IsEnd = model.is_end;
                }
            }
            else
                msg = "status为假时不能修改is_start、is_end";

            entity.Status = model.status;
            entity.Color = model.color;
            entity.ICON = model.icon;
            entity.Left = model.left;
            entity.Index = model.index;
            entity.Top = model.top;
            entity.NodeID = model.node_id;
            entity.Piece = model.piece;
            entity.ProcessTo = model.process_to;

            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 修改所有节点信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SaveAllNode(Model.ProjectFlow model, out string msg)
        {
            msg = "";
            var db = new DataCore.EFDBContext();
            var entity_project = db.Projects.Find(model.project_id);
            if (entity_project == null)
            {
                db.Dispose();
                msg = "项目不存在";
                return false;
            }
            foreach (var item in model.list)
            {
                SaveNode(db, item, out msg);
            }

            entity_project.Pieces = model.reference_pieces;
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 添加流程节点
        /// </summary>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int AddNode(int project_id, string project_pieces, Model.ProjectFlowNode model, out string msg)
        {
            msg = "";
            var db = new DataCore.EFDBContext();
            var entity_project = db.Projects.Find(project_id);
            if (entity_project == null)
            {
                msg = "项目不存在";
                return 0;
            }
            entity_project.Pieces = project_pieces;

            var entity = new Entity.ProjectFlowNode();
            if (model.status)
            {
                if (entity.IsStart != model.is_start)
                {
                    if (model.is_start)
                        entity.BeginTime = DateTime.Now;
                    else
                        entity.BeginTime = null;
                    entity.IsStart = model.is_start;
                }

                if (entity.IsEnd != model.is_end)
                {
                    if (model.is_end)
                        entity.EndTime = DateTime.Now;
                    else
                        entity.EndTime = null;
                    entity.IsEnd = model.is_end;
                }
            }
            else
                msg = "status为假时不能修改is_start、is_end";

            entity.Status = model.status;
            entity.Color = model.color;
            entity.ICON = model.icon;
            entity.Index = model.index;
            entity.Left = model.left;
            entity.Top = model.top;
            entity.NodeID = model.node_id;
            entity.Piece = model.piece;
            entity.ProcessTo = model.process_to;
            entity.ProjectID = project_id;
            db.ProjectFlowNodes.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
    }
}
