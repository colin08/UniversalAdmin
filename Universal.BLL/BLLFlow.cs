﻿using System;
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
            entity.IsDefault = true;
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// 添加流程节点
        /// </summary>
        /// <param name="flow_id">所属流程ID</param>
        /// <param name="node_id">节点ID</param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <param name="icon"></param>
        /// <param name="color"></param>
        /// <param name="process_to"></param>
        /// <returns></returns>
        public static int[] AddFlowNode(int user_id, int flow_id, int node_id, int top, int left, string icon, string color, string process_to, out string msg)
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
            var entity_flow = new Entity.Flow();
            if (flow_id != -1)
            {
                entity_flow = db.Flows.Find(flow_id);
                if (entity_flow == null)
                {
                    msg = "流程不存在";
                    return result;
                }
            }
            else
            {
                entity_flow.CusUserID = user_id;
                entity_flow.Title = entity_node.Title;
                db.Flows.Add(entity_flow);
            }
            var entity_flow_node = new Entity.FlowNode();
            entity_flow_node.Color = color;
            entity_flow_node.Flow = entity_flow;
            entity_flow_node.ICON = icon;
            entity_flow_node.Left = left;
            entity_flow_node.NodeID = entity_node.ID;
            entity_flow_node.ProcessTo = process_to;
            entity_flow_node.Top = top;
            db.FlowNodes.Add(entity_flow_node);
            db.SaveChanges();
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
            var db_list = bll.GetListBy(0, p => p.FlowID == flow_id, "ID ASC", p => p.Node);
            foreach (var item in db_list)
            {
                Model.WebFlowNode model = new Model.WebFlowNode();
                model.flow_node_id = item.NodeID;
                model.flow_node_title = item.Node.Title;
                model.icon = item.ICON;
                model.process_to = item.ProcessTo;
                model.style = "width:120px;height:30px;line-height:30px;color:" + item.ICON + ";left:" + item.Left + "px;top:" + item.Top + "px;";
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
                response_entity.flow_node_id = entity.NodeID;
                response_entity.flow_node_title = entity.Node.Title;
                response_entity.icon = entity.ICON;
                response_entity.process_to = entity.ProcessTo;
                response_entity.style = "width:120px;height:30px;line-height:30px;color:" + entity.ICON + ";left:" + entity.Left + "px;top:" + entity.Top + "px;";
            }
            return response_entity;
        }

        /// <summary>
        /// 前端：删除某个流程节点信息
        /// </summary>
        /// <param name="flow_node_id"></param>
        /// <returns></returns>
        public static bool DelWebFlowNode(int flow_node_id)
        {
            BLL.BaseBLL<Entity.FlowNode> bll = new BaseBLL<Entity.FlowNode>();
            bll.DelBy(p => p.ID == flow_node_id);
            return true;
        }

        /// <summary>
        /// 前端：保存流程信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool WebSaveFlowData(Model.WebSaveFlow model, out string msg)
        {
            msg = "";
            if (model == null)
            {
                msg = "非法参数";
                return false;
            }
            if (model.flow_node_list == null)
            {
                msg = "节点信息不能为空";
                return false;
            }

            var db = new DataCore.EFDBContext();
            var entity_flow = db.Flows.Find(model.flow_id);
            if (entity_flow == null)
            {
                msg = "流程不存在";
                return false;
            }

            foreach (var item in model.flow_node_list)
            {
                var entity_flow_node = db.FlowNodes.Find(item.flow_node_id);
                if (entity_flow_node == null)
                {
                    msg = "节点" + item.flow_node_id.ToString() + "不存在";
                    return false;
                }
                entity_flow_node.Top = item.top;
                entity_flow_node.ProcessTo = item.process_to;
                entity_flow_node.Left = item.left;
                entity_flow_node.ICON = item.icon;
                entity_flow_node.Color = item.color;
            }
            db.SaveChanges();
            db.Dispose();
            return true;
        }

        /// <summary>
        /// 前端：修改某个流程节点信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool WebSaveFlowNodeData(Model.WebSaveFlowNode model, out string msg)
        {
            msg = "";
            if (model == null)
            {
                msg = "非法参数";
                return false;
            }

            var db = new DataCore.EFDBContext();
            var entity_flow_node = db.FlowNodes.Find(model.flow_node_id);
            if (entity_flow_node == null)
            {
                msg = "节点" + model.flow_node_id.ToString() + "不存在";
                return false;
            }
            entity_flow_node.Top = model.top;
            entity_flow_node.ProcessTo = model.process_to;
            entity_flow_node.Left = model.left;
            entity_flow_node.ICON = model.icon;
            entity_flow_node.Color = model.color;
            db.SaveChanges();
            db.Dispose();
            return true;
        }

    }
}
