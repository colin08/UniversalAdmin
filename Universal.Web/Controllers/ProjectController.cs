using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Universal.Tools;
using Universal.Web.Framework;
using System.Data.Entity;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 项目管理(普通用户)
    /// </summary>
    [BasicUserAuth]
    public class ProjectController : BaseHBLController
    {
        public ActionResult Index()
        {
            ViewData["IsAdmin"] =!(BLL.BLLCusUser.CheckUserIsAdmin(WorkContext.UserInfo.ID));
            LoadNodeCategory();
            return View();
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PageData(int page_size, int page_index, bool only_mine, string keyword, int status, int node_category_id, DateTime? begin_time, DateTime? end_time, bool is_admin)
        {
            BLL.BaseBLL<Entity.Project> bll = new BLL.BaseBLL<Entity.Project>();
            BLL.BaseBLL<Entity.CusUserProjectFavorites> bll_fav = new BLL.BaseBLL<Entity.CusUserProjectFavorites>();

            int rowCount = 0;
            var list = BLL.BLLProject.GetPageData(page_index, page_size, ref rowCount, WorkContext.UserInfo.ID, keyword, only_mine, status, node_category_id, begin_time, end_time, is_admin);
            foreach (var item in list)
            {
                item.IsFavorites = bll_fav.Exists(p => p.CusUserID == WorkContext.UserInfo.ID && p.ProjectID == item.ID);
            }
            WebAjaxEntity<List<Entity.Project>> result = new WebAjaxEntity<List<Entity.Project>>();
            result.msg = 1;
            result.msgbox = CalculatePage(rowCount, page_size).ToString();
            result.data = list;
            result.total = rowCount;

            return Json(result);
        }

        /// <summary>
        /// 导出项目附件
        /// </summary>
        /// <param name="project_id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportFile(int id)
        {
            string msg = "";
            var is_ok = BLL.BLLProjectFlowNode.ImportProject(id, out msg);
            WorkContext.AjaxStringEntity.msg = is_ok ? 1 : 0;
            WorkContext.AjaxStringEntity.data = msg;
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Del(int id)
        {
            if (id <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            string msg = "";
            if (BLL.BLLProject.Del(id, WorkContext.UserInfo.ID, out msg))
                WorkContext.AjaxStringEntity.msg = 1;

            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity);

        }

        /// <summary>
        ///  收藏
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddFavorites(int id)
        {
            if (id <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            string msg = "";
            bool isOK = BLL.BllCusUserFavorites.AddProjectFav(id, WorkContext.UserInfo.ID, out msg);
            WorkContext.AjaxStringEntity.msgbox = msg;
            if (!isOK)
            {
                return Json(WorkContext.AjaxStringEntity);
            }

            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);

        }


        /// <summary>
        /// 项目基本信息
        /// </summary>
        /// <returns></returns>
        public ActionResult BasicInfo(int id, string b)
        {
            ViewData["IsAdmin"] = !(BLL.BLLCusUser.CheckUserIsAdmin(WorkContext.UserInfo.ID));
            if (TypeHelper.ObjectToInt(b, 0) != 0)
                ViewData["BackUrl"] = "/";
            else
                ViewData["BackUrl"] = "/Project/Index";

            ViewData["FlowName"] = "";
            if (id <= 0)
                return Content("项目不存在");
            var entity = BLL.BLLProject.GetModel(id);
            if (entity == null)
                return Content("项目不存在");
            var entity_flow = new BLL.BaseBLL<Entity.Flow>().GetModel(p => p.ID == entity.FlowID);
            if (entity_flow != null)
                ViewData["FlowName"] = entity_flow.Title;
            List<Models.ViewModelDocumentCategory> users_list = new List<Models.ViewModelDocumentCategory>();
            foreach (var item in entity.ProjectUsers)
            {
                users_list.Add(new Models.ViewModelDocumentCategory(item.CusUser.ID, item.CusUser.NickName));
            }
            ViewData["UserList"] = users_list;


            return View(entity);
        }

        /// <summary>
        /// 项目审批
        /// </summary>
        /// <param name="project_id"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public JsonResult DoApprove(int project_id, string status, string remark)
        {
            string msg = "";
            int sta = TypeHelper.ObjectToInt(status);
            if (sta != 1 && sta != 2)
            {
                WorkContext.AjaxStringEntity.msgbox = "审核状态不正确";
                return Json(WorkContext.AjaxStringEntity);
            }
            bool isOK = BLL.BLLProject.Approve(WorkContext.UserInfo.ID, project_id, (Entity.ApproveStatusType)sta, remark, out msg);
            WorkContext.AjaxStringEntity.msg = isOK ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 项目信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Info(int id)
        {
            ViewData["IsAdmin"] = !(BLL.BLLCusUser.CheckUserIsAdmin(WorkContext.UserInfo.ID));
            if (id <= 0)
                return Content("项目不存在");
            BLL.BaseBLL<Entity.Project> bll = new BLL.BaseBLL<Entity.Project>();
            var entity = bll.GetModel(p => p.ID == id);
            if (entity == null)
                return Content("项目不存在");

            return View(entity);
        }

        /// <summary>
        /// 流程信息
        /// </summary>
        /// <returns></returns>
        public ActionResult FlowInfo(int id)
        {
            //是否可以编辑节点
            ViewData["can_edit"] = false;
            ViewData["IsAdmin"] = !(BLL.BLLCusUser.CheckUserIsAdmin(WorkContext.UserInfo.ID));
            if (id <= 0)
                return Content("项目不存在");
            BLL.BaseBLL<Entity.Project> bll = new BLL.BaseBLL<Entity.Project>();
            var entity = bll.GetModel(p => p.ID == id, p => p.ProjectUsers);
            if (entity == null)
                return Content("项目不存在");
            ViewData["project_id"] = id;
            if(entity.ProjectUsers.ToList().Any(p=>p.CusUserID == WorkContext.UserInfo.ID))
                ViewData["can_edit"] = true;

            return View(entity);
        }

        /// <summary>
        /// 流程节点信息
        /// </summary>
        /// <param name="id">流程节点的id，不是项目id，也不是节点id</param>
        /// <returns></returns>
        public ActionResult FlowNodeInfo(int id)
        {
            Entity.ProjectFlowNode model_flow = BLL.BLLProjectFlowNode.GetModel(id);
            if (model_flow == null)
                return Content("流程节点不存在");
            Entity.Node node_info = BLL.BLLNode.GetMode(model_flow.NodeID);
            if (node_info == null)
                return Content("节点不存在");

            Models.ViewModelProjectFlowNode entity = new Models.ViewModelProjectFlowNode();
            entity.flow_info = model_flow;
            entity.node_info = node_info;

            //foreach (var item in node_info.NodeUsers)
            //    entity.users_entity.Add(new Models.ViewModelDocumentCategory(item.CusUser.ID, item.CusUser.NickName));

            entity.BuildViewModelListFile(node_info.NodeFiles.ToList());
            entity.BuildViewModelNodeListFile(model_flow.ProjectFlowNodeFiles.ToList());

            return View(entity);
        }

        /// <summary>
        /// 保存流程节点备注信息
        /// </summary>
        /// <param name="id">流程节点的id，不是项目id，也不是节点id</param>
        /// <param name="remark"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveFlowNodeRemark(int id, string remark, string files)
        {
            string msg = "";
            var is_ok = BLL.BLLProjectFlowNode.ModifyRemarkFile(id, WorkContext.UserInfo.ID, remark, files, out msg);
            WorkContext.AjaxStringEntity.msg = is_ok ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 流拆迁信息
        /// </summary>
        /// <returns></returns>
        public ActionResult StageInfo(int id)
        {
            ViewData["IsAdmin"] = !(BLL.BLLCusUser.CheckUserIsAdmin(WorkContext.UserInfo.ID));
            if (id <= 0)
                return Content("项目不存在");
            BLL.BaseBLL<Entity.Project> bll = new BLL.BaseBLL<Entity.Project>();
            var entity = bll.GetModel(p => p.ID == id);
            if (entity == null)
                return Content("项目不存在");
            ViewData["ProjectID"] = id;
            return View(entity);
        }


        public ActionResult Modify(int? id)
        {
            //判断审核人是否必填
            var req_approve = BLL.BLLCusUser.CheckUserIsAdmin(WorkContext.UserInfo.ID);
            ViewData["RequieAPPID"] = req_approve;
            //默认流程
            ViewData["DefaultFlowID"] = BLL.BLLFlow.GetDefault();
            LoadFlow();
            int ids = TypeHelper.ObjectToInt(id);

            Models.ViewModelProject model = new Models.ViewModelProject();
            if (!BLL.BLLCusRoute.CheckUserAuthority(BLL.CusRouteType.项目操作, WorkContext.UserInfo.ID))
            {
                model.Msg = 4;
                return View(model);
            }
            if (ids != 0)
            {
                var entity = BLL.BLLProject.GetModel(ids);
                

                if (entity != null)
                {
                    model.title = entity.Title;
                    model.id = ids;

                    model.approve_user_id = TypeHelper.ObjectToInt(entity.ApproveUserID, 0);
                    model.approve_user_name = entity.ApproveUser == null ? "" : entity.ApproveUser.NickName;
                    model.flow_id = TypeHelper.ObjectToInt(entity.FlowID, 0);
                    model.post_see = entity.See;
                    model.area = entity.Area;
                    model.GaiZaoXingZhi = entity.GaiZaoXingZhi;
                    model.ZhongDiHao = entity.ZhongDiHao;
                    model.ShenBaoZhuTi = entity.ShenBaoZhuTi;
                    model.ZongMianJi = entity.ZongMianJi;
                    model.ZongMianJiOther = entity.ZongMianJiOther;
                    model.WuLeiQuanMianJi = entity.WuLeiQuanMianJi;
                    model.LaoWuCunMianJi = entity.LaoWuCunMianJi;
                    model.FeiNongMianJi = entity.FeiNongMianJi;
                    model.KaiFaMianJi = entity.KaiFaMianJi;
                    model.GengXinDanYuanYongDiMianJi = entity.GengXinDanYuanYongDiMianJi;
                    model.RongJiLv = entity.RongJiLv;
                    model.TuDiShiYongQuan = entity.TuDiShiYongQuan;
                    model.JianSheGuiHuaZheng = entity.JianSheGuiHuaZheng;
                    model.ChaiQianYongDiMianJi = entity.ChaiQianYongDiMianJi;
                    model.ChaiQianJianZhuMianJi = entity.ChaiQianJianZhuMianJi;
                    model.LiXiangTime = entity.LiXiangTime;
                    model.ZhuanXiangTime = entity.ZhuanXiangTime;
                    model.ZhuTiTime = entity.ZhuTiTime;
                    model.YongDiTime = entity.YongDiTime;
                    model.KaiPanTime = entity.KaiPanTime;
                    model.FenChengBiLi = entity.FenChengBiLi;
                    model.JunJia = entity.JunJia;


                    #region 权限展示
                    System.Text.StringBuilder str_see_ids = new System.Text.StringBuilder();
                    switch (entity.See)
                    {
                        case Entity.DocPostSee.everyone:
                            break;
                        case Entity.DocPostSee.department:
                            foreach (var item in BLL.BLLDepartment.GetListByIds(entity.TOID))
                            {
                                str_see_ids.Append(item.ID.ToString() + ",");
                                model.see_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.Title));
                            }
                            break;
                        case Entity.DocPostSee.user:
                            foreach (var item in BLL.BLLCusUser.GetListByIds(entity.TOID))
                            {
                                str_see_ids.Append(item.ID.ToString() + ",");
                                model.see_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.NickName));
                            }
                            break;
                        default:
                            break;
                    }
                    if (str_see_ids.Length > 0)
                    {
                        str_see_ids = str_see_ids.Remove(str_see_ids.Length - 1, 1);
                    }
                    model.see_ids = str_see_ids.ToString();
                    #endregion

                    model.BuildViewModelListFile(entity.ProjectFiles.ToList());

                    #region 项目联系人
                    System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
                    foreach (var item in entity.ProjectUsers)
                    {
                        str_ids.Append(item.ID.ToString() + ",");
                        model.users_entity.Add(new Models.ViewModelDocumentCategory(item.CusUser.ID, item.CusUser.NickName));
                    }
                    if (str_ids.Length > 0)
                    {
                        str_ids = str_ids.Remove(str_ids.Length - 1, 1);
                    }
                    model.user_ids = str_ids.ToString();
                    #endregion
                }
                else
                {
                    model.Msg = 2;
                    model.MsgBox = "数据不存在";
                }

            }
            else
            {
                //默认审核用户
                string nick_name = "";
                int user_id = BLL.BLLCusUser.GetDefaultApproveUser(out nick_name);
                if (user_id != 0)
                {
                    model.approve_user_id = user_id;
                    model.approve_user_name = nick_name;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Modify(Models.ViewModelProject entity)
        {
            int app_id = TypeHelper.ObjectToInt(entity.approve_user_id, 0);
            var isAdd = entity.id == 0 ? true : false;
            LoadFlow();
            //默认流程
            ViewData["DefaultFlowID"] = BLL.BLLFlow.GetDefault();
            //判断审核人是否必填
            var requie_approve = BLL.BLLCusUser.CheckUserIsAdmin(WorkContext.UserInfo.ID);
            ViewData["RequieAPPID"] = requie_approve;

            BLL.BaseBLL<Entity.Project> bll = new BLL.BaseBLL<Entity.Project>();
            if (!isAdd)
            {
                if (!bll.Exists(p => p.ID == entity.id))
                {
                    entity.Msg = 2;
                    ModelState.AddModelError("title", "信息不存在");
                }
            }


            if (requie_approve && app_id == 0)
            {
                ModelState.AddModelError("approve_user_id", "审核人必选");
            }
            if(app_id != 0)
            {
                if(app_id == WorkContext.UserInfo.ID)
                {
                    entity.Msg = 5;
                    entity.MsgBox = "不能自己审批自己的项目";
                    ModelState.AddModelError("title", "不能自己审批自己的项目");
                }

                entity.approve_user_name = BLL.BLLCusUser.GetNickName(app_id);
            }


            #region 处理联系人ID
            System.Text.StringBuilder str_user_ids = new System.Text.StringBuilder();
            entity.users_entity = new List<Models.ViewModelDocumentCategory>();
            foreach (var item in BLL.BLLCusUser.GetListByIds(entity.user_ids))
            {
                str_user_ids.Append(item.ID.ToString() + ",");
                entity.users_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.NickName));
            }
            string final_user_ids = "";
            if (str_user_ids.Length > 0)
            {
                final_user_ids = str_user_ids.Remove(str_user_ids.Length - 1, 1).ToString();
            }
            #endregion

            #region 处理可见用户或部门的ID
            System.Text.StringBuilder str_see_ids = new System.Text.StringBuilder();
            entity.see_entity = new List<Models.ViewModelDocumentCategory>();
            switch (entity.post_see)
            {
                case Entity.DocPostSee.everyone:
                    break;
                case Entity.DocPostSee.department:
                    foreach (var item in BLL.BLLDepartment.GetListByIds(entity.see_ids))
                    {
                        str_see_ids.Append(item.ID.ToString() + ",");
                        entity.see_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.Title));
                    }
                    break;
                case Entity.DocPostSee.user:
                    foreach (var item in BLL.BLLCusUser.GetListByIds(entity.see_ids))
                    {
                        str_see_ids.Append(item.ID.ToString() + ",");
                        entity.see_entity.Add(new Models.ViewModelDocumentCategory(item.ID, item.NickName));
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

            if (ModelState.IsValid)
            {
                Entity.Project model = null;
                if (isAdd)
                {
                    model = new Entity.Project();
                    model.CusUserID = WorkContext.UserInfo.ID;
                }
                else
                {
                    model = bll.GetModel(p => p.ID == entity.id);
                    //判断有没有权限编辑
                    if (!BLL.BLLCusRoute.CheckUserAuthority(BLL.CusRouteType.项目操作, WorkContext.UserInfo.ID))
                    {
                        entity.Msg = 4;
                        return View(entity);
                    }
                }

                model.ApproveUserID = entity.approve_user_id;

                //设置默认审核用户
                if (app_id != 0)
                    BLL.BLLCusUser.SetDefaultApproveUser(app_id);

                if (entity.flow_id == 0)
                    model.FlowID = null;
                else
                    model.FlowID = entity.flow_id;
                model.See = entity.post_see;
                model.Area = entity.area;
                model.GaiZaoXingZhi = entity.GaiZaoXingZhi;
                model.ZhongDiHao = entity.ZhongDiHao;
                model.ShenBaoZhuTi = entity.ShenBaoZhuTi;
                model.ZongMianJi = entity.ZongMianJi;
                model.GengXinDanYuanYongDiMianJi = entity.GengXinDanYuanYongDiMianJi;
                model.ZongMianJiOther = entity.ZongMianJiOther;
                model.WuLeiQuanMianJi = entity.WuLeiQuanMianJi;
                model.LaoWuCunMianJi = entity.LaoWuCunMianJi;
                model.FeiNongMianJi = entity.FeiNongMianJi;
                model.KaiFaMianJi = entity.KaiFaMianJi;
                model.RongJiLv = entity.RongJiLv;
                model.TuDiShiYongQuan = entity.TuDiShiYongQuan;
                model.JianSheGuiHuaZheng = entity.JianSheGuiHuaZheng;
                model.ChaiQianYongDiMianJi = entity.ChaiQianYongDiMianJi;
                model.ChaiQianJianZhuMianJi = entity.ChaiQianJianZhuMianJi;
                model.LiXiangTime = entity.LiXiangTime;
                model.ZhuanXiangTime = entity.ZhuanXiangTime;
                model.ZhuTiTime = entity.ZhuTiTime;
                model.YongDiTime = entity.YongDiTime;
                model.KaiPanTime = entity.KaiPanTime;
                model.FenChengBiLi = entity.FenChengBiLi;
                model.JunJia = entity.JunJia;

                model.SetYear();
                model.SetQuarter();
                model.Title = entity.title;
                model.TOID = final_see_ids;
                model.See = entity.post_see;
                model.LastUpdateUserName = WorkContext.UserInfo.NickName;
                model.ProjectFiles = entity.BuildFileList();
                model.LastUpdateUserName = WorkContext.UserInfo.NickName;
                string msg = "";
                if (isAdd)
                    BLL.BLLProject.Add(model, final_user_ids, out msg);
                else
                    BLL.BLLProject.Modify(model, final_user_ids, WorkContext.UserInfo.ID, out msg);

                if (msg != "")
                {
                    entity.Msg = 5;
                    entity.MsgBox = msg;
                }
                else
                {

                    entity.Msg = 1;
                }
            }
            else
            {
                entity.Msg = 3;
            }

            return View(entity);
        }

        public void LoadNodeCategory()
        {
            BLL.BaseBLL<Entity.NodeCategory> bll = new BLL.BaseBLL<Entity.NodeCategory>();
            List<Entity.NodeCategory> list = bll.GetListBy(0, new List<BLL.FilterSearch>(), "ID ASC", true);

            ViewData["NodeCategoryList"] = list;
        }

        /// <summary>
        /// 加载项目流程
        /// </summary>
        public void LoadFlow()
        {
            BLL.BaseBLL<Entity.Flow> bll = new BLL.BaseBLL<Entity.Flow>();
            List<Entity.Flow> list = bll.GetListBy(0, p => p.FlowType == Entity.FlowType.basic, "ID ASC", true);

            List<SelectListItem> userRoleList = new List<SelectListItem>();
            userRoleList.Add(new SelectListItem() { Text = "请选择..", Value = "0" });
            foreach (var item in list)
            {
                userRoleList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString() });
            }
            ViewData["FlowList"] = userRoleList;

            List<SelectListItem> typeList = new List<SelectListItem>();
            foreach (var item in EnumHelper.EnumToDictionary(typeof(Entity.ProjectArea)))
            {
                typeList.Add(new SelectListItem() { Text = item.Value, Value = item.Key.ToString() });
            }
            ViewData["AreaList"] = typeList;
        }

        #region 前端流程接口


        /// <summary>
        /// 获取当前项目已进行的的流程信息
        /// </summary>
        /// <param name="project_id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult APIGetProjectFlow(int project_id)
        {
            WebAjaxEntity<List<BLL.Model.ProjectFlowNode>> result = new WebAjaxEntity<List<BLL.Model.ProjectFlowNode>>();
            result.data = BLL.BLLProjectFlowNode.GetProjectFlow(project_id);
            result.msg = 1;
            result.msgbox = "ok";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取下一个要进行的节点
        /// </summary>
        /// <param name="project_id"></param>
        /// <param name="project_flow_node_id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult APIGetNextFlowNode(int project_id, int project_flow_node_id)
        {
            WebAjaxEntity<List<BLL.Model.ProjectFlowNode>> result = new WebAjaxEntity<List<BLL.Model.ProjectFlowNode>>();
            result.data = BLL.BLLProjectFlowNode.GetNextFlowNode(project_id, project_flow_node_id);
            result.msg = 1;
            result.msgbox = "ok";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取项目的单个流程信息
        /// </summary>
        /// <param name="project_flow_node_id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult APIGetProjectFlowNodeInfo(int project_flow_node_id)
        {
            WebAjaxEntity<BLL.Model.ProjectFlowNode> result = new WebAjaxEntity<BLL.Model.ProjectFlowNode>();
            result.data = BLL.BLLProjectFlowNode.GetProjectFlowNodeInfo(project_flow_node_id);
            result.msg = 1;
            result.msgbox = "ok";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存备注
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult APISaveRemark(int project_flow_node_id, string remark)
        {
            BLL.BaseBLL<Entity.ProjectFlowNode> bll = new BLL.BaseBLL<Entity.ProjectFlowNode>();
            var entity = bll.GetModel(p => p.ID == project_flow_node_id);
            if (entity == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "节点不存在";
                return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
            }
            entity.Remark = remark;
            bll.Modify(entity, "Remark");
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存节点附件
        /// </summary>
        /// <param name="project_flow_node_id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult APISaveFile(int project_flow_node_id)
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<BLL.Model.ProjectFlowNodeFile> data = null;
            try
            {
                data = js.Deserialize<List<BLL.Model.ProjectFlowNodeFile>>(stream);
            }
            catch
            {
                WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (data == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
                return Json(WorkContext.AjaxStringEntity);
            }
            bool is_ok = BLL.BLLProjectFlowNode.SaveFile(project_flow_node_id, data);
            if (is_ok)
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "保存成功";
            }
            else
                WorkContext.AjaxStringEntity.msgbox = "保存失败";

            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 保存修改的节点位置信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult APISaveLocaltion()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<BLL.Model.ProjectFlowNode> data = null;
            try
            {
                data = js.Deserialize<List<BLL.Model.ProjectFlowNode>>(stream);
            }
            catch
            {
                WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (data == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BLLProjectFlowNode.SaveLocation(data, WorkContext.UserInfo.ID);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 设置节点结束
        /// </summary>
        /// <param name="project_flow_node_id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult APISetEnd(int project_flow_node_id)
        {
            WorkContext.AjaxStringEntity.msg = BLL.BLLProjectFlowNode.SetEnd(project_flow_node_id, WorkContext.UserInfo.ID) ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置节点开启/关闭
        /// </summary>
        /// <param name="project_flow_node_id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult APISetStatus(int project_flow_node_id)
        {
            string msg = "";
            WorkContext.AjaxStringEntity.msg = BLL.BLLProjectFlowNode.SetStatus(project_flow_node_id, WorkContext.UserInfo.ID, out msg) ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置条件节点选中
        /// </summary>
        /// <param name="project_flow_node_id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult APISetSelect(int project_flow_node_id)
        {
            string msg = "";
            WorkContext.AjaxStringEntity.msg = BLL.BLLProjectFlowNode.SetSelect(project_flow_node_id, WorkContext.UserInfo.ID, out msg) ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpPost]
        //public JsonResult APIDelProjectFlowNode(int id, string project_pieces)
        //{
        //    string msg = "";
        //    BLL.BLLProjectFlowNode.DelProjectFlowNode(id, project_pieces, out msg);
        //    if (msg == "")
        //        WorkContext.AjaxStringEntity.msg = 1;
        //    WorkContext.AjaxStringEntity.msgbox = msg;
        //    return Json(WorkContext.AjaxStringEntity);
        //}

        ///// <summary>
        ///// 保存修改的节点信息
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult APISave()
        //{
        //    var sr = new StreamReader(Request.InputStream);
        //    var stream = sr.ReadToEnd();
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    BLL.Model.ProjectFlowNode data = null;
        //    try
        //    {
        //        data = js.Deserialize<BLL.Model.ProjectFlowNode>(stream);
        //    }
        //    catch
        //    {
        //        WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
        //        return Json(WorkContext.AjaxStringEntity);
        //    }
        //    if (data == null)
        //    {
        //        WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
        //        return Json(WorkContext.AjaxStringEntity);
        //    }

        //    string msg = "";
        //    WorkContext.AjaxStringEntity.msg = BLL.BLLProjectFlowNode.SaveNode(data, out msg) ? 1 : 0;
        //    WorkContext.AjaxStringEntity.msgbox = msg;
        //    return Json(WorkContext.AjaxStringEntity);
        //}

        ///// <summary>
        ///// 保存修改的节点信息
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult APISaveALL()
        //{
        //    var sr = new StreamReader(Request.InputStream);
        //    var stream = sr.ReadToEnd();
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    BLL.Model.ProjectFlow data = null;
        //    try
        //    {
        //        data = js.Deserialize<BLL.Model.ProjectFlow>(stream);
        //    }
        //    catch
        //    {
        //        WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
        //        return Json(WorkContext.AjaxStringEntity);
        //    }
        //    if (data == null)
        //    {
        //        WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
        //        return Json(WorkContext.AjaxStringEntity);
        //    }

        //    string msg = "";
        //    WorkContext.AjaxStringEntity.msg = BLL.BLLProjectFlowNode.SaveAllNode(data, out msg) ? 1 : 0;
        //    WorkContext.AjaxStringEntity.msgbox = msg;
        //    return Json(WorkContext.AjaxStringEntity);
        //}

        ///// <summary>
        ///// 添加项目流程节点
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult APIAddProjectFlowNode(int project_id, string project_pieces)
        //{
        //    WorkContext.AjaxStringEntity.msgbox = "暂不允许添加节点";
        //    return Json(WorkContext.AjaxStringEntity);

        //    var sr = new StreamReader(Request.InputStream);
        //    var stream = sr.ReadToEnd();
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    BLL.Model.ProjectFlowNode data = null;
        //    try
        //    {
        //        data = js.Deserialize<BLL.Model.ProjectFlowNode>(stream);
        //    }
        //    catch
        //    {
        //        WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
        //        return Json(WorkContext.AjaxStringEntity);
        //    }
        //    if (data == null)
        //    {
        //        WorkContext.AjaxStringEntity.msgbox = "json序列化后为空";
        //        return Json(WorkContext.AjaxStringEntity);
        //    }

        //    string msg = "";
        //    int id = BLL.BLLProjectFlowNode.AddNode(project_id, project_pieces, data, out msg);
        //    if (id > 0)
        //    {
        //        WorkContext.AjaxStringEntity.msg = 1;
        //        WorkContext.AjaxStringEntity.data = id.ToString();
        //    }
        //    WorkContext.AjaxStringEntity.msgbox = msg;
        //    return Json(WorkContext.AjaxStringEntity);
        //}

        #endregion

        #region 给前端拆迁的接口

        /// <summary>
        /// 获取所有拆迁信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllStage(int project_id)
        {
            WebAjaxEntity<List<BLL.Model.ProjectStage>> response_entity = new WebAjaxEntity<List<BLL.Model.ProjectStage>>();
            response_entity.data = BLL.BLLProjectStage.GetAllStage(project_id);
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            response_entity.total = response_entity.data.Count;
            return Json(response_entity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取某个拆迁的信息
        /// </summary>
        /// <param name="stage_id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetStage(int stage_id)
        {
            WebAjaxEntity<BLL.Model.ProjectStage> response_entity = new WebAjaxEntity<BLL.Model.ProjectStage>();
            response_entity.data = BLL.BLLProjectStage.GetStage(stage_id);
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            return Json(response_entity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存某个拆迁节点的信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveStage(int project_id)
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            BLL.Model.ProjectStage data = null;
            try
            {
                data = js.Deserialize<BLL.Model.ProjectStage>(stream);
            }
            catch
            {
                WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (data == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "序列化后为空";
                return Json(WorkContext.AjaxStringEntity);
            }
            string msg = "";
            if (data.stage_id <= 0)
            {
                int id = BLL.BLLProjectStage.Add(project_id, data,WorkContext.UserInfo.ID, out msg);
                WorkContext.AjaxStringEntity.msg = id > 0 ? 1 : 0;
                WorkContext.AjaxStringEntity.msgbox = msg;
                WorkContext.AjaxStringEntity.data = id.ToString();
                return Json(WorkContext.AjaxStringEntity);
            }
            else
            {
                var isOK = BLL.BLLProjectStage.Modfify(data,WorkContext.UserInfo.ID, out msg);
                WorkContext.AjaxStringEntity.msg = isOK ? 1 : 0;
                WorkContext.AjaxStringEntity.msgbox = msg;
                return Json(WorkContext.AjaxStringEntity);
            }
        }

        #endregion
    }
}