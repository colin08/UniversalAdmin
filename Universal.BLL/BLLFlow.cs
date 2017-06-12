using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 流程
    /// </summary>
    public class BLLFlow
    {
        /// <summary>
        /// 删除流程，同时删除其子数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Del(int id)
        {
            if (id <= 0)
                return false;
            var db = new DataCore.EFDBContext();
            var use_total = db.Projects.AsNoTracking().Count(p => p.FlowID == id);
            if (use_total > 0)
            {
                db.Dispose();
                return false;
            }
            var entity = db.Flows.Find(id);
            if (entity == null)
            {
                db.Dispose();
                return false;
            }

            db.Flows.Remove(entity);
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// 获取所有流程
        /// </summary>
        /// <returns></returns>
        public static List<Entity.Flow> GetAllFlow()
        {
            using (var db = new DataCore.EFDBContext())
            {
                return db.Flows.Where(p => p.FlowType == Entity.FlowType.basic).AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// 设置默认
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SetDefault(int id, out string msg)
        {
            msg = "设置成功";
            if (id <= 0)
            {
                msg = "非法参数";
                return false;
            }

            var db = new DataCore.EFDBContext();
            var entity = db.Flows.Find(id);
            if (entity.IsDefault)
            {
                msg = "已经是默认了";
                db.Dispose();
                return false;
            }
            db.Database.ExecuteSqlCommand("update Flow set IsDefault = 0 where IsDefault = 1");
            entity.IsDefault = true;
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// 获取默认流程id
        /// </summary>
        /// <returns></returns>
        public static int GetDefault()
        {
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.Flows.Where(p => p.IsDefault == true).AsNoTracking().FirstOrDefault();
                if (entity == null)
                    return 0;
                else
                    return entity.ID;
            }
        }

        /// <summary>
        /// 获取前端需要的流程节点数据
        /// </summary>
        /// <param name="flow_id"></param>
        /// <returns></returns>
        public static List<Model.FlowNode> GetUIFlowNode(int flow_id)
        {
            List<Model.FlowNode> result = new List<Model.FlowNode>();
            var db = new DataCore.EFDBContext();
            if (flow_id != -1)
            {
                var entity_flow = db.Flows.Where(p => p.ID == flow_id).AsNoTracking().FirstOrDefault();
                if (entity_flow == null)
                {
                    db.Dispose();
                    return result;
                }
            }
            var category_list = db.NodeCategorys.AsNoTracking().OrderByDescending(p => p.AddTime).ToList();
            foreach (var category in category_list)
            {
                Model.FlowNode model = new Model.FlowNode();
                model.category_id = category.ID;
                model.category_name = category.Title;
                List<Model.FlowNodeList> node_list = new List<Model.FlowNodeList>();
                var db_node_list = db.Nodes.Where(p => p.NodeCategoryID == category.ID).OrderByDescending(p => p.AddTime).AsNoTracking().ToList();
                foreach (var node in db_node_list)
                {
                    Model.FlowNodeList model_node = new Model.FlowNodeList();
                    model_node.node_id = node.ID;
                    model_node.node_name = node.Title;
                    model_node.exists = db.FlowNodes.Any(p => p.FlowID == flow_id && p.NodeID == node.ID);
                    List<Model.FlowCNodeList> c_node_list = new List<Model.FlowCNodeList>();
                    if (flow_id != -1 && model_node.exists)
                    {
                        string sql = "SELECT * from dbo.Node where ID in(SELECT NodeID FROM [dbo].[FlowNode] where charindex(','+rtrim(ID)+',', (select top 1 ','+ISNULL(ProcessTo, '')+',' as ProcessTo from dbo.FlowNode where FlowID = " + flow_id.ToString() + " and NodeID = " + node.ID.ToString() + "))>0)";
                        var db_p_node_list = db.Nodes.SqlQuery(sql).ToList();
                        foreach (var p_node in db_p_node_list)
                        {
                            Model.FlowCNodeList model_p_node = new Model.FlowCNodeList();
                            model_p_node.c_node_id = p_node.ID;
                            model_p_node.c_node_name = p_node.Title;
                            c_node_list.Add(model_p_node);
                        }
                    }
                    model_node.c_node_list = c_node_list;
                    node_list.Add(model_node);
                }
                model.node_list = node_list;
                result.Add(model);
            }
            db.Dispose();
            return result;
        }

        /// <summary>
        /// 设置节点子级信息
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="flow_id"></param>
        /// <param name="title"></param>
        /// <param name="node_id"></param>
        /// <param name="cids"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int[] SetNodeCids(int user_id, int flow_id, string title, int node_id, string cids, out string msg)
        {
            int[] result = new int[2];
            result[0] = -1;
            result[1] = -1;
            msg = "ok";
            if (flow_id < -1 || flow_id == 0 || node_id <= 0 || user_id <= 0)
            {
                msg = "非法参数";
                return result;
            }
            if (flow_id == -1 && string.IsNullOrWhiteSpace(title))
            {
                msg = "添加流程时必须传入标题";
                return result;
            }
            if (string.IsNullOrWhiteSpace(cids))
                cids = "";
            //if (cids.Trim().Length > 0)
            //{
            //    if (!cids.StartsWith(","))
            //        cids = "," + cids;

            //    if (!cids.EndsWith(","))
            //        cids = cids + ",";
            //}


            var db = new DataCore.EFDBContext();
            var entity_node = db.Nodes.Where(p => p.ID == node_id).AsNoTracking().FirstOrDefault();
            if (!db.CusUsers.Any(p => p.ID == user_id))
            {
                msg = "用户不存在";
                return result;
            }
            if (entity_node == null)
            {
                msg = "节点不存在";
                return result;
            }

            var entity_flow = new Entity.Flow();
            if (flow_id != -1)
            {
                entity_flow = db.Flows.Find(flow_id);
                if (entity_flow == null)
                {
                    msg = "流程不存在";
                    return result;
                }
                if (entity_flow.Title != title && !string.IsNullOrWhiteSpace(title))
                {
                    entity_flow.Title = title;
                }
            }
            else
            {
                entity_flow.CusUserID = user_id;
                entity_flow.Title = title;
                entity_flow.FlowType = Entity.FlowType.basic;
                db.Flows.Add(entity_flow);
                db.SaveChanges();
                flow_id = entity_flow.ID;
            }
            StringBuilder str_cids = new StringBuilder();
            //处理子id
            foreach (var item in cids.Split(','))
            {
                int c_node_id = Tools.TypeHelper.ObjectToInt(item);
                if (c_node_id == node_id)
                    continue;

                if (!db.Nodes.Any(p => p.ID == c_node_id))
                    continue;

                var entity_temp = db.FlowNodes.Where(p => p.FlowID == flow_id && p.NodeID == c_node_id).AsNoTracking().FirstOrDefault();
                if (entity_temp == null)
                {
                    entity_temp = new Entity.FlowNode();
                    entity_temp.FlowID = flow_id;
                    entity_temp.NodeID = c_node_id;
                    entity_temp.ProcessTo = "";
                    entity_temp.Left = 100;
                    entity_temp.Top = 100;
                    db.FlowNodes.Add(entity_temp);
                    db.SaveChanges();
                }
                str_cids.Append(entity_temp.ID.ToString() + ",");
            }
            if (str_cids.Length > 0)
                str_cids.Remove(str_cids.Length - 1, 1);

            var entity_flow_node = db.FlowNodes.Where(p => p.FlowID == flow_id && p.NodeID == node_id).FirstOrDefault();
            if (entity_flow_node == null)
            {
                entity_flow_node = new Entity.FlowNode();
                entity_flow_node.FlowID = flow_id;
                entity_flow_node.ProcessTo = str_cids.ToString();
                entity_flow_node.NodeID = entity_node.ID;
                entity_flow_node.Left = 100;
                entity_flow_node.Top = 100;
                db.FlowNodes.Add(entity_flow_node);
            }
            else
            {
                entity_flow_node.ProcessTo = str_cids.ToString();
            }
            db.SaveChanges();
            db.Dispose();
            result[0] = entity_flow.ID;
            result[1] = entity_flow_node.ID;
            return result;
        }

        /// <summary>
        /// 删除一条节点数据
        /// </summary>
        /// <param name="node_id"></param>
        /// <returns></returns>
        public static bool DelFlowNode(int flow_id, int node_id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.FlowNodes.AsNoTracking().Where(p => p.FlowID == flow_id && p.NodeID == node_id).FirstOrDefault();
                if (entity != null)
                {
                    //迭代获取子节点自增ID
                    StringBuilder child_ids = new StringBuilder();
                    GetChildNodeID(db, flow_id, node_id, child_ids);
                    if (child_ids.Length > 0)
                        child_ids.Remove(child_ids.Length - 1, 1);
                    if(child_ids.Length>0)
                    {
                        string strSql = "delete FlowNode where ID in(" + child_ids.ToString() + ")";
                        db.Database.ExecuteSqlCommand(strSql);
                    }

                    //删除子节点
                    //if (!string.IsNullOrWhiteSpace(entity.ProcessTo))
                    //{
                    //    string strSql = "delete FlowNode where ID in(" + entity.ProcessTo + ")";
                    //    db.Database.ExecuteSqlCommand(strSql);
                    //}
                    //db.FlowNodes.Remove(entity);
                    //db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public static void GetChildNodeID(DataCore.EFDBContext db, int flow_id, int node_id, System.Text.StringBuilder ids)
        {
            if (node_id == 0) return;

            var entity = db.FlowNodes.Where(p => p.FlowID == flow_id && p.NodeID == node_id).FirstOrDefault();
            if (entity == null) return;
            ids.Append(entity.ID.ToString() + ",");
            if (!string.IsNullOrWhiteSpace(entity.ProcessTo))
            {
                string sql_child = "select * from FlowNode where ID in(" + entity.ProcessTo + ")";
                var child_list = db.FlowNodes.SqlQuery(sql_child).ToList();
                foreach (var item in child_list)
                {
                    GetChildNodeID(db, flow_id, item.NodeID, ids);
                }

            }
            return;
        }

        /// <summary>
        /// 设置流程起始节点
        /// </summary>
        /// <param name="flow_id"></param>
        /// <param name="node_id"></param>
        /// <returns></returns>
        public static bool SetFlowFristNode(int flow_id, int node_id, out string msg)
        {
            msg = "设置成功";
            using (var db = new DataCore.EFDBContext())
            {
                var entity_flow_node = db.FlowNodes.Where(p => p.FlowID == flow_id && p.NodeID == node_id).FirstOrDefault();
                if (entity_flow_node == null)
                {
                    msg = "该节点不属于该流程";
                    return false;
                }
                if (entity_flow_node.is_frist)
                {
                    msg = "该节点已经是开始节点了";
                    return false;
                }

                string sql = "update FlowNode set is_frist = 0 where FlowID =" + flow_id.ToString();
                entity_flow_node.is_frist = true;
                db.Database.ExecuteSqlCommand(sql);
                db.SaveChanges();
                return true;
            }
        }

        ///// <summary>
        ///// 生成插件所需流程节点数据
        ///// </summary>
        ///// <returns></returns>
        //public static bool GenerateFlowNodeCompact(int flow_id)
        //{
        //    var db = new DataCore.EFDBContext();
        //    var entity_flow = db.Flows.Where(p => p.ID == flow_id).AsNoTracking().FirstOrDefault();
        //    if (entity_flow == null)
        //        return false;

        //    db.FlowNodeCompacts.Where(p => p.FlowID == flow_id).ToList().ForEach(p => db.FlowNodeCompacts.Remove(p));

        //    //先获取所有用到的节点id
        //    string sql = "SELECT (Pids + Cast(NodeID as nvarchar(10))) as ids  FROM [dbo].[FlowNode] where FlowID = " + flow_id.ToString() + ";";
        //    var all_use_node_ids = db.Database.SqlQuery<string>(sql).ToList();
        //    List<int> use_node_id_list = new List<int>();
        //    foreach (var node_ids in all_use_node_ids)
        //    {
        //        foreach (var node in node_ids.Split(','))
        //        {
        //            int val = Tools.TypeHelper.ObjectToInt(node, -1);
        //            if (val != -1)
        //            {
        //                if (!use_node_id_list.Contains(val))
        //                    use_node_id_list.Add(val);
        //            }
        //        }
        //    }
        //    //新表，节点对应数据ID
        //    Hashtable new_tab = new Hashtable();
        //    //将用到的节点id全部添加到compact表里
        //    foreach (var node_id in use_node_id_list)
        //    {
        //        //判断是否是顶级ID
        //        var entity_node = db.FlowNodes.Where(p => p.NodeID == node_id).AsNoTracking().FirstOrDefault();
        //        bool is_frist = true;
        //        if (entity_node != null)
        //        {
        //            if (!string.IsNullOrWhiteSpace(entity_node.PIds))
        //                is_frist = false;
        //        }

        //        var entity_flow_node_compact = new Entity.FlowNodeCompact();
        //        entity_flow_node_compact.FlowID = flow_id;
        //        entity_flow_node_compact.IsFrist = is_frist;
        //        entity_flow_node_compact.NodeID = node_id;
        //        entity_flow_node_compact.ProcessTo = "";
        //        db.FlowNodeCompacts.Add(entity_flow_node_compact);
        //        db.SaveChanges();
        //        new_tab.Add(node_id, entity_flow_node_compact.ID);
        //    }
        //    //修改对应关系
        //    foreach (var item in new_tab.Keys)
        //    {
        //        int node_id = Tools.TypeHelper.ObjectToInt(item);
        //        //查找改节点在映射表里的子节点
        //        var child_list = db.FlowNodes.SqlQuery("SELECT * FROM [dbo].[FlowNode] where FlowID = 59 and CHARINDEX('," + node_id.ToString() + ",',PIds) > 0 ").AsNoTracking().ToList();
        //        StringBuilder str_process = new StringBuilder();
        //        foreach (var flow_node in child_list)
        //        {
        //            str_process.Append(new_tab[flow_node.NodeID].ToString() + ",");
        //        }
        //        if (str_process.Length > 0)
        //        {
        //            str_process.Remove(str_process.Length - 1, 1);
        //            db.Database.ExecuteSqlCommand("update [dbo].[FlowNodeCompact] set ProcessTo = '" + str_process.ToString() + "' where FlowID = " + flow_id.ToString() + " and NodeID = " + node_id.ToString());
        //        }

        //    }
        //    db.Dispose();
        //    return true;
        //}


        /// <summary>
        /// 获取的流程信息
        /// </summary>
        /// <returns></returns>
        public static List<Model.ProjectFlowNode> GetFlowData(int flow_id)
        {
            List<Model.ProjectFlowNode> response_entity = new List<Model.ProjectFlowNode>();

            if (flow_id <= 0)
                return response_entity;
            var db = new DataCore.EFDBContext();
            var entity_flow = db.Flows.Where(p => p.ID == flow_id).AsNoTracking().FirstOrDefault();
            if (entity_flow == null)
                return response_entity;
            var flow_node_list = db.FlowNodes.AsNoTracking().Include(p => p.Node).Where(p => p.FlowID == flow_id).ToList();
            foreach (var item in flow_node_list)
            {
                Model.ProjectFlowNode model = new Model.ProjectFlowNode();
                model.icon = item.ICON;
                model.process_to = item.ProcessTo;
                model.node_id = item.NodeID;
                model.node_title = item.Node.Title;
                model.color = item.Color;
                model.left = item.Left;
                model.top = item.Top;
                model.is_frist = item.is_frist;
                model.project_flow_node_id = item.ID;
                response_entity.Add(model);
            }
            db.Dispose();
            return response_entity;
        }

        /// <summary>
        /// 修改所有演示节点信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SaveCompactNode(List<Model.ProjectFlowNode> list_model)
        {
            using (var db = new DataCore.EFDBContext())
            {
                foreach (var model in list_model)
                {
                    var entity = db.FlowNodes.Find(model.project_flow_node_id);
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
        /// 添加流程节点
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="flow_id">所属流程ID</param>
        /// <param name="title">流程标题</param>
        /// <param name="node_id">节点ID</param>
        /// <param name="parent_flow_node_id">父级流程节点ID</param>
        /// <param name="type">添加类别，1：插入节点，2：添加子节点，3：添加合并节点</param>
        /// <returns></returns>
        public static int[] AddFlowNode(int user_id, int flow_id, string title, int node_id, int parent_flow_node_id, int type, out string msg)
        {
            int[] result = new int[2];
            result[0] = -1;
            result[1] = -1;
            msg = "ok";
            if (flow_id < -1 || flow_id == 0 || node_id <= 0 || user_id <= 0)
            {
                msg = "非法参数";
                return result;
            }
            if (flow_id == -1 && string.IsNullOrWhiteSpace(title))
            {
                msg = "添加流程时必须传入标题";
                return result;
            }

            var db = new DataCore.EFDBContext();
            var entity_node = db.Nodes.Find(node_id);
            if (!db.CusUsers.Any(p => p.ID == user_id))
            {
                msg = "用户不存在";
                return result;
            }
            if (entity_node == null)
            {
                msg = "节点不存在";
                return result;
            }
            switch (type)
            {
                case 1:
                case 2:
                    break;
                case 3:
                    if (parent_flow_node_id == 0)
                    {
                        msg = "添加并行节点时必须传入父级ID";
                        return result;
                    }
                    break;
                default:
                    msg = "添加类别不明确";
                    return result;
            }
            string pids = "";
            string end_sql = "";
            //父级节点
            Entity.FlowNode parent_node = null;
            if (parent_flow_node_id > 0)
            {
                parent_node = db.FlowNodes.AsNoTracking().Where(p => p.ID == parent_flow_node_id).FirstOrDefault();
                if (parent_node == null)
                {
                    msg = "父级流程节点不存在";
                    return result;
                }
                if (type == 1)
                {
                    //插入节点(下级节点往下移动)，并修改下级节点的父ID为当前添加节点的ID
                    var child_list = db.FlowNodes.SqlQuery("SELECT * FROM [dbo].[FlowNode] where CHARINDEX('," + parent_flow_node_id.ToString() + ",',PIds) > 0 ").AsNoTracking().ToList();
                    if (child_list.Count > 0)
                    {
                        StringBuilder str_ids = new StringBuilder();
                        foreach (var item in child_list)
                            str_ids.Append(item.ID.ToString() + ",");
                        if (str_ids.Length > 0)
                        {
                            str_ids.Remove(str_ids.Length - 1, 1);
                            end_sql = "update [dbo].[FlowNode] set Pids=',{0},' where ID in(" + str_ids + ")";
                        }
                    }
                    //当前添加的节点
                    pids = "," + parent_flow_node_id.ToString() + ",";
                }
                else if (type == 2)
                {
                    //添加子并行节点，没啥特别的
                    var first_child = db.FlowNodes.SqlQuery("SELECT top 1 * FROM [dbo].[FlowNode] where CHARINDEX('," + parent_flow_node_id.ToString() + ",',PIds) > 0 ").AsNoTracking().FirstOrDefault();
                    if (first_child != null)
                    {
                        var first_child_node = db.Nodes.AsNoTracking().Where(p => p.ID == first_child.NodeID).FirstOrDefault();
                        if (first_child_node != null)
                        {
                            if (first_child_node.IsFactor != entity_node.IsFactor)
                            {
                                string txt = first_child_node.IsFactor ? "条件节点" : "普通节点";
                                msg = "需要添加节点的类型为:" + txt;
                                return result;
                            }
                        }
                        else
                        {
                            msg = "兄弟节点所属Node不存在";
                            return result;
                        }
                    }
                    pids = "," + parent_flow_node_id.ToString() + ",";
                }
                else if (type == 3)
                {
                    //合并节点，查找父级所有的同级ID，逗号分割作为当前添加的节点父级IDs
                    var child_list = db.FlowNodes.SqlQuery("SELECT * FROM [dbo].[FlowNode] where CHARINDEX('" + parent_node.PIds + "',PIds) > 0 ").AsNoTracking().ToList();
                    if (child_list.Count > 0)
                    {
                        StringBuilder str_ids = new StringBuilder();
                        foreach (var item in child_list)
                            str_ids.Append(item.ID.ToString() + ",");
                        if (str_ids.Length > 0)
                            pids = "," + str_ids;
                        else
                        {
                            msg = "找不到父级的同级节点，无法合并。";
                            return result;
                        }
                    }
                    else
                    {
                        msg = "添加类别不明确";
                        return result;
                    }
                }
            }

            var entity_flow = new Entity.Flow();
            if (flow_id != -1)
            {
                entity_flow = db.Flows.Find(flow_id);
                if (entity_flow == null)
                {
                    msg = "流程不存在";
                    return result;
                }
                if (entity_flow.Title != title && !string.IsNullOrWhiteSpace(title))
                {
                    entity_flow.Title = title;
                }
            }
            else
            {
                entity_flow.CusUserID = user_id;
                entity_flow.Title = title;
                entity_flow.FlowType = Entity.FlowType.basic;
                db.Flows.Add(entity_flow);
            }


            var entity_flow_node = new Entity.FlowNode();
            entity_flow_node.Flow = entity_flow;
            entity_flow_node.PIds = pids;
            entity_flow_node.NodeID = entity_node.ID;

            db.FlowNodes.Add(entity_flow_node);
            db.SaveChanges();
            int now_node_id = entity_flow_node.ID;
            if (end_sql.Length > 0)
                db.Database.ExecuteSqlCommand(string.Format(end_sql, now_node_id.ToString()));
            db.Dispose();
            result[0] = entity_flow.ID;
            result[1] = entity_flow_node.ID;
            return result;
        }

        /// <summary>
        /// 前端：获取流程节点信息
        /// </summary>
        /// <param name="flow_id">流程ID</param>
        /// <returns></returns>
        public static Model.WebFlow GetWebFlowData(int flow_id)
        {
            Model.WebFlow response_entity = new Model.WebFlow();
            List<Model.WebFlowNode> response_list = new List<Model.WebFlowNode>();
            var db = new DataCore.EFDBContext();
            BLL.BaseBLL<Entity.FlowNode> bll = new BaseBLL<Entity.FlowNode>();
            var db_list = bll.GetListBy(0, p => p.FlowID == flow_id, "SortNo ASC", p => p.Node);
            foreach (var item in db_list)
            {

                Model.WebFlowNode model = new Model.WebFlowNode();
                model.flow_node_id = item.ID;
                model.node_id = item.NodeID;
                model.flow_node_title = item.Node.Title;
                model.pids = item.PIds;
                response_list.Add(model);
            }
            response_entity.flow_id = flow_id;
            response_entity.total = response_list.Count;
            response_entity.list = response_list;
            db.Dispose();
            return response_entity;
        }

        /// <summary>
        /// 前端：获取某个流程节点信息
        /// </summary>
        /// <param name="flow_id">流程ID</param>
        /// <returns></returns>
        public static Model.WebFlowNode GetWebFlowNodeData(int flow_node_id)
        {
            Model.WebFlowNode response_entity = null;
            BLL.BaseBLL<Entity.FlowNode> bll = new BaseBLL<Entity.FlowNode>();
            var entity = bll.GetModel(p => p.ID == flow_node_id, p => p.Node);
            if (entity != null)
            {
                response_entity = new Model.WebFlowNode();
                response_entity.flow_node_id = entity.ID;
                response_entity.node_id = entity.NodeID;
                response_entity.flow_node_title = entity.Node.Title;
                response_entity.pids = entity.PIds;
            }
            return response_entity;
        }

        /// <summary>
        /// 前端：删除某个流程节点信息
        /// </summary>
        /// <param name="flow_node_id"></param>
        /// <returns></returns>
        public static bool DelWebFlowNode(int flow_node_id, out string msg)
        {
            msg = "ok";
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.FlowNodes.Find(flow_node_id);
                if (entity == null)
                    return false;
                db.FlowNodes.Remove(entity);

                //该元素是否有同级节点
                var tongji_list = db.FlowNodes.SqlQuery("SELECT * FROM [dbo].[FlowNode] where CHARINDEX( '" + entity.PIds + "',PIds) > 0 ").AsNoTracking().ToList();

                //该元素的子id
                var child_list = db.FlowNodes.SqlQuery("SELECT * FROM [dbo].[FlowNode] where CHARINDEX( '," + flow_node_id.ToString() + ",',PIds) > 0 ").AsNoTracking().ToList();

                //如果当前节点有并行节点，并且有子节点，则提醒不可删除
                if (tongji_list.Count > 1 && child_list.Count > 0)
                {
                    msg = "该元素有同级节点，并且有子节点，所以不可删除。";
                    return false;
                }
                else
                {
                    //将子级节点的父id修改为当前节点的父id
                    StringBuilder str_ids = new StringBuilder();
                    foreach (var item in child_list)
                        str_ids.Append(item.ID.ToString() + ",");
                    if (str_ids.Length > 0)
                    {
                        str_ids.Remove(str_ids.Length - 1, 1);
                        string sql = "update [dbo].[FlowNode] set Pids='" + entity.PIds + "' where ID in(" + str_ids + ")";
                        db.Database.ExecuteSqlCommand(sql);
                    }
                }
                db.SaveChanges();
                return true;
            }
        }

        ///// <summary>
        ///// 前端：保存流程信息
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public static bool WebSaveFlowData(Model.WebSaveFlow model, out string msg)
        //{
        //    msg = "";
        //    if (model == null)
        //    {
        //        msg = "非法参数";
        //        return false;
        //    }
        //    if (model.flow_node_list == null)
        //    {
        //        msg = "节点信息不能为空";
        //        return false;
        //    }

        //    var db = new DataCore.EFDBContext();
        //    var entity_flow = db.Flows.Find(model.flow_id);
        //    if (entity_flow == null)
        //    {
        //        msg = "流程不存在";
        //        return false;
        //    }
        //    //修改引用的块
        //    entity_flow.Pieces = model.reference_pieces;

        //    foreach (var item in model.flow_node_list)
        //    {
        //        var entity_flow_node = db.FlowNodes.Find(item.flow_node_id);
        //        if (entity_flow_node == null)
        //        {
        //            msg = "节点" + item.flow_node_id.ToString() + "不存在";
        //            return false;
        //        }
        //        if (entity_flow_node.FlowID != model.flow_id)
        //        {
        //            msg = "流程节点" + item.flow_node_id.ToString() + "不属于流程" + model.flow_id.ToString();
        //            return false;
        //        }

        //        entity_flow_node.Top = item.top;
        //        entity_flow_node.ProcessTo = item.process_to;
        //        entity_flow_node.Left = item.left;
        //        entity_flow_node.Piece = item.piece;
        //        entity_flow_node.ICON = item.icon;
        //        entity_flow_node.Color = item.color;
        //    }
        //    db.SaveChanges();
        //    db.Dispose();
        //    return true;
        //}

        ///// <summary>
        ///// 前端：修改某个流程节点信息
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public static bool WebSaveFlowNodeData(Model.WebSaveFlowNode model, out string msg)
        //{
        //    msg = "";
        //    if (model == null)
        //    {
        //        msg = "非法参数";
        //        return false;
        //    }

        //    var db = new DataCore.EFDBContext();
        //    var entity_flow_node = db.FlowNodes.Find(model.flow_node_id);
        //    if (entity_flow_node == null)
        //    {
        //        msg = "节点" + model.flow_node_id.ToString() + "不存在";
        //        return false;
        //    }
        //    entity_flow_node.Top = model.top;
        //    entity_flow_node.ProcessTo = model.process_to;
        //    entity_flow_node.Left = model.left;
        //    entity_flow_node.ICON = model.icon;
        //    entity_flow_node.Piece = model.piece;
        //    entity_flow_node.Color = model.color;
        //    db.SaveChanges();
        //    db.Dispose();
        //    return true;
        //}

    }
}
