using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;
using System.Data;
using System.Data.Entity;

namespace Universal.Web.Areas.MP.Controllers
{
    /// <summary>
    /// 账户支出明细
    /// </summary>
    public class AmountDetailsController : BaseMPController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Ajax
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <returns></returns>
        public JsonResult GetPageList(int page_size, int page_index)
        {
            UnifiedResultEntity<List<Entity.MPUserAmountDetails>> result = new UnifiedResultEntity<List<Entity.MPUserAmountDetails>>();
            if (page_size <= 0 || page_index <= 0)
            {
                result.msgbox = "非法参数";
                return Json(result);
            }
            int user_id = WorkContext.UserInfo.ID;
            BLL.BaseBLL<Entity.MPUser> bll_user = new BLL.BaseBLL<Entity.MPUser>();
            var entity = bll_user.GetModel(p => p.ID == user_id, "ID ASC");
            if(entity == null)
            {
                result.msgbox = "用户不存在";
                return Json(result);
            }
            BLL.BaseBLL<Entity.MPUserAmountDetails> bll = new BLL.BaseBLL<Entity.MPUserAmountDetails>();
            int row_count = 0;            
            var db_list = bll.GetPagedList(page_index, page_size, ref row_count, p => p.MPUserID == user_id, "AddTime DESC");
            result.msg = 1;
            result.msgbox = entity.AccountBalance.ToString("F2");
            result.data = db_list;
            result.total = row_count;
            return Json(result);
        }

    }
}