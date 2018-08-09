using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 数字展示-创意视觉
    /// </summary>
    public class CaseShowController : BaseWebController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">Digital-Display/Creative-Vision</param>
        /// <param name="e">二级分类CallName</param>
        /// <returns></returns>
        public ActionResult Index(string t,string e)
        {
            if(string.IsNullOrWhiteSpace(t) || string.IsNullOrWhiteSpace(t)) return ErrorView("找不到相关页面");
            //大分类必须是数字展示或创意视觉
            if (t.ToLower() != "digital-display" && t.ToLower() != "creative-vision") return ErrorView("找不到相关页面");
            //设置导航标识
            WorkContext.CategoryMark = t;
            WorkContext.CategoryErMark = e;

            var result_model = BLL.BLLCaseShow.GetWebData(e);
            return View(result_model);
        }

        /// <summary>
        /// 分页加载更多案例
        /// </summary>
        /// <param name="page_index"></param>
        /// <param name="page_size"></param>
        /// <param name="category_id">分类ID</param>
        /// <param name="type">类别</param>
        /// <returns></returns>
        public JsonResult LoadCase(int page_index, int page_size,int category_id, int type)
        {
            UnifiedResultEntity<List<Entity.CaseShow>> result_model = new UnifiedResultEntity<List<Entity.CaseShow>>();
            if (type != 1 && type != 2)
            {
                result_model.msgbox = "非法类别";
                return Json(result_model);
            }
            Entity.CaseShowType ttt = (Entity.CaseShowType)type;
            BLL.BaseBLL<Entity.CaseShow> bll = new BLL.BaseBLL<Entity.CaseShow>();
            int total = 0;
            List<Entity.CaseShow> db_data = new List<Entity.CaseShow>();

            if(category_id > 0)
                bll.GetPagedList(page_index, page_size, ref total, p => p.Status && p.CategoryID == category_id && p.Type == ttt, "Weight DESC");
            else
                bll.GetPagedList(page_index, page_size, ref total, p => p.Status && p.Type == ttt, "Weight DESC");

            result_model.msg = 1;
            result_model.msgbox = "ok";
            result_model.data = db_data;
            result_model.total = total;
            return Json(result_model);
        }

        /// <summary>
        /// 案例详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return ErrorView("找不到相关案例");
            int iid = Tools.Base64.DecodeBase64IntID(id);
            if (iid == 0) return ErrorView("找不到相关案例0");
            
            BLL.BaseBLL<Entity.CaseShow> bll = new BLL.BaseBLL<Entity.CaseShow>();
            var result_model = bll.GetModel(p => p.ID == iid, "ID DESC");
            if(result_model == null) return ErrorView("找不到相关案例NULL");

            //详情页不属于任何分类
            WorkContext.CategoryMark = "";
            WorkContext.CategoryErMark = "";

            return View(result_model);
        }

    }
}