using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.BLL
{
    /// <summary>
    /// 流程
    /// </summary>
    public class BLLFlow
    {

        /// <summary>
        /// 根据部门ID获取部门子或父数据
        /// </summary>
        /// <param name="up">查找父级，否则为查找子级</param>
        /// <param name="id">当前分类ID</param>
        /// <returns></returns>
        public static List<Entity.Flow> GetList(bool up, int id)
        {
            List<Entity.Flow> list = new List<Entity.Flow>();
            var db = new DataCore.EFDBContext();
            SqlParameter[] param = { new SqlParameter("@Id", id) };
            string proc_name = "dbo.sp_GetParentFlows @Id";
            if (!up)
                proc_name = "dbo.sp_GetChildFlows @Id";
            list = db.Flows.SqlQuery(proc_name, param).ToList();
            db.Dispose();
            return list;
        }

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
            List<Entity.Flow> child_list = GetList(false, id);
            foreach (var item in child_list)
            {
                db.Set<Entity.Flow>().Attach(item);
                db.Set<Entity.Flow>().Remove(item);
            }
            var entity = db.Flows.Find(id);
            db.Flows.Remove(entity);
            db.SaveChanges();
            db.Dispose();
            return true;
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
            if (entity.TopPID != null)
            {
                msg = "非顶级节点";
                db.Dispose();
                return false;
            }
            if (entity.IsDefault)
            {
                msg = "已经是默认了";
                db.Dispose();
                return false;
            }
            var db_list = db.Flows.Where(p => p.TopPID == null && p.IsDefault == true).ToList();
            foreach (var item in db_list)
            {
                item.IsDefault = false;
            }
            entity.IsDefault = true;
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// 修改流程指向箭头
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool SetFlowToID(int flow_id,string toid, out string msg)
        {
            msg = "ok";
            if (flow_id <= 0 || string.IsNullOrWhiteSpace(toid))
            {
                msg = "非法参数";
                return false;
            }
            var db = new DataCore.EFDBContext();
            var entity = db.Flows.Find(flow_id);
            if(entity == null)
            {
                msg = "流程不存在";
                return false;
            }
            entity.TOID = toid;
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// 添加流程
        /// </summary>
        /// <param name="pid">父级流程ID</param>
        /// <param name="node_id">节点ID</param>
        /// <returns></returns>
        public static int AddFlow(int pid, int node_id, out string msg)
        {
            msg = "成功";
            if (pid == 0 || pid < -1 || node_id == 0)
            {
                msg = "非法参数";
                return 0;
            }

            var db = new DataCore.EFDBContext();
            var entity_node = db.Nodes.Find(node_id);
            if (entity_node == null)
            {
                msg = "节点不存在";
                return 0;
            }

            var entity = new Entity.Flow();
            if (pid == -1)
            {
                var p_entity = db.Flows.Find(pid);
                if(p_entity == null)
                {
                    msg = "父级流程不存在";
                    return 0;
                }
                entity.PID = pid;
                entity.TopPID = p_entity.TopPID == null ? p_entity.ID : p_entity.ID;
                entity.Depth = p_entity.Depth +1;                
            }else
            {
                entity.PID = null;
                entity.TopPID = null;
                entity.Depth = 1;
                entity.FlowName = entity_node.Title;
            }
            entity.NodeID = node_id;
            db.SaveChanges();
            db.Dispose();
            return entity.ID;

        }

        /// <summary>
        /// 修改流程
        /// </summary>
        /// <param name="flow_id"></param>
        /// <param name="node_id"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool ModifyFlow(int flow_id,int node_id,out string msg)
        {
            msg = "ok";
            if (flow_id <= 0 || node_id == 0)
            {
                msg = "非法参数";
                return false;
            }
            var db = new DataCore.EFDBContext();
            var entity_node = db.Nodes.Find(node_id);
            if (entity_node == null)
            {
                msg = "节点不存在";
                return false;
            }
            var entity_flow = db.Flows.Find(flow_id);
            if (entity_flow == null)
            {
                msg = "要修改的流程不存在";
                return false;
            }
            if (entity_flow.TopPID == null)
                entity_flow.FlowName = entity_node.Title;
            entity_flow.LastUpdateTime = DateTime.Now;
            entity_flow.NodeID = node_id;
            db.SaveChanges();
            db.Dispose();
            return true;
        }

    }
}
