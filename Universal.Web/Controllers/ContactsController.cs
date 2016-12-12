using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using System.Data.Entity;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 通讯录
    /// </summary>
    public class ContactsController : BaseHBLController
    {
        // GET: Msg
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="page">分页数据</param>
        /// <param name="d">部门ID，可为0</param>
        /// <param name="w">用户ID，可为空</param>
        /// <returns></returns>
        public ActionResult Users(int page,string d,string w)
        {
            if (page <= 0)
                page = 1;
            int department_id = TypeHelper.ObjectToInt(d,0);
            Models.ViewModelContactsUser response_model = new Models.ViewModelContactsUser();
            response_model.page = page;
            response_model.page_size = 40;
            response_model.w = w;
            response_model.d = department_id;
            string department_title = "";
            if(department_id > 0)
            {
                var entity_department = new BLL.BaseBLL<Entity.CusDepartment>().GetModel(p => p.ID == department_id);
                if (entity_department != null)
                    department_title = entity_department.Title;
            }

            int total = 0;
            List<Entity.CusUser> list = BLL.BLLCusUser.GetPageData(page, response_model.page_size, ref total, department_id, w);
            response_model.department_title = department_title;
            response_model.DataList = list;
            response_model.total = total;
            response_model.total_page = CalculatePage(total, response_model.page_size);

            return View(response_model);
        }

    }
}