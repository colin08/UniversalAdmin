using System;
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
            List<Entity.Flow> list = bll.GetPagedList(page_index, page_size, ref rowCount, new List<BLL.FilterSearch>(), "LastUpdateTime desc");
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
            if (BLL.BLLFlow.SetDefault(id, out msg))
                WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity);

        }

        public ActionResult Modify(int? id)
        {
            int temp_id = TypeHelper.ObjectToInt(id, -1);
            ViewData["flow_title"] = "";
            ViewData["flow_id"] = temp_id;
            if(temp_id != -1)
            {
                BLL.BaseBLL<Entity.Flow> bll = new BLL.BaseBLL<Entity.Flow>();
                var entity = bll.GetModel(p => p.ID == temp_id);
                if(entity == null)
                {
                    ViewData["flow_id"] = 0;
                }
                ViewData["flow_title"] = entity.Title;
            }
            return View();
        }


        #region  给前端的接口

        /// <summary>
        /// 获取所有流程节点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllFlowNode(int flow_id)
        {
            var result= BLL.BLLFlow.GetWebFlowData(flow_id);
            return Json(result);
        }

        /// <summary>
        /// 添加流程
        /// </summary>
        /// <param name="flow_id">为-1时为顶级</param>
        /// <param name="node_id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddNode(int flow_id, int node_id, int top, int left, string icon, string color, string process_to)
        {
            string msg = "";
            var ids = BLL.BLLFlow.AddFlowNode(WorkContext.UserInfo.ID, flow_id, node_id, top, left, icon, color, process_to, out msg);
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
        /// 修改流程的节点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveFlowNode()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            var flow_info = js.Deserialize<BLL.Model.WebSaveFlow>(stream);
            if (flow_info != null)
            {
                WorkContext.AjaxStringEntity.msgbox = "json序列化失败";
                return Json(WorkContext.AjaxStringEntity);
            }
            string msg = "";
            if(BLL.BLLFlow.WebSaveFlowData(flow_info, out msg))
                WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = msg;
            return Json(WorkContext.AjaxStringEntity);
        }
        
        #endregion


    }
}