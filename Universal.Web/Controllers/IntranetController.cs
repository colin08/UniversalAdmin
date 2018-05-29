using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Universal.Tools;
using Universal.Entity;
using Universal.Web.Framework;

namespace Universal.Web.Controllers
{
    /// <summary>
    /// 供内网调用的接口
    /// </summary>
    public class IntranetController : Controller
    {
        
        ///// <summary>
        ///// 咨询退款操作
        ///// </summary>
        ///// <param name="type">类别，1:咨询</param>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public ContentResult DoRefund(int type,string id)
        //{
        //    if(type == 1)
        //    {
        //        int c_id = TypeHelper.ObjectToInt(id);
        //        decimal money = 0;
        //        string order_num = BLL.BLLConsultation.CloseAndRefundOnDocNoReply(c_id,out money);
        //        if (!string.IsNullOrWhiteSpace(order_num)) MPHelper.WXPay.Refund(order_num, money, money);
        //        else return Content("Error");
        //    }

        //    return Content("ok");
        //}
        

        /// <summary>
        /// 用户体检报告
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserMedicalReport()
        {
            UnifiedResultEntity<string> result = new UnifiedResultEntity<string>();
            result.data = "";
            string idcardnumber = WebHelper.GetRequestString("id_number", "");
            if (string.IsNullOrWhiteSpace(idcardnumber))
            {
                result.msgbox = "身份证号码不能为空";
                return Json(result);
            }
            string report_title = WebHelper.GetRequestString("report_title", "");
            if (string.IsNullOrWhiteSpace(report_title))
            {
                result.msgbox = "报告名称不能为空";
                return Json(result);
            }

            string file_name = DateTime.Now.ToFileTime().ToString();
            string file_ext = "";
            string file_folder = "/uploads/report/";
            string server_path = file_folder + file_name;
            string file_io_folder = IOHelper.GetMapPath(file_folder);
            if (!System.IO.Directory.Exists(file_io_folder)) System.IO.Directory.CreateDirectory(file_io_folder);
            if(Request.Files.Count == 0)
            {
                result.msgbox = "缺少报告附件";
                return Json(result);
            }
            try
            {
                var file = Request.Files[0];
                file_ext = "." + IOHelper.GetFileExt(file.FileName).ToLower();
                server_path = server_path + file_ext;
                file.SaveAs(file_io_folder + file_name + file_ext);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("内网提交提交报告文件接收出错：" + ex.Message);
                result.msgbox = ex.Message;
                return Json(result);
            }
            string msg = "";
            var status = BLL.BLLMpUserMedicalReport.Add(report_title, idcardnumber, server_path, out msg);
            if(!status)
            {
                result.msgbox = msg;
                return Json(result);
            }
            result.msg = 1;
            result.msgbox = "ok";
            return Json(result);
        }


        #region 计划任务使用


        /// <summary>
        /// 设置咨询已结束
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentResult TaskSetAdvisoryDone(int id)
        {
            SetAdvisoryIsDone(id);
            return Content("ok");
        }


        /// <summary>
        /// 设置咨询超时未回复-退款操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ContentResult TaskSetAdvisoryRefund(int id)
        {
            SetAdvisoryToRefund(id);
            return Content("ok");
        }


        /// <summary>
        /// 获取3天后要设置为超时结束的咨询
        /// </summary>
        /// <returns></returns>
        public JsonResult TaskGetAdvisoryDoneIds()
        {
            var WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);
            var db_list = BLL.BLLConsultation.TaskGetAdvisoryTimeOutIds();
            List<Entity.TaskModel.AdvisoryTimeOut> data_list = new List<Entity.TaskModel.AdvisoryTimeOut>();
            foreach (var item in db_list)
            {
                //支付时间小于设置的超时时间
                if (WebHelper.DateTimeDiff(item.pay_time, DateTime.Now, "ah") < WebSite.AdvisoryTimeOut)
                {
                    //添加到列表返回出去
                    data_list.Add(item);
                }
                else {
                    //否则直接设置为已结束
                    //设置咨询已结束
                    SetAdvisoryIsDone(item.id);
                }
            }
            Entity.TaskModel.AdvisoryTimeOutAPI result = new Entity.TaskModel.AdvisoryTimeOutAPI();
            result.data_list = data_list;
            result.time_out = WebSite.AdvisoryTimeOut;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取超时后未回复用户的咨询-医生
        /// </summary>
        /// <returns></returns>
        public JsonResult TaskGetAdvisoryRefundIds()
        {
            var WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);

            var db_list = BLL.BLLConsultation.TaskGetAdvisoryNoNoReplyTimeOutIds();
            List<Entity.TaskModel.AdvisoryTimeOut> data_list = new List<Entity.TaskModel.AdvisoryTimeOut>();
            foreach (var item in db_list)
            {
                //支付时间小于设置的超时时间
                if (WebHelper.DateTimeDiff(item.pay_time, DateTime.Now, "ah") < WebSite.AdvisoryNoReplyTimeOut)
                {
                    //添加到列表返回出去
                    data_list.Add(item);
                }
                else
                {
                    //否则直接为用户退款
                    SetAdvisoryToRefund(item.id);
                }
            }
            Entity.TaskModel.AdvisoryTimeOutAPI result = new Entity.TaskModel.AdvisoryTimeOutAPI();
            result.data_list = data_list;
            result.time_out = WebSite.AdvisoryNoReplyTimeOut;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #region 私有方法

        /// <summary>
        /// 设置咨询已结束
        /// </summary>
        private void SetAdvisoryIsDone(int id)
        {
            var WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);
            var status = BLL.BLLConsultation.CloseOnTimeOut(id, WebSite.AdvisoryTimeOut.ToString() + "小时");
            if (!status)
            {
                System.Diagnostics.Trace.WriteLine("设置咨询结束失败");
                return;
            }

            //给医生和用户发送消息
            MPHelper.TemplateMessage.SendDoctorsAndUserAdvisoryIsDone(id, WebSite.AdvisoryTimeOut.ToString() + "小时");
        }

        /// <summary>
        /// 设置咨询退款
        /// </summary>
        private void SetAdvisoryToRefund(int id)
        {
            var WebSite = ConfigHelper.LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);

            decimal money = 0;
            string order_num = BLL.BLLConsultation.CloseAndRefundOnDocNoReply(id, out money);
            if (!string.IsNullOrWhiteSpace(order_num)) MPHelper.WXPay.Refund(order_num, money, money);

            //给医生和用户发送消息
            MPHelper.TemplateMessage.SendDoctorsAndUserAdvisoryIsRefund(id, WebSite.AdvisoryNoReplyTimeOut.ToString() + "小时");
        }

        #endregion

        #endregion

    }
}