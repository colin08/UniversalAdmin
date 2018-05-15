using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 医学通识
    /// </summary>
    public class NewsController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Models.NewsIndex result = new Models.NewsIndex();
            BLL.BaseBLL<Entity.NewsBanner> bll_banner = new BLL.BaseBLL<Entity.NewsBanner>();
            var banner_list=  bll_banner.GetListBy(0, p => p.Status, "Weight DESC");
            result.banner_list = banner_list;
            var db_category_list = BLL.BLLNewsCategory.GetList();
            foreach (var item in db_category_list)
            {
                Models.NewsCategory model = new Models.NewsCategory();
                model.category_id = item.ID;
                model.category_title = item.Title;
                if(item.NewsTags != null)
                {
                    foreach (var item2 in item.NewsTags.OrderByDescending(p=>p.Weight).ToList())
                    {
                        Models.NewsTag tag = new Models.NewsTag();
                        tag.tag_id = item2.ID;
                        tag.tag_title = item2.Title;
                        model.tag_list.Add(tag);
                    }
                }
                result.category_list.Add(model);
            }
            return View(result);
        }

        /// <summary>
        /// Json加载新闻
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoadNews(int page,int size,int cid,int tid)
        {
            Framework.UnifiedResultEntity<List<Entity.ViewModel.News>> resultEntity = new Framework.UnifiedResultEntity<List<Entity.ViewModel.News>>();
            int total = 0;
            var db_list = BLL.BLLNews.GetPageList(size, page, cid, tid, out total);
            resultEntity.msg = 1;
            resultEntity.msgbox = "ok";
            resultEntity.total = total;
            resultEntity.data = db_list;
            return Json(resultEntity);
        }

        /// <summary>
        /// 新闻详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            BLL.BaseBLL<Entity.News> bll = new BLL.BaseBLL<Entity.News>();
            var entity = bll.GetModel(p => p.ID == id, "ID ASC");
            if (entity == null) return Content("文字不存在");
            if (!entity.Status) return Content("文章已下架");

            return View(entity);
        }

    }
}