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
    /// 诊所科室
    /// </summary>
    public class ClinicDepartmentController : BaseAdminController
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="page">当前第几页</param>
        /// <param name="role">筛选：组ID</param>
        /// <param name="word">筛选：关键字</param>
        /// <returns></returns>
        [AdminPermissionAttribute("诊所科室", "诊所科室首页")]
        public ActionResult Index(int page = 1,int cli=0, string word = "")
        {
            if(cli == 0)
            {
                return PromptView("/admin/Clinic", "404", "Not Found", "非法参数", 5);
            }

            BLL.BaseBLL<Entity.Clinic> bll_cli = new BLL.BaseBLL<Entity.Clinic>();
            var entity_cli = bll_cli.GetModel(p => p.ID == cli, "ID ASC");
            if(entity_cli == null)
            {
                return PromptView("/admin/Clinic", "404", "Not Found", "诊所不存在", 5);
            }
            ViewData["CliTitle"] = entity_cli.Title;
            ViewData["CliID"] = cli;

            word = WebHelper.UrlDecode(word);
            Models.ViewModelClinicDepartmentList response_model = new Models.ViewModelClinicDepartmentList();
            response_model.page = page;
            response_model.word = word;
            response_model.cli = cli;
            //获取每页大小的Cookie
            response_model.page_size = TypeHelper.ObjectToInt(WebHelper.GetCookie(WorkContext.PageKeyCookie), SiteKey.AdminDefaultPageSize);

            int total = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            filter.Add(new BLL.FilterSearch("ClinicID", cli.ToString(), BLL.FilterSearchContract.等于));
            if (!string.IsNullOrWhiteSpace(word))
            {
                filter.Add(new BLL.FilterSearch("Title", word, BLL.FilterSearchContract.like));
            }


            BLL.BaseBLL<Entity.ClinicDepartment> bll = new BLL.BaseBLL<Entity.ClinicDepartment>();
            var list = bll.GetPagedList(page, response_model.page_size, ref total, filter, "Weight desc");
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);
            return View(response_model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [AdminPermissionAttribute("诊所科室", "诊所科室编辑页面")]
        public ActionResult Edit(int cli, int? id)
        {
            if (cli == 0)
            {
                return PromptView("/admin/Clinic", "404", "Not Found", "非法参数", 5);
            }

            BLL.BaseBLL<Entity.Clinic> bll_cli = new BLL.BaseBLL<Entity.Clinic>();
            var entity_cli = bll_cli.GetModel(p => p.ID == cli, "ID ASC");
            if (entity_cli == null)
            {
                return PromptView("/admin/Clinic", "404", "Not Found", "诊所不存在", 5);
            }
            ViewData["CliTitle"] = entity_cli.Title;
            ViewData["CliID"] = cli;

            BLL.BaseBLL<Entity.ClinicDepartment> bll = new BLL.BaseBLL<Entity.ClinicDepartment>();
            Entity.ClinicDepartment entity = new Entity.ClinicDepartment();
            int num = TypeHelper.ObjectToInt(id, 0);
            if (num != 0)
            {
                entity = bll.GetModel(p => p.ID == num, null);
                if (entity == null)
                {
                    return PromptView("/admin/ClinicDepartment?page=1&cli="+cli, "404", "Not Found", "信息不存在或已被删除", 5);
                }
                if(entity.ClinicID != cli)
                {
                    return PromptView("/admin/ClinicDepartment?page=1&cli=" + cli, "404", "Not Found", "该科室不属于该诊所", 5);
                }
            }
            return View(entity);
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminPermissionAttribute("诊所科室", "保存诊所科室编辑信息")]
        public ActionResult Edit(int cli,Entity.ClinicDepartment entity)
        {
            if (cli == 0)
            {
                return PromptView("/admin/Clinic", "404", "Not Found", "非法参数", 5);
            }

            BLL.BaseBLL<Entity.Clinic> bll_cli = new BLL.BaseBLL<Entity.Clinic>();
            var entity_cli = bll_cli.GetModel(p => p.ID == cli, "ID ASC");
            if (entity_cli == null)
            {
                return PromptView("/admin/Clinic", "404", "Not Found", "诊所不存在", 5);
            }
            ViewData["CliTitle"] = entity_cli.Title;
            ViewData["CliID"] = cli;

            var isAdd = entity.ID == 0 ? true : false;
            entity.ClinicID = cli;
            BLL.BaseBLL<Entity.ClinicDepartment> bll = new BLL.BaseBLL<Entity.ClinicDepartment>();
            //数据验证
            if (isAdd)
            {

            }
            else
            {
                //如果要编辑的用户不存在
                if (!bll.Exists(p => p.ID == entity.ID))
                {
                    return PromptView("/admin/ClinicDepartment?page=1&cli="+entity.ClinicID.ToString(), "404", "Not Found", "信息不存在或已被删除", 5);
                }

            }

            if (ModelState.IsValid)
            {
                var pinyin = Hz2Py.ConvertToPin(entity.Title);
                if (!string.IsNullOrWhiteSpace(pinyin))
                {
                    entity.SZM = pinyin.Substring(0, 1);
                }
                else
                {
                    entity.SZM = "&";
                }
                //添加
                if (entity.ID == 0)
                {
                    bll.Add(entity);
                }
                else //修改
                {
                    var data = bll.GetModel(p => p.ID == entity.ID, null);
                    data.Title = entity.Title;
                    data.Status = entity.Status;
                    data.Weight = entity.Weight;
                    data.SZM = entity.SZM;
                    bll.Modify(data);
                }

                return PromptView("/admin/ClinicDepartment?page=1&cli="+entity.ClinicID.ToString(), "OK", "Success", "操作成功", 5);
            }
            else
                return View(entity);
        }
        /// <summary>
        /// 禁用
        /// </summary>
        [HttpPost]
        [AdminPermissionAttribute("诊所科室", "禁用地区")]
        public JsonResult Del(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "缺少参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            var db_ids = string.Join(",", id_list);
            BLL.BLLClinicDepartment.DisEnble(db_ids);
            AddAdminLogs(Entity.SysLogMethodType.Delete, "禁用诊所科室：" + ids + "");

            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "success";
            return Json(WorkContext.AjaxStringEntity);
        }
    }
}