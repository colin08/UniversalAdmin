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
        public ActionResult Index(string t, string e)
        {
            if (string.IsNullOrWhiteSpace(t) || string.IsNullOrWhiteSpace(t)) return ErrorView("找不到相关页面");
            //大分类必须是数字展示或创意视觉
            if (t.ToLower() != "digital-display" && t.ToLower() != "creative-vision") return ErrorView("找不到相关页面");
            //设置导航标识
            WorkContext.CategoryMark = t.Replace("-", "");
            WorkContext.CategoryErMark = e;

            var result_model = BLL.BLLCaseShow.GetWebData(e);
            return View(result_model);
        }


        /// <summary>
        /// 分页加载更多案例
        /// </summary>
        /// <param name="page_index"></param>
        /// <param name="category_id">分类ID</param>
        /// <param name="type">类别</param>
        /// <returns></returns>
        public JsonResult LoadCase(int page_index, int category_id, int type)
        {
            int page_size = 6;
            UnifiedResultEntity<List<Models.SearchCase>> result_model = new UnifiedResultEntity<List<Models.SearchCase>>();
            if (type != 1 && type != 2)
            {
                result_model.msgbox = "非法类别";
                return Json(result_model, JsonRequestBehavior.AllowGet);
            }
            Entity.CaseShowType ttt = (Entity.CaseShowType)type;
            switch (ttt)
            {
                case Entity.CaseShowType.New:
                    page_size = 3;
                    break;
                case Entity.CaseShowType.Classic:
                    page_size = 6;
                    break;
                default:
                    break;
            }
            var db_data = BLL.BLLCaseShow.GetWebPageList(page_size, page_index, category_id, ttt);
            List<Models.SearchCase> result_list = new List<Models.SearchCase>();
            foreach (var item in db_data)
            {
                string open_url = "/CaseShow/Detail?id=" + item.GetBase64ID;
                result_list.Add(new Models.SearchCase(item.ImgUrl, open_url, item.Title, item.Time, item.Address));
            }
            result_model.msg = 1;
            result_model.msgbox = "ok";
            result_model.data = result_list;
            return Json(result_model, JsonRequestBehavior.AllowGet);
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
            if (result_model == null) return ErrorView("找不到相关案例NULL");

            //详情页不属于任何分类
            WorkContext.CategoryMark = "";
            WorkContext.CategoryErMark = "";

            return View(result_model);
        }

        /// <summary>
        /// 合作企业的案例列表
        /// </summary>
        /// <returns></returns>
        public ActionResult TeamWork(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return ErrorView("找不到相关合作企业");
            int iid = Tools.Base64.DecodeBase64IntID(id);
            if (iid == 0) return ErrorView("找不到相关合作企业0");

            WorkContext.CategoryMark = "";
            WorkContext.CategoryErMark = "";

            var result_model = BLL.BLLCaseShow.GetTeamWorkCaseList(iid);
            if (result_model == null) return ErrorView("找不到相关合作企业NULL");
            return View(result_model);
        }

        /// <summary>
        /// 分页加载更多企业案例
        /// </summary>
        /// <param name="page_index"></param>
        /// <param name="id">合作企业ID</param>
        /// <returns></returns>
        public JsonResult LoadTeamWordCase(int page_index, int id)
        {
            int page_size = 6;
            UnifiedResultEntity<List<Models.SearchCase>> result_model = new UnifiedResultEntity<List<Models.SearchCase>>();
            var db_data = BLL.BLLCaseShow.GetWebTeamWordPageList(page_size, page_index, id);
            List<Models.SearchCase> result_list = new List<Models.SearchCase>();
            foreach (var item in db_data)
            {
                string open_url = "/CaseShow/Detail?id=" + item.GetBase64ID;
                result_list.Add(new Models.SearchCase(item.ImgUrl, open_url, item.Title, item.Time, item.Address));
            }
            result_model.msg = 1;
            result_model.msgbox = "ok";
            result_model.data = result_list;
            return Json(result_model, JsonRequestBehavior.AllowGet);
        }


    }
}