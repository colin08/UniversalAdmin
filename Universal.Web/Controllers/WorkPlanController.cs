using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    [BasicUserAuth]
    public class WorkPlanController : BaseHBLController
    {

        public ActionResult Index()
        {
            BLL.BaseBLL<Entity.CusDepartmentAdmin> bll = new BLL.BaseBLL<Entity.CusDepartmentAdmin>();
            bool isAdmin = bll.Exists(p => p.CusUserID == WorkContext.UserInfo.ID);
            ViewData["IsDepartmentAdmin"] = isAdmin;
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
            BLL.BaseBLL<Entity.WorkPlan> bll = new BLL.BaseBLL<Entity.WorkPlan>();
            int rowCount = 0;
            List<Entity.WorkPlan> list = bll.GetPagedList(page_index, page_size, ref rowCount, p => p.CusUserID == WorkContext.UserInfo.ID, "AddTime desc");
            WebAjaxEntity<List<Entity.WorkPlan>> result = new WebAjaxEntity<List<Entity.WorkPlan>>();
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
            BLL.BaseBLL<Entity.WorkPlan> bll = new BLL.BaseBLL<Entity.WorkPlan>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);

        }

        /// <summary>
        /// 计划审批
        /// </summary>
        /// <returns></returns>
        public ActionResult Approve()
        {
            return View();
        }

        /// <summary>
        /// 审核工作计划数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApprovePageData(int page_size, int page_index, string keyword)
        {
            int rowCount = 0;
            List<Entity.WorkPlan> list = BLL.BLLWorkPlan.GetAdminApprovePageData(page_index, page_size, ref rowCount, WorkContext.UserInfo.ID);
            //foreach (var item in list)
            //    item.ApproveNickName = BLL.BLLCusUser.GetUserDepartmentAdminText(item.CusUserID);
            WebAjaxEntity<List<Entity.WorkPlan>> result = new WebAjaxEntity<List<Entity.WorkPlan>>();
            result.msg = 1;
            result.msgbox = CalculatePage(rowCount, page_size).ToString();
            result.data = list;
            result.total = rowCount;

            return Json(result);
        }

        /// <summary>
        /// 计划审批
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DoApprove(int id)
        {
            BLL.BaseBLL<Entity.WorkPlan> bll = new BLL.BaseBLL<Entity.WorkPlan>();
            var entity = bll.GetModel(p => p.ID == id);
            if (entity == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "要审批的计划不存在";
                return Json(WorkContext.AjaxStringEntity);
            }

            if (entity.IsApprove)
            {
                WorkContext.AjaxStringEntity.msgbox = "已审批";
                return Json(WorkContext.AjaxStringEntity);
            }

            entity.IsApprove = true;
            entity.ApproveTime = DateTime.Now;
            if (bll.Modify(entity, "IsApprove", "ApproveTime") > 0)
            {
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "审批成功";
                return Json(WorkContext.AjaxStringEntity);
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = "审批失败";
                return Json(WorkContext.AjaxStringEntity);
            }

        }


        public ActionResult Modify(int? id)
        {
            int num = TypeHelper.ObjectToInt(id, 0);
            Models.ViewModelWorkPlan entity = new Models.ViewModelWorkPlan();
            LoadStatus();
            if (num != 0)
            {
                Entity.WorkPlan model = BLL.BLLWorkPlan.GetModel(num);
                if (entity == null)
                {
                    entity.Msg = 2;
                }
                else
                {
                    if (model.IsApprove)
                    {
                        //审批过的不能再编辑
                        entity.Msg = 4;
                    }
                    else
                    {
                        entity.approve_status = model.IsApprove;
                        //LoadPlatform(model.WeekText);
                        entity.id = model.ID;
                        entity.week_text = model.WeekText;
                        DateTime dt = TypeHelper.ObjectToDateTime(model.BeginTime);
                        entity.year = dt.Year.ToString();
                        entity.month = dt.Month.ToString();
                        entity.day = dt.Day.ToString();

                        DateTime dt2 = TypeHelper.ObjectToDateTime(model.EndTime);
                        entity.year2 = dt2.Year.ToString();
                        entity.month2 = dt2.Month.ToString();
                        entity.day2 = dt2.Day.ToString();

                        entity.approve_user_id = model.ApproveUserID;
                        entity.approve_user_name = model.ApproveUser.NickName;
                        var item_list = model.WorkPlanItemList.ToList();
                        if (item_list.Count > 0)
                        {
                            entity.plan_item.Clear();
                            entity.plan_item = item_list;
                        }

                    }
                }

            }
            return View(entity);
        }


        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Modify(Models.ViewModelWorkPlan entity)
        {
            var isAdd = entity.id == 0 ? true : false;
            LoadStatus();

            BLL.BaseBLL<Entity.WorkPlan> bll = new BLL.BaseBLL<Entity.WorkPlan>();
            if (!isAdd)
            {
                if (!bll.Exists(p => p.ID == entity.id))
                {
                    entity.Msg = 2;
                    ModelState.AddModelError("week_text", "信息不存在");
                }
                if (bll.GetModel(p => p.ID == entity.id).IsApprove)
                {
                    //审批过的不能再编辑
                    entity.Msg = 4;
                }
            }

            DateTime BeginTime = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(entity.year) && !string.IsNullOrWhiteSpace(entity.month) && !string.IsNullOrWhiteSpace(entity.day))
                BeginTime = TypeHelper.ObjectToDateTime(entity.year + "/" + entity.month + "/" + entity.day);

            DateTime EndTime = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(entity.year2) && !string.IsNullOrWhiteSpace(entity.month2) && !string.IsNullOrWhiteSpace(entity.day2))
                EndTime = TypeHelper.ObjectToDateTime(entity.year2 + "/" + entity.month2 + "/" + entity.day2);
            else
            {
                entity.Msg = 2;
                ModelState.AddModelError("week_text", "信息不存在");
            }

            if (ModelState.IsValid)
            {

                Entity.WorkPlan model = null;

                if (isAdd)
                {
                    model = new Entity.WorkPlan();
                    model.IsApprove = false;
                    model.ApproveTime = null;
                    model.CusUserID = WorkContext.UserInfo.ID;

                }
                else
                    model = bll.GetModel(p => p.ID == entity.id);

                model.ApproveUserID = entity.approve_user_id;
                model.BeginTime = BeginTime;
                model.EndTime = EndTime;
                model.WeekText = entity.week_text;
                model.WorkPlanItemList = entity.plan_item;
                if (isAdd)
                    bll.Add(model);
                else
                    BLL.BLLWorkPlan.Modify(model);

                entity.Msg = 1;
            }
            else
            {
                entity.Msg = 3;
            }

            return View(entity);
        }

        /// <summary>
        /// 加载平台列表
        /// </summary>
        private void LoadPlatform(string def_week = "")
        {
            //加载审批人
            ViewData["ApproveUser"] = BLL.BLLCusUser.GetUserDepartmentAdminText(WorkContext.UserInfo.ID);

            List<SelectListItem> WeekList = new List<SelectListItem>();
            WeekList.Add(new SelectListItem() { Text = "请选择", Value = "" });
            DateTime dt = DateTime.Now;
            DateTime startWeek = GetMondayInWeek(DateTime.Now);  //本周周一
            DateTime endWeek = startWeek.AddDays(6);  //本周周日

            DateTime up_1 = startWeek.AddDays(-7);
            DateTime up_1_end = up_1.AddDays(6);
            string up_1_str = up_1.ToString("yyyy/MM/dd") + "-" + up_1_end.ToString("yyyy/MM/dd");

            DateTime up_2 = up_1.AddDays(-7);
            DateTime up_2_end = up_2.AddDays(6);
            string up_2_str = up_2.ToString("yyyy/MM/dd") + "-" + up_2_end.ToString("yyyy/MM/dd");

            DateTime now = up_1_end.AddDays(1);
            DateTime now_end = now.AddDays(6);
            string now_str = now.ToString("yyyy/MM/dd") + "-" + now_end.ToString("yyyy/MM/dd");

            DateTime next_1 = now_end.AddDays(1);
            DateTime next_1_end = next_1.AddDays(6);
            string next_1_str = next_1.ToString("yyyy/MM/dd") + "-" + next_1_end.ToString("yyyy/MM/dd");

            DateTime next_2 = next_1_end.AddDays(1);
            DateTime next_2_end = next_2.AddDays(6);
            string next_2_str = next_2.ToString("yyyy/MM/dd") + "-" + next_2_end.ToString("yyyy/MM/dd");

            if (!string.IsNullOrWhiteSpace(def_week))
                WeekList.Add(new SelectListItem() { Text = def_week, Value = def_week });
            WeekList.Add(new SelectListItem() { Text = up_2_str, Value = up_2_str });
            WeekList.Add(new SelectListItem() { Text = up_1_str, Value = up_1_str });
            WeekList.Add(new SelectListItem() { Text = now_str, Value = now_str });
            WeekList.Add(new SelectListItem() { Text = next_1_str, Value = next_1_str });
            WeekList.Add(new SelectListItem() { Text = next_2_str, Value = next_2_str });

            ViewData["WeekText"] = WeekList;
        }

        private void LoadStatus()
        {
            List<SelectListItem> StatusList = new List<SelectListItem>();
            foreach (var item in EnumHelper.BEnumToDictionary(typeof(Entity.WorkStatus)))
            {
                string text = EnumHelper.GetDescription<Entity.WorkStatus>((Entity.WorkStatus)item.Key);
                StatusList.Add(new SelectListItem() { Text = text, Value = item.Key.ToString() });
            }
            ViewData["StatusList"] = StatusList;
        }

        #region 私有方法

        /// <summary>
        /// 获取本周一的DateTime
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        static DateTime GetMondayInWeek(DateTime dt)
        {
            int week = (int)dt.DayOfWeek;

            if (week == 0)
            {
                week = 7;
            }

            if (week == 1)
            {
                return dt;
            }

            else
            {
                DateTime result = dt.AddDays(-(week - 1));
                return result;
            }

        }

        #endregion

    }
}