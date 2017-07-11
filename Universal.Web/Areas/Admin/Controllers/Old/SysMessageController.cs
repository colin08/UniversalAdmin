//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Universal.Tools;
//using Universal.Web.Framework;

//namespace Universal.Web.Areas.Admin.Controllers
//{
//    public class SysMessageController : BaseAdminController
//    {
//        /// <summary>
//        /// 消息列表
//        /// </summary>
//        /// <param name="page">当前第几页</param>
//        /// <param name="word">筛选：关键字</param>
//        /// <returns></returns>
//        [AdminPermissionAttribute("系统消息", "系统消息首页", true)]
//        public ActionResult Index(int page = 1, string word = "")
//        {
//            word = WebHelper.UrlDecode(word);
//            Models.ViewModelSysMessage response_model = new Models.ViewModelSysMessage();
//            response_model.page = page;
//            response_model.word = word;
//            //获取每页大小的Cookie
//            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie("sysmessageindex"), SiteKey.AdminDefaultPageSize);

//            int total = 0;
//            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
//            if (!string.IsNullOrWhiteSpace(word))
//            {
//                filter.Add(new BLL.FilterSearch("Content", word, BLL.FilterSearchContract.like));
//            }


//            BLL.BaseBLL<Entity.SysMessage> bll = new BLL.BaseBLL<Entity.SysMessage>();
//            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "IsRead ASC,AddTime desc");
//            response_model.DataList = list;
//            response_model.total = total;
//            response_model.total_page = CalculatePage(total, response_model.page_size);
//            return View(response_model);
//        }

//        /// <summary>
//        /// 删除消息
//        /// </summary>
//        [HttpPost]
//        public JsonResult Del(string ids)
//        {
//            if (string.IsNullOrWhiteSpace(ids))
//            {
//                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
//                return Json(WorkContext.AjaxStringEntity);
//            }
//            BLL.BaseBLL<Entity.SysMessage> bll = new BLL.BaseBLL<Entity.SysMessage>();
//            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
//            bll.DelBy(p => id_list.Contains(p.ID));
//            AddAdminLogs(Entity.SysLogMethodType.Delete, "删除系统消息：" + ids + "");

//            WorkContext.AjaxStringEntity.msg = 1;
//            WorkContext.AjaxStringEntity.msgbox = "success";
//            return Json(WorkContext.AjaxStringEntity);
//        }

//        /// <summary>
//        /// 设置消息已读
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        public JsonResult SetMsgRead(int id)
//        {
//            string msg = "";
//            WorkContext.AjaxStringEntity.msg = BLL.BLLSysMessage.SetMsgRead(id, out msg) ? 1 : 0;
//            WorkContext.AjaxStringEntity.msgbox = msg;
//            return Json(WorkContext.AjaxStringEntity);
//        }

//    }
//}