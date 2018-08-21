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
    /// 医生管理
    /// </summary>
    public class MPDoctorController : BaseAdminController
    {
        /// <summary>
        /// 用户分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role">筛选：组ID</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("医生管理", "医生管理首页")]
        public ActionResult Index(int page = 1,int role=0, string word = "")
        {
            word = WebHelper.UrlDecode(word);
            Models.ViewModelMPDoctorList response_model = new Models.ViewModelMPDoctorList();
            response_model.page = page;
            response_model.word = word;
            response_model.role = role;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);

            Load();

            int total = 0;

            var list = BLL.BLLMPDoctor.GetPageList(response_model.page_size, response_model.page, role, word, out total);
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        /// <summary>
        /// 选择诊所
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SelectClinic(int id)
        {
            ViewData["id"] = id;
            BLL.BaseBLL<Entity.Clinic> bll = new BLL.BaseBLL<Entity.Clinic>();
            var db_list = bll.GetListBy(0, p => p.Status, "Weight DESC");
            List<SelectListItem> dataList = new List<SelectListItem>();
            foreach (var item in db_list)
            {
                dataList.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Title });
            }
            ViewData["ClinicList"] = dataList;
            return View();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [AdminPermissionAttribute("医生管理", "修改医生信息页面")]
        public ActionResult Edit(int clinic_id,int id)
        {
            Load();
            var model = BLL.BLLMPDoctor.GetModel(id);
            if(model == null) return PromptView("/admin/MPDoctor", "404", "Not Found", "信息不存在或已被删除", 5);
            Models.ViewModelEditMPDoctor result = new Models.ViewModelEditMPDoctor();
            result.id = id;
            result.real_name = model.RealName;
            result.Gender = model.Gender;
            result.Status = model.Status;
            result.telphone = model.Telphone;
            result.Avatar = model.Avatar;
            result.clinic_id = clinic_id;
            string techang_ids = "";
            string dep_ids = "";
            if (model.DoctorsInfo == null)
            {
                result.can_adv = false;
                result.adv_price = 99;
                result.clinic_id = 1;
                result.dep_ids = "";
                result.touxian = "医师";
                result.show_me = "";
            }
            else
            {
                result.can_adv = model.DoctorsInfo.CanAdvisory;
                result.adv_price = model.DoctorsInfo.AdvisoryPrice;
                if (model.DoctorsInfo.ClinicID == null) result.clinic_id = 1;
                else result.clinic_id = TypeHelper.ObjectToInt(model.DoctorsInfo.ClinicID, 1);
                result.dep_ids = "";
                result.show_me = model.DoctorsInfo.ShowMe;
                if(model.DoctorsInfo.DoctorsSpecialtyList!= null)
                {
                    System.Text.StringBuilder str_techang = new System.Text.StringBuilder();
                    foreach (var item in model.DoctorsInfo.DoctorsSpecialtyList.ToList())
                    {
                        str_techang.Append(item.ID.ToString() + ",");
                    }
                    techang_ids = str_techang.ToString();
                }

                if (model.DoctorsInfo.ClinicDepartmentList != null)
                {
                    System.Text.StringBuilder str_dep = new System.Text.StringBuilder();
                    foreach (var item in model.DoctorsInfo.ClinicDepartmentList.ToList())
                    {
                        str_dep.Append(item.ID.ToString() + ",");
                    }
                    dep_ids = str_dep.ToString();
                }
            }
            result.shanchang = techang_ids;
            result.dep_ids = dep_ids;
            LoadTeChang(techang_ids);
            Loaddep(clinic_id, dep_ids);
            return View(result);
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminPermissionAttribute("医生管理", "保存医生编辑信息")]
        public ActionResult Edit(Models.ViewModelEditMPDoctor entity)
        {
            Load();
            LoadTeChang();
            Loaddep(1);
            
            if (ModelState.IsValid)
            {
                var old_model = BLL.BLLMPDoctor.GetModel(entity.id);
                old_model.RealName = entity.real_name;
                old_model.Telphone = entity.telphone;
                old_model.Gender = entity.Gender;
                old_model.IsFullInfo = true;
                if (old_model.DoctorsInfo == null) old_model.DoctorsInfo = new Entity.MPUserDoctors();
                old_model.DoctorsInfo.ShowMe = entity.show_me;
                old_model.DoctorsInfo.TouXian = entity.touxian;
                old_model.DoctorsInfo.ClinicID =entity.clinic_id;
                old_model.DoctorsInfo.CanAdvisory = entity.can_adv;
                old_model.DoctorsInfo.AdvisoryPrice = entity.adv_price;
                string msg = "";


                bool status = BLL.BLLMPDoctor.Modify(old_model, entity.shanchang, entity.dep_ids, out msg);
                if (!status)
                {
                    return PromptView("/admin/MPDoctor/Edit?clinic_id=" + entity.clinic_id + "&id=" + entity.id, "404", "Not Found", "信息不存在或已被删除", 5);
                }

                return PromptView("/admin/MPDoctor", "OK", "Success", "修改成功", 5);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 禁用用户
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("医生管理", "首页禁用医生")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            var db_ids = string.Join(",", id_list);
            BLL.BLLMPUser.DisEnbleUser(db_ids);
            AddAdminLogs(Entity.SysLogMethodType.Delete, "禁用医生管理：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }



        /// <summary>
        /// 加载医院列表
        /// </summary>
        private void Load()
        {
            List<SelectListItem> dataList = new List<SelectListItem>();
            dataList.Add(new SelectListItem() { Text = "选择诊所", Value = "0" });
            var db_list = new BLL.BaseBLL<Entity.Clinic>().GetListBy(0, p => p.Status, "Weight DESC");

            foreach (var item in db_list)
            {
                dataList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString() });
            }
            ViewData["ClinicList"] = dataList;

            
        }

        /// <summary>
        /// 特长
        /// </summary>
        private void LoadTeChang(string default_val="")
        {
            //特长
            string temp_def_val = "," + default_val.Replace(" ", "") + ",";
            List<SelectListItem> dataList = new List<SelectListItem>();
            BLL.BaseBLL<Entity.DoctorsSpecialty> bll = new BLL.BaseBLL<Entity.DoctorsSpecialty>();
            var db_list = bll.GetListBy(0, p => p.ID > 0, "AddTime DESC");
            foreach (var item in db_list)
            {
                string temp_val = "," + item.ID.ToString() + ",";
                if (temp_def_val.IndexOf(temp_val) != -1)
                    dataList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString(), Selected = true });
                else
                    dataList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString() });
            }
            ViewData["shanchangList"] = dataList;
        }

        /// <summary>
        /// 科室
        /// </summary>
        private void Loaddep(int clinic_id,string default_val = "")
        {
            //特长
            string temp_def_val = "," + default_val.Replace(" ", "") + ",";
            List<SelectListItem> dataList = new List<SelectListItem>();
            BLL.BaseBLL<Entity.ClinicDepartment> bll = new BLL.BaseBLL<Entity.ClinicDepartment>();
            var db_list = bll.GetListBy(0, p => p.ClinicID == clinic_id && p.Status, "Weight DESC");
            foreach (var item in db_list)
            {
                string temp_val = "," + item.ID.ToString() + ",";
                if (temp_def_val.IndexOf(temp_val) != -1)
                    dataList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString(), Selected = true });
                else
                    dataList.Add(new SelectListItem() { Text = item.Title, Value = item.ID.ToString() });
            }
            ViewData["depList"] = dataList;
        }

    }
}