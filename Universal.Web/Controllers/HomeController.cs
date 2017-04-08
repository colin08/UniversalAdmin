using System.Linq;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    public class HomeController : BaseHBLController
    {
        public ActionResult Index()
        {
            Models.ViewModelIndex view_model = new Models.ViewModelIndex();
            int rowCount = 0;
            var doc_list = BLL.BLLDocument.GetPowerPageData(1, 5, ref rowCount, WorkContext.UserInfo.ID, "", 0);
            BLL.BaseBLL<Entity.DocFile> bll_file = new BLL.BaseBLL<Entity.DocFile>();
            foreach (var item in doc_list)
            {
                item.FileList = bll_file.GetListBy(0, p => p.DocPostID == item.ID, "ID ASC").ToList();
            }
            view_model.DocumentList = doc_list;
            int cus_user_id = WorkContext.UserInfo.ID;
            var notice_msg = new BLL.BaseBLL<Entity.CusUserMessage>().GetModel(p => p.CusUserID == cus_user_id && p.Type == Entity.CusUserMessageType.notice, "AddTime DESC");
            if(notice_msg != null)
            {
                int li_id = TypeHelper.ObjectToInt(notice_msg.LinkID) ;
                view_model.TopNotice = new BLL.BaseBLL<Entity.CusNotice>().GetModel(p => p.ID == li_id);
            }
            int total = 0;
            view_model.JobTask = BLL.BLLCusUser.GetJobTaskPageList(9, 1, WorkContext.UserInfo.ID, out total);

            return View(view_model);
        }

        /// <summary>
        /// 添加下载记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddDownLog(string title)
        {
            BLL.BLLCusUser.AddDownLog(WorkContext.UserInfo.ID, title);
            WorkContext.AjaxStringEntity.msg = 1;
            return Json(WorkContext.AjaxStringEntity);
        }

    }

}