using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    public class ToolsController : BaseHBLController
    {
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendCode(string guid, string telphone)
        {
            if (string.IsNullOrWhiteSpace(guid) || string.IsNullOrWhiteSpace(telphone))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }

            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            if (!bll.Exists(p => p.Telphone == telphone))
            {
                WorkContext.AjaxStringEntity.msgbox = "该手机号不存在";
                return Json(WorkContext.AjaxStringEntity);
            }

            string msg = "";
            BLL.BLLVerification.Send(telphone, new Guid(guid), Entity.CusVerificationType.RestPwd, out msg);
            if (msg.Equals("OK"))
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "success";
                return Json(WorkContext.AjaxStringEntity);
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
                return Json(WorkContext.AjaxStringEntity);
            }
        }

        /// <summary>
        /// 上传用户头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadAvatar(HttpPostedFileBase file)
        {
            if (file == null)
                file = Request.Files[0];
            UploadHelper uh = new UploadHelper();
            WebAjaxEntity<string> result = uh.Upload(file, "/uploads/avatar/");
            return Json(result);
        }

        /// <summary>
        /// 上传秘籍附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadDocument(HttpPostedFileBase file)
        {
            if (file == null)
                file = Request.Files[0];
            UploadHelper uh = new UploadHelper();
            WebAjaxEntity<string> result = uh.Upload(file, "/uploads/doc/");
            if (result.msg == 1)
            {
                result.msgbox = IOHelper.GetFileSizeTxt(result.data);
            }
            return Json(result);
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadFile(HttpPostedFileBase file)
        {
            if (file == null)
                file = Request.Files[0];
            UploadHelper uh = new UploadHelper();
            WebAjaxEntity<string> result = uh.Upload(file, "/uploads/file/");
            if (result.msg == 1)
            {
                result.msgbox = IOHelper.GetFileSizeTxt(result.data);
            }
            return Json(result);
        }

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public JsonResult UploadTextArea(HttpPostedFileBase fileData)
        {
            if (fileData == null)
            {
                fileData = HttpContext.Request.Files["imgFile"];
            }

            string file_name = fileData.FileName;
            string file_ext = "";
            if (!string.IsNullOrWhiteSpace(file_name))
                file_ext = IOHelper.GetFileExt(file_name).ToLower();

            //上传文件夹
            string operation = WebHelper.GetFormString("operation", "");
            if (string.IsNullOrWhiteSpace(operation))
                operation = WebHelper.GetQueryString("operation");

            if (string.IsNullOrWhiteSpace(operation))
            {
                WorkContext.AjaxStringEntity.msgbox = "保存位置不明确";
                return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
            }


            //保存的目录
            string filePath = "/uploads/" + operation + "/";

            UploadHelper up_helper = new UploadHelper();

            WorkContext.AjaxStringEntity = up_helper.Upload(fileData, filePath);
            Hashtable ht2 = new Hashtable();
            if (WorkContext.AjaxStringEntity.msg == 1)
            {
                ht2["error"] = 0;
                ht2["url"] = WorkContext.AjaxStringEntity.data;
            }
            else
            {
                ht2["error"] = 0;
                ht2["message"] = WorkContext.AjaxStringEntity.msgbox;
            }
            return Json(ht2, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 上传APK
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadAPK(HttpPostedFileBase file)
        {
            if (file == null)
                file = Request.Files[0];
            UploadHelper uh = new UploadHelper();
            Hashtable ht = new Hashtable();
            ht = uh.Upload_APK(file);
            return Json(ht);
        }


        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserList(string search)
        {
            WebAjaxEntity<List<Models.ViewModelNoticeUser>> result = new WebAjaxEntity<List<Models.ViewModelNoticeUser>>();
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            List<Models.ViewModelNoticeUser> list = new List<Models.ViewModelNoticeUser>();
            foreach (var item in bll.GetListBy(0, p => p.Telphone.Contains(search) || p.NickName.Contains(search), "ID asc"))
            {
                Models.ViewModelNoticeUser model = new Models.ViewModelNoticeUser();
                model.id = item.ID;
                model.nick_name = item.NickName;
                model.telphone = item.Telphone;
                list.Add(model);
            }
            result.data = list;
            result.msg = 1;
            result.total = list.Count;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetDepartmentList()
        {
            WebAjaxEntity<List<Models.ViewModelDepartment>> result = new WebAjaxEntity<List<Models.ViewModelDepartment>>();
            BLL.BaseBLL<Entity.CusDepartment> bll = new BLL.BaseBLL<Entity.CusDepartment>();
            List<Models.ViewModelDepartment> list = new List<Models.ViewModelDepartment>();
            foreach (var item in bll.GetListBy(0, new List<BLL.FilterSearch>(), "Priority desc"))
            {
                Models.ViewModelDepartment model = new Models.ViewModelDepartment();
                model.department_id = item.ID;
                model.parent_id = item.PID == null ? 0 : item.PID;
                model.title = item.Title;
                list.Add(model);
            }
            result.data = list;
            result.msg = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有职位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetJobList()
        {
            WebAjaxEntity<List<Models.ViewModelJob>> result = new WebAjaxEntity<List<Models.ViewModelJob>>();
            List<Models.ViewModelJob> list = new List<Models.ViewModelJob>();
            BLL.BaseBLL<Entity.CusUserJob> bll = new BLL.BaseBLL<Entity.CusUserJob>();
            foreach (var item in bll.GetListBy(0, new List<BLL.FilterSearch>(), "AddTime Asc"))
            {
                Models.ViewModelJob model = new Models.ViewModelJob();
                model.id = item.ID;
                model.title = item.Title;
                list.Add(model);
            }
            result.data = list;
            result.msg = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        #region  流程节点给前端的接口

        /// <summary>
        /// 获取所有系统节点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllNode(int flow_id, int t)
        {
            bool is_factor = TypeHelper.ObjectToInt(t, 0) == 1;
            //WebAjaxEntity<List<Models.ViewModelAllNode>> result = new WebAjaxEntity<List<Models.ViewModelAllNode>>();
            //List<Models.ViewModelAllNode> response_entity = new List<Models.ViewModelAllNode>();
            //BLL.BaseBLL<Entity.Node> bll_node = new BLL.BaseBLL<Entity.Node>();
            //BLL.BaseBLL<Entity.NodeCategory> bll = new BLL.BaseBLL<Entity.NodeCategory>();
            //foreach (var category in bll.GetListBy(0, new List<BLL.FilterSearch>(), "AddTime Asc"))
            //{
            //    Models.ViewModelAllNode model_category = new Models.ViewModelAllNode();
            //    model_category.category_id = category.ID;
            //    model_category.category_name = category.Title;
            //    List<Models.ViewModelAllNodeList> node_list = new List<Models.ViewModelAllNodeList>();
            //    foreach (var node in bll_node.GetListBy(0, p => p.NodeCategoryID == category.ID && p.IsFactor == is_factor, "AddTime Asc"))
            //    {
            //        Models.ViewModelAllNodeList model_node = new Models.ViewModelAllNodeList();
            //        model_node.node_id = node.ID;
            //        model_node.node_name = node.Title;
            //        node_list.Add(model_node);
            //    }
            //    model_category.node_list = node_list;
            //    response_entity.Add(model_category);                
            //}
            WebAjaxEntity<List<BLL.Model.AllNode>> result = new WebAjaxEntity<List<BLL.Model.AllNode>>();
            result.data = BLL.BLLNode.GetFlowSelectNode(flow_id, is_factor);
            result.msg = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取流程的节点
        /// </summary>
        /// <param name="flow_id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetFlowNode(int flow_id)
        {
            WebAjaxEntity<List<BLL.Model.FlowNode>> result = new WebAjaxEntity<List<BLL.Model.FlowNode>>();
            result.data = BLL.BLLFlow.GetUIFlowNode(flow_id);
            result.msg = 1;
            result.msgbox = "ok";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取节点详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetNodeInfo(int id)
        {
            int ids = TypeHelper.ObjectToInt(id, 0);
            Models.ViewModelNode model = new Models.ViewModelNode();
            Entity.Node entity = BLL.BLLNode.GetMode(ids);
            if (entity != null)
            {
                model.content = entity.Content;
                model.title = entity.Title;
                model.location = entity.Location;
                model.id = ids;
                model.is_factor = entity.IsFactor;
                model.category_id = entity.NodeCategoryID;
                System.Text.StringBuilder str_ids = new System.Text.StringBuilder();
                foreach (var item in entity.NodeUsers)
                {
                    str_ids.Append(item.ID.ToString() + ",");
                    model.users_entity.Add(new Models.ViewModelDocumentCategory(item.CusUser.ID, item.CusUser.Telphone + "(" + item.CusUser.NickName + ")"));
                }
                if (str_ids.Length > 0)
                {
                    str_ids = str_ids.Remove(str_ids.Length - 1, 1);
                }
                model.user_ids = str_ids.ToString();

                model.BuildViewModelListFile(entity.NodeFiles.ToList());
            }
            else
            {
                model.Msg = 2;
                model.MsgBox = "数据不存在";
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置节点父级信息
        /// </summary>
        /// <param name="flow_id">为-1时为顶级</param>
        /// <param name="title">流程标题</param>
        /// <param name="node_id"></param>
        /// <param name="pids">父级id，逗号分割</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetNodePids(int flow_id, string title, int node_id, string pids)
        {
            string msg = "";
            var ids = BLL.BLLFlow.SetNodePids(WorkContext.UserInfo.ID, flow_id, title, node_id, pids, out msg);
            if (TypeHelper.ObjectToInt(ids[0]) != -1)
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.data = ids[1].ToString();
                WorkContext.AjaxStringEntity.total = TypeHelper.ObjectToInt(ids[0]);
            }
            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 将节点从流程中删除
        /// </summary>
        /// <param name="node_id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult DelFlowNode(int flow_id,int node_id)
        {
            var isOK = BLL.BLLFlow.DelFlowNode(flow_id,node_id);
            WorkContext.AjaxStringEntity.msg = isOK ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = "";
            return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 生成流程图插件所需节点数据
        /// </summary>
        /// <param name="flow_id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GenerateFlowNodeCompact(int flow_id)
        {
            WebAjaxEntity<string> result = new WebAjaxEntity<string>();
            var isOK = BLL.BLLFlow.GenerateFlowNodeCompact(flow_id);
            result.msg = isOK ? 1 : 0;
            result.msgbox = "";
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取流程效果页面里的流程信息
        /// </summary>
        /// <param name="flow_id"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetFlowCompact(int flow_id)
        {
            WebAjaxEntity<List<BLL.Model.ProjectFlowNode>> result = new WebAjaxEntity<List<BLL.Model.ProjectFlowNode>>();
            result.data = BLL.BLLFlow.GetFlowCompact(flow_id);
            result.msg = 1;
            result.msgbox = "ok";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存流程效果页面里的流程位置信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveFlowCompact()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            List<BLL.Model.ProjectFlowNode> flow_info = null;
            try
            {
                flow_info = js.Deserialize<List<BLL.Model.ProjectFlowNode>>(stream);
            }
            catch
            {
                WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (flow_info == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (BLL.BLLFlow.SaveCompactNode(flow_info))
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "ok";
            }
            else
                WorkContext.AjaxStringEntity.msgbox = "保存失败";

            return Json(WorkContext.AjaxStringEntity);
        }


        ///// <summary>
        ///// 获取所有流程节点
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public JsonResult GetAllFlowNode(int flow_id)
        //{
        //    var result = BLL.BLLFlow.GetWebFlowData(flow_id);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// 获取单个流程节点
        ///// </summary>
        ///// <param name="flow_node_id"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public JsonResult GetFLowNode(int flow_node_id)
        //{
        //    WebAjaxEntity<BLL.Model.WebFlowNode> response_entity = new WebAjaxEntity<BLL.Model.WebFlowNode>();
        //    response_entity.msg = 1;
        //    response_entity.msgbox = "ok";
        //    response_entity.data = BLL.BLLFlow.GetWebFlowNodeData(flow_node_id);
        //    return Json(response_entity, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// 删除某个流程节点
        /// </summary>
        /// <param name="flow_node_id"></param>
        /// <returns></returns>
        //[HttpPost]
        //public JsonResult DelFlowNode(int flow_node_id)
        //{
        //    string msg = "";
        //    if (BLL.BLLFlow.DelWebFlowNode(flow_node_id,out msg))
        //        WorkContext.AjaxStringEntity.msg = 1;
        //    WorkContext.AjaxStringEntity.msgbox = msg;
        //    return Json(WorkContext.AjaxStringEntity);
        //}

        ///// <summary>
        ///// 添加流程
        ///// </summary>
        ///// <param name="flow_id">为-1时为顶级</param>
        ///// <param name="parent_flow_node_id">父级ID</param>
        ///// <param name="title">流程标题</param>
        ///// <param name="node_id"></param>
        ///// <param name="type">添加类别，1：插入节点，2：添加子节点，3：添加合并节点</param>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult AddNode(int flow_id, string title, int node_id, int parent_flow_node_id,int type)
        //{
        //    string msg = "";
        //    var ids = BLL.BLLFlow.AddFlowNode(WorkContext.UserInfo.ID, flow_id, title, node_id, parent_flow_node_id, type, out msg);
        //    if (TypeHelper.ObjectToInt(ids[0]) != -1)
        //    {
        //        WorkContext.AjaxStringEntity.msg = 1;
        //        WorkContext.AjaxStringEntity.data = ids[1].ToString();
        //        WorkContext.AjaxStringEntity.total = TypeHelper.ObjectToInt(ids[0]);
        //    }
        //    WorkContext.AjaxStringEntity.msgbox = msg;
        //    return Json(WorkContext.AjaxStringEntity);
        //}

        /// <summary>
        /// 修改流程标题
        /// </summary>
        /// <param name="flow_id"></param>
        /// <param name="new_title"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyFlowTitle(int flow_id, string new_title)
        {
            BLL.BaseBLL<Entity.Flow> bll = new BLL.BaseBLL<Entity.Flow>();
            Entity.Flow entity = bll.GetModel(p => p.ID == flow_id);
            if (entity == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "流程不存在";
                return Json(WorkContext.AjaxStringEntity);
            }
            entity.Title = new_title;
            bll.Modify(entity, "Title");
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "ok";
            return Json(WorkContext.AjaxStringEntity);
        }

        ///// <summary>
        ///// 修改流程的节点
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult SaveFlow()
        //{
        //    var sr = new StreamReader(Request.InputStream);
        //    var stream = sr.ReadToEnd();
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    //BLL.Model.WebSaveFlow flow_info = null;
        //    //try
        //    //{
        //    //    flow_info = js.Deserialize<BLL.Model.WebSaveFlow>(stream);
        //    //}
        //    //catch
        //    //{
        //    //    WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
        //    //    return Json(WorkContext.AjaxStringEntity);
        //    //}
        //    //if (flow_info == null)
        //    //{
        //    //    WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
        //    //    return Json(WorkContext.AjaxStringEntity);
        //    //}
        //    string msg = "";
        //    //if (BLL.BLLFlow.WebSaveFlowData(flow_info, out msg))
        //    //    WorkContext.AjaxStringEntity.msg = 1;
        //    WorkContext.AjaxStringEntity.msgbox = msg;
        //    return Json(WorkContext.AjaxStringEntity);
        //}

        ///// <summary>
        ///// 修改某个流程节点
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult SaveFlowNode()
        //{
        //    var sr = new StreamReader(Request.InputStream);
        //    var stream = sr.ReadToEnd();
        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    //BLL.Model.WebSaveFlowNode flow_node_info = null;
        //    //try
        //    //{
        //    //    flow_node_info = js.Deserialize<BLL.Model.WebSaveFlowNode>(stream);
        //    //}
        //    //catch
        //    //{
        //    //    WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
        //    //    return Json(WorkContext.AjaxStringEntity);
        //    //}
        //    //if (flow_node_info == null)
        //    //{
        //    //    WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
        //    //    return Json(WorkContext.AjaxStringEntity);
        //    //}
        //    string msg = "";
        //    //if (BLL.BLLFlow.WebSaveFlowNodeData(flow_node_info, out msg))
        //    //{
        //    //    WorkContext.AjaxStringEntity.msg = 1;
        //    //    WorkContext.AjaxStringEntity.msgbox = "ok";
        //    //}
        //    WorkContext.AjaxStringEntity.msgbox = msg;
        //    return Json(WorkContext.AjaxStringEntity);
        //}

        #endregion

    }

}