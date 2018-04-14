using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 体检套餐订单列表
    /// </summary>
    public class OrderMedicalController : BaseAdminController
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role"></param>
        /// <param name="role2"></param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("体检套餐订单", "体检套餐订单首页")]
        public ActionResult Index(int page = 1, int role = 0, int role2=0,string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelOrderMedicalList response_model = new Models.ViewModelOrderMedicalList();
            response_model.page = page;
            response_model.word = word;
            response_model.role = role;
            response_model.role2 = role2;
            Load();
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);
            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (role != 0)
                filter.Add(new BLL.FilterSearch("Status", role.ToString(), BLL.FilterSearchContract.等于));
            else
                filter.Add(new BLL.FilterSearch("Status", "0", BLL.FilterSearchContract.不等于));//过滤掉临时订单
            if (role2 != 0)
                filter.Add(new BLL.FilterSearch("PayType", role2.ToString(), BLL.FilterSearchContract.等于));

            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("OrderNum", word, BLL.FilterSearchContract.like));
                filter.Add(new BLL.FilterSearch("RealName", word, BLL.FilterSearchContract.like, BLL.FilterRelate.Or));
                filter.Add(new BLL.FilterSearch("IDCardNumber", word, BLL.FilterSearchContract.like, BLL.FilterRelate.Or));
                filter.Add(new BLL.FilterSearch("Telphone", word, BLL.FilterSearchContract.like, BLL.FilterRelate.Or));
            }


            BLL.BaseBLL<Entity.OrderMedical> bll = new BLL.BaseBLL<Entity.OrderMedical>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "PayTime DESC,AddTime DESC");
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminPermissionAttribute("体检套餐订单", "订单详情")]
        public ActionResult Info(int id)
        {
            BLL.BaseBLL<Entity.OrderMedical> bll = new BLL.BaseBLL<Entity.OrderMedical>();
            var entity = bll.GetModel(p => p.ID == id, "ID DESC", "OrderMedicalItems");
            if(entity == null)
            {
                return PromptView("/admin/OrderMedical", "404", "Not Found", "信息不存在或已被删除", 5);
            }
            return View(entity);
        }


        /// <summary>
        /// 加载用户组
        /// </summary>
        private void Load()
        {
            List<SelectListItem> dataList1 = new List<SelectListItem>();
            dataList1.Add(new SelectListItem() { Text = "选择状态", Value = "0" });
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.OrderStatus)))
            {
                string text = EnumHelper.GetDescription((Entity.OrderStatus)item.Key);
                if (item.Key == 0) continue;
                dataList1.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["StatusList"] = dataList1;

            List<SelectListItem> dataList2 = new List<SelectListItem>();
            dataList2.Add(new SelectListItem() { Text = "支付类别", Value = "0" });
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.OrderPayType)))
            {
                string text = EnumHelper.GetDescription((Entity.OrderPayType)item.Key);
                dataList2.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["PayTypeList"] = dataList2;
        }
    }
}