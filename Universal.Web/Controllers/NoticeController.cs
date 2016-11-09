using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using System.Data.Entity;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 秘籍
    /// </summary>
    [BasicAdminAuth]
    public class NoticeController : BaseHBLController
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
        public JsonResult NoticeData(int page_size, int page_index, string keyword)
        {
            BLL.BaseBLL<Entity.CusNotice> bll = new BLL.BaseBLL<Entity.CusNotice>();
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (!string.IsNullOrWhiteSpace(keyword))
                filter.Add(new BLL.FilterSearch("Title", keyword, BLL.FilterSearchContract.like));
            List<Entity.CusNotice> list = bll.GetPagedList(page_index, page_size, ref rowCount, filter, "AddTime desc", p => p.CusUser);
            WebAjaxEntity<List<Entity.CusNotice>> result = new WebAjaxEntity<List<Entity.CusNotice>>();
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
        public JsonResult DelNotice(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.CusNotice> bll = new BLL.BaseBLL<Entity.CusNotice>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);

        }

        public ActionResult Modify(int? id)
        {
            int ids = TypeHelper.ObjectToInt(id);
            Models.ViewModelNotice model = new Models.ViewModelNotice();
            if (ids != 0)
            {
                string id_str = "";
                model.user_list = LoadNoticeUser(ids, out id_str);
                model.user_id_str = id_str;
                BLL.BaseBLL<Entity.CusNotice> bll = new BLL.BaseBLL<Entity.CusNotice>();
                Entity.CusNotice entity = bll.GetModel(p => p.ID == id);
                if (entity != null)
                {
                    model.content = entity.Content;
                    model.title = entity.Title;
                }
                else
                {
                    model.Msg = 2;
                    model.MsgBox = "数据不存在";
                }

            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Modify(Models.ViewModelNotice entity)
        {
            var isAdd = entity.id == 0 ? true : false;
            

            BLL.BaseBLL<Entity.CusNotice> bll = new BLL.BaseBLL<Entity.CusNotice>();
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

                Entity.CusNotice model = null;

                if (isAdd)
                {
                    model = new Entity.CusNotice();
                    model.CusUserID = WorkContext.UserInfo.ID;
                }
                else
                    model = bll.GetModel(p => p.ID == entity.id);

                model.Content = entity.content;
                model.Title = entity.title;
                if (isAdd)
                    BLL.BLLCusNotice.Add(model, entity.user_id_str);
                else
                    BLL.BLLCusNotice.Edit(model, entity.user_id_str);

                entity.Msg = 1;
            }

            return View(entity);
        }
        

        /// <summary>
        /// 加载通知用户数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="id_str"></param>
        /// <returns></returns>
        private List<Models.ViewModelNoticeUser> LoadNoticeUser(int id,out string id_str)
        {
            System.Text.StringBuilder user_str = new System.Text.StringBuilder();
            List<Models.ViewModelNoticeUser> result = new List<Models.ViewModelNoticeUser>();
            BLL.BaseBLL<Entity.CusNoticeUser> bll = new BLL.BaseBLL<Entity.CusNoticeUser>();
            List<Entity.CusNoticeUser> list = bll.GetListBy(0, p => p.CusNoticeID == id, "ID asc", p => p.CusUser);
            foreach (var item in list)
            {
                user_str.Append(item.ID + ",");
                Models.ViewModelNoticeUser model = new Models.ViewModelNoticeUser();
                model.id = item.ID;
                if(item.CusUser != null)
                {
                    model.nick_name = item.CusUser.NickName;
                    model.telphone = item.CusUser.Telphone;
                }else
                {
                    model.nick_name = "用户不存在";
                    model.telphone = "";
                }
                result.Add(model);
            }
            if(user_str.Length > 0)
            {
                user_str = user_str.Remove(user_str.Length - 1, 1);
            }
            id_str = user_str.ToString();
            return result;
        }

    }
}