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
            ViewData["KJS"] = BLL.BLLConsultation.GetKJSAmount(WorkContext.UserInfo.ID).ToString("F2");
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
            //if (WorkContext.UserInfo.IsFullInfo)
            //{
            //    return PromptView("/MP/BasicUser/Info", "资料不能再修改了");
            //}

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
        public JsonResult Modify(string realname, string idnumber, string telphone, int gender, string bri, int clinic_id, string keshi_ids, string touxian, string shanchang_ids, string jianjie,string code)
        {

            //if (WorkContext.UserInfo.IsFullInfo)
            //{
            //    WorkContext.AjaxStringEntity.msgbox = "资料不能再修改";
            //    return Json(WorkContext.AjaxStringEntity);
            //}

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

            if (string.IsNullOrWhiteSpace(code))
            {
                WorkContext.AjaxStringEntity.msgbox = "请输入验证码";
                return Json(WorkContext.AjaxStringEntity);
            }
            if (code.Length != 4)
            {
                WorkContext.AjaxStringEntity.msgbox = "验证码只有4位";
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

            string ver_msg = "";
            var ver_s = BLL.BLLVerificationCode.VerCode(telphone, Entity.VerificationCodeType.FullInfo, code, out ver_msg);
            if (!ver_s)
            {
                WorkContext.AjaxStringEntity.msgbox = ver_msg;
                return Json(WorkContext.AjaxStringEntity);
            }

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
        /// 获取咨询列表分页json数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAdvisoryPageList(int page_size, int page_index, int type)
        {
            UnifiedResultEntity<List<Entity.ViewModel.ConsultationDoctor>> result = new UnifiedResultEntity<List<Entity.ViewModel.ConsultationDoctor>>();
            if (type != 1 && type != 2)
            {
                result.msgbox = "非法类别";
                return Json(result);
            }
            int total = 0;
            var db_list = BLL.BLLConsultation.GetDoctorsMsgList(page_size, page_index, WorkContext.UserInfo.ID, type, out total);
            result.msg = 1;
            result.msgbox = "ok";
            result.data = db_list;
            result.total = total;
            return Json(result);
        }

        /// <summary>
        /// 咨询详情
        /// </summary>
        /// <returns></returns>
        public ActionResult AdvisoryInfo(int id)
        {
            Universal.Entity.ViewModel.ConsultationDetail entity = BLL.BLLConsultation.GetConsultationInfo(id);
            string msg = "";
            var can_reply = BLL.BLLConsultation.CheckConsultationCanReply(id, WorkContext.WebSite.AdvisoryTimeOut, out msg);
            ViewData["CanReply"] = can_reply ? "1" : "0";
            ViewData["ErrorMSG"] = msg;
            return View(entity);
        }


        /// <summary>
        /// 继续回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AdvisoryReply(int id)
        {
            ViewData["ID"] = id;
            ViewData["BackUrl"] = "/MP/Doctors/AdvisoryInfo?id=" + id.ToString();
            var jssdkUiPackage = Senparc.Weixin.MP.Helpers.JSSDKHelper.GetJsSdkUiPackage(WorkContext.WebSite.WeChatAppID, WorkContext.WebSite.WeChatAppSecret, Request.Url.AbsoluteUri);

            return View(jssdkUiPackage);
        }

        /// <summary>
        /// 添加回复json
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AdvisoryReplyJson(int id, string remark, string media_pic_ids, string voice_id)
        {
            List<Entity.ConsultationListFile> file_list = new List<Entity.ConsultationListFile>();
            List<string> image_list = new List<string>();
            if (!string.IsNullOrWhiteSpace(media_pic_ids))
            {
                foreach (var item in media_pic_ids.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                    var img_path = MPHelper.MediaApi.DownloadImage(item);
                    if (!string.IsNullOrWhiteSpace(img_path))
                    {
                        file_list.Add(new Entity.ConsultationListFile(Entity.ConsultationFileType.Image, img_path));
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(voice_id))
            {
                var voice_path = MPHelper.MediaApi.DownloadVoice(voice_id);
                if (!string.IsNullOrWhiteSpace(voice_path)) file_list.Add(new Entity.ConsultationListFile(Entity.ConsultationFileType.Voice, voice_path));
            }

            string msg = "";
            var status = BLL.BLLConsultation.AddReplay(id, remark, Entity.ReplayUserType.Doctor, file_list,WorkContext.WebSite.AdvisoryTimeOut, out msg);
            if (!status)
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
                return Json(WorkContext.AjaxStringEntity);
            }
            if(msg== "TASK")
            {
                //设置72小时后自动结束咨询-接口中同时移除超时退款任务
                TaskJobHelper.AddAdvisoryDone(id, DateTime.Now, WorkContext.WebSite.AdvisoryTimeOut);
            }
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "OK";
            WorkContext.AjaxStringEntity.data = "回复成功";
            //给医生发送提醒
            MPHelper.TemplateMessage.SendReplyNotify(Entity.ReplayUserType.Doctor, id, remark);
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 手动关闭咨询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetAvdisoryDone(int id)
        {
            if (id <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }

            //System.Threading.Thread.Sleep(2000);
            //WorkContext.AjaxStringEntity.msg = 1;
            //WorkContext.AjaxStringEntity.msgbox = "操作成功";
            //return Json(WorkContext.AjaxStringEntity);

            var status = BLL.BLLConsultation.CloseOnDoc(id);
            if (status)
            {
                MPHelper.TemplateMessage.SendDoctorsAndUserAdvisoryIsDone(id);
                WorkContext.AjaxStringEntity.msg = 1;
                WorkContext.AjaxStringEntity.msgbox = "操作成功";
                return Json(WorkContext.AjaxStringEntity);
            }
            else
            {
                WorkContext.AjaxStringEntity.msgbox = "操作失败";
                return Json(WorkContext.AjaxStringEntity);
            }

        }

        /// <summary>
        /// 咨询结算
        /// </summary>
        /// <returns></returns>
        public ActionResult AdvisoryClear()
        {
            return View();
        }

        /// <summary>
        /// 获取咨询结算列表分页json数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAdvisoryClearPageList(int page_size, int page_index, int type)
        {
            UnifiedResultEntity<List<Entity.ViewModel.ConsultationDoctor>> result = new UnifiedResultEntity<List<Entity.ViewModel.ConsultationDoctor>>();
            if (type != 1 && type != 2 && type != 3)
            {
                result.msgbox = "非法类别";
                return Json(result);
            }
            int total = 0;
            var db_list = BLL.BLLConsultation.GetDoctorsMsgStteList(page_size, page_index, WorkContext.UserInfo.ID, type, out total);
            result.msg = 1;
            result.msgbox = "ok";
            result.data = db_list;
            result.total = total;
            return Json(result);
        }
        

        /// <summary>
        /// 提交结算申请
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ToAdvisoryClear(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "请选择需要结算的咨询";
                return Json(WorkContext.AjaxStringEntity);
            }
            string msg = "";
            var status = BLL.BLLConsultationSettlement.Add(WorkContext.UserInfo.ID, ids, out msg);
            if(!status)
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
                return Json(WorkContext.AjaxStringEntity);
            }
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "结算申请提交成功";
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 申请咨询结算历史
        /// </summary>
        /// <returns></returns>
        public ActionResult AdvisoryClearHostory()
        {


            return View();
        }

        /// <summary>
        /// 获取咨询结算历史列表分页json数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAdvisoryClearHostoryPageList(int page_size, int page_index, int type)
        {
            UnifiedResultEntity<List<Entity.ViewModel.Settlement>> result = new UnifiedResultEntity<List<Entity.ViewModel.Settlement>>();
            if (type != 1 && type != 2 && type != 3)
            {
                result.msgbox = "非法类别";
                return Json(result);
            }
            int total = 0;
            var db_list = BLL.BLLConsultationSettlement.GetPageList(page_size, page_index, WorkContext.UserInfo.ID, type, out total);
            result.msg = 1;
            result.msgbox = "ok";
            result.data = db_list;
            result.total = total;
            return Json(result);
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
            var code = HttpUtility.UrlDecode(Tools.WebHelper.GetCookie("doc_code"));

            Models.DoctorsInfo model = new Models.DoctorsInfo();
            model.name = name;
            model.telphone = telphone;
            model.brithday = brithday;
            model.gender = gender == "1" ? "男" : "女";
            model.idnumber = idnumber;
            model.touxian = touxian;
            model.jianjie = jianjie;
            model.code = code;

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