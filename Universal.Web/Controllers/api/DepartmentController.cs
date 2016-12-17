using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Universal.Tools;
using Universal.Web.Framework;
using System.Data.Entity;

namespace Universal.Web.Controllers.api
{
    /// <summary>
    /// 部门接口
    /// </summary>
    public class DepartmentController: BaseAPIController
    {
        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/department/all")]
        public WebAjaxEntity<List<Models.Response.TreeData>> GetDepartment()
        {
            WebAjaxEntity<List<Models.Response.TreeData>> response_entity = new WebAjaxEntity<List<Models.Response.TreeData>>();
            BLL.BaseBLL<Entity.CusDepartment> bll = new BLL.BaseBLL<Entity.CusDepartment>();
            List<Models.Response.TreeData> response_list = new List<Models.Response.TreeData>();
            foreach (var item in bll.GetListBy(0, p => p.Status == true, "Priority Desc", false))
            {
                Models.Response.TreeData model = new Models.Response.TreeData();
                model.id = item.ID;
                model.pid = TypeHelper.ObjectToInt(item.PID, 0);
                model.title = item.Title;
                model.user_total = BLL.BLLDepartment.GetDepartChildDataTotal(item.ID);
                response_list.Add(model);
            }
            response_entity.data = response_list;
            response_entity.msg = 1;
            response_entity.msgbox = "success";
            return response_entity;
        }
                
    }
}