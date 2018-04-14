using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;
using System.Data;
using System.Data.Entity;
using Universal.Tools;

namespace Universal.Web.Areas.MP.Controllers
{
    /// <summary>
    /// 用户体检报告
    /// </summary>
    public class MedicalReportController : BaseMPController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string backUrl,string user_id)
        {
            if(string.IsNullOrWhiteSpace(backUrl))
            {
                return PromptView("返回地址不明确");
            }
            int u_id = WorkContext.UserInfo.ID;
            if (!string.IsNullOrWhiteSpace(user_id)) u_id = Tools.TypeHelper.ObjectToInt(user_id, 0);
            if(u_id == 0)
            {
                return PromptView(backUrl, "非法用户参数");
            }
            ViewData["BackUrl"] = backUrl;
            ViewData["UID"] = u_id;
            BLL.BaseBLL<Entity.MPUser> bll = new BLL.BaseBLL<Entity.MPUser>();
            var model = bll.GetModel(p => p.ID == u_id, "ID DESC");
            if (model == null) return PromptView(backUrl,"用户不存在");
            ViewData["RealName"] = model.RealName;
            //BLL.BaseBLL<Entity.MpUserMedicalReport> bll_report = new BLL.BaseBLL<Entity.MpUserMedicalReport>();
            //var db_list = bll_report.GetListBy(20, p => p.MPUserID == u_id, "AddTime DESC");
            return View();
        }

        /// <summary>
        /// Ajax加载
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPageList(int page_size,int page_index)
        {
            UnifiedResultEntity<List<Entity.MpUserMedicalReport>> result = new UnifiedResultEntity<List<Entity.MpUserMedicalReport>>();
            if(page_size<=0 || page_index <= 0)
            {
                result.msgbox = "非法参数";
                return Json(result);
            }

            BLL.BaseBLL<Entity.MpUserMedicalReport> bll = new BLL.BaseBLL<Entity.MpUserMedicalReport>();
            int row_count = 0;
            int user_id = WorkContext.UserInfo.ID;
            var db_list = bll.GetPagedList(page_index, page_size, ref row_count, p => p.MPUserID == user_id, "AddTime DESC");
            result.msg = 1;
            result.msgbox = "ok";
            result.data = db_list;
            result.total = row_count;
            return Json(result);
        }

    }
}