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
    /// 在线咨询结算申请
    /// </summary>
    public class ConsultationSettlementController : BaseAdminController
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role"></param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("在线咨询结算申请", "在线咨询结算申请")]
        public ActionResult Index(int page = 1, int role = 0, int role2 = 0, string word = "")
        {
            LoadType();

            word = WebHelper.UrlDecode(word);
            Models.ViewModelConsultationSettlementList response_model = new Models.ViewModelConsultationSettlementList();
            response_model.page = page;
            response_model.word = word;
            response_model.role = role;
            response_model.role2 = role2;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);
            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            if (role != 0)
            {
                filter.Add(new BLL.FilterSearch("Status", role.ToString(), BLL.FilterSearchContract.like));
            }

            if (role2 != 0)
            {
                filter.Add(new BLL.FilterSearch("PayStatus", role2.ToString(), BLL.FilterSearchContract.like));
            }

            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like));
            }


            BLL.BaseBLL<Entity.ConsultationSettlement> bll = new BLL.BaseBLL<Entity.ConsultationSettlement>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "AddTime desc", "MPDoctorInfo");
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        /// <summary>
        /// 详情-未完善
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminPermissionAttribute("在线咨询结算申请", "在线咨询结算申请详情")]
        public ActionResult Info(int id)
        {
            BLL.BaseBLL<Entity.ConsultationSettlement> bll = new BLL.BaseBLL<Entity.ConsultationSettlement>();
            var entity = bll.GetModel(p => p.ID == id, "ID ASC");
            if (entity == null)
            {
                return PromptView("/admin/ConsultationSettlement", "404", "Not Found", "信息不存在或已被删除", 5);
            }
            return View(entity);
        }


        private void LoadType()
        {
            List<SelectListItem> dataList1 = new List<SelectListItem>();
            dataList1.Add(new SelectListItem() { Text = "选择审核状态", Value = "0" });
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.ConsultationSettlementStatus)))
            {
                string text = EnumHelper.GetDescription((Entity.ConsultationSettlementStatus)item.Key);
                dataList1.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["StatusList"] = dataList1;

            List<SelectListItem> dataList2 = new List<SelectListItem>();
            dataList2.Add(new SelectListItem() { Text = "选择打款状态", Value = "0" });
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.ConsultationSettlementPayStatus)))
            {
                string text = EnumHelper.GetDescription((Entity.ConsultationSettlementPayStatus)item.Key);
                dataList2.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["PayStatusList"] = dataList2;
        }

    }
}