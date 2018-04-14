using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;
using Universal.Tools;
using Universal.Entity;
using System.Data;
using System.Data.Entity;

namespace Universal.Web.Areas.MP.Controllers
{
    /// <summary>
    /// 优惠体检套餐
    /// </summary>
    public class MedicalController : BaseMPController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            BLL.BaseBLL<Entity.MedicalBanner> bll_banner = new BLL.BaseBLL<MedicalBanner>();
            var banner_list = bll_banner.GetListBy(5, p => p.Status, "Weight DESC");
            ViewData["BannerList"] = banner_list;
            BLL.BaseBLL<Entity.Medical> bll_m = new BLL.BaseBLL<Medical>();
            var m_list = bll_m.GetListBy(10, p => p.Status == MedicalStatus.Up, "Weight DESC");
            return View(m_list);
        }

        /// <summary>
        /// 套餐详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Info(int mid)
        {
            if (mid <= 0) return PromptView("/MP/Medical/Index", "非法参数");
            BLL.BaseBLL<Entity.Medical> bll = new BLL.BaseBLL<Medical>();
            var entity = bll.GetModel(p => p.ID == mid, "ID DESC", "MedicalItems");
            if (entity == null) return PromptView("/MP/Medical/Index", "套餐不存在");
            if (entity.Status == MedicalStatus.Down) return PromptView("/MP/Medical/Index", "该套餐已下架");
            return View(entity);
        }

        /// <summary>
        /// 套餐购买
        /// </summary>
        /// <param name="mid">套餐ID</param>
        /// <param name="o">临时订单号</param>
        /// <returns></returns>
        public ActionResult GoBuy(int mid, string o,string back)
        {
            if (mid <= 0) return PromptView("/MP/Medical/Index", "非法参数");
            ViewData["MID"] = mid;
            BLL.BaseBLL<Entity.Medical> bll = new BLL.BaseBLL<Medical>();
            var entity = bll.GetModel(p => p.ID == mid, "ID DESC", "MedicalItems");
            if (entity == null) return PromptView("/MP/Medical/Index", "套餐不存在");
            if (entity.Status == MedicalStatus.Down) return PromptView("/MP/Medical/Index", "该套餐已下架");
            ViewData["Medical"] = entity;
            ViewData["HaveOrder"] = "0";
            //如果订单号不为空，则标识已经生成了一个临时订单
            if (!string.IsNullOrWhiteSpace(o))
            {
                ViewData["HaveOrder"] = "1";
                BLL.BaseBLL<Entity.OrderMedical> bll_order = new BLL.BaseBLL<OrderMedical>();
                var entity_order = bll_order.GetModel(p => p.OrderNum == o, "ID ASC", "OrderMedicalItems");
                if (entity_order == null) return PromptView("/MP/Medical/Info?mid=" + mid, "订单不存在");
                if (entity_order.MPUserID != WorkContext.UserInfo.ID) return PromptView("/MP/Medical/Info?mid=" + mid, "该订单不属于您");
                if (entity_order.Status != OrderStatus.临时订单) return PromptView("/MP/Medical/Info?mid=" + mid, "订单状态错误");
                ViewData["TempOrder"] = entity_order;
                o = entity_order.OrderNum;
            }
            ViewData["OrderNum"] = o;
            ViewData["GoSelectItem"] = "/MP/Medical/SelectItem?mid=" + mid + "&o=" + o;
            ViewData["NextUrl"] = "/MP/Medical/AddMInfo?mid=" + mid.ToString() + "&o=" + o;
            if(!string.IsNullOrWhiteSpace(back))
            {
                ViewData["BackUrl"] = back;
            }
            else
            {
                ViewData["BackUrl"] = "info?mid=" + mid.ToString();
            }
            return View();
        }

        /// <summary>
        /// 订单加项
        /// </summary>
        /// <param name="mid">套餐ID</param>
        /// <param name="o"></param>
        /// <returns></returns>
        public ActionResult SelectItem(int mid, string o)
        {
            if (mid <= 0) return PromptView("/MP/Medical/Index", "非法参数");
            BLL.BaseBLL<Entity.Medical> bll = new BLL.BaseBLL<Medical>();
            var entity = bll.GetModel(p => p.ID == mid, "ID DESC");
            if (entity == null) return PromptView("/MP/Medical/Index", "套餐不存在");
            if (entity.Status == MedicalStatus.Down) return PromptView("/MP/Medical/Index", "该套餐已下架");
            ViewData["MID"] = mid;
            ViewData["Medical"] = entity;

            if (string.IsNullOrWhiteSpace(o))
            {
                //添加订单
                string msg = "";
                var order_num = BLL.BLLOrderMedical.AddTempOrder(mid, WorkContext.UserInfo.ID, out msg);
                if (!msg.Equals("ok"))
                {
                    return PromptView("/MP/Medical/GoBuy?mid=" + mid.ToString(), msg);
                }
                o = order_num;
            }
            BLL.BaseBLL<OrderMedical> bll_order = new BLL.BaseBLL<OrderMedical>();
            var model = bll_order.GetModel(p => p.OrderNum == o, "ID DESC", "OrderMedicalItems");
            if (model == null) return PromptView("/MP/Medical/GoBuy?mid=" + mid.ToString(), "新增临时订单不存在");
            if (model.MedicalID != entity.ID) return PromptView("/MP/Medical/GoBuy?mid=" + mid.ToString(), "该套餐不属于该订单");
            if (model.Status != OrderStatus.临时订单) return PromptView("/MP/Medical/GoBuy?mid=" + mid.ToString(), "该订单已不能再加项");
            ViewData["BackUrl"] = "/MP/Medical/GoBuy?mid=" + mid.ToString() + "&o=" + o;

            LoadAllSelectItem(model);
            return View(model);
        }

        /// <summary>
        /// 填写套餐预约信息
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public ActionResult AddMInfo(int mid, string o)
        {
            if (mid <= 0) return PromptView("/MP/Medical/Index", "非法参数");
            BLL.BaseBLL<Entity.Medical> bll = new BLL.BaseBLL<Medical>();
            var entity = bll.GetModel(p => p.ID == mid, "ID DESC");
            if (entity == null) return PromptView("/MP/Medical/Index", "套餐不存在");
            if (entity.Status == MedicalStatus.Down) return PromptView("/MP/Medical/Index", "该套餐已下架");
            ViewData["MID"] = mid;
            ViewData["Medical"] = entity;

            if (string.IsNullOrWhiteSpace(o))
            {
                //添加订单
                string msg = "";
                var order_num = BLL.BLLOrderMedical.AddTempOrder(mid, WorkContext.UserInfo.ID, out msg);
                if (!msg.Equals("ok"))
                {
                    return PromptView("/MP/Medical/GoBuy?mid=" + mid.ToString(), msg);
                }
                o = order_num;
            }
            BLL.BaseBLL<OrderMedical> bll_order = new BLL.BaseBLL<OrderMedical>();
            var model = bll_order.GetModel(p => p.OrderNum == o, "ID DESC");
            if (model == null) return PromptView("/MP/Medical/GoBuy?mid=" + mid.ToString(), "新增临时订单不存在");
            if (model.MedicalID!= entity.ID) return PromptView("/MP/Medical/GoBuy?mid=" + mid.ToString(), "该套餐不属于该订单");
            if (model.Status != OrderStatus.临时订单) return PromptView("/MP/Medical/GoBuy?mid=" + mid.ToString(), "该订单已不能再加项");
            ViewData["BackUrl"] = "/MP/Medical/GoBuy?mid=" + mid.ToString() + "&o=" + o;
            ViewData["NextUrl"] = "/MP/Pay/OrderMedical?o=" + o;
            ViewData["OrderNum"] = o;
            Universal.Web.Areas.MP.Models.OrderMedicalInfo result_model = new Models.OrderMedicalInfo();
            if (!string.IsNullOrWhiteSpace(model.RealName))
            {
                //订单里有用户数据
                result_model.RealName = model.RealName;
                result_model.IDNumber = model.IDCardNumber;
                if (model.Gender != MPUserGenderType.unknown) result_model.Gender = (int)model.Gender;
                result_model.Telphone = model.Telphone;
                result_model.YuYueDate = model.YuYueDate;
                result_model.Brithday = model.Brithday;
                result_model.YuYueStr = model.YuYueDate.ToString("yyyy-MM-dd");
                if (model.YuYueDate.Hour < 13) result_model.YuYueStr += " 上午";
                else result_model.YuYueStr += " 下午";
            }
            else
            {
                //否则用自己账户里资料
                result_model.RealName = WorkContext.UserInfo.RealName;
                result_model.IDNumber = WorkContext.UserInfo.IDCardNumber;
                result_model.Telphone = WorkContext.UserInfo.Telphone;
                if (WorkContext.UserInfo.Gender != MPUserGenderType.unknown) result_model.Gender = (int)WorkContext.UserInfo.Gender;
                result_model.YuYueDate = DateTime.Now.AddDays(1);
                result_model.Brithday = WorkContext.UserInfo.Brithday;
                result_model.YuYueStr = result_model.YuYueDate.ToString("yyyy-MM-dd") + " 上午";
            }
            return View(result_model);
        }


        /// <summary>
        /// 我的订单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderList()
        {
            int u_id = WorkContext.UserInfo.ID;
            BLL.BaseBLL<Entity.OrderMedical> bll = new BLL.BaseBLL<OrderMedical>();
            List<Universal.Entity.OrderMedical> db_list =  bll.GetListBy(0, p => p.MPUserID == u_id, "AddTime DESC");
            return View(db_list);
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public ActionResult OrderInfo(string o)
        {
            BLL.BaseBLL<Entity.OrderMedical> bll = new BLL.BaseBLL<OrderMedical>();
            Universal.Entity.OrderMedical model = bll.GetModel(p => p.OrderNum == o, "ID ASC", "OrderMedicalItems");
            if(model == null) return PromptView("/MP/Medical/OrderList", "订单不存在");
            if (model.MPUserID != WorkContext.UserInfo.ID) return PromptView("/MP/Medical/OrderList", "该订单不属于您");
            var yuyue_str = model.YuYueDate.ToString("yyyy-MM-dd");
            if (model.YuYueDate.Hour < 13) yuyue_str += " 上午";
            else yuyue_str += " 下午";
            ViewData["YuYueStr"] = yuyue_str;
            if (model.Gender == MPUserGenderType.male) ViewData["Gender"] = "男";
            else if (model.Gender == MPUserGenderType.female) ViewData["Gender"] = "女";
            else ViewData["Gender"] = "未知";
            return View(model);
        }


        /// <summary>
        /// 修改订单的预约用户信息
        /// </summary>
        /// <param name="o"></param>
        /// <param name="realname"></param>
        /// <param name="idnumber"></param>
        /// <param name="telphone"></param>
        /// <param name="gender"></param>
        /// <param name="bri"></param>
        /// <param name="yuyue"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyOrderUserInfo(string o,string realname, string idnumber,string telphone, int gender,string bri,string yuyue)
        {
            #region 数据验证

            if (realname.Length > 10 || realname.Length == 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "姓名长度在0~10之间";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (idnumber.Length > 18 || idnumber.Length == 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "身份证号码长度在0~18之间";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (!Tools.ValidateHelper.IsMobile(telphone))
            {
                WorkContext.AjaxStringEntity.msgbox = "手机号码格式不正确";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (gender != 1 && gender != 2)
            {
                WorkContext.AjaxStringEntity.msgbox = "性别数据非法";
                return Json(WorkContext.AjaxStringEntity);
            }
            var brithday = Tools.TypeHelper.ObjectToDateTime(bri, DateTime.Now);

            var yuyue_date = TypeHelper.ObjectToDateTime(yuyue.Replace("上午", "10:00").Replace("下午", "15:00"));

            #endregion
            BLL.BaseBLL<Entity.OrderMedical> bll = new BLL.BaseBLL<OrderMedical>();
            var model = bll.GetModel(p => p.OrderNum == o, "ID DESC");
            if(model == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "订单不存在";
                return Json(WorkContext.AjaxStringEntity);
            }
            model.RealName = realname;
            model.Telphone = telphone;
            model.IDCardNumber = idnumber;
            model.Gender = (MPUserGenderType)gender;
            model.YuYueDate = yuyue_date;
            model.Brithday = brithday;
            model.Status = OrderStatus.等待支付;
            bll.Modify(model, "RealName", "Telphone", "IDCardNumber", "Gender", "YuYueDate", "Brithday", "Status");
            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 修改订单增加项
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifySelectItem(string o, string ids)
        {
            if (string.IsNullOrWhiteSpace(o))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法订单参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<OrderMedical> bll_order = new BLL.BaseBLL<OrderMedical>();
            var model = bll_order.GetModel(p => p.OrderNum == o, "ID DESC", "OrderMedicalItems");
            if (model == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "订单不存在";
                return Json(WorkContext.AjaxStringEntity);
            }

            string msg = "";
            var status = BLL.BLLOrderMedical.ModifyOrderItem(o, ids, out msg);
            if (status)
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "OK";
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
            }
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 加载所有的选择项
        /// </summary>
        private void LoadAllSelectItem(Entity.OrderMedical entity_order)
        {
            List<string> SZMList = new List<string>();
            var db_list = BLL.BLLMedical.LoadAllSelectList(out SZMList);
            var order_medical_item = entity_order.OrderMedicalItems.ToList();
            var SelectItem = new List<Universal.Web.Areas.MP.Models.MedicalSelectItem>();
            foreach (var item in db_list)
            {
                Models.MedicalSelectItem model = new Models.MedicalSelectItem();

                var db_mecal_id = item.ID;
                var mach_entity = order_medical_item.Find(p => p.MedicalID == db_mecal_id);
                if (mach_entity != null) model.type = (int)mach_entity.Type;
                else model.type = 0;
                model.MedicalID = item.ID;
                model.OnlyID = item.OnlyID;
                model.SZM = item.SZM;
                model.Title = item.Title;
                model.Weight = item.Weight;
                SelectItem.Add(model);
            }
            ViewData["DBSelectItem"] = SelectItem;
            ViewData["SZMList"] = SZMList;
        }


    }
}