using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 节点分类
    /// </summary>
    [BasicAdminAuth]
    public class NodeCategoryController : BaseHBLController
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
            BLL.BaseBLL<Entity.NodeCategory> bll = new BLL.BaseBLL<Entity.NodeCategory>();
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (!string.IsNullOrWhiteSpace(keyword))
                filter.Add(new BLL.FilterSearch("Title", keyword, BLL.FilterSearchContract.like));
            List<Entity.NodeCategory> list = bll.GetPagedList(page_index, page_size, ref rowCount, filter, "ID ASC");
            WebAjaxEntity<List<Entity.NodeCategory>> result = new WebAjaxEntity<List<Entity.NodeCategory>>();
            result.msg = 1;
            result.msgbox = CalculatePage(rowCount, page_size).ToString();
            result.data = list;
            result.total = rowCount;
            return Json(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.NodeCategory> bll = new BLL.BaseBLL<Entity.NodeCategory>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);

        }
        
        public ActionResult Modify(int? id)
        {
            int ids = TypeHelper.ObjectToInt(id);
            Models.ViewModelNodeCategory model = new Models.ViewModelNodeCategory();
            if (ids != 0)
            {
                Entity.NodeCategory entity = new BLL.BaseBLL<Entity.NodeCategory>().GetModel(p => p.ID == ids);
                if(entity == null)
                {
                    model.Msg = 2;
                    model.MsgBox = "数据不存在";
                }else
                {
                    model.id = entity.ID;
                    model.title = entity.Title;
                    //model.remark = "";
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Modify(Models.ViewModelNodeCategory entity)
        {
            var isAdd = entity.id == 0 ? true : false;
            BLL.BaseBLL<Entity.NodeCategory> bll = new BLL.BaseBLL<Entity.NodeCategory>();
            if (!isAdd)
            {
                if (!bll.Exists(p => p.ID == entity.id))
                {
                    entity.Msg = 2;
                    ModelState.AddModelError("title", "信息不存在");
                }
            }
                        
            if (ModelState.IsValid)
            {
                Entity.NodeCategory model = null;

                if (isAdd)
                {
                    model = new Entity.NodeCategory();
                    model.Title = entity.title;
                    //model.Remark = entity.remark;
                    bll.Add(model);
                }
                else
                {
                    model = bll.GetModel(p => p.ID == entity.id);
                    model.Title = entity.title;
                    //model.Remark = entity.remark;
                    bll.Modify(model, "Title");
                }

                entity.Msg = 1;
            }
            else
            {
                entity.Msg = 3;
            }

            return View(entity);
        }

    }
}