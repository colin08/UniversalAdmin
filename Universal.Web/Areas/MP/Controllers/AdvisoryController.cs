using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Web.Framework;
using System.Data;
using System.Data.Entity;

namespace Universal.Web.Areas.MP.Controllers
{
    /// <summary>
    /// 在线咨询
    /// </summary>
    [OnlyBasicUser]
    public class AdvisoryController : BaseMPController
    {
        // GET: MP/Advisory
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return RedirectToAction("Index");
            ViewData["KeyWord"] = keyword;
            return View();
        }

        /// <summary>
        /// 医生页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Doctor(int id,string back)
        {
            ViewData["BackUrl"] = string.IsNullOrWhiteSpace(back) ? "/mp/advisory/index" : back;
            var model = BLL.BLLMPDoctor.GetDoctorSearchModel(id);
            if(model == null)
            {
                return PromptView("/mp/advisory/index", "医生不存在");
            }
            ViewData["ToLink"] = "/mp/advisory/toad?id=" + id + "&back=" + "/mp/advisory/doctor?id=" + id + "";
            return View(model);
        }

        /// <summary>
        /// 咨询须知
        /// </summary>
        /// <returns></returns>
        public ActionResult ToAd(int id,string back)
        {
            ViewData["BackUrl"] = string.IsNullOrWhiteSpace(back) ? "/mp/advisory/index" : back;
            if (id <= 0) return PromptView("非法参数");
            ViewData["ID"] = id;
            ViewData["NextUrl"] = "fristad?id=" + id + "&back=" + Tools.WebHelper.UrlEncode("ToAd?id=" + id + "&back=" + ViewData["BackUrl"]);
            return View();
        }

        /// <summary>
        /// 开始咨询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult FristAd(int id,string back)
        {
            ViewData["BackUrl"] = string.IsNullOrWhiteSpace(back) ? "/mp/advisory/index" : back;
            var model = BLL.BLLMPDoctor.GetDoctorSearchModel(id);
            if (model == null)
            {
                return PromptView("/mp/advisory/index", "医生不存在");
            }

            ViewData["DocID"] = model.id;
            ViewData["DocName"] = model.name;
            ViewData["DocAvatar"] = model.avatar;
            ViewData["PriceShow"] = model.GetPrice;
            ViewData["Price"] = model.adv_price;

            LoadData();

            var jssdkUiPackage = Senparc.Weixin.MP.Helpers.JSSDKHelper.GetJsSdkUiPackage(WorkContext.WebSite.WeChatAppID, WorkContext.WebSite.WeChatAppSecret, Request.Url.AbsoluteUri);

            return View(jssdkUiPackage);
        }

        /// <summary>
        /// 添加咨询
        /// </summary>
        /// <param name="doc_id"></param>
        /// <param name="c_type_id"></param>
        /// <param name="area"></param>
        /// <param name="remark"></param>
        /// <param name="media_pic_ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddFristAd(int doc_id,int c_type_id,string area,string remark,string media_pic_ids,string voice_id)
        {
            List<Entity.ConsultationFile> file_list = new List<Entity.ConsultationFile>();
            List<string> image_list = new List<string>();
            if (!string.IsNullOrWhiteSpace(media_pic_ids))
            {                
                foreach (var item in media_pic_ids.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                    var img_path = MPHelper.MediaApi.DownloadImage(item);
                    if (!string.IsNullOrWhiteSpace(img_path))
                    {
                        file_list.Add(new Entity.ConsultationFile(Entity.ConsultationFileType.Image, img_path));
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(voice_id))
            {
                var voice_path = MPHelper.MediaApi.DownloadVoice(voice_id);
                if(!string.IsNullOrWhiteSpace(voice_path)) file_list.Add(new Entity.ConsultationFile(Entity.ConsultationFileType.Voice, voice_path));
            }

            string msg = "";
            var status = BLL.BLLConsultation.Add(WorkContext.UserInfo.ID, doc_id, c_type_id, area, remark, file_list, out msg);
            if (!status)
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
                return Json(WorkContext.AjaxStringEntity);
            }
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "OK";
            WorkContext.AjaxStringEntity.data = msg;
            return Json(WorkContext.AjaxStringEntity);
        }


        /// <summary>
        /// 继续咨询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Add(int id)
        {
            ViewData["ID"] = id;
            ViewData["BackUrl"] = "/MP/Advisory/AdvisoryInfo?id=" + id.ToString();
            var jssdkUiPackage = Senparc.Weixin.MP.Helpers.JSSDKHelper.GetJsSdkUiPackage(WorkContext.WebSite.WeChatAppID, WorkContext.WebSite.WeChatAppSecret, Request.Url.AbsoluteUri);

            return View(jssdkUiPackage);
        }

        /// <summary>
        /// 添加咨询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddJson(int id, string remark, string media_pic_ids, string voice_id)
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
            var status = BLL.BLLConsultation.AddReplay(id, remark, Entity.ReplayUserType.User, file_list, out msg);
            if (!status)
            {
                WorkContext.AjaxStringEntity.msgbox = msg;
                return Json(WorkContext.AjaxStringEntity);
            }
            WorkContext.AjaxStringEntity.msg = 1;
            WorkContext.AjaxStringEntity.msgbox = "OK";
            WorkContext.AjaxStringEntity.data = msg;
            return Json(WorkContext.AjaxStringEntity);
        }


        /// <summary>
        /// 获取医生分页json数据
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="area_id"></param>
        /// <param name="hospital_id"></param>
        /// <param name="dep_id"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDocData(int page_size, int page_index, int area_id, int hospital_id, int dep_id, string keyword)
        {
            UnifiedResultEntity<Entity.ViewModel.AdvisoryIndex> resultEntity = new UnifiedResultEntity<Entity.ViewModel.AdvisoryIndex>();
            resultEntity.data = BLL.BLLMPDoctor.GetAdvisoryIndex(page_size, page_index, area_id, hospital_id, dep_id, keyword);
            resultEntity.msg = 1;
            resultEntity.msgbox = "ok";
            return Json(resultEntity);
        }


        /// <summary>
        /// 咨询列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AdvisoryList()
        {
            //ViewData["BackUrl"] = string.IsNullOrWhiteSpace(back) ? "/mp/advisory/index" : back;
            
            return View();
        }

        /// <summary>
        /// 咨询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AdvisoryInfo(int id)
        {
            Universal.Entity.ViewModel.ConsultationDetail entity = BLL.BLLConsultation.GetConsultationInfo(id);
            return View(entity);
        }

        /// <summary>
        /// 获取咨询列表json数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAdvisoryJsonList(int page_size,int page_index,int type)
        {
            UnifiedResultEntity<List<Entity.ViewModel.ConsultationUser>> result = new UnifiedResultEntity<List<Entity.ViewModel.ConsultationUser>>();
            if (type != 1 && type != 2 && type != 3)
            {
                result.msgbox = "非法类别";
                return Json(result);
            }
            int total = 0;
            var db_data = BLL.BLLConsultation.GetUserMsgList(page_size, page_index, WorkContext.UserInfo.ID, type, out total);
            result.msg = 1;
            result.msgbox = "ok";
            result.total = total;
            result.data = db_data;
            return Json(result);
        }


        private void LoadData()
        {
            BLL.BaseBLL<Entity.ConsultationDisease> bll = new BLL.BaseBLL<Entity.ConsultationDisease>();
            var db_data = bll.GetListBy(0, p => p.Status, "Weight DESC");
            System.Text.StringBuilder str_data = new System.Text.StringBuilder();
            str_data.Append("[");
            for (int i = 0; i < db_data.Count; i++)
            {
                str_data.Append("{value:'" + db_data[i].ID + "',text:'" + db_data[i].Title + "'},");
            }
            if (str_data.Length > 1) str_data.Remove(str_data.Length - 1, 1);
            str_data.Append("]");
            ViewData["DiseaseJson"] = str_data.ToString();

        }

    }
}