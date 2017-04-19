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
        /// 获取节点实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.ProjectFlowNode GetModel(int id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                return db.ProjectFlowNodes.Include(p => p.ProjectFlowNodeFiles).Include(p => p.EditUser).Include(p => p.Node).Where(p => p.ID == id).AsNoTracking().FirstOrDefault();
            }
        }

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

            bool is_join = true;
            string last_end_process = "";
            //先获取is_end为true的节点
            var is_end_list = db.ProjectFlowNodes.AsNoTracking().Include(p => p.ProjectFlowNodeFiles).Include(p => p.Node).Include(p => p.EditUser).Where(p => p.ProjectID == project_id && p.IsEnd == true).OrderBy(p => p.EndTime).ToList();
            if (is_end_list.Count == 0)
            {
                is_join = false;
                //如果没有为true的，则查找frist节点
                is_end_list = db.ProjectFlowNodes.AsNoTracking().Include(p => p.ProjectFlowNodeFiles).Include(p => p.Node).Include(p => p.EditUser).Where(p => p.ProjectID == project_id && p.IsFrist).ToList();
            }
            foreach (var item in is_end_list)
            {
                last_end_process = item.ProcessTo;
                Model.ProjectFlowNode model = new Model.ProjectFlowNode();
                model.icon = item.ICON;
                model.remark = Tools.WebHelper.UrlDecode(item.Remark == null ? "" : item.Remark);
                model.user_name = item.EditUser.NickName;
                model.user_id = item.EditUserId;
                model.piece = item.Piece;
                model.last_update_time = item.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
                model.process_to = item.ProcessTo;
                model.node_id = item.NodeID;
                model.node_title = item.Node.Title;
                model.node_is_fator = item.Node.IsFactor;
                model.color = item.Color;
                model.left = item.Left;
                model.top = item.Top;
                model.is_end = item.IsEnd;
                model.project_flow_node_id = item.ID;
                model.status = item.Status;
                model.BuildFileList(item.ProjectFlowNodeFiles.ToList());
                response_entity.Add(model);
            }
            if (is_join)
            {
                //将下级节点一并查找出来
                var next_list = db.ProjectFlowNodes.SqlQuery("SELECT * FROM [dbo].[ProjectFlowNode] where charindex(','+ltrim(ID)+',','," + last_end_process + ",') > 0").AsNoTracking().ToList();
                foreach (var item in next_list)
                {
                    var entity_node = db.Nodes.Where(p => p.ID == item.NodeID).AsNoTracking().FirstOrDefault();
                    if (entity_node == null)
                        continue;
                    var entity_user = db.CusUsers.Where(p => p.ID == item.EditUserId).AsNoTracking().FirstOrDefault();
                    if (entity_user == null)
                        continue;
                    var node_file_list = db.ProjectFlowNodeFiles.Where(p => p.ProjectFlowNodeID == item.ID).AsNoTracking().ToList();

                    Model.ProjectFlowNode model = new Model.ProjectFlowNode();
                    model.icon = item.ICON;
                    model.piece = item.Piece;
                    model.process_to = item.ProcessTo;
                    model.node_id = item.NodeID;
                    model.remark = Tools.WebHelper.UrlDecode(item.Remark == null ? "" : item.Remark);
                    model.user_name = entity_user.NickName;
                    model.last_update_time = item.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    model.node_title = entity_node.Title;
                    model.node_is_fator = entity_node.IsFactor;
                    model.color = item.Color;
                    model.left = item.Left;
                    model.top = item.Top;
                    model.is_end = item.IsEnd;
                    model.project_flow_node_id = item.ID;
                    model.status = item.Status;
                    model.BuildFileList(node_file_list);
                    response_entity.Add(model);
                }
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
                var entity_user = db.CusUsers.Where(p => p.ID == item.EditUserId).AsNoTracking().FirstOrDefault();
                if (entity_user == null)
                    continue;
                var node_file_list = db.ProjectFlowNodeFiles.Where(p => p.ProjectFlowNodeID == item.ID).AsNoTracking().ToList();

                Model.ProjectFlowNode model = new Model.ProjectFlowNode();
                model.icon = item.ICON;
                model.piece = item.Piece;
                model.process_to = item.ProcessTo;
                model.node_id = item.NodeID;
                model.last_update_time = item.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
                model.node_title = entity_node.Title;
                model.node_is_fator = entity_node.IsFactor;
                model.color = item.Color;
                model.remark = Tools.WebHelper.UrlDecode(item.Remark == null ? "" : item.Remark);
                model.user_name = entity_user.NickName;
                model.left = item.Left;
                model.top = item.Top;
                model.is_end = item.IsEnd;
                model.project_flow_node_id = item.ID;
                model.status = item.Status;
                model.BuildFileList(node_file_list);
                response_entity.Add(model);
            }
            return response_entity;
        }

        /// <summary>
        /// 导出项目附件
        /// </summary>
        /// <param name="project_id"></param>
        /// <returns></returns>
        public static bool ImportProject(int project_id, out string zip_path)
        {
            zip_path = "";
            var project_entity = new BLL.BaseBLL<Entity.Project>().GetModel(p => p.ID == project_id);
            if (project_entity == null)
                return false;
            var frist_entity = new BLL.BaseBLL<Entity.ProjectFlowNode>().GetModel(p => p.ProjectID == project_id && p.IsFrist, p => p.ProjectFlowNodeFiles);
            if (frist_entity == null)
                return false;
            var entity_node = new BLL.BaseBLL<Entity.Node>().GetModel(p => p.ID == frist_entity.NodeID);
            if (entity_node == null)
                return false;
            List<Model.ProjectFlowNode> out_list = new List<Model.ProjectFlowNode>();
            //拼装第一个
            Model.ProjectFlowNode model = new Model.ProjectFlowNode();
            model.project_flow_node_id = frist_entity.ID;
            model.node_id = entity_node.ID;
            model.node_is_fator = entity_node.IsFactor;
            model.node_title = entity_node.Title;
            model.BuildFileList(frist_entity.ProjectFlowNodeFiles.ToList());
            out_list.Add(model);
            RecursiveGetNext(project_id, model.project_flow_node_id, out_list);

            #region  生成ZIP
            string temp_base_floder = "/uploads/temp/";
            string project_floder_name = Tools.IOHelper.FileNameFilter(project_entity.Title);
            string temp_floder_path = temp_base_floder + project_floder_name + "/";
            Tools.IOHelper.CreateDirectory(temp_floder_path);

            for (int i = 0; i < out_list.Count; i++)
            {
                if (!out_list[i].node_is_fator)
                {
                    //子目录名称
                    string child_floder_name = (i + 1).ToString() + "-" + out_list[i].node_title + "/";
                    //子目录相对路径
                    string temp_child_floder = temp_floder_path + child_floder_name;
                    Tools.IOHelper.CreateDirectory(temp_child_floder);
                    //循环复制当前节点的附件到临时目录
                    foreach (var node_files in out_list[i].files)
                    {
                        string new_file_name = Tools.IOHelper.GetIOFileName(node_files.file_name);
                        Tools.IOHelper.CopyFile(node_files.file_path, temp_child_floder + new_file_name);
                    }

                }
            }
            zip_path = "/uploads/zip/" + project_floder_name + ".zip";
            //return Tools.ZipHelper.Zip(Tools.IOHelper.GetMapPath(temp_floder_path), Tools.IOHelper.GetMapPath(zip_path));
            var is_ok = Tools.WebHelper.ToZip(Tools.IOHelper.GetMapPath(zip_path), Tools.IOHelper.GetMapPath(temp_floder_path));
            Tools.IOHelper.DeleteDirectory(temp_floder_path);
            return is_ok;
            #endregion
        }

        private static void RecursiveGetNext(int project_id, int project_flow_node_id, List<Model.ProjectFlowNode> out_list)
        {
            if (out_list == null) out_list = new List<Model.ProjectFlowNode>();
            var db_list = GetNextFlowNode(project_id, project_flow_node_id);
            if (db_list.Count != 0)
            {
                var node_list = db_list.Where(p => p.is_end == true);
                if (node_list.Count()!= 0)
                {
                    out_list.Add(node_list.First());
                    RecursiveGetNext(project_id, node_list.FirstOrDefault().project_flow_node_id, out_list);
                }
            }
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
            var entity_flow_node = db.ProjectFlowNodes.Include(p => p.Node).Include(p => p.ProjectFlowNodeFiles).Include(p => p.EditUser).Where(p => p.ID == project_flow_node_id).AsNoTracking().FirstOrDefault();
            if (entity_flow_node == null)
                return response_entity;
            response_entity.icon = entity_flow_node.ICON;
            response_entity.piece = entity_flow_node.Piece;
            response_entity.process_to = entity_flow_node.ProcessTo;
            response_entity.node_id = entity_flow_node.NodeID;
            response_entity.remark = Tools.WebHelper.UrlDecode(entity_flow_node.Remark == null ? "" : entity_flow_node.Remark);
            response_entity.user_name = entity_flow_node.EditUser.NickName;
            response_entity.node_title = entity_flow_node.Node.Title;
            response_entity.node_is_fator = entity_flow_node.Node.IsFactor;
            response_entity.color = entity_flow_node.Color;
            response_entity.left = entity_flow_node.Left;
            response_entity.last_update_time = entity_flow_node.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            response_entity.top = entity_flow_node.Top;
            response_entity.is_end = entity_flow_node.IsEnd;
            response_entity.project_flow_node_id = entity_flow_node.ID;
            response_entity.status = entity_flow_node.Status;
            response_entity.BuildFileList(entity_flow_node.ProjectFlowNodeFiles.ToList());
            db.Dispose();
            return response_entity;
        }

        /// <summary>
        /// 保存节点位置信息
        /// </summary>
        /// <returns></returns>
        public static bool SaveLocation(List<Model.ProjectFlowNode> list_model, int user_id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                foreach (var model in list_model)
                {
                    var entity = db.ProjectFlowNodes.Find(model.project_flow_node_id);
                    if (entity == null)
                        continue;

                    entity.LastUpdateTime = DateTime.Now;
                    entity.EditUserId = user_id;
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
        public static bool SetEnd(int project_flow_node_id, int user_id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.ProjectFlowNodes.Include(p => p.Node).Where(p => p.ID == project_flow_node_id).FirstOrDefault();
                if (entity == null)
                    return false;
                entity.EditUserId = user_id;
                entity.IsEnd = true;
                entity.LastUpdateTime = DateTime.Now;
                entity.EndTime = DateTime.Now;

                //发送消息提醒
                List<int> user_list = db.Database.SqlQuery<int>("select CusUserID from ProjectUser where ProjectID = " + entity.ProjectID.ToString()).ToList();
                string content = string.Format(BLLMsgTemplate.ProjectFlowDone, entity.Node.Title);
                foreach (var item in user_list)
                {
                    Entity.CusUserMessage entity_msg = new Entity.CusUserMessage();
                    entity_msg.Content = content;
                    entity_msg.CusUserID = item;
                    entity_msg.Type = Entity.CusUserMessageType.projectflowdone;
                    entity_msg.LinkID = entity.ProjectID.ToString();
                    db.CusUserMessages.Add(entity_msg);
                }
                db.SaveChanges();
                string ids = string.Join(",", user_list.ToArray());
                var telphone_list = db.Database.SqlQuery<string>("select Telphone from CusUser where id in (" + ids + ")").ToList();
                Tools.JPush.PushALl(string.Join(",", telphone_list.ToArray()), content, (int)Entity.CusUserMessageType.favdocupdate, entity.ProjectID.ToString());
                return true;
            }
        }

        /// <summary>
        /// 开启/关闭节点
        /// </summary>
        /// <param name="project_flow_node_id"></param>
        /// <returns></returns>
        public static bool SetStatus(int project_flow_node_id, int user_id, out string msg)
        {
            msg = "";
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.ProjectFlowNodes.Find(project_flow_node_id);
                if (entity == null)
                    return false;
                if (entity.Status)
                {
                    entity.Status = false;
                    entity.IsEnd = true;
                    entity.EndTime = DateTime.Now;
                    msg = "关闭成功";
                }
                else
                {
                    entity.Status = true;
                    msg = "开启成功";
                }
                entity.LastUpdateTime = DateTime.Now;
                entity.EditUserId = user_id;
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 设置条件节点选中
        /// </summary>
        /// <param name="project_flow_node_id"></param>
        /// <returns></returns>
        public static bool SetSelect(int project_flow_node_id, int user_id, out string msg)
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
                if (!entity.Node.IsFactor)
                {
                    msg = "该节点不是条件节点";
                    return false;
                }
                //获取同级条件节点
                string sql = "SELECT * FROM [dbo].[ProjectFlowNode] where charindex(','+ltrim(ID)+',',(SELECT ','+ProcessTo+',' FROM [dbo].[ProjectFlowNode] where charindex('," + project_flow_node_id.ToString() + ",',','+ltrim(ProcessTo)+',') > 0)) > 0";
                var tj_list = db.ProjectFlowNodes.SqlQuery(sql).AsNoTracking().ToList();
                foreach (var item in tj_list)
                {
                    if (item.ID != project_flow_node_id && item.IsSelect)
                    {
                        msg = "已有其他条件节点设置为选中状态";
                        return false;
                    }
                }
                //条件都满足
                entity.EditUserId = user_id;
                entity.IsEnd = true;
                entity.LastUpdateTime = DateTime.Now;
                entity.EndTime = DateTime.Now;
                entity.IsSelect = true;
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 修改节点的备注和附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="remark"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool ModifyRemarkFile(int id, int user_id, string remark, string files, out string msg)
        {
            msg = "保存成功";
            files = files == null ? "" : files;
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.ProjectFlowNodes.Find(id);
                if (entity == null)
                {
                    msg = "节点不存在";
                    return false;
                }

                if(!db.ProjectUsers.Any(p=>p.CusUserID == user_id && p.ProjectID == entity.ProjectID))
                {
                    msg = "没有权限操作该节点";
                    return false;
                }

                if (!db.CusUsers.Any(p => p.ID == user_id))
                {
                    msg = "用户不存在";
                    return false;
                }

                db.ProjectFlowNodeFiles.Where(p => p.ProjectFlowNodeID == id).ToList().ForEach(p => db.ProjectFlowNodeFiles.Remove(p));

                entity.LastUpdateTime = DateTime.Now;
                entity.EditUserId = user_id;
                entity.Remark = remark;

                foreach (var item in files.Split('|'))
                {
                    if (string.IsNullOrWhiteSpace(item))
                        continue;

                    var file = item.Split(',');
                    if (file.Length != 3)
                        continue;
                    Entity.ProjectFlowNodeFile entity_file = new Entity.ProjectFlowNodeFile();
                    entity_file.FilePath = file[0];
                    entity_file.FileName = file[1];
                    entity_file.FileSize = file[2];
                    entity_file.ProjectFlowNodeID = id;
                    db.ProjectFlowNodeFiles.Add(entity_file);
                }
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 保存节点附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool SaveFile(int id, List<Model.ProjectFlowNodeFile> list)
        {
            using (var db = new DataCore.EFDBContext())
            {
                if (!db.ProjectFlowNodes.Any(p => p.ID == id))
                    return false;

                db.ProjectFlowNodeFiles.Where(p => p.ProjectFlowNodeID == id).ToList().ForEach(p => db.ProjectFlowNodeFiles.Remove(p));
                foreach (var item in list)
                {
                    Entity.ProjectFlowNodeFile entity = new Entity.ProjectFlowNodeFile();
                    entity.FileName = item.file_name;
                    entity.FilePath = item.file_path;
                    entity.FileSize = item.file_size;
                    entity.ProjectFlowNodeID = id;
                    db.ProjectFlowNodeFiles.Add(entity);
                }
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
