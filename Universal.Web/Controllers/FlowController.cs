using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 秘籍
    /// </summary>
    [BasicAdminAuth]
    public class FlowController : BaseHBLController
    {
        public ActionResult Index()
        {
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
        public JsonResult PageData(int page_size, int page_index, string keyword)
        {
            BLL.BaseBLL<Entity.Flow> bll = new BLL.BaseBLL<Entity.Flow>();
            int rowCount = 0;
            List<Entity.Flow> list = bll.GetPagedList(page_index, page_size, ref rowCount, p => p.TopPID == null, "LastUpdateTime desc");
            WebAjaxEntity<List<Entity.Flow>> result = new WebAjaxEntity<List<Entity.Flow>>();
            result.msg = 1;
            result.msgbox = CalculatePage(rowCount, page_size).ToString();
            result.data = list;
            result.total = rowCount;
            return Json(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Del(int id)
        {
            if (id <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BLLFlow.Del(id);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);

        }

        /// <summary>
        /// 设置默认
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetDefault(int id)
        {
            if (id <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            string msg = "";
            BLL.BLLFlow.SetDefault(id, out msg);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity);

        }

        public ActionResult Modify()
        {
            return View();
        }


        #region  给前端的接口

        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllNode()
        {
            WebAjaxEntity<List<Models.WebNode>> response_entity = new WebAjaxEntity<List<Models.WebNode>>();
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            List<Models.WebNode> response_list = new List<Models.WebNode>();
            BLL.BaseBLL<Entity.Node> bll = new BLL.BaseBLL<Entity.Node>();
            var db_list = bll.GetListBy(0, new List<BLL.FilterSearch>(), "ID ASC");
            foreach (var item in db_list)
            {
                Models.WebNode model = new Models.WebNode();
                model.node_id = item.ID;
                model.node_title = item.Title;
                response_list.Add(model);
            }
            response_entity.data = response_list;
            return Json(response_entity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有流程
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFlow(int? pid)
        {
            WebAjaxEntity<List<Models.WebFlow>> response_entity = new WebAjaxEntity<List<Models.WebFlow>>();
            response_entity.msg = 1;
            response_entity.msgbox = "ok";
            List<Models.WebFlow> response_list = new List<Models.WebFlow>();
            BLL.BaseBLL<Entity.Flow> bll = new BLL.BaseBLL<Entity.Flow>();
            var db_list = new List<Entity.Flow>();
            if (TypeHelper.ObjectToInt(pid, 0) >= 0)
                db_list = bll.GetListBy(0, p => p.PID == pid, "Priority Desc", p => p.Node);
            else
                db_list = bll.GetListBy(0, new List<BLL.FilterSearch>(), "Priority Desc", p => p.Node);
            foreach (var item in db_list)
            {
                Models.WebFlow model = new Models.WebFlow();
                model.flow_id = item.ID;
                model.node_id = item.NodeID;
                model.node_title = item.Node.Title;
                model.parent_id = item.PID;
                model.toid = item.TOID;
                response_list.Add(model);
            }
            response_entity.data = response_list;
            return Json(response_entity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加流程
        /// </summary>
        /// <param name="pid">为-1时为添加顶级</param>
        /// <param name="node_id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddFlow(int pid, int node_id)
        {
            if (pid == 0 || pid < -1 || node_id <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            string msg = "";
            var id = BLL.BLLFlow.AddFlow(pid, node_id, out msg);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = msg;
            WorkContext.AjaxStringEntity.data = id.ToString();
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 修改流程的节点
        /// </summary>
        /// <param name="flow_id">流程ID</param>
        /// <param name="node_id">新节点ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyFlowNode(int flow_id, int node_id)
        {
            if (flow_id <= 0 || node_id <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            string msg = "";
            if (BLL.BLLFlow.ModifyFlow(flow_id, node_id, out msg))
                WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 设置流程箭头指引方向
        /// </summary>
        /// <param name="flow_id">流程ID</param>
        /// <param name="to_id">指向的ID</param>
        /// <returns></returns>
        public JsonResult SetFlowTOID(int flow_id, string to_id)
        {
            if (flow_id <= 0 || string.IsNullOrWhiteSpace(to_id))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            string msg = "";
            if (BLL.BLLFlow.SetFlowToID(flow_id, to_id, out msg))
                WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity);
        }

        #endregion


    }
}