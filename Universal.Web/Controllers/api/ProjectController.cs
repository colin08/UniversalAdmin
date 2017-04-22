﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Universal.Tools;
using Universal.Web.Framework;
using System.Data.Entity;

namespace Universal.Web.Controllers.api
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectController : BaseAPIController
    {

        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/project/node/all")]
        public WebAjaxEntity<List<Models.Response.NodeInfo>> GetAllNode()
        {
            WebAjaxEntity<List<Models.Response.NodeInfo>> response_entity = new WebAjaxEntity<List<Models.Response.NodeInfo>>();
            List<Models.Response.NodeInfo> response_list = new List<Models.Response.NodeInfo>();
            var db_list = new BLL.BaseBLL<Entity.Node>().GetListBy(0, new List<BLL.FilterSearch>(), "AddTime ASC");
            foreach (var item in db_list)
            {
                Models.Response.NodeInfo model = new Models.Response.NodeInfo();
                model.node_id = item.ID;
                model.title = item.Title;
                response_list.Add(model);
            }
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = response_list;
            return response_entity;
        }

        /// <summary>
        /// 流程指引
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/flow/menu/list")]
        public WebAjaxEntity<List<Models.Response.NodeInfo>> GetFlowMenu()
        {
            WebAjaxEntity<List<Models.Response.NodeInfo>> response_entity = new WebAjaxEntity<List<Models.Response.NodeInfo>>();
            List<Models.Response.NodeInfo> response_list = new List<Models.Response.NodeInfo>();
            var db_list = BLL.BLLFlow.GetAllFlow();
            foreach (var item in db_list)
            {
                Models.Response.NodeInfo model = new Models.Response.NodeInfo();
                model.node_id = item.ID;
                model.title = item.Title;
                response_list.Add(model);
            }
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = response_list;
            return response_entity;
        }

        /// <summary>
        /// 获取所有节点分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/node/category/all")]
        public WebAjaxEntity<List<Models.Response.NodeInfo>> GetAllNodeCategory()
        {
            WebAjaxEntity<List<Models.Response.NodeInfo>> response_entity = new WebAjaxEntity<List<Models.Response.NodeInfo>>();
            List<Models.Response.NodeInfo> response_list = new List<Models.Response.NodeInfo>();
            var db_list = new BLL.BaseBLL<Entity.NodeCategory>().GetListBy(0, new List<BLL.FilterSearch>(), "AddTime ASC");
            foreach (var item in db_list)
            {
                Models.Response.NodeInfo model = new Models.Response.NodeInfo();
                model.node_id = item.ID;
                model.title = item.Title;
                response_list.Add(model);
            }
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = response_list;
            return response_entity;
        }

        /// <summary>
        /// 获取所有项目流程，供添加项目选择时使用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/project/flow/all")]
        public WebAjaxEntity<List<Models.Response.NodeInfo>> GetAllFlow()
        {
            WebAjaxEntity<List<Models.Response.NodeInfo>> response_entity = new WebAjaxEntity<List<Models.Response.NodeInfo>>();
            List<Models.Response.NodeInfo> response_list = new List<Models.Response.NodeInfo>();
            var db_list = new BLL.BaseBLL<Entity.Flow>().GetListBy(0, p => p.FlowType == Entity.FlowType.basic, "AddTime ASC");
            foreach (var item in db_list)
            {
                Models.Response.NodeInfo model = new Models.Response.NodeInfo();
                model.node_id = item.ID;
                model.title = item.Title;
                model.is_def = item.IsDefault;
                response_list.Add(model);
            }
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = response_list;
            return response_entity;
        }

        /// <summary>
        /// 获取用户可见的项目列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/project/list")]
        public WebAjaxEntity<List<Models.Response.ProjectListInfo>> SearchProject([FromBody]Models.Request.ProjectList req)
        {
            WebAjaxEntity<List<Models.Response.ProjectListInfo>> response_entity = new WebAjaxEntity<List<Models.Response.ProjectListInfo>>();
            List<Models.Response.ProjectListInfo> response_list = new List<Models.Response.ProjectListInfo>();
            BLL.BaseBLL<Entity.CusUserProjectFavorites> bll_fav = new BLL.BaseBLL<Entity.CusUserProjectFavorites>();
            int rowCount = 0;
            var db_list = BLL.BLLProject.GetPageData(req.page_index, req.page_size, ref rowCount, req.user_id, req.keyword, req.only_mine, req.status, req.node_category_id, req.begin_time, req.end_time, false);
            foreach (var item in db_list)
            {
                Models.Response.ProjectListInfo model = new Models.Response.ProjectListInfo();
                var fav_entity = bll_fav.GetModel(p => p.CusUserID == req.user_id && p.ProjectID == item.ID);
                if (fav_entity != null)
                {
                    model.is_fav = true;
                    model.favorites_id = fav_entity.ID;
                }
                model.last_update_time = item.LastUpdateTime;
                model.project_id = item.ID;
                model.title = item.Title;
                model.user_name = item.CusUser.NickName;
                model.create_user_id = item.CusUserID;
                if (item.ProjectFiles != null)
                {
                    foreach (var file in item.ProjectFiles.Where(p => p.Type == Entity.ProjectFileType.album).ToList())
                    {
                        Models.Response.ProjectFile model_file = new Models.Response.ProjectFile();
                        model_file.file_name = file.FileName;
                        model_file.file_path = GetSiteUrl() + file.FilePath;
                        model_file.file_size = file.FileSize;
                        model_file.type = Entity.ProjectFileType.album;
                        model.file_list.Add(model_file);
                    }
                }
                //获取当前节点信息
                model.node_title = item.ProjectFlowNodeDoing.node_title;
                model.node_content = item.ProjectFlowNodeDoing.flow_node_remark;
                response_list.Add(model);
            }

            response_entity.total = rowCount;
            response_entity.data = response_list;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/project/add")]
        public WebAjaxEntity<string> AddProject([FromBody]Models.Request.AddProject req)
        {
            if (req.flow_id <= 0)
                req.flow_id = 0;

            var entity_user = new BLL.BaseBLL<Entity.CusUser>().GetModel(p => p.ID == req.user_id);
            if (entity_user == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return WorkContext.AjaxStringEntity;
            }

            Entity.Project entity = new Entity.Project();
            entity.LastUpdateUserName = entity_user.NickName;
            entity.CusUserID = req.user_id;
            entity.ApproveUserID = req.approve_user_id;
            entity.FlowID = req.flow_id;
            entity.Area = req.area;
            entity.GaiZaoXingZhi = req.GaiZaoXingZhi;
            entity.ZhongDiHao = req.ZhongDiHao;
            entity.ShenBaoZhuTi = req.ShenBaoZhuTi;
            entity.ZongMianJi = req.ZongMianJi;
            entity.GengXinDanYuanYongDiMianJi = req.GengXinDanYuanYongDiMianJi;
            entity.ZongMianJiOther = req.ZongMianJiOther;
            entity.WuLeiQuanMianJi = req.WuLeiQuanMianJi;
            entity.LaoWuCunMianJi = req.LaoWuCunMianJi;
            entity.FeiNongMianJi = req.FeiNongMianJi;
            entity.KaiFaMianJi = req.KaiFaMianJi;
            entity.RongJiLv = req.RongJiLv;
            entity.TuDiShiYongQuan = req.TuDiShiYongQuan;
            entity.JianSheGuiHuaZheng = req.JianSheGuiHuaZheng;
            entity.ChaiQianYongDiMianJi = req.ChaiQianYongDiMianJi;
            entity.ChaiQianJianZhuMianJi = req.ChaiQianJianZhuMianJi;
            entity.LiXiangTime = req.LiXiangTime;
            entity.ZhuanXiangTime = req.ZhuanXiangTime;
            entity.ZhuTiTime = req.ZhuTiTime;
            entity.YongDiTime = req.YongDiTime;
            entity.KaiPanTime = req.KaiPanTime;
            entity.FenChengBiLi = req.FenChengBiLi;
            entity.JunJia = req.JunJia;

            entity.SetYear();
            entity.SetQuarter();
            entity.Title = req.title;
            entity.See = req.post_see;
            if (req.file_list != null)
            {
                foreach (var item in req.file_list)
                {
                    Entity.ProjectFile entity_file = new Entity.ProjectFile();
                    entity_file.FileName = item.file_name;
                    entity_file.FilePath = item.file_path;
                    entity_file.FileSize = item.file_size;
                    entity_file.Type = item.type;
                    entity.ProjectFiles.Add(entity_file);
                }
            }
            string msg = "";
            BLL.BLLProject.Add(entity, req.user_ids, out msg);
            if (entity.ID > 0)
                WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = msg;

            return WorkContext.AjaxStringEntity;
        }


        /// <summary>
        /// 获取项目基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/project/info/basic")]
        public WebAjaxEntity<Models.Response.ProjectInfo> GetProjectInfo(int user_id, int project_id)
        {
            WebAjaxEntity<Models.Response.ProjectInfo> response_entity = new WebAjaxEntity<Models.Response.ProjectInfo>();
            var entity = BLL.BLLProject.GetModel(project_id);
            if (entity != null)
            {
                Models.Response.ProjectInfo response_model = new Models.Response.ProjectInfo();

                BLL.BaseBLL<Entity.CusUserProjectFavorites> bll_fav = new BLL.BaseBLL<Entity.CusUserProjectFavorites>();
                var fav_entity = bll_fav.GetModel(p => p.CusUserID == user_id && p.ProjectID == project_id);
                if (fav_entity != null)
                {
                    response_model.is_fav = true;
                    response_model.favorites_id = fav_entity.ID;
                }
                if (entity.FlowID != null)
                {
                    int fl_id = TypeHelper.ObjectToInt(entity.FlowID, 0);
                    var entity_flow = new BLL.BaseBLL<Entity.Flow>().GetModel(p => p.ID == fl_id);
                    response_model.flow_id = entity_flow == null ? 0 : entity_flow.ID;
                    response_model.flow_name = entity_flow == null ? "" : entity_flow.Title;
                }
                response_model.project_id = project_id;
                response_model.title = entity.Title;
                response_model.user_id = entity.CusUserID;
                response_model.user_name = entity.CusUser.NickName;
                response_model.user_telphone = entity.CusUser.Telphone;
                response_model.approve_id = TypeHelper.ObjectToInt(entity.ApproveUserID, 0);
                response_model.approve_name = entity.ApproveUser == null ? "" : entity.ApproveUser.NickName;
                response_model.approve_status = entity.ApproveStatus;
                response_model.approve_remark = entity.ApproveRemark;

                response_model.post_see = entity.See;

                System.Text.StringBuilder str_see_ids = new System.Text.StringBuilder();
                switch (entity.See)
                {
                    case Entity.DocPostSee.everyone:
                        break;
                    case Entity.DocPostSee.department:
                        foreach (var item in BLL.BLLDepartment.GetListByIds(entity.TOID))
                        {
                            str_see_ids.Append(item.ID.ToString() + ",");
                            response_model.see_model.Add(new Models.Response.SeeModel(item.ID, item.Title));
                        }
                        break;
                    case Entity.DocPostSee.user:
                        foreach (var item in BLL.BLLCusUser.GetListByIds(entity.TOID))
                        {
                            str_see_ids.Append(item.ID.ToString() + ",");
                            response_model.see_model.Add(new Models.Response.SeeModel(item.ID, item.NickName));
                        }
                        break;
                    default:
                        break;
                }
                if (str_see_ids.Length > 0)
                {
                    str_see_ids = str_see_ids.Remove(str_see_ids.Length - 1, 1);
                }

                response_model.see_ids = str_see_ids.ToString();

                StringBuilder contact_users = new StringBuilder();
                foreach (var item in entity.ProjectUsers)
                {
                    contact_users.Append(item.CusUserID + ",");
                    response_model.contact_users.Add(BuilderSelectUser(item.CusUser));
                }
                if (contact_users.Length > 0)
                    contact_users = contact_users.Remove(contact_users.Length - 1, 1);
                response_model.user_ids = contact_users.ToString();
                foreach (var item in entity.ProjectFiles)
                {
                    Models.Response.ProjectFile file = new Models.Response.ProjectFile();
                    file.file_name = item.FileName;
                    file.file_path = GetSiteUrl() + item.FilePath;
                    file.file_size = item.FileSize;
                    file.type = item.Type;
                    response_model.file_list.Add(file);
                }
                response_entity.data = response_model;
            }

            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }

        /// <summary>
        /// 获取项目的项目信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/project/info/pro")]
        public WebAjaxEntity<Models.Response.ProjectInfoPro> GetProjectInfoInfo(int user_id, int project_id)
        {
            WebAjaxEntity<Models.Response.ProjectInfoPro> response_entity = new WebAjaxEntity<Models.Response.ProjectInfoPro>();
            var entity = new BLL.BaseBLL<Entity.Project>().GetModel(p => p.ID == project_id);
            if (entity != null)
            {
                Models.Response.ProjectInfoPro response_model = new Models.Response.ProjectInfoPro();
                BLL.BaseBLL<Entity.CusUserProjectFavorites> bll_fav = new BLL.BaseBLL<Entity.CusUserProjectFavorites>();
                var fav_entity = bll_fav.GetModel(p => p.CusUserID == user_id && p.ProjectID == project_id);
                if (fav_entity != null)
                {
                    response_model.is_fav = true;
                    response_model.favorites_id = fav_entity.ID;
                }
                response_model.project_id = project_id;
                response_model.Area = entity.Area;
                response_model.ChaiQianJianZhuMianJi = entity.ChaiQianJianZhuMianJi;
                response_model.ChaiQianYongDiMianJi = entity.ChaiQianYongDiMianJi;
                response_model.FeiNongMianJi = entity.FeiNongMianJi;
                response_model.FenChengBiLi = entity.FenChengBiLi;
                response_model.GaiZaoXingZhi = entity.GaiZaoXingZhi;
                response_model.GengXinDanYuanYongDiMianJi = entity.GengXinDanYuanYongDiMianJi;
                response_model.JianSheGuiHuaZheng = entity.JianSheGuiHuaZheng;
                response_model.JunJia = entity.JunJia;
                response_model.KaiFaMianJi = entity.KaiFaMianJi;
                response_model.KaiPanTime = entity.KaiPanTime;
                response_model.LaoWuCunMianJi = entity.LaoWuCunMianJi;
                response_model.LiXiangTime = entity.LiXiangTime;
                response_model.RongJiLv = entity.RongJiLv;
                response_model.ShenBaoZhuTi = entity.ShenBaoZhuTi;
                response_model.TuDiShiYongQuan = entity.TuDiShiYongQuan;
                response_model.WuLeiQuanMianJi = entity.WuLeiQuanMianJi;
                response_model.YongDiTime = entity.YongDiTime;
                response_model.ZhongDiHao = entity.ZhongDiHao;
                response_model.ZhuanXiangTime = entity.ZhuanXiangTime;
                response_model.ZhuTiTime = entity.ZhuTiTime;
                response_model.ZongMianJi = entity.ZongMianJi;
                response_model.ZongMianJiOther = entity.ZongMianJiOther;
                response_entity.data = response_model;
            }

            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }

        /// <summary>
        /// 获取项目的审批状态
        /// </summary>
        /// <param name="project_id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/project/info/approve")]
        public WebAjaxEntity<int> ProjectApproveStatus(int project_id)
        {
            WebAjaxEntity<int> result = new WebAjaxEntity<int>();

            BLL.BaseBLL<Entity.Project> bll = new BLL.BaseBLL<Entity.Project>();
            var entity = bll.GetModel(p => p.ID == project_id);
            if (entity == null)
            {
                result.msgbox = "项目不存在";
                return result;
            }
            result.msg = 1;
            result.data = (int)entity.ApproveStatus;
            result.msgbox = entity.ApproveUserID == null ? "0" : entity.ApproveUserID.ToString();
            return result;
        }

        /// <summary>
        /// 项目审批
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/project/approve")]
        public WebAjaxEntity<string> ProjectApprove([FromBody]Models.Request.ProjectApprove req)
        {
            string msg = "";
            bool isOK = BLL.BLLProject.Approve(req.user_id, req.id, req.status, req.remark, out msg);
            WorkContext.AjaxStringEntity.msg = isOK ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 修改项目的基本信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/project/modify/basic")]
        public WebAjaxEntity<string> ModifyProjectInfoBasic([FromBody]Models.Request.EditProject req)
        {
            var entity_user = new BLL.BaseBLL<Entity.CusUser>().GetModel(p => p.ID == req.user_id);
            if (entity_user == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return WorkContext.AjaxStringEntity;
            }

            BLL.BaseBLL<Entity.Project> bll = new BLL.BaseBLL<Entity.Project>();
            var entity = bll.GetModel(p => p.ID == req.project_id);
            if (entity == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "项目不存在";
                return WorkContext.AjaxStringEntity;
            }

            #region 处理联系人ID
            System.Text.StringBuilder str_user_ids = new System.Text.StringBuilder();
            foreach (var item in BLL.BLLCusUser.GetListByIds(req.user_ids))
            {
                str_user_ids.Append(item.ID.ToString() + ",");
            }
            string final_user_ids = "";
            if (str_user_ids.Length > 0)
            {
                final_user_ids = str_user_ids.Remove(str_user_ids.Length - 1, 1).ToString();
            }
            #endregion

            #region 处理可见用户或部门的ID
            System.Text.StringBuilder str_see_ids = new System.Text.StringBuilder();
            switch (entity.See)
            {
                case Entity.DocPostSee.everyone:
                    break;
                case Entity.DocPostSee.department:
                    foreach (var item in BLL.BLLDepartment.GetListByIds(req.see_ids))
                    {
                        str_see_ids.Append(item.ID.ToString() + ",");
                    }
                    break;
                case Entity.DocPostSee.user:
                    foreach (var item in BLL.BLLCusUser.GetListByIds(req.see_ids))
                    {
                        str_see_ids.Append(item.ID.ToString() + ",");
                    }
                    break;
                default:
                    break;
            }
            string final_see_ids = "";
            if (str_see_ids.Length > 0)
            {
                final_see_ids = "," + str_see_ids.ToString();
            }
            #endregion

            entity.Title = req.title;
            entity.ApproveUserID = req.approve_user_id;
            entity.See = req.post_see;
            entity.TOID = final_see_ids;
            entity.FlowID = req.flow_id;
            entity.LastUpdateUserName = entity_user.NickName;
            List<Entity.ProjectFile> file_list_entity = new List<Entity.ProjectFile>();
            if (req.file_list != null)
            {
                foreach (var item in req.file_list)
                {
                    Entity.ProjectFile model = new Entity.ProjectFile();
                    model.FileName = item.file_name;
                    model.FilePath = item.file_path;
                    model.FileSize = item.file_size;
                    model.Type = item.type;
                    file_list_entity.Add(model);
                }
            }
            entity.ProjectFiles = file_list_entity;
            string msg = "";
            int result = BLL.BLLProject.Modify(entity, final_user_ids,req.user_id, out msg);

            WorkContext.AjaxStringEntity.msg = result > 0 ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return WorkContext.AjaxStringEntity;
        }


        /// <summary>
        /// 修改项目的项目信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/project/modify/pro")]
        public WebAjaxEntity<string> ModifyProjectInfoPro([FromBody]Models.Request.ProjectInfoPro req)
        {
            BLL.BaseBLL<Entity.Project> bll = new BLL.BaseBLL<Entity.Project>();
            var entity = bll.GetModel(p => p.ID == req.project_id);
            if (entity == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "项目不存在";
                return WorkContext.AjaxStringEntity;
            }
            entity.Area = req.Area;
            entity.GaiZaoXingZhi = req.GaiZaoXingZhi;
            entity.ZhongDiHao = req.ZhongDiHao;
            entity.ShenBaoZhuTi = req.ShenBaoZhuTi;
            entity.ZongMianJi = req.ZongMianJi;
            entity.GengXinDanYuanYongDiMianJi = req.GengXinDanYuanYongDiMianJi;
            entity.ZongMianJiOther = req.ZongMianJiOther;
            entity.WuLeiQuanMianJi = req.WuLeiQuanMianJi;
            entity.LaoWuCunMianJi = req.LaoWuCunMianJi;
            entity.FeiNongMianJi = req.FeiNongMianJi;
            entity.KaiFaMianJi = req.KaiFaMianJi;
            entity.RongJiLv = req.RongJiLv;
            entity.TuDiShiYongQuan = req.TuDiShiYongQuan;
            entity.JianSheGuiHuaZheng = req.JianSheGuiHuaZheng;
            entity.ChaiQianYongDiMianJi = req.ChaiQianYongDiMianJi;
            entity.ChaiQianJianZhuMianJi = req.ChaiQianJianZhuMianJi;
            entity.ZhuanXiangTime = req.ZhuanXiangTime;
            entity.ZhuTiTime = req.ZhuTiTime;
            entity.YongDiTime = req.YongDiTime;
            entity.KaiPanTime = req.KaiPanTime;
            entity.FenChengBiLi = req.FenChengBiLi;
            entity.JunJia = req.JunJia;
            bll.Modify(entity);

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 项目拆迁信息
        /// </summary>
        [HttpGet]
        [Route("api/v1/project/info/stage")]
        public WebAjaxEntity<List<Models.Response.ProjectInfoStage>> GetProjectStage(int user_id, int project_id)
        {
            WebAjaxEntity<List<Models.Response.ProjectInfoStage>> response_entity = new WebAjaxEntity<List<Models.Response.ProjectInfoStage>>();
            List<Models.Response.ProjectInfoStage> response_list = new List<Models.Response.ProjectInfoStage>();
            Entity.Project entity_project = new BLL.BaseBLL<Entity.Project>().GetModel(p => p.ID == project_id);
            if (entity_project == null)
            {
                response_entity.msgbox = "项目不存在";
                return response_entity;
            }

            var db_list = new BLL.BaseBLL<Entity.ProjectStage>().GetListBy(0, p => p.ProjectID == project_id, "ID ASC", p => p.FileList);
            BLL.BaseBLL<Entity.CusUserProjectFavorites> bll_fav = new BLL.BaseBLL<Entity.CusUserProjectFavorites>();
            foreach (var item in db_list)
            {
                Models.Response.ProjectInfoStage model = BuildProjectStage(item);
                model.is_fav = bll_fav.Exists(p => p.CusUserID == user_id && p.ProjectID == project_id);
                model.project_id = project_id;
                model.create_user_id = entity_project.CusUserID;
                response_list.Add(model);
            }

            response_entity.data = response_list;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }

        /// <summary>
        /// 添加/修改 拆迁分期
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/project/modify/stage/modify")]
        public WebAjaxEntity<string> ModifyProjectStage([FromBody]Models.Response.ProjectInfoStage req)
        {
            BLL.Model.ProjectStage model = BuildAddProjectStage(req);
            string msg = "";
            if (model.stage_id <= 0)
            {
                int id = BLL.BLLProjectStage.Add(req.project_id, model,req.login_user_id, out msg);
                WorkContext.AjaxStringEntity.msg = id > 0 ? 1 : 0;
                WorkContext.AjaxStringEntity.msgbox = msg;
                return WorkContext.AjaxStringEntity;
            }
            else
            {
                var isOK = BLL.BLLProjectStage.Modfify(model, req.login_user_id, out msg);
                WorkContext.AjaxStringEntity.msg = isOK ? 1 : 0;
                WorkContext.AjaxStringEntity.msgbox = msg;
                return WorkContext.AjaxStringEntity;
            }
        }

        /// <summary>
        /// 删除项目拆迁分期
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/project/modify/stage/del")]
        public WebAjaxEntity<string> DelProjectStage(int stage_id,int login_user_id)
        {
            if (stage_id <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }

            string msg = "";
            var is_ok = BLL.BLLProjectStage.Del(stage_id, login_user_id, out msg);
            WorkContext.AjaxStringEntity.msg = is_ok ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 上传项目附件，返回的数据格式跟上面项目附件的接口一致
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/project/upload")]
        public async Task<HttpResponseMessage> UpdateProjectFile()
        {
            WebAjaxEntity<List<Models.Response.ProjectFile>> response_entiy = new WebAjaxEntity<List<Models.Response.ProjectFile>>();

            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.OK);
            // 检查是否是 multipart/form-data 
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                response_entiy.msgbox = "缺少 enctype='multipart/form-data";
                response.Content = new StringContent(JsonConvert.SerializeObject(response_entiy), Encoding.GetEncoding("UTF-8"), "application/json");
                return response;
            }
            //文件保存目录路径 
            string SaveTempPath = "/uploads/file";
            String dirTempPath = Tools.IOHelper.GetMapPath(SaveTempPath);
            if (!Directory.Exists(dirTempPath))
                Directory.CreateDirectory(dirTempPath);

            // 设置上传目录 
            var provider = new MultipartFormDataStreamProvider(dirTempPath);

            // Read the form data.  这一步文件已经保存了
            await Request.Content.ReadAsMultipartAsync(provider);

            if (provider.FileData.Count > 5 || provider.FileData.Count == 0)
            {
                response_entiy.msgbox = "文件数量在1~5之间";
                response.Content = new StringContent(JsonConvert.SerializeObject(response_entiy), Encoding.GetEncoding("UTF-8"), "application/json");
                return response;
            }

            List<Models.Response.ProjectFile> file_list = new List<Models.Response.ProjectFile>();

            //取得原始文件名
            for (int i = 0; i < provider.FileData.Count; i++)
            {
                string source_file_name = "";
                string source_file_ext = "";
                List<System.Net.Http.Headers.NameValueHeaderValue> temp_list = provider.FileData[i].Headers.ContentDisposition.Parameters.ToList();
                if (temp_list.Count != 2)
                {
                    response_entiy.msgbox = "数据错误";
                    response.Content = new StringContent(JsonConvert.SerializeObject(response_entiy), Encoding.GetEncoding("UTF-8"), "application/json");
                    return response;
                }
                else
                {
                    if (temp_list[1].Name.ToLower().Equals("filename"))
                    {
                        source_file_name = temp_list[1].Value.Replace("\"", "");
                        source_file_ext = source_file_name.Substring(source_file_name.LastIndexOf('.') + 1).ToLower().Replace("\"", "");
                    }
                }
                if (string.IsNullOrWhiteSpace(source_file_name) || string.IsNullOrWhiteSpace(source_file_ext))
                {
                    response_entiy.msgbox = "未取得文件名";
                    response.Content = new StringContent(JsonConvert.SerializeObject(response_entiy), Encoding.GetEncoding("UTF-8"), "application/json");
                    return response;
                }

                FileInfo fileinfo = new FileInfo(provider.FileData[i].LocalFileName);
                string io_path = fileinfo.FullName;  //保存的完整绝对路径
                string md5 = Tools.IOHelper.GetMD5HashFromFile(io_path);
                string new_path = dirTempPath + "\\" + md5 + "." + source_file_ext;
                string server_path = (SaveTempPath + "/" + md5 + "." + source_file_ext).Replace(" ", "");
                if (System.IO.File.Exists(new_path))
                    System.IO.File.Delete(io_path); //把刚刚上传的给删掉，只用原有的文件
                else //不存在，改名为md5值保存
                    System.IO.File.Move(io_path, new_path); //给文件改名
                string size_txt = IOHelper.ConvertLongSizeToTxt(IOHelper.GetFileSize(server_path));
                Models.Response.ProjectFile model = new Models.Response.ProjectFile();
                model.file_name = source_file_name;
                model.file_path = server_path;
                model.file_size = size_txt;
                model.type = Entity.ProjectFileType.file;
                file_list.Add(model);
            }
            response_entiy.data = file_list;
            response_entiy.msg = 1;
            response_entiy.msgbox = GetSiteUrl();
            response.Content = new StringContent(JsonConvert.SerializeObject(response_entiy), Encoding.GetEncoding("UTF-8"), "application/json");
            return response;
        }


        /// <summary>
        /// 移除秘籍收藏
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/favorites/remove/project")]
        public WebAjaxEntity<string> RemoveFavorites([FromBody]Models.Request.RemoveFav req)
        {
            if (string.IsNullOrWhiteSpace(req.ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }
            BLL.BaseBLL<Entity.CusUserProjectFavorites> bll = new BLL.BaseBLL<Entity.CusUserProjectFavorites>();
            var id_list = Array.ConvertAll<string, int>(req.ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 批量收藏项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/favorites/add/project")]
        public WebAjaxEntity<string> AddFavMany([FromBody]Models.Request.AddFavorites req)
        {
            if (req.user_id <= 0 || string.IsNullOrWhiteSpace(req.doc_ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return WorkContext.AjaxStringEntity;
            }
            BLL.BaseBLL<Entity.Project> bll_doc = new BLL.BaseBLL<Entity.Project>();
            BLL.BaseBLL<Entity.CusUserProjectFavorites> bll_fav = new BLL.BaseBLL<Entity.CusUserProjectFavorites>();
            StringBuilder str_ids = new StringBuilder();
            foreach (var item in req.doc_ids.Split(','))
            {
                int id = TypeHelper.ObjectToInt(item, 0);
                if (id <= 0)
                    continue;
                if (!bll_doc.Exists(p => p.ID == id))
                    continue;
                var entity_fav = new Entity.CusUserProjectFavorites();
                entity_fav.CusUserID = req.user_id;
                entity_fav.ProjectID = id;
                bll_fav.Add(entity_fav);
                str_ids.Append(entity_fav.ID.ToString() + ",");
            }
            if (str_ids.Length > 0)
                str_ids.Remove(str_ids.Length - 1, 1);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            WorkContext.AjaxStringEntity.data = str_ids.ToString();
            return WorkContext.AjaxStringEntity;
        }

        /// <summary>
        /// 获取我的项目收藏
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/v1/favorites/project/list")]
        public WebAjaxEntity<List<Models.Response.ProjectListInfo>> GetFavProject([FromBody]Models.Request.ProjectFav req)
        {
            WebAjaxEntity<List<Models.Response.ProjectListInfo>> response_entity = new WebAjaxEntity<List<Models.Response.ProjectListInfo>>();
            List<Models.Response.ProjectListInfo> response_list = new List<Models.Response.ProjectListInfo>();
            BLL.BaseBLL<Entity.CusUserProjectFavorites> bll_fav = new BLL.BaseBLL<Entity.CusUserProjectFavorites>();
            int rowCount = 0;
            var db_list = BLL.BllCusUserFavorites.GetProjectPageData(req.page_index, req.page_size, ref rowCount, req.user_id, req.search_word);
            foreach (var item in db_list)
            {
                Models.Response.ProjectListInfo model = new Models.Response.ProjectListInfo();
                var fav_entity = bll_fav.GetModel(p => p.CusUserID == req.user_id && p.ProjectID == item.ID);
                if (fav_entity != null)
                    model.favorites_id = fav_entity.ID;
                model.last_update_time = item.LastUpdateTime;
                model.project_id = item.ID;
                model.title = item.Title;
                model.user_name = item.CusUser.NickName;

                if (item.ProjectFiles != null)
                {
                    foreach (var file in item.ProjectFiles.Where(p => p.Type == Entity.ProjectFileType.album).ToList())
                    {
                        Models.Response.ProjectFile model_file = new Models.Response.ProjectFile();
                        model_file.file_name = file.FileName;
                        model_file.file_path = GetSiteUrl() + file.FilePath;
                        model_file.file_size = file.FileSize;
                        model_file.type = Entity.ProjectFileType.album;
                        model.file_list.Add(model_file);
                    }
                }
                //获取当前节点信息
                model.node_title = item.ProjectFlowNodeDoing.node_title;
                model.node_content = item.ProjectFlowNodeDoing.flow_node_remark;
                response_list.Add(model);
            }

            response_entity.total = rowCount;
            response_entity.data = response_list;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }

        /// <summary>
        /// 面积统计
        /// </summary>
        /// <param name="project_ids">项目id，多个逗号分割</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/statctics/domain")]
        public WebAjaxEntity<Models.Response.HighCharts> GetStatctics(string project_ids)
        {
            WebAjaxEntity<Models.Response.HighCharts> response_entity = new WebAjaxEntity<Models.Response.HighCharts>();
            Models.Response.HighCharts model = new Models.Response.HighCharts();
            BLL.Model.Statctics result = BLL.BLLStatctics.Domain(project_ids);
            model.x_data = result.x_data;
            model.y_data = result.y_data;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = model;
            return response_entity;
        }

        /// <summary>
        /// 数量统计
        /// </summary>
        /// <param name="area">区域</param>
        /// <param name="gz">改造性质</param>
        /// <param name="jidu">季度</param>
        /// <param name="node_id">节点ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/statctics/count")]
        public WebAjaxEntity<Models.Response.HighCharts> GetStatctisCount(int jidu, int area, int node_id)
        {
            WebAjaxEntity<Models.Response.HighCharts> response_entity = new WebAjaxEntity<Models.Response.HighCharts>();
            Models.Response.HighCharts model = new Models.Response.HighCharts();
            BLL.Model.Statctics result = BLL.BLLStatctics.ProjectTotal(jidu, area,0,node_id);
            model.x_data = result.x_data;
            model.y_data = result.y_data;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.data = model;
            return response_entity;
        }

        /// <summary>
        /// 获取统计图筛选所需参数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/statctics/condition")]
        public WebAjaxEntity<Models.Response.StatcticsCondition> GetStatctisCondition()
        {
            WebAjaxEntity<Models.Response.StatcticsCondition> response_entity = new WebAjaxEntity<Models.Response.StatcticsCondition>();
            Models.Response.StatcticsCondition result = new Models.Response.StatcticsCondition();
            //年度
            var year_list = BLL.BLLProject.GetYearGroup();
            foreach (var item in year_list)
                result.niandu.Add(new Models.Response.SimpleEntity(TypeHelper.ObjectToInt(item.x_data), item.x_data));

            //季度
            result.jidu.Add(new Models.Response.SimpleEntity(1, "第一季度"));
            result.jidu.Add(new Models.Response.SimpleEntity(2, "第二季度"));
            result.jidu.Add(new Models.Response.SimpleEntity(3, "第三季度"));
            result.jidu.Add(new Models.Response.SimpleEntity(4, "第四季度"));

            //区域
            foreach (var item in Tools.EnumHelper.EnumToDictionary(typeof(Universal.Entity.ProjectArea)))
                result.area.Add(new Models.Response.SimpleEntity(item.Key, item.Value));
            
            response_entity.data = result;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }

        /// <summary>
        /// 面积统计--筛选获取查询的项目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/statctics/domain/searchproject")]
        public WebAjaxEntity<List<Models.Response.SimpleEntity>> GetDomainCon(int year, int jidu, int area, int node_id)
        {
            WebAjaxEntity<List<Models.Response.SimpleEntity>> response_entity = new WebAjaxEntity<List<Models.Response.SimpleEntity>>();
            List<Models.Response.SimpleEntity> response_list = new List<Models.Response.SimpleEntity>();
            var db_list = BLL.BLLProject.GetProjectTitle(year, jidu, area,0, node_id);
            foreach (var item in db_list)
                response_list.Add(new Models.Response.SimpleEntity(item.id, item.title));

            response_entity.data = response_list;
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return response_entity;
        }


        /// <summary>
        /// 构造项目拆迁分期信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Models.Response.ProjectInfoStage BuildProjectStage(Entity.ProjectStage entity)
        {
            if (entity == null)
                return null;
            Models.Response.ProjectInfoStage model = new Models.Response.ProjectInfoStage();
            model.stage_id = entity.ID;
            model.title = entity.Title;
            model.begin_time = entity.BeginTime;
            model.ChaiBuChangjinE = entity.ChaiBuChangjinE;
            model.ChaiBuChangMianJi = entity.ChaiBuChangMianJi;
            model.ChaiJianZhuMianJi = entity.ChaiJianZhuMianJi;
            model.ChaiZhanDiMianJi = entity.ChaiZhanDiMianJi;
            model.JiDiMianJi = entity.JiDiMianJi;
            model.KongDiMianJi = entity.KongDiMianJi;
            model.WeiQYHuShu = entity.WeiQYHuShu;
            model.WeiQYMianJi = entity.WeiQYMianJi;
            model.YiQYHuShu = entity.YiQYHuShu;
            model.YiQYMianJi = entity.YiQYMianJi;
            model.ZhanDiMianJi = entity.ZhanDiMianJi;
            model.ZongHuShu = entity.ZongHuShu;
            if (entity.FileList != null)
            {
                foreach (var item in entity.FileList)
                {
                    Models.Response.ProjectFile file = new Models.Response.ProjectFile();
                    file.type = Entity.ProjectFileType.file;
                    file.file_name = item.FileName;
                    file.file_path = GetSiteUrl() + item.FilePath;
                    file.file_size = item.FileSize;
                    model.file_list.Add(file);
                }
            }

            return model;
        }

        /// <summary>
        /// 编辑拆迁分期model转换
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private BLL.Model.ProjectStage BuildAddProjectStage(Models.Response.ProjectInfoStage entity)
        {
            BLL.Model.ProjectStage model = new BLL.Model.ProjectStage();
            model.begin_time = entity.begin_time == null ? "" : Tools.TypeHelper.ObjectToDateTime(entity.begin_time).ToString("yyyy-MM-dd");
            model.ChaiBuChangjinE = entity.ChaiBuChangjinE;
            model.ChaiBuChangMianJi = entity.ChaiBuChangMianJi;
            model.ChaiJianZhuMianJi = entity.ChaiJianZhuMianJi;
            model.ChaiZhanDiMianJi = entity.ChaiZhanDiMianJi;
            model.JiDiMianJi = entity.JiDiMianJi;
            model.KongDiMianJi = entity.KongDiMianJi;
            model.stage_id = entity.stage_id;
            model.title = entity.title;
            model.WeiQYHuShu = entity.WeiQYHuShu;
            model.WeiQYMianJi = entity.WeiQYMianJi;
            model.YiQYHuShu = entity.YiQYHuShu;
            model.YiQYMianJi = entity.YiQYMianJi;
            model.ZhanDiMianJi = entity.ZhanDiMianJi;
            model.ZongHuShu = entity.ZongHuShu;
            if (entity.file_list != null)
            {
                foreach (var item in entity.file_list)
                {
                    BLL.Model.ProjectStageFile file = new BLL.Model.ProjectStageFile();
                    file.file_name = item.file_name;
                    file.file_path = item.file_path;
                    file.file_size = item.file_size;
                    model.file_list.Add(file);
                }

            }
            return model;

        }

        /// <summary>
        /// 构造用户基本信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Models.Response.SelectUser BuilderSelectUser(Entity.CusUser entity)
        {
            if (entity == null)
                return null;
            Models.Response.SelectUser model = new Models.Response.SelectUser();
            model.user_id = entity.ID;
            model.telphone = entity.Telphone;
            model.nick_name = entity.NickName;
            model.short_num = entity.ShorNum;
            model.avatar = GetSiteUrl() + entity.Avatar;
            return model;
        }

    }
}
