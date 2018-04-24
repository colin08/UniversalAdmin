using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;

namespace Universal.Web.Areas.MP.Controllers
{
    /// <summary>
    /// 医生端首页
    /// </summary>
    [OnlyDoctors]
    public class DoctorsController : BaseMPController
    {
        // GET: MP/Doctors
        public ActionResult Index()
        {
            var entity = BLL.BLLMPDoctor.GetModel(WorkContext.UserInfo.ID);
            if (entity.DoctorsInfo != null)
            {
                ViewData["TouXian"] = entity.DoctorsInfo.TouXian;
                ViewData["AdvisoryStatus"] = entity.DoctorsInfo.CanAdvisory ? "mui-active" : "";
                ViewData["AdvisoryPrice"] = entity.DoctorsInfo.AdvisoryPrice.ToString("F2");
            }
            else
            {
                ViewData["TouXian"] = "";
                ViewData["AdvisoryStatus"] = "";
                ViewData["AdvisoryPrice"] = "0";
            }
            return View();
        }

        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyAdvisoryPrice(decimal price)
        {
            bool isOK = BLL.BLLMPDoctor.SetAdvisoryPrice(WorkContext.UserInfo.OpenID, price);
            WorkContext.AjaxStringEntity.msg = isOK ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = isOK ? "修改成功" : "修改失败";
            BLL.BLLMPUserState.SetLogin(WorkContext.UserInfo.OpenID);
            return Json(WorkContext.AjaxStringEntity);
        }
        /// <summary>
        /// 修改是否可以咨询
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ModifyAdvisoryStatus(int status)
        {
            bool isOK = BLL.BLLMPDoctor.SetAdvisoryStatus(WorkContext.UserInfo.OpenID, status);
            WorkContext.AjaxStringEntity.msg = isOK ? 1 : 0;
            WorkContext.AjaxStringEntity.msgbox = isOK ? "修改成功" : "修改失败";
            BLL.BLLMPUserState.SetLogin(WorkContext.UserInfo.OpenID);
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 个人资料
        /// </summary>
        /// <returns></returns>
        public ActionResult Info()
        {
            return View(LoadDocInfo());
        }

        #region 修改医生资料

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <returns></returns>
        public ActionResult Modify(string f)
        {
            LoadData();
            if (string.IsNullOrWhiteSpace(f))
            {
                var model = LoadDocInfo();
                return View(model);
            }
            else
            {
                var model = LoadDocInfoForCookie();
                return View(model); ;
            }
        }

        /// <summary>
        /// 修改用户基本资料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Modify(string realname, string idnumber, string telphone, int gender, string bri, int clinic_id, string keshi_ids, string touxian, string shanchang_ids, string jianjie)
        {
            #region 数据验证

            if (realname.Length > 5 || realname.Length == 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "姓名长度在0~5之间";
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
            if (clinic_id <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "诊所数据非法";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (keshi_ids.Length == 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "请选择科室";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (touxian.Length > 10 || touxian.Length == 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "头衔长度在0~10之间";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (jianjie.Length > 100)
            {
                WorkContext.AjaxStringEntity.msgbox = "简介不能超过100个字符";
                return Json(WorkContext.AjaxStringEntity);
            }

            #endregion

            var old_model = BLL.BLLMPDoctor.GetModel(WorkContext.UserInfo.ID);
            old_model.RealName = realname;
            old_model.IDCardNumber = idnumber;
            old_model.Telphone = telphone;
            old_model.Gender = (Entity.MPUserGenderType)gender;
            old_model.Brithday = brithday;
            old_model.IsFullInfo = true;
            if (old_model.DoctorsInfo == null) old_model.DoctorsInfo = new Entity.MPUserDoctors();
            old_model.DoctorsInfo.ShowMe = jianjie;
            old_model.DoctorsInfo.TouXian = touxian;
            old_model.DoctorsInfo.ClinicID = clinic_id;
            string msg = "";
            bool status = BLL.BLLMPDoctor.Modify(old_model, shanchang_ids, keshi_ids, out msg);
            if (!status)
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
                return Json(WorkContext.AjaxStringEntity);
            }
            //更新Session中的用户信息
            BLL.BLLMPUserState.SetLogin(old_model.OpenID);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "修改成功";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 选择科室数据
        /// </summary>
        /// <param name="clinic_id"></param>
        /// <returns></returns>
        public ActionResult SelectDepart(int clinic_id, string de_ids)
        {
            if (clinic_id <= 0) return PromptView("/MP/Doctors/Modify?f=1", "非法参数");
            LoadDepartIndexedListData(clinic_id, string.IsNullOrWhiteSpace(de_ids) ? "" : de_ids);
            return View();
        }

        /// <summary>
        /// 选择特长数据
        /// </summary>
        /// <param name="clinic_id"></param>
        /// <returns></returns>
        public ActionResult SelectSpec(string de_ids)
        {
            LoadSpecIndexedListData(string.IsNullOrWhiteSpace(de_ids) ? "" : de_ids);
            return View();
        }

        #endregion

        /// <summary>
        /// 预约列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Reservation()
        {

            return View();
        }

        /// <summary>
        /// 预约设置
        /// </summary>
        /// <returns></returns>
        public ActionResult ReservationSetting()
        {

            return View();
        }

        /// <summary>
        /// 咨询列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Advisory()
        {
            return View();
        }

        /// <summary>
        /// 咨询结算
        /// </summary>
        /// <returns></returns>
        public ActionResult AdvisoryClear()
        {
            return View();
        }


        private void LoadData()
        {
            BLL.BaseBLL<Entity.Clinic> bll_clinic = new BLL.BaseBLL<Entity.Clinic>();
            var db_clinic_list = bll_clinic.GetListBy(0, p => p.Status, "Weight DESC");

            var clinic_id = Tools.TypeHelper.ObjectToInt(ViewData["ClinicID"], 0);
            System.Text.StringBuilder str_clinic = new System.Text.StringBuilder();
            //ViewData["SelectClinicID"] = 0;
            str_clinic.Append("[");
            for (int i = 0; i < db_clinic_list.Count; i++)
            {
                str_clinic.Append("{value:'" + db_clinic_list[i].ID + "',text:'" + db_clinic_list[i].Title + "'},");
                //if(clinic_id == db_clinic_list[i].ID) ViewData["SelectClinicID"] = i;
            }
            if (str_clinic.Length > 1) str_clinic.Remove(str_clinic.Length - 1, 1);
            str_clinic.Append("]");
            ViewData["ClinicJson"] = str_clinic.ToString();

        }

        /// <summary>
        /// 加载科室索引数据
        /// </summary>
        private void LoadDepartIndexedListData(int clinic_id, string select_ids)
        {
            int[] keshi_id_list;
            if (string.IsNullOrWhiteSpace(select_ids))
            {
                keshi_id_list = new int[0];
                select_ids = "";
            }
            else
            {
                keshi_id_list = Array.ConvertAll<string, int>(select_ids.Split(','), int.Parse);
            }
            List<string> SZMList = new List<string>();
            var SelectItem = new List<Universal.Web.Areas.MP.Models.MedicalSelectItem>();
            var db_list = BLL.BLLClinicDepartment.LoadAllSelectList(clinic_id, out SZMList);
            int select_total = 0;
            foreach (var item in db_list)
            {
                Models.MedicalSelectItem model = new Models.MedicalSelectItem();
                if (keshi_id_list.Contains(item.ID))
                {
                    select_total++;
                    model.type = 2;
                }
                else model.type = 0;
                model.Title = item.Title;
                model.MedicalID = item.ID;
                model.SZM = item.SZM;
                model.Weight = item.Weight;
                SelectItem.Add(model);
            }

            if (select_total == 0)
            {
                ViewData["BtnText"] = "完成";
            }
            else
            {
                ViewData["BtnText"] = "完成(" + select_total.ToString() + ")";
            }

            ViewData["DBSelectItem"] = SelectItem;
            ViewData["SZMList"] = SZMList;

        }

        /// <summary>
        /// 加载特长索引数据
        /// </summary>
        private void LoadSpecIndexedListData(string select_ids)
        {
            int[] select_id_list;
            if (string.IsNullOrWhiteSpace(select_ids))
            {
                select_id_list = new int[0];
                select_ids = "";
            }
            else
            {
                select_id_list = Array.ConvertAll<string, int>(select_ids.Split(','), int.Parse);
            }
            List<string> SZMList = new List<string>();
            var SelectItem = new List<Universal.Web.Areas.MP.Models.MedicalSelectItem>();
            var db_list = BLL.BLLDoctorsSpecialty.LoadAllSelectList(out SZMList);
            int select_total = 0;
            foreach (var item in db_list)
            {
                Models.MedicalSelectItem model = new Models.MedicalSelectItem();
                if (select_id_list.Contains(item.ID))
                {
                    select_total++;
                    model.type = 2;
                }
                else model.type = 0;
                model.Title = item.Title;
                model.MedicalID = item.ID;
                model.SZM = item.SZM;
                model.Weight = 0;
                SelectItem.Add(model);
            }

            if (select_total == 0)
            {
                ViewData["BtnText"] = "完成";
            }
            else
            {
                ViewData["BtnText"] = "完成(" + select_total.ToString() + ")";
            }

            ViewData["DBSelectItem"] = SelectItem;
            ViewData["SZMList"] = SZMList;

        }

        /// <summary>
        /// 加载医生资料数据
        /// </summary>
        /// <returns></returns>
        private Models.DoctorsInfo LoadDocInfo()
        {
            var entity = BLL.BLLMPDoctor.GetModel(WorkContext.UserInfo.ID);
            if (entity == null) return new Models.DoctorsInfo();
            Models.DoctorsInfo model = new Models.DoctorsInfo();
            model.name = entity.RealName;
            model.telphone = entity.Telphone;
            model.brithday = entity.GetBrithday;
            model.gender = entity.GetGenderStr;
            model.idnumber = entity.IDCardNumber;
            if (entity.DoctorsInfo != null)
            {
                model.jianjie = entity.DoctorsInfo.ShowMe;
                model.touxian = entity.DoctorsInfo.TouXian;
                if (entity.DoctorsInfo.ClinicDepartmentList != null)
                {
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
                    foreach (var item in entity.DoctorsInfo.ClinicDepartmentList.ToList())
                    {
                        stringBuilder.Append(item.Title + ",");
                        stringBuilder2.Append(item.ID + ",");
                    }
                    if (stringBuilder.Length > 0) stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    if (stringBuilder2.Length > 0) stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
                    model.keshi = stringBuilder.ToString();
                    model.keshi_ids = stringBuilder2.ToString();
                }
                if (entity.DoctorsInfo.DoctorsSpecialtyList != null)
                {
                    System.Text.StringBuilder builder = new System.Text.StringBuilder();
                    System.Text.StringBuilder builder2 = new System.Text.StringBuilder();
                    foreach (var item in entity.DoctorsInfo.DoctorsSpecialtyList.ToList())
                    {
                        builder.Append(item.Title + ",");
                        builder2.Append(item.ID + ",");
                    }
                    if (builder.Length > 0) builder.Remove(builder.Length - 1, 1);
                    if (builder2.Length > 0) builder2.Remove(builder2.Length - 1, 1);
                    model.shanchang = builder.ToString();
                    model.shanchang_ids = builder2.ToString();
                }
                if (entity.DoctorsInfo.Clinic != null)
                {
                    ViewData["ClinicID"] = entity.DoctorsInfo.ClinicID;
                    model.zhensuo = entity.DoctorsInfo.Clinic.Title;
                    model.zhensuo_id = entity.DoctorsInfo.Clinic.ID;
                }
                else
                {
                    ViewData["ClinicID"] = 0;
                }
            }

            return model;
        }

        /// <summary>
        /// 加载医生资料数据 从Cookie里
        /// </summary>
        /// <returns></returns>
        private Models.DoctorsInfo LoadDocInfoForCookie()
        {
            var name = HttpUtility.UrlDecode(Tools.WebHelper.GetCookie("doc_name"));
            var gender = HttpUtility.UrlDecode(Tools.WebHelper.GetCookie("doc_gender"));
            var brithday = HttpUtility.UrlDecode(Tools.WebHelper.GetCookie("doc_brithday"));
            var idnumber = HttpUtility.UrlDecode(Tools.WebHelper.GetCookie("doc_idnumber"));
            var telphone = HttpUtility.UrlDecode(Tools.WebHelper.GetCookie("doc_telphone"));
            var zhensuo_id = HttpUtility.UrlDecode(Tools.WebHelper.GetCookie("doc_zhensuo_id"));
            var keshi_ids = HttpUtility.UrlDecode(Tools.WebHelper.GetCookie("doc_keshi_ids"));
            var shanchang_ids = HttpUtility.UrlDecode(Tools.WebHelper.GetCookie("doc_shanchang_ids"));
            var touxian = HttpUtility.UrlDecode(Tools.WebHelper.GetCookie("doc_touxian"));
            var jianjie = HttpUtility.UrlDecode(Tools.WebHelper.GetCookie("doc_jianjie"));

            Models.DoctorsInfo model = new Models.DoctorsInfo();
            model.name = name;
            model.telphone = telphone;
            model.brithday = brithday;
            model.gender = gender == "1" ? "男" : "女";
            model.idnumber = idnumber;
            model.touxian = touxian;
            model.jianjie = jianjie;

            //组装科室数据
            if (!string.IsNullOrWhiteSpace(keshi_ids))
            {
                BLL.BaseBLL<Entity.ClinicDepartment> bll_de = new BLL.BaseBLL<Entity.ClinicDepartment>();
                var keshi_id_list = Array.ConvertAll<string, int>(keshi_ids.Split(','), int.Parse);
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
                foreach (var item in bll_de.GetListBy(0, p => keshi_id_list.Contains(p.ID), "Weight DESC").ToList())
                {
                    stringBuilder.Append(item.Title + ",");
                    stringBuilder2.Append(item.ID + ",");
                }
                if (stringBuilder.Length > 0) stringBuilder.Remove(stringBuilder.Length - 1, 1);
                if (stringBuilder2.Length > 0) stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
                model.keshi = stringBuilder.ToString();
                model.keshi_ids = stringBuilder2.ToString();
            }
            //组装特长数据
            if (!string.IsNullOrWhiteSpace(shanchang_ids))
            {
                var shanchagn_id_list = Array.ConvertAll<string, int>(shanchang_ids.Split(','), int.Parse);
                BLL.BaseBLL<Entity.DoctorsSpecialty> bll_sp = new BLL.BaseBLL<Entity.DoctorsSpecialty>();
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                System.Text.StringBuilder builder2 = new System.Text.StringBuilder();
                foreach (var item in bll_sp.GetListBy(0, p => shanchagn_id_list.Contains(p.ID), "ID ASC"))
                {
                    builder.Append(item.Title + ",");
                    builder2.Append(item.ID + ",");
                }
                if (builder.Length > 0) builder.Remove(builder.Length - 1, 1);
                if (builder2.Length > 0) builder2.Remove(builder2.Length - 1, 1);
                model.shanchang = builder.ToString();
                model.shanchang_ids = builder2.ToString();
            }
            //组装诊所ID
            if (!string.IsNullOrWhiteSpace(zhensuo_id))
            {
                BLL.BaseBLL<Entity.Clinic> bll_cli = new BLL.BaseBLL<Entity.Clinic>();
                var z_id = Tools.TypeHelper.ObjectToInt(zhensuo_id);
                var entity_zhensuo = bll_cli.GetModel(p => p.ID == z_id, "Weight DESC");
                if (entity_zhensuo != null)
                {
                    ViewData["ClinicID"] = entity_zhensuo.ID;
                    model.zhensuo = entity_zhensuo.Title;
                    model.zhensuo_id = entity_zhensuo.ID;
                }
                else
                {
                    ViewData["ClinicID"] = 0;
                }
            }
            return model;
        }

    }
}