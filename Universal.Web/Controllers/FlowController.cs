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
            ViewData["IsAdmin"] = !(BLL.BLLCusUser.CheckUserIsAdmin(WorkContext.UserInfo.ID));
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
            List<Entity.Flow> list = bll.GetPagedList(page_index, page_size, ref rowCount, p => p.FlowType == Entity.FlowType.basic, "LastUpdateTime desc");
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
            if((BLL.BLLCusUser.CheckUserIsAdmin(WorkContext.UserInfo.ID)))
            {
                WorkContext.AjaxStringEntity.msgbox = "没有权限进行删除";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (id <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            bool isOK = BLL.BLLFlow.Del(id);
            if (isOK)
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "删除成功";

            }else
            {
                WorkContext.AjaxStringEntity.msgbox = "删除失败，可能该流程已被项目使用.";
            }
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
            if (temp_id != -1)
            {
                BLL.BaseBLL<Entity.Flow> bll = new BLL.BaseBLL<Entity.Flow>();
                var entity = bll.GetModel(p => p.ID == temp_id);
                if (entity == null)
                {
                    ViewData["flow_id"] = 0;
                }
                ViewData["flow_title"] = entity.Title;
            }
            return View();
        }

        /// <summary>
        /// 流程演示
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Compact(int id)
        {
            ViewData["flow_id"] = id;
            return View();
        }

    }
}