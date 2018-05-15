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
        public ActionResult Index(string backUrl,string idnumber)
        {
            if(string.IsNullOrWhiteSpace(backUrl))
            {
                return PromptView("返回地址不明确");
            }
            string id_card_number = WorkContext.UserInfo.IDCardNumber;
            if (!string.IsNullOrWhiteSpace(idnumber)) id_card_number = idnumber;
            ViewData["BackUrl"] = backUrl;
            ViewData["IDNumber"] = id_card_number;
            BLL.BaseBLL<Entity.MPUser> bll = new BLL.BaseBLL<Entity.MPUser>();
            var model = bll.GetModel(p => p.IDCardNumber == id_card_number, "ID DESC");
            if (model == null)
            {
                ViewData["RealName"] = "";
            }
            ViewData["RealName"] = model.RealName + " - ";
            return View();
        }

        /// <summary>
        /// Ajax加载
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPageList(int page_size,int page_index,string id_number)
        {
            UnifiedResultEntity<List<Entity.MpUserMedicalReport>> result = new UnifiedResultEntity<List<Entity.MpUserMedicalReport>>();
            if(page_size<=0 || page_index <= 0)
            {
                result.msgbox = "非法参数";
                return Json(result);
            }
            if(string.IsNullOrWhiteSpace(id_number))
            {
                result.msg = -1;
                result.msgbox = "请先去完善资料";
                return Json(result);
            }

            BLL.BaseBLL<Entity.MpUserMedicalReport> bll = new BLL.BaseBLL<Entity.MpUserMedicalReport>();
            int row_count = 0;
            int user_id = WorkContext.UserInfo.ID;
            var db_list = bll.GetPagedList(page_index, page_size, ref row_count, p => p.IDCardNumber == id_number, "AddTime DESC");
            result.msg = 1;
            result.msgbox = "ok";
            result.data = db_list;
            result.total = row_count;
            return Json(result);
        }

    }
}