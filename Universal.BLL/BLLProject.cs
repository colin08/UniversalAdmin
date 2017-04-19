using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections;

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
            var entity = db.Projects.AsNoTracking().Include(p => p.CusUser).Include(p => p.ProjectUsers.Select(s => s.CusUser)).Include(p => p.ProjectFiles).Include(p => p.ApproveUser).Where(p => p.ID == id).FirstOrDefault();

            db.Dispose();
            return entity;
        }

        /// <summary>
        /// 项目审批
        /// </summary>
        /// <returns></returns>
        public static bool Approve(int user_id, int project_id, Entity.ApproveStatusType status, string remark, out string msg)
        {
            msg = "";
            if (status == Entity.ApproveStatusType.no)
            {
                if (string.IsNullOrWhiteSpace(remark))
                {
                    msg = "当审核不通过时，必须填写不通过原因";
                    return false;
                }
            }
            var entity_user = new BLL.BaseBLL<Entity.CusUser>().GetModel(p => p.ID == user_id);
            if (entity_user == null)
            {
                msg = "审核的用户不存在";
                return false;
            }
            BLL.BaseBLL<Entity.Project> bll = new BLL.BaseBLL<Entity.Project>();
            var entity = bll.GetModel(p => p.ID == project_id);
            if (entity == null)
            {
                msg = "要审批的项目不存在";
                return false;
            }

            if (entity.ApproveUserID != user_id)
            {
                msg = "该项目不是由您来审核";
                return false;
            }

            if (entity.ApproveStatus == Entity.ApproveStatusType.yes)
            {
                msg = "已审批";
                return false;
            }


            entity.ApproveStatus = status;
            entity.ApproveRemark = remark;
            if (bll.Modify(entity, "ApproveStatus", "ApproveRemark") > 0)
            {
                msg = "审批成功";
                switch (status)
                {
                    case Entity.ApproveStatusType.nodo:
                        break;
                    case Entity.ApproveStatusType.yes:
                        BLL.BLLMsg.PushMsg(entity.CusUserID, Entity.CusUserMessageType.appproveok, string.Format(BLL.BLLMsgTemplate.AppproveOK, entity.Title), entity.ID);
                        //向收藏此项目的用户发送通知
                        BLL.BLLMsg.PushFavProjectUser(entity.ID);
                        break;
                    case Entity.ApproveStatusType.no:
                        BLL.BLLMsg.PushMsg(entity.CusUserID, Entity.CusUserMessageType.appproveno, string.Format(BLL.BLLMsgTemplate.AppproveNo, entity.Title, remark), entity.ID);
                        break;
                    default:
                        break;
                }

                return true;
            }
            else
            {
                msg = "审批失败";
                return false;
            }
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

            int app_id = Tools.TypeHelper.ObjectToInt(entity.ApproveUserID, 0);
            if (app_id != 0)
            {
                if (entity.ApproveUserID == entity.CusUserID)
                {
                    msg = "不能自己审批自己的项目";
                    return 0;
                }
            }
            else
            {
                entity.ApproveUserID = null;
                //没有审核用户，那他就直接审核通过
                entity.ApproveStatus = Entity.ApproveStatusType.yes;
                entity.ApproveRemark = "无审核用户，直接通过";
            }


            var db = new DataCore.EFDBContext();
            db.Set<Entity.Project>().Add(entity);

            var flow_entity = db.Flows.Find(entity.FlowID);

            var entity_user = db.CusUsers.Find(entity.CusUserID);
            if (entity_user == null)
            {
                msg = "该用户不存在";
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

            if (flow_entity != null)
            {
                entity.Pieces = flow_entity.Pieces;
            }
            entity.SetApproveStatus();
            db.SaveChanges();
            if (flow_entity != null)
            {
                //复制节点
                CopyFlowNodeFromCompact(db, entity.ID, false, flow_entity.ID, entity.CusUserID);
            }
            db.Dispose();
            if (app_id != 0)
                BLL.BLLMsg.PushMsg(app_id, Entity.CusUserMessageType.approveproject, string.Format(BLL.BLLMsgTemplate.ApproveProject, entity.Title), entity.ID);
            return entity.ID;
        }

        /// <summary>
        /// 修改项目
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user_ids">项目联系人</param>
        /// <returns></returns>
        public static int Modify(Entity.Project entity, string user_ids, int login_user_id, out string msg)
        {
            msg = "";
            int app_id = Tools.TypeHelper.ObjectToInt(entity.ApproveUserID, 0);
            if (app_id != 0)
            {
                if (entity.ApproveUserID == entity.CusUserID)
                {
                    msg = "不能自己审批自己的项目";
                    return 0;
                }
            }
            var db = new DataCore.EFDBContext();
            var old_entity = db.Projects.AsNoTracking().Where(p => p.ID == entity.ID).FirstOrDefault();
            if (old_entity == null)
            {
                msg = "该项目不存在或已被删除";
                return 0;
            }
            if (old_entity.FlowID != null && entity.FlowID != null && old_entity.FlowID != entity.FlowID)
            {
                msg = "已有流程，不可再修改";
                return 0;
            }

            Entity.Flow flow_entity = null;
            if (old_entity.FlowID == null && Tools.TypeHelper.ObjectToInt(entity.FlowID, 0) > 0)
            {
                //修改了流程
                flow_entity = db.Flows.Find(entity.FlowID);
                if (flow_entity == null)
                {
                    msg = "所选流程不存在";
                    return 0;
                }
            }

            var entity_user = db.CusUsers.Find(entity.CusUserID);
            if (entity_user == null)
            {
                msg = "该用户不存在";
                return 0;
            }
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
            bool is_copy = false;
            if (old_entity.FlowID == null && Tools.TypeHelper.ObjectToInt(entity.FlowID, 0) > 0)
            {
                //修改了流程
                entity.Pieces = flow_entity.Pieces;
                is_copy = true;
            }

            if (app_id == 0)
            {
                entity.ApproveUserID = null;
                //没有审核用户，那他就直接审核通过
                entity.ApproveStatus = Entity.ApproveStatusType.yes;
                entity.ApproveRemark = "无审核用户，直接通过";
            }
            else
                entity.ApproveUserID = app_id;
            entity.SetApproveStatus();
            entity.SetYear();
            entity.SetQuarter();
            var db_entity = db.Entry<Entity.Project>(entity);
            db_entity.State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            //复制节点
            if (is_copy)
                CopyFlowNodeFromCompact(db, entity.ID, true, flow_entity.ID, login_user_id);
            db.Dispose();
            if (app_id != 0)
                BLL.BLLMsg.PushMsg(app_id, Entity.CusUserMessageType.approveproject, string.Format(BLL.BLLMsgTemplate.ApproveProject, entity.Title), entity.ID);
            //项目更新提醒
            BLLMsg.PushSomeUser(user_ids, Entity.CusUserMessageType.projectupdate, string.Format(BLLMsgTemplate.ProjectUpdate, entity.Title), entity.ID);
            return entity.ID;
        }

        /// <summary>
        /// 从演示流程里拷贝流程节点到项目里
        /// </summary>
        /// <param name="db"></param>
        /// <param name="project_id"></param>
        /// <param name="is_clear"></param>
        /// <param name="flow_id"></param>
        public static void CopyFlowNodeFromCompact(DataCore.EFDBContext db, int project_id, bool is_clear, int flow_id, int login_user_id)
        {
            if (db == null)
                db = new DataCore.EFDBContext();
            if (is_clear)
                db.ProjectFlowNodes.Where(p => p.ProjectID == project_id).ToList().ForEach(p => db.ProjectFlowNodes.Remove(p));
            if (!db.FlowNodes.Any(p => p.FlowID == flow_id))
                return;

            var db_flow_node_compact_list = db.FlowNodes.Where(p => p.FlowID == flow_id).AsNoTracking().ToList();
            //旧的父子关系对应
            List<FlowP> old_list = new List<FlowP>();
            //新旧id对应
            Hashtable new_dy = new Hashtable();
            foreach (var flow_node in db_flow_node_compact_list)
            {
                FlowP p = new FlowP();
                p.pid = flow_node.ID;
                List<FlowC> list_c = new List<FlowC>();
                foreach (var item in flow_node.ProcessTo.Split(','))
                {
                    int old_id = Tools.TypeHelper.ObjectToInt(item, -1);
                    if (old_id != -1)
                    {
                        FlowC c = new FlowC();
                        c.id = old_id;
                        list_c.Add(c);
                    }
                }
                p.cids = list_c;
                old_list.Add(p);

                //查询是否顶级
                var sql = "select count(1) from FlowNode where charindex('," + flow_node.ID.ToString() + ",',','+ProcessTo+',')> 0";
                bool is_frist = db.Database.SqlQuery<int>(sql).ToList()[0] == 0 ? true : false;

                Entity.ProjectFlowNode entity_node = new Entity.ProjectFlowNode();
                entity_node.Color = flow_node.Color;
                entity_node.ICON = flow_node.ICON;
                entity_node.Left = flow_node.Left;
                entity_node.NodeID = flow_node.NodeID;
                entity_node.ProcessTo = "";
                entity_node.ProjectID = project_id;
                entity_node.Status = true;
                entity_node.EditUserId = login_user_id;
                entity_node.LastUpdateTime = DateTime.Now;
                entity_node.Top = flow_node.Top;
                entity_node.IsFrist = is_frist;
                db.ProjectFlowNodes.Add(entity_node);
                db.SaveChanges();
                //旧的的id对应新的id
                new_dy.Add(flow_node.ID, entity_node.ID);
            }

            //修改箭头指向方向
            foreach (var item in old_list)
            {
                int project_flow_id = Tools.TypeHelper.ObjectToInt(new_dy[item.pid]);
                StringBuilder str_child = new StringBuilder();
                foreach (var citem in item.cids)
                    str_child.Append(new_dy[citem.id].ToString() + ",");
                if (str_child.Length > 0)
                    str_child.Remove(str_child.Length - 1, 1);
                string sql = "update ProjectFlowNode set ProcessTo='" + str_child.ToString() + "' where id = " + project_flow_id.ToString();
                db.Database.ExecuteSqlCommand(sql);
            }
        }

        /// <summary>
        /// 复制流程节点到项目里
        /// </summary>
        private static void ExecFlowNodeProcessTo(DataCore.EFDBContext db, int project_id, bool is_clear, int flow_id)
        {
            if (db == null)
                db = new DataCore.EFDBContext();
            if (is_clear)
                db.ProjectFlowNodes.Where(p => p.ProjectID == project_id).ToList().ForEach(p => db.ProjectFlowNodes.Remove(p));

            var db_flow_node_list = db.FlowNodes.Where(p => p.FlowID == flow_id).ToList();
            List<int> frist_node = db.Database.SqlQuery<int>("SELECT ID FROM [dbo].[FlowNode] where Pids = '' or Pids is NULL;").ToList();
            if (frist_node.Count == 0)
                return;

            //旧的父子关系对应
            List<FlowP> old_list = new List<FlowP>();
            //新旧关系id对应
            Hashtable new_dy = new Hashtable();
            foreach (var flow_node in db_flow_node_list)
            {
                FlowP p = new FlowP();
                p.pid = flow_node.ID;
                List<FlowC> list_c = new List<FlowC>();
                //获取子id
                var child_list = db.FlowNodes.SqlQuery("SELECT * FROM [dbo].[FlowNode] where CHARINDEX('," + flow_node.ID.ToString() + ",',PIds) > 0 ").AsNoTracking().ToList();
                foreach (var child_node in child_list)
                {
                    FlowC temp_c = new FlowC();
                    temp_c.id = child_node.ID;
                    list_c.Add(temp_c);
                }
                p.cids = list_c;
                old_list.Add(p);

                Entity.ProjectFlowNode entity_node = new Entity.ProjectFlowNode();
                entity_node.Color = flow_node.Color;
                entity_node.ICON = flow_node.ICON;
                entity_node.Left = 100;
                entity_node.NodeID = flow_node.NodeID;
                entity_node.ProcessTo = "";
                entity_node.ProjectID = project_id;
                entity_node.Status = true;
                entity_node.Piece = flow_node.Piece;
                entity_node.Top = 100;
                entity_node.IsFrist = frist_node.Contains(flow_node.ID);
                db.ProjectFlowNodes.Add(entity_node);
                db.SaveChanges();
                //旧的的id对应新的id
                new_dy.Add(flow_node.ID, entity_node.ID);
            }

            //修改箭头指向方向
            foreach (var item in old_list)
            {
                int project_flow_id = Tools.TypeHelper.ObjectToInt(new_dy[item.pid]);
                StringBuilder str_child = new StringBuilder();
                foreach (var citem in item.cids)
                    str_child.Append(new_dy[citem.id].ToString() + ",");
                if (str_child.Length > 0)
                    str_child.Remove(str_child.Length - 1, 1);
                string sql = "update ProjectFlowNode set ProcessTo='" + str_child.ToString() + "' where id = " + project_flow_id.ToString();
                db.Database.ExecuteSqlCommand(sql);
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Del(int id, int user_id, out string msg)
        {
            msg = "非法参数";
            if (id <= 0)
                return false;
            var db = new DataCore.EFDBContext();
            var entity = db.Projects.Find(id);
            if (entity == null)
            {
                msg = "项目不存在";
                return false;
            }
            if (entity.CusUserID != user_id)
            {
                msg = "只能删除自己添加的项目";
                return false;
            }
            //删除附件
            db.ProjectFiles.Where(p => p.ProjectID == id).ToList().ForEach(p => db.ProjectFiles.Remove(p));

            //删除流程节点
            db.ProjectFlowNodes.Where(p => p.ProjectID == id).ToList().ForEach(p => db.ProjectFlowNodes.Remove(p));

            //删除拆迁
            db.ProjectStages.Where(p => p.ProjectID == id).ToList().ForEach(p => db.ProjectStages.Remove(p));

            //删除项目联系人
            db.ProjectUsers.Where(p => p.ProjectID == id).ToList().ForEach(p => db.ProjectUsers.Remove(p));

            //删除项目
            db.Projects.Remove(entity);
            db.SaveChanges();
            db.Dispose();
            msg = "删除成功";
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
        /// <param name="status">项目进度，0：所有；1：完成；2：进行中</param>
        /// <param name="node_category_id">查询某个节点分类</param>
        /// <param name="is_admin">是否管理员</param>
        /// <returns></returns>
        public static List<Entity.Project> GetPageData(int page_index, int page_size, ref int rowCount, int user_id, string search_title, bool only_mine, int status, int node_category_id, DateTime? begin_time, DateTime? end_time, bool is_admin)
        {
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
            //string title_where = "";
            //string jindu_where = " where ID > 0"; //进度
            //string node_where = ""; //节点状态搜索
            //string only_mine_str = "";//只获取我的

            string inner_select = ""; //查询字段
            string auth_where = " where I.ID>0";//权限条件
            string inner_where = " where ID > 0";//字段判断
            string time_where = ""; //筛选时间

            if (only_mine)
            {
                inner_select += ",(select count(1) from ProjectUser where ProjectID = P.ID and CusUserID = " + user_id.ToString() + ") as IsMine";
                inner_where += " and IsMine >0";
            }
            else
            {
                //权限
                if (!is_admin)
                {
                    auth_where += " and (ApproveStatus =1 or CusUserID = "+user_id.ToString()+") and (See = 0 or CHARINDEX(isnull((case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end),''),isnull((case CusUserID when " + user_id + " then(case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end) end) + isnull(TOID,''),''))> 0)";

                }
            }

            //筛选时间
            if (begin_time != null)
            {
                string dt_begin = Tools.TypeHelper.ObjectToDateTime(begin_time).ToString("yyyy-MM-dd");
                time_where = " and AddTime between '" + dt_begin + " 00:00:00' and '" + dt_begin + " 23:59:59'";
            }
            if (begin_time != null && end_time != null)
            {
                string dt_begin = Tools.TypeHelper.ObjectToDateTime(begin_time).ToString("yyyy-MM-dd");
                string dt_end = Tools.TypeHelper.ObjectToDateTime(begin_time).ToString("yyyy-MM-dd");
                time_where = " and AddTime between '" + dt_begin + " 00:00:00' and '" + dt_end + " 23:59:59'";
            }
            inner_where += time_where;

            //项目筛选/用户
            if (!string.IsNullOrWhiteSpace(search_title))
            {
                auth_where += " and (CHARINDEX('" + search_title + "',I.Title)>0 or CHARINDEX('" + search_title + "',U.Telphone)>0 or CHARINDEX('" + search_title + "',U.NickName)>0)";
            }

            //状态筛选
            if (status != 0)
                inner_select += ",(select count(1) as total from ProjectFlowNode where ProjectID = P.ID and IsEnd =0) as NodeNoCompTotal";
            switch (status)
            {
                case 1:
                    inner_where += " and NodeNoCompTotal =0";
                    break;
                case 2:
                    inner_where += " and NodeNoCompTotal != 0";
                    break;
            }

            //节点分类筛选
            if (node_category_id > 0)
            {
                inner_select += ",(select dbo.fn_ProjectHaveNode(P.Id," + node_category_id.ToString() + ")) as HaveNodeCategory";
                inner_where += " and HaveNodeCategory > 0";
            }

            sql = "select * from ( select ROW_NUMBER() OVER(ORDER BY LastUpdateTime DESC) as row,* from (select * " + inner_select + " from( select I.*, U.Telphone, U.NickName from Project as I left JOIN CusUser as U on I.CusUserID = U.ID " + auth_where + ") as P ) as S " + inner_where + ") as T where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
            sql_total = "select count(1) from (select *" + inner_select + " from( select I.*, U.Telphone, U.NickName from Project as I left JOIN CusUser as U on I.CusUserID = U.ID " + auth_where + ") as P) as S " + inner_where + ";";

            rowCount = db.Database.SqlQuery<int>(sql_total).ToList()[0];
            response_entity = db.Database.SqlQuery<Entity.Project>(sql).ToList();
            foreach (var item in response_entity)
            {
                //var entity = db.CusUsers.AsNoTracking().Where(p => p.ID == item.CusUserID).FirstOrDefault();
                //if (entity == null)
                //{
                //    entity = new Entity.CusUser();
                //}
                //item.CusUser = entity;

                item.ProjectFiles = db.ProjectFiles.AsNoTracking().Where(p => p.ProjectID == item.ID).ToList();

                //获取当前进行的节点信息
                //如果项目已完成，则直接显示已完成，否则查找最后正在进行的节点，如果查找不到，则查找初始节点，标识未未开始
                var entity_node = new Entity.ProjectFlowNodeDoing();
                var entity_action_user = new Entity.CusUser();
                var flow_list =BLL.BLLProjectFlowNode.GetProjectFlow(item.ID);
                if(flow_list.Count == 0)
                {
                    entity_node.flow_node_id = -1;
                    entity_node.node_title = "全部节点已完成";
                    entity_node.flow_node_remark = "";

                    entity_action_user.NickName = "";
                }
                else
                {
                    var temp_entity = flow_list[flow_list.Count -1];
                    entity_node.edit_id = temp_entity.user_id;
                    entity_node.flow_node_id = temp_entity.project_flow_node_id;
                    entity_node.flow_node_remark = temp_entity.remark;
                    entity_node.node_title = temp_entity.node_title;
                    //设置用户
                    entity_action_user.ID = temp_entity.user_id;
                    entity_action_user.NickName = temp_entity.user_name;
                }
                //if (db.Database.SqlQuery<int>("select count(1) from ProjectFlowNode where ProjectID = " + item.ID + " and IsEnd = 0").ToList()[0] > 0)
                //{
                //    entity_node = db.Database.SqlQuery<Entity.ProjectFlowNodeDoing>("select F.ID as flow_node_id,N.Title as node_title, F.Remark as flow_node_remark,F.EditUserID as edit_id from (select top 1 * from ProjectFlowNode where ProjectID = " + item.ID.ToString() + " and Status=1 and IsEnd = 1 order by[EndTime] DESC) as F left JOIN Node as N on F.NodeID = N.ID").FirstOrDefault();
                //    if (entity_node == null)
                //    {
                //        entity_node = db.Database.SqlQuery<Entity.ProjectFlowNodeDoing>("select F.ID as flow_node_id,N.Title as node_title, F.Remark as flow_node_remark,F.EditUserID as edit_id from (select top 1 * from ProjectFlowNode where ProjectID = " + item.ID.ToString() + " and IsFrist =1 order by[EndTime] ASC) as F left JOIN Node as N on F.NodeID = N.ID").FirstOrDefault();
                //    }
                //}
                //else
                //{
                //    entity_node.flow_node_id = -1;
                //    entity_node.node_title = "全部节点已完成";
                //    entity_node.flow_node_remark = "";
                //}
                //if (entity_node == null)
                //{
                //    entity_node = new Entity.ProjectFlowNodeDoing();
                //    entity_node.flow_node_id = -1;
                //    entity_node.node_title = "无节点";
                //    entity_node.flow_node_remark = "无备注";
                //}
                //if (entity_node.edit_id != 0)
                //{
                //    entity_action_user = new Entity.CusUser();// db.CusUsers.Where(p => p.ID == entity_node.edit_id).AsNoTracking().FirstOrDefault();
                    
                //}
                //if (entity_action_user.ID == 0)
                //{
                //    entity_action_user.NickName = "";
                //}
                item.CusUser = entity_action_user;
                item.ProjectFlowNodeDoing = entity_node;

            }
            db.Dispose();
            return response_entity;
        }

        /// <summary>
        /// 根据条件获取项目
        /// </summary>
        /// <returns></returns>
        public static List<Model.AdminUserRoute> GetProjectTitle(int year, int jidu, int area, int gz, int node_id)
        {
            List<Model.AdminUserRoute> response_entity = new List<Model.AdminUserRoute>();
            var db = new DataCore.EFDBContext();
            var sql = "";
            string strSelect = "";
            string strWhere = " where ID>0 ";
            if (year > 0)
            {
                strWhere += " and TJYear = " + year.ToString() + " ";
            }
            if (jidu > 0)
            {
                strWhere += " and TJQuarter = " + jidu.ToString() + " ";
            }
            if (gz > 0)
            {
                strWhere += " and GaiZaoXingZhi = " + gz.ToString();
            }
            if (node_id > 0)
            {
                strSelect = ",(select [dbo].[fn_ProjectHaveNode](P.ID," + node_id.ToString() + "))as NodeTotal";
                strWhere += " and  NodeTotal>0";
            }
            sql = "select * from (SELECT *" + strSelect + " FROM Project as P) as S " + strWhere;
            var db_list = db.Database.SqlQuery<Entity.Project>(sql).ToList();
            foreach (var item in db_list)
            {
                Model.AdminUserRoute model = new Model.AdminUserRoute();
                model.id = item.ID;
                model.title = item.Title;
                response_entity.Add(model);
            }
            db.Dispose();
            return response_entity;
        }

        /// <summary>
        /// 分组获取已存在项目的年份
        /// </summary>
        /// <returns></returns>
        public static List<Model.Statctics> GetYearGroup()
        {
            var db = new DataCore.EFDBContext();
            string strSql = "select cast(TJYear as varchar(10)) as x_data,'1' as y_data from Project Group BY TJYear ORDER BY TJYear DESC";
            var db_data = db.Database.SqlQuery<Model.Statctics>(strSql).ToList();
            if (db_data == null)
                db_data = new List<Model.Statctics>();
            db.Dispose();
            return db_data;
        }

    }

    public class FlowP
    {
        /// <summary>
        /// 父级ID
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// 对应的子ID
        /// </summary>
        public List<FlowC> cids { get; set; }
    }

    public class FlowC
    {
        public int id { get; set; }
    }

}
