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

        public static Entity.Project GetModel(int id)
        {
            if (id <= 0)
                return null;

            var db = new DataCore.EFDBContext();
            var entity = db.Projects.AsNoTracking().Include(p=>p.CusUser).Include(p => p.ProjectUsers.Select(s => s.CusUser)).Include(p => p.ProjectFiles).Include(p => p.ApproveUser).Where(p => p.ID == id).FirstOrDefault();

            db.Dispose();
            return entity;
        }

        /// <summary>
        /// 项目审批
        /// </summary>
        /// <returns></returns>
        public static bool Approve(int user_id, int project_id,Entity.ApproveStatusType status,string remark, out string msg)
        {
            msg = "";
            if(status == Entity.ApproveStatusType.no)
            {
                if(string.IsNullOrWhiteSpace(remark))
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

            int app_id = Tools.TypeHelper.ObjectToInt(entity.ApproveUserID,0);
            if (app_id != 0)
            {
                if (entity.ApproveUserID == entity.CusUserID)
                {
                    msg = "不能自己审批自己的项目";
                    return 0;
                }
            } else
                entity.ApproveUserID = null;
            

            var db = new DataCore.EFDBContext();
            db.Set<Entity.Project>().Add(entity);

            var flow_entity = db.Flows.Find(entity.FlowID);

            var entity_user = db.CusUsers.Find(entity.CusUserID);
            if(entity_user == null)
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

            if(flow_entity != null)
            {
                entity.Pieces = flow_entity.Pieces;
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
                    entity_node.Piece = item.Piece;
                    entity_node.Top = item.Top;
                    db.ProjectFlowNodes.Add(entity_node);
                }
            }
            entity.SetApproveStatus();
            db.SaveChanges();
            db.Dispose();
            if (app_id != 0)
                BLL.BLLMsg.PushMsg(app_id, Entity.CusUserMessageType.approveproject, string.Format(BLL.BLLMsgTemplate.ApproveProject, entity_user.NickName, entity.Title), entity.ID);
            return entity.ID;
        }

        /// <summary>
        /// 修改项目
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user_ids">项目联系人</param>
        /// <returns></returns>
        public static int Modify(Entity.Project entity, string user_ids, out string msg)
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
            if(old_entity == null)
            {
                msg = "该项目不存在或已被删除";
                return 0;
            }
            if (old_entity.FlowID != null && entity.FlowID!= null && old_entity.FlowID != entity.FlowID)
            {
                msg = "已有流程，不可再修改";
                return 0;
            }

            Entity.Flow flow_entity = null;
            if (old_entity.FlowID == null && Tools.TypeHelper.ObjectToInt(entity.FlowID, 0) > 0)
            {
                //修改了流程
                flow_entity = db.Flows.Find(entity.FlowID);
                if(flow_entity == null)
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

            if(old_entity.FlowID ==null && Tools.TypeHelper.ObjectToInt(entity.FlowID,0) >0)
            {
                //修改了流程
                entity.Pieces = flow_entity.Pieces;
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
                    entity_node.Piece = item.Piece;
                    entity_node.Top = item.Top;
                    db.ProjectFlowNodes.Add(entity_node);
                }
            }

            if (app_id == 0)
                entity.ApproveUserID = null;
            else
                entity.ApproveUserID = app_id;
            entity.SetApproveStatus();
            entity.SetYear();
            entity.SetQuarter();
            var db_entity = db.Entry<Entity.Project>(entity);
            db_entity.State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            db.Dispose();
            if (app_id != 0)
                BLL.BLLMsg.PushMsg(app_id, Entity.CusUserMessageType.approveproject, string.Format(BLL.BLLMsgTemplate.ApproveProject, entity_user.NickName, entity.Title), entity.ID);
            msg = "ok";
            return entity.ID;
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Del(int id,int user_id,out string msg)
        {
            msg = "非法参数";
            if (id <= 0)
                return false;
            var db = new DataCore.EFDBContext();
            var entity = db.Projects.Find(id);
            if(entity == null)
            {
                msg = "项目不存在";
                return false;
            }
            if(entity.CusUserID != user_id)
            {
                msg = "只能删除自己添加的项目";
                return false;
            }
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
        /// <param name="node_id">查询某个节点</param>
        /// <param name="node_status">某个节点状态,0所有；1：未开始；2：已开始；3：已结束</param>
        /// <param name="node_begin">节点</param>
        /// <param name="is_admin">是否管理员</param>
        /// <returns></returns>
        public static List<Entity.Project> GetPageData(int page_index, int page_size, ref int rowCount, int user_id, string search_title, bool only_mine,int status,int node_id,int node_status,DateTime? node_begin,DateTime? node_end,bool is_admin)
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
            string title_where = "";
            string user_where = "" ;
            string jindu_where = " where ID > 0"; //进度
            string node_where = ""; //节点状态搜索
            //只获取我的项目
            if(only_mine)
                user_where = " where CusUserID = " + user_id;
            else
            {
                if(!is_admin)
                    user_where = " where See = 0 or CHARINDEX(isnull((case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end),''),isnull((case CusUserID when " + user_id + " then(case See when 2 then '" + user_id_str + "' when 1 then '" + user_department_str + "' end) end) + isnull(TOID,''),''))> 0";
            }
            if(!string.IsNullOrWhiteSpace(search_title))
            {
                title_where = " where (CHARINDEX('" + search_title + "',I.Title)>0 or CHARINDEX('" + search_title + "',U.Telphone)>0 or CHARINDEX('" + search_title + "',U.NickName)>0)";
            }
            switch (status)
            {
                case 1:
                    jindu_where += " and Status = 1";
                    break;
                case 2:
                    jindu_where += " and Status = 0";
                    break;
                default:
                    break;
            }
            if(node_id > 0)
            {
                node_where = "and NodeID = "+node_id.ToString();
                switch (node_status)
                {
                    //未开始
                    case 1:
                        node_where += " and IsStart = 0 and IsEnd = 0";
                        break;
                    //已开始
                    case 2:
                        node_where += " and IsStart = 1";
                        //筛选开始时间
                        if (node_begin != null)
                        {
                            string dt_begin = Tools.TypeHelper.ObjectToDateTime(node_begin).ToString("yyyy-MM-dd");
                            node_where += " and BeginTime between '" + dt_begin + " 00:00:00' and '" + dt_begin + " 23:59:59'";
                        }
                        if (node_begin != null && node_end != null)
                        {
                            string dt_begin = Tools.TypeHelper.ObjectToDateTime(node_begin).ToString("yyyy-MM-dd");
                            string dt_end = Tools.TypeHelper.ObjectToDateTime(node_begin).ToString("yyyy-MM-dd");
                            node_where += " and BeginTime between '" + dt_begin + " 00:00:00' and '" + dt_end + " 23:59:59'";
                        }
                        break;
                    //已结束
                    case 3:
                        node_where += " and IsEnd = 1";
                        //筛选结束时间
                        if (node_begin != null)
                        {
                            string dt_begin = Tools.TypeHelper.ObjectToDateTime(node_begin).ToString("yyyy-MM-dd");
                            node_where += " and EndTime between '" + dt_begin + " 00:00:00' and '" + dt_begin + " 23:59:59'";
                        }
                        if (node_begin != null && node_end != null)
                        {
                            string dt_begin = Tools.TypeHelper.ObjectToDateTime(node_begin).ToString("yyyy-MM-dd");
                            string dt_end = Tools.TypeHelper.ObjectToDateTime(node_begin).ToString("yyyy-MM-dd");
                            node_where += " and EndTime between '" + dt_begin + " 00:00:00' and '" + dt_end + " 23:59:59'";
                        }
                        break;                    
                    default:
                        break;
                }

                jindu_where += " and NodeTotal>0 ";

            }




            sql = "select * from (SELECT ROW_NUMBER() OVER(ORDER BY LastUpdateTime DESC) as row,* from(select S.* FROM (select (select count(1) as total from ProjectFlowNode where ProjectID = P.ID and Status =1 " + node_where + ") as NodeTotal, ISNULL((select top 1 IsEnd from ProjectFlowNode where ProjectID = P.ID order by[Index] DESC), 0) as Status,*from( select I.*, U.Telphone, U.NickName from Project as I left JOIN CusUser as U on I.CusUserID = U.ID " + title_where + " ) as P " + user_where + ") AS S " + jindu_where+ ")as T) as Z  where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
            sql_total = "select count(1) FROM (select (select count(1) as total from ProjectFlowNode where ProjectID = P.ID and Status =1 " + node_where + ") as NodeTotal, ISNULL((select top 1 IsEnd from ProjectFlowNode where ProjectID = P.ID order by[Index] DESC), 0) as Status,*from( select I.*, U.Telphone, U.NickName from Project as I left JOIN CusUser as U on I.CusUserID = U.ID " + title_where + " ) as P " + user_where + ") AS S " + jindu_where;

            rowCount = db.Database.SqlQuery<int>(sql_total).ToList()[0];
            response_entity = db.Database.SqlQuery<Entity.Project>(sql).ToList();
            foreach (var item in response_entity)
            {
                var entity = db.CusUsers.AsNoTracking().Where(p => p.ID == item.CusUserID).FirstOrDefault();
                if (entity == null)
                {
                    entity = new Entity.CusUser();
                }
                item.CusUser = entity;

                item.ProjectFiles = db.ProjectFiles.AsNoTracking().Where(p => p.ProjectID == item.ID).ToList();

                //获取当前进行的节点信息
                //如果项目已完成，则直接显示已完成，否则查找最后正在进行的节点，如果查找不到，则查找初始节点，标识未未开始
                var entity_node = new Entity.ProjectFlowNodeDoing();
                if(!item.Status)
                {
                    entity_node = db.Database.SqlQuery<Entity.ProjectFlowNodeDoing>("select F.ID as flow_node_id,N.Title as node_title, F.Remark as flow_node_remark from (select top 1 * from ProjectFlowNode where ProjectID = "+item.ID.ToString()+" and Status = 1 and IsStart = 1 order by[Index] DESC) as F left JOIN Node as N on F.NodeID = N.ID").FirstOrDefault();
                    if (entity_node == null)
                    {
                        entity_node = db.Database.SqlQuery<Entity.ProjectFlowNodeDoing>("select F.ID as flow_node_id,N.Title as node_title, F.Remark as flow_node_remark from (select top 1 * from ProjectFlowNode where ProjectID = " + item.ID.ToString() + " order by[Index] ASC) as F left JOIN Node as N on F.NodeID = N.ID").FirstOrDefault();
                    }
                    
                }
                if (entity_node == null)
                {
                    entity_node = new Entity.ProjectFlowNodeDoing();
                    entity_node.flow_node_id = -1;
                    entity_node.node_title = "无节点";
                    entity_node.flow_node_remark = "无备注";
                }

                item.ProjectFlowNodeDoing = entity_node;

            }
            db.Dispose();
            return response_entity;
        }

        /// <summary>
        /// 根据条件获取项目
        /// </summary>
        /// <returns></returns>
        public static List<Model.AdminUserRoute> GetProjectTitle(int year,int jidu,int area,int gz,int node_id)
        {
            List<Model.AdminUserRoute> response_entity = new List<Model.AdminUserRoute>();
            var db = new DataCore.EFDBContext();
            var sql = "";
            string strSelect = "";
            string strWhere = " where ID>0 ";
            if(year>0)
            {
                strWhere += " and TJYear = " + year.ToString() + " ";
            }
            if(jidu >0)
            {
                strWhere += " and TJQuarter = "+jidu.ToString() +" ";
            }
            if(gz>0)
            {
                strWhere += " and GaiZaoXingZhi = "+gz.ToString();
            }
            if(node_id>0)
            {
                strSelect = ",(select [dbo].[fn_ProjectHaveNode](P.ID,"+node_id.ToString()+"))as NodeTotal";
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
}
