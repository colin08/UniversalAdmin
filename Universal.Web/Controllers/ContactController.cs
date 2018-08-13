using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 联系我们
    /// </summary>
    public class ContactController : BaseWebController
    {
        /// <summary>
        /// 加入我们
        /// </summary>
        /// <returns></returns>
        public ActionResult Join()
        {
            WorkContext.CategoryMark = "ContactUS";
            WorkContext.CategoryErMark = "Join-US";

            var result_model = BLL.BLLContact.GetJobList();
            return View(result_model);
        }

        /// <summary>
        /// 职位详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Job(string id)
        {
            WorkContext.CategoryMark = "ContactUS";
            WorkContext.CategoryErMark = "Join-US";

            if (string.IsNullOrWhiteSpace(id)) return ErrorView("找不到相关职位");
            int iid = Tools.Base64.DecodeBase64IntID(id);
            if (iid == 0) return ErrorView("找不到相关职位0");

            BLL.BaseBLL<Entity.JoinUS> bll = new BLL.BaseBLL<Entity.JoinUS>();
            var result_model = bll.GetModel(p => p.ID == iid, "ID DESC");
            if (result_model == null) return ErrorView("找不到相关职位NULL");
            
            return View(result_model);
        }

        /// <summary>
        /// 分页加载更多职位
        /// </summary>
        /// <param name="page_index"></param>
        /// <returns></returns>
        public JsonResult LoadJob(int page_index)
        {
            UnifiedResultEntity<List<Entity.JoinUS>> result_model = new UnifiedResultEntity<List<Entity.JoinUS>>();
            BLL.BaseBLL<Entity.JoinUS> bll = new BLL.BaseBLL<Entity.JoinUS>();
            int total = 0;
            var db_data = bll.GetPagedList(page_index, 6, ref total, p => p.Status, "Weight DESC");
            result_model.msg = 1;
            result_model.msgbox = "ok";
            result_model.data = db_data;
            result_model.total = total;
            return Json(result_model);
        }


        /// <summary>
        /// 最新资讯
        /// </summary>
        /// <returns></returns>
        public ActionResult News()
        {
            WorkContext.CategoryMark = "ContactUS";
            WorkContext.CategoryErMark = "News";

            var result_model = BLL.BLLContact.GetNewsListData();
            return View(result_model);

        }

        /// <summary>
        /// 分页加载更多资讯
        /// </summary>
        /// <param name="page_index"></param>
        /// <param name="page_size"></param>
        /// <param name="type">类别</param>
        /// <returns></returns>
        public JsonResult LoadNews(int page_index,int page_size,int type)
        {
            UnifiedResultEntity<List<Entity.News>> result_model = new UnifiedResultEntity<List<Entity.News>>();
            if(type != 1&& type != 2)
            {
                result_model.msgbox = "非法类别";
                return Json(result_model);
            }
            Entity.NewsType ttt = (Entity.NewsType)type;
            BLL.BaseBLL<Entity.News> bll = new BLL.BaseBLL<Entity.News>();
            int total = 0;
            var db_data = bll.GetPagedList(page_index, page_size, ref total, p => p.Status && p.Type == ttt, "Weight DESC");
            result_model.msg = 1;
            result_model.msgbox = "ok";
            result_model.data = db_data;
            result_model.total = total;
            return Json(result_model);
        }


        /// <summary>
        /// 资讯详情
        /// </summary>
        /// <returns></returns>
        public ActionResult NewsDetail(string id)
        {
            WorkContext.CategoryMark = "ContactUS";
            WorkContext.CategoryErMark = "News";

            if (string.IsNullOrWhiteSpace(id)) return ErrorView("找不到相关新闻");
            int iid = Tools.Base64.DecodeBase64IntID(id);
            if (iid == 0) return ErrorView("找不到相关新闻0");

            BLL.BaseBLL<Entity.News> bll = new BLL.BaseBLL<Entity.News>();
            var result_model = bll.GetModel(p => p.ID == iid, "ID DESC");
            if (result_model == null) return ErrorView("找不到相关新闻NULL");

            return View(result_model);
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult Search(string word,int page=1)
        {
            ViewData["SearchKeyWord"] = word;

            return View();
        }

    }
}