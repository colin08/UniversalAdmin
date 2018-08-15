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
            int page_size = 6;
            var db_data = BLL.BLLContact.GetWebJobPageList(page_size, page_index);
            result_model.msg = 1;
            result_model.msgbox = "ok";
            result_model.data = db_data;
            return Json(result_model,JsonRequestBehavior.AllowGet);
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
        /// <param name="type">类别</param>
        /// <returns></returns>
        public JsonResult LoadNews(int page_index,int type)
        {
            UnifiedResultEntity<List<Entity.News>> result_model = new UnifiedResultEntity<List<Entity.News>>();
            if(type != 1&& type != 2)
            {
                result_model.msgbox = "非法类别";
                return Json(result_model);
            }
            int page_size = 6;
            Entity.NewsType ttt = (Entity.NewsType)type;
            //BLL.BaseBLL<Entity.News> bll = new BLL.BaseBLL<Entity.News>();
            //int total = 0;
            var db_data = BLL.BLLContact.GetWebNewsPageList(page_size, page_index, ttt);// bll.GetPagedList(page_index, page_size, ref total, p => p.Status && p.Type == ttt, "Weight DESC");
            result_model.msg = 1;
            result_model.msgbox = "ok";
            result_model.data = db_data;
            result_model.total = 0;
            return Json(result_model,JsonRequestBehavior.AllowGet);
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
        public ActionResult Search(string word)
        {
            ViewData["SearchKeyWord"] = word;
            
            return View();
        }

        /// <summary>
        /// Ajax加载搜索数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public JsonResult LoadSearch(int page, int type, string word)
        {
            int page_size = 6;
            if (type != 1 && type != 2)
            {
                WorkContext.AjaxStringEntity.msgbox = "不明确的分类";
                return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrWhiteSpace(word))
            {
                WorkContext.AjaxStringEntity.msgbox = "请输入关键字";
                return Json(WorkContext.AjaxStringEntity, JsonRequestBehavior.AllowGet);
            }
            int total = 0;
            if (type == 1)
            {
                //案例
                UnifiedResultEntity<List<Models.SearchCase>> result = new UnifiedResultEntity<List<Models.SearchCase>>();
                BLL.BaseBLL<Entity.CaseShow> bll = new BLL.BaseBLL<Entity.CaseShow>();
                var db_data = bll.GetPagedList(page, page_size, ref total, p => p.Status && p.Title.Contains(word), "Weight DESC").ToList();
                List<Models.SearchCase> result_list = new List<Models.SearchCase>();
                foreach (var item in db_data)
                {
                    string open_url = "/CaseShow/Detail?id=" + item.GetBase64ID;
                    result_list.Add(new Models.SearchCase(item.ImgUrl, open_url, item.Title, item.Time, item.Address));
                }
                result.msg = 1;
                result.msgbox = "ok";
                result.data = result_list;
                result.total = total;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //新闻
                UnifiedResultEntity<List<Models.SearchNews>> result = new UnifiedResultEntity<List<Models.SearchNews>>();
                BLL.BaseBLL<Entity.News> bll = new BLL.BaseBLL<Entity.News>();
                var db_data = bll.GetPagedList(page, page_size, ref total, p => p.Status && p.Title.Contains(word), "Weight DESC").ToList();
                List<Models.SearchNews> result_list = new List<Models.SearchNews>();
                foreach (var item in db_data)
                {
                    string open_url = "/Contact/NewsDetail?id=" + item.GetBase64ID;
                    result_list.Add(new Models.SearchNews(item.ImgUrl, open_url, item.Title, item.Summary));
                }
                result.msg = 1;
                result.msgbox = "ok";
                result.data = result_list;
                result.total = total;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

    }
}