using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;
using System.Data.Entity;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 管理员
    /// </summary>
    [BasicAdminAuth]
    public class AdminUserController : BaseHBLController
    {
        public ActionResult Index()
        {
            int default_id = 0;
            string data = BLL.BLLDepartment.CreateDepartmentTreeData(out default_id);
            ViewData["TreeData"] = data;
            ViewData["DefaultID"] = default_id;
            return View();
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="department_id">部门ID</param>
        /// <param name="keyword">搜索关键字</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserData(int page_size,int page_index,int department_id,string keyword)
        {
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            int rowCount = 0;
            List<BLL.FilterSearch> filter = new List<BLL.FilterSearch>();
            filter.Add(new BLL.FilterSearch("CusDepartmentID", department_id.ToString(), BLL.FilterSearchContract.等于));
            if(!string.IsNullOrWhiteSpace(keyword))
            {
                filter.Add(new BLL.FilterSearch("NickName", keyword, BLL.FilterSearchContract.like));
                filter.Add(new BLL.FilterSearch("Telphone", keyword, BLL.FilterSearchContract.like));
            }
            List<Entity.CusUser> list = bll.GetPagedList(page_index, page_size, ref rowCount, filter, "RegTime desc", p => p.CusUserJob);
            WebAjaxEntity<List<Entity.CusUser>> result = new WebAjaxEntity<List<Entity.CusUser>>();
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
        public JsonResult DelUser(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            var id_list = Array.ConvertAll<string, int>(ids.Split(','), int.Parse);
            bll.DelBy(p => id_list.Contains(p.ID));
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "删除成功";
            return Json(WorkContext.AjaxStringEntity);

        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ResetUserPwd(int id)
        {
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            Entity.CusUser model = bll.GetModel(p => p.ID == id);
            if(model == null)
            {
                WorkContext.AjaxStringEntity.msgbox = "用户不存在";
                return Json(WorkContext.AjaxStringEntity);
            }
            model.Password = SecureHelper.MD5(WebSite.ResetPwd);
            bll.Modify(model, "Password");
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "密码已重置为："+WebSite.ResetPwd;
            return Json(WorkContext.AjaxStringEntity);
        }


        /// <summary>
        /// 编辑员工信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Modify(int? id)
        {
            ViewData["DefaultPwd"] = "默认为初始密码" + WebSite.ResetPwd;
            int user_id = Tools.TypeHelper.ObjectToInt(id);
            Models.ViewModelAdminUser entity = new Models.ViewModelAdminUser();
            entity.user_route = BLL.BLLCusRoute.GetRouteExists(user_id);
            if (user_id > 0)
            {
                Entity.CusUser model = BLL.BLLCusUser.GetModel(user_id);
                if (model != null)
                {
                    entity.about_me = model.AboutMe;
                    entity.avatar = model.Avatar;
                    entity.department_id = model.CusDepartmentID;
                    entity.department_title = model.CusDepartment.Title;
                    entity.email = model.Email;
                    entity.gender = model.Gender;
                    entity.id = model.ID;
                    entity.job_id = model.CusUserJobID;
                    entity.jon_title = model.CusUserJob.Title;
                    entity.nick_name = model.NickName;
                    entity.password = "litdev";
                    entity.short_num = model.ShorNum;
                    entity.telphone = model.Telphone;
                }
                else
                {
                    entity.Msg = 2;
                    entity.MsgBox = "数据不存在";
                }
            }
            return View(entity);
        }

        /// <summary>
        /// 保存员工信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken, ValidateInput(false)]
        [HttpPost]
        public ActionResult Modify(Models.ViewModelAdminUser entity)
        {
            ViewData["DefaultPwd"] = "默认为初始密码" + WebSite.ResetPwd;
            var isAdd = entity.id == 0 ? true : false;
            BLL.BaseBLL<Entity.CusUser> bll = new BLL.BaseBLL<Entity.CusUser>();
            if (!isAdd)
            {
                if (!bll.Exists(p => p.ID == entity.id))
                {
                    ModelState.AddModelError("telphone", "信息不存在或已被删除");
                }
            }
            else
            {
                if (bll.Exists(p => p.Telphone == entity.telphone))
                {
                    ModelState.AddModelError("Telphone", "该手机号已存在");
                }

                if (bll.Exists(p => p.Email == entity.email))
                {
                    ModelState.AddModelError("email", "该邮箱已存在");
                }
            }

            string pwd = "";
            if (string.IsNullOrWhiteSpace(entity.password))
                pwd = "123456";
            else
                pwd = entity.password;


            if (ModelState.IsValid)
            {
                Entity.CusUser model = null;

                //添加
                if (isAdd)
                {
                    model = new Entity.CusUser();                    
                    entity.password = SecureHelper.MD5(pwd);
                    if (string.IsNullOrWhiteSpace(entity.avatar))
                        entity.avatar = "/Content/images/tx_icon01.jpg";
                    model.Password = entity.password;
                }
                else //修改
                {
                    model = bll.GetModel(p => p.ID == entity.id);
                    if (pwd != "litdev")
                        model.Password = SecureHelper.MD5(pwd);
                }

                model.AboutMe = entity.about_me;
                model.Avatar = entity.avatar;
                //TOOD 部门职位定死值
                model.CusDepartmentID = 5;
                model.CusUserJobID = 1;
                model.Email = entity.email;
                model.Gender = entity.gender;
                model.IsAdmin = string.IsNullOrWhiteSpace(entity.user_route_str) ? false : true;
                model.NickName = entity.nick_name;
                model.ShorNum = entity.short_num;
                model.Status = true;
                model.Telphone = entity.telphone;

                if (isAdd)
                    BLL.BLLCusUser.Modify(model, entity.user_route_str);
                else
                    BLL.BLLCusUser.Modify(model, entity.user_route_str);

            }
            entity.Msg = 1;
            return View(entity);
        }


        /// <summary>
        /// 职位管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Job()
        {
            BLL.BaseBLL<Entity.CusUserJob> bll = new BLL.BaseBLL<Entity.CusUserJob>();
            List<Entity.CusUserJob> list = bll.GetListBy(0, new List<BLL.FilterSearch>() { }, "AddTime Desc", false);
            return View(list);
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelJob(int id)
        {
            if (id <= 0)
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            if(id == 1)
            {
                WorkContext.AjaxStringEntity.msgbox = "为保证测试继续进行，初始化的数据不能删除";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.CusUserJob> bll = new BLL.BaseBLL<Entity.CusUserJob>();
            bll.DelBy(p => p.ID == id);
            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);
        }

        /// <summary>
        /// 新增职位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddJob(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                WorkContext.AjaxStringEntity.msgbox = "非法参数";
                return Json(WorkContext.AjaxStringEntity);
            }
            BLL.BaseBLL<Entity.CusUserJob> bll = new BLL.BaseBLL<Entity.CusUserJob>();
            var entit = new Entity.CusUserJob();
            entit.Title = title;
            entit = bll.AddReturnModel(entit);
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = entit.ID.ToString();
            return Json(WorkContext.AjaxStringEntity);
        }

    }
}