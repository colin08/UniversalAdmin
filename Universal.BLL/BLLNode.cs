using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 节点
    /// </summary>
    public class BLLNode
    {
        /// <summary>
        /// 节点是否可以删除,如果不能删除，则提示所有引用的数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CanDel(int id, out string msg)
        {
            msg = "";
            StringBuilder result = new StringBuilder();
            string flow_node_sql = "select Cast(ID as varchar(10))+':'+Title from Flow where ID in(SELECT FlowID FROM [dbo].[FlowNode] where NodeID=" + id.ToString() + " or charindex(',"+id.ToString()+",',','+ProcessTo+',')>0 GROUP BY(FlowID));";
            var db = new DataCore.EFDBContext();
            var flow_node_arr = db.Database.SqlQuery<string>(flow_node_sql).ToArray();
            if (flow_node_arr.Count() > 0)
            {
                result.Append("引用该节点的流程如下：\r\n");
                result.Append(string.Join(",", flow_node_arr) + "\r\n");

            }
            string project_flow_node_sql = "select Cast(ID as varchar(10))+':'+Title from Project where ID in(SELECT ProjectID FROM [dbo].[ProjectFlowNode] where NodeID=" + id.ToString() + " or charindex('," + id.ToString() + ",',','+ProcessTo+',')>0 GROUP BY(ProjectID));";
            var project_flow_node_arr = db.Database.SqlQuery<string>(project_flow_node_sql).ToArray();
            if(project_flow_node_arr.Count()>0)
            {
                result.Append("引用该节点的项目如下：\r\n");
                result.Append(string.Join(",", project_flow_node_arr));
            }
            db.Dispose();
            if (result.Length > 0)
            {
                msg = result.ToString();
                return false;
            }
            else return true;
        }

        /// <summary>
        /// 获取流程选择时可用的节点集合
        /// </summary>
        /// <param name="flow_id"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<Model.AllNode> GetFlowSelectNode(int flow_id, bool is_factor)
        {
            List<BLL.Model.AllNode> result = new List<Model.AllNode>();
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
                BLL.Model.AllNode model_category = new Model.AllNode();
                model_category.category_id = category.ID;
                model_category.category_name = category.Title;
                List<BLL.Model.AllNodeList> node_list = new List<Model.AllNodeList>();
                //string sql = "SELECT * FROM [dbo].[Node] WHERE NodeCategoryID = " + category.ID.ToString() + " and IsFactor = " + (is_factor ? "1" : "0") + " and ID not in(SELECT NodeID FROM [dbo].[FlowNode] where FlowID = " + flow_id.ToString() + " group by NodeID) order BY AddTime DESC";
                string sql = "SELECT * FROM [dbo].[Node] WHERE NodeCategoryID = " + category.ID.ToString() + " and IsFactor = " + (is_factor ? "1" : "0") + " order BY AddTime DESC";
                var db_node_list = db.Nodes.SqlQuery(sql).AsNoTracking().ToList();
                foreach (var node in db_node_list)
                {
                    BLL.Model.AllNodeList model_node = new Model.AllNodeList();
                    model_node.node_id = node.ID;
                    model_node.node_name = node.Title;
                    node_list.Add(model_node);
                }
                model_category.node_list = node_list;
                result.Add(model_category);
            }

            return result;
        }

        /// <summary>
        /// 获取视图，包含incloud
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.Node GetMode(int id)
        {
            Entity.Node entity = null;
            using (var db = new DataCore.EFDBContext())
            {
                entity = db.Nodes.Include(p => p.NodeCategory).Include(p => p.NodeFiles).Where(p => p.ID == id).FirstOrDefault();
            }

            return entity;
        }

        /// <summary>
        /// 添加节点数据
        /// </summary>
        /// <returns></returns>
        public static bool Add(Entity.Node model)
        {
            if (model == null)
                return false;
            using (var db = new DataCore.EFDBContext())
            {
                db.Nodes.Add(model);
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 修改节点数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Modify(Entity.Node model)
        {
            if (model == null)
                return false;
            if (model.ID <= 0)
                return false;

            var db = new DataCore.EFDBContext();
            if (!db.Nodes.Any(p => p.ID == model.ID))
                return false;
            //删除之前的附件
            db.NodeFiles.Where(p => p.NodeID == model.ID).ToList().ForEach(p => db.NodeFiles.Remove(p));
            //删除之前的联系人员
            //db.NodeUsers.Where(p => p.NodeID == model.ID).ToList().ForEach(p => db.NodeUsers.Remove(p));

            //if (!string.IsNullOrWhiteSpace(ids))
            //{
            //    foreach (var item in ids.Split(','))
            //    {
            //        int id = Tools.TypeHelper.ObjectToInt(item);
            //        var entity_user = db.CusUsers.Find(id);
            //        if (entity_user != null)
            //        {
            //            var user = new Entity.NodeUser();
            //            user.CusUserID = id;
            //            user.NodeID = model.ID;
            //            db.NodeUsers.Add(user);
            //        }
            //    }
            //}
            List<Entity.NodeFile> file_list = model.NodeFiles.ToList();
            foreach (var item in file_list)
            {
                item.NodeID = model.ID;
                db.NodeFiles.Add(item);
            }
            model.NodeFiles.Clear();
            //附件到DbContext上下文
            var entity = db.Entry<Entity.Node>(model);
            //标识状态为修改
            entity.State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            db.Dispose();
            return true;

        }


        /// <summary>
        /// 获取所有的节点分类
        /// </summary>
        /// <returns></returns>
        public static List<Entity.NodeCategory> GetNodeCategory()
        {
            using (var db = new DataCore.EFDBContext())
            {
                return db.NodeCategorys.OrderBy(p => p.ID).AsNoTracking().ToList();
            }
        }

    }
}
