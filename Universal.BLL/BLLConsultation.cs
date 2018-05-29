using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 在线咨询
    /// </summary>
    public class BLLConsultation
    {
        /// <summary>
        /// 添加主贴
        /// </summary>
        /// <param name="user_id">用户Id</param>
        /// <param name="doc_id">医生ID</param>
        /// <param name="d_type_id">疾病类型</param>
        /// <param name="area">所在地</param>
        /// <param name="content">咨询内容</param>
        /// <param name="files">附件</param>
        /// <returns></returns>
        public static bool Add(int user_id, int doc_id, int d_type_id, string area, string content, List<Entity.ConsultationFile> files, out string msg)
        {
            msg = "ok";
            if (files == null) files = new List<Entity.ConsultationFile>();
            using (var db = new DataCore.EFDBContext())
            {
                var entity_user = db.MPUsers.Where(p => p.ID == user_id).AsNoTracking().FirstOrDefault();
                if (entity_user == null) { msg = "用户不存在"; return false; }
                var entity_doctor = db.MPUsers.Where(p => p.ID == doc_id).Include(p => p.DoctorsInfo).AsNoTracking().FirstOrDefault();
                if (entity_doctor == null) { msg = "医生不存在"; return false; }
                if (entity_doctor.DoctorsInfo == null) { msg = "医生信息尚不完善"; return false; }
                if (!entity_doctor.DoctorsInfo.CanAdvisory) { msg = "该医生关闭了在线咨询"; return false; }
                var entity_d_type = db.ConsultationDiseases.Where(p => p.ID == d_type_id).AsNoTracking().FirstOrDefault();
                if (entity_d_type == null) { msg = "疾病类型不存在"; return false; }

                var order_num = DateTime.Now.ToString("yyyyMMddHHmmss") + Tools.WebHelper.GenerateRandomIntNumber(10);
                msg = order_num;
                var entity = new Entity.Consultation();
                entity.Area = area;
                entity.ConsultationDiseaseID = d_type_id;
                entity.Content = content;
                entity.LastReplyContent = content;
                entity.MPDoctorID = doc_id;
                entity.MPUserID = user_id;
                entity.PayMoney = entity_doctor.DoctorsInfo.AdvisoryPrice;
                entity.PayNumber = order_num;
                db.Consultations.Add(entity);
                foreach (var item in files)
                {
                    item.Consultation = entity;
                    db.ConsultationFiles.Add(item);
                }
                db.SaveChanges();
                msg = order_num;
            }
            return true;
        }

        /// <summary>
        /// 设置帖子已支付
        /// </summary>
        /// <param name="order_num">本站订单号</param>
        /// <param name="msg">消息状态</param>
        /// <param name="wx_num">微信订单号</param>
        /// <returns></returns>
        public static bool SetPayOK(string order_num, string wx_num, out string msg)
        {
            msg = "ok";
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.Consultations.Where(p => p.PayNumber == order_num).FirstOrDefault();
                if (entity == null) { msg = "咨询帖子不存在"; return false; }
                if (entity.Status != Entity.ConsultationStatus.待支付) { msg = "该咨询不是待支付状态"; return false; }
                entity.Status = Entity.ConsultationStatus.已支付;
                entity.PayWXNumber = wx_num;
                entity.SettDesc = "等待您的回复";
                entity.PayTime = DateTime.Now;
                db.SaveChanges();
                msg = entity.ID.ToString();
            }
            return true;
        }



        /// <summary>
        /// 获取可结算金额
        /// </summary>
        /// <param name="doc_id"></param>
        /// <returns></returns>
        public static decimal GetKJSAmount(int doc_id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                var strSql = "SELECT ISNULL(Sum(PayMoney),0) FROM [dbo].[Consultation] where MPDoctorID =" + doc_id.ToString() + " and Settlement = 2";
                return db.Database.SqlQuery<decimal>(strSql).FirstOrDefault();
            }
        }

        /// <summary>
        /// 关闭咨询|完成咨询-咨询时间超时
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeStr">时间字符串，比如:3天、72小时</param>
        /// <returns></returns>
        public static bool CloseOnTimeOut(int id, string timeStr)
        {
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.Consultations.Find(id);
                if (entity == null) return false;
                if (entity.Status == Entity.ConsultationStatus.已完成) return true;
                entity.Status = Entity.ConsultationStatus.已完成;
                entity.CloseDesc = "咨询时间超过" + timeStr + "，自动完成";
                entity.Settlement = Entity.ConsultaionSett.待结算;
                entity.SettDesc = "咨询完成，可以结算";
                db.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 关闭咨询-第一次医生长时间没有回复,调用后同时调用退款接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string CloseAndRefundOnDocNoReply(int id, out decimal money)
        {
            string order = "";
            money = 0;
            using (var db = new DataCore.EFDBContext())
            {
                var entity = db.Consultations.Find(id);
                if (entity == null) return order;
                if (entity.Status == Entity.ConsultationStatus.已关闭) return order;
                entity.Status = Entity.ConsultationStatus.已关闭;
                entity.IsRefund = true;
                entity.CloseDesc = "医生长时间未回复，已为您退款￥" + entity.PayMoney.ToString("F2") + "。";
                entity.Settlement = Entity.ConsultaionSett.不可结算;
                entity.SettDesc = "您未进行回复，已为用户退款";
                db.SaveChanges();
                money = entity.PayMoney;
                order = entity.PayNumber;
            }
            return order;
        }

        /// <summary>
        /// 添加回复
        /// </summary>
        /// <param name="main_id">主贴ID</param>
        /// <param name="content">回复内容</param>
        /// <param name="type">回复用户身份类别</param>
        /// <param name="files">回复附件</param>
        /// <param name="msg">状态消息</param>
        /// <returns></returns>
        public static bool AddReplay(int main_id, string content, Entity.ReplayUserType type, List<Entity.ConsultationListFile> files, out string msg)
        {
            msg = "ok";
            if (files == null) files = new List<Entity.ConsultationListFile>();
            using (var db = new DataCore.EFDBContext())
            {
                var main_entity = db.Consultations.Where(p => p.ID == main_id).FirstOrDefault();
                if (main_entity == null) { msg = "咨询不存在"; return false; }
                if (main_entity.IsRefund) { msg = "该咨询已退款"; return false; }
                if (Tools.WebHelper.DateTimeDiff(main_entity.AddTime, DateTime.Now, "ad") > 10) if (main_entity == null) { msg = "该咨询太久远了，已不能再回复"; return false; }
                if (main_entity.Status == Entity.ConsultationStatus.待支付) { msg = "该咨询未支付"; return false; }
                main_entity.LastReplayType = type;
                main_entity.LastReplyContent = content;
                main_entity.LastReplyTime = DateTime.Now;
                main_entity.Settlement = Entity.ConsultaionSett.不可结算;
                if (main_entity.Status == Entity.ConsultationStatus.已支付 && type == Entity.ReplayUserType.Doctor)
                {
                    msg = "TASK";
                    main_entity.Status = Entity.ConsultationStatus.进行中;
                    main_entity.SettDesc = "咨询进行中，不可结算";
                }

                var entity = new Entity.ConsultationList();
                entity.ConsultationID = main_id;
                entity.Content = content;
                entity.UserType = type;
                db.ConsultationLists.Add(entity);
                foreach (var item in files)
                {
                    item.ConsultationInfo = entity;
                    db.ConsultationListFiles.Add(item);
                }
                db.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// 根据订单号获取咨询实体
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Entity.Consultation GetModel(string order_num)
        {
            if (string.IsNullOrWhiteSpace(order_num)) return null;
            using (var db=new DataCore.EFDBContext())
            {
                return db.Consultations.Where(p => p.PayNumber == order_num).Include(p=>p.ConsultationDisease).Include(p => p.MPDoctorInfo).Include(p => p.MPUserInfo).AsNoTracking().FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据订单号获取咨询实体
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Entity.Consultation GetModel(int id)
        {
            if (id<=0) return null;
            using (var db = new DataCore.EFDBContext())
            {
                return db.Consultations.Where(p => p.ID == id).Include(p => p.ConsultationDisease).Include(p => p.MPDoctorInfo).Include(p => p.MPUserInfo).AsNoTracking().FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取医生端用户咨询列表
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="doc_id">医生ID</param>
        /// <param name="type">咨询类别，1：进行中，2：已关闭</param>
        /// <returns></returns>
        public static List<Entity.ViewModel.ConsultationDoctor> GetDoctorsMsgList(int page_size, int page_index, int doc_id, int type, out int total)
        {
            List<Entity.ViewModel.ConsultationDoctor> result = new List<Entity.ViewModel.ConsultationDoctor>();
            total = 0;
            if (page_size <= 1) page_size = 8;
            if (page_index <= 0) page_index = 1;

            int begin_index = page_size * (page_index - 1) + 1;
            int end_index = page_size * page_index;

            string strWhere = " where MPDoctorID = " + doc_id.ToString();
            if (type == 1)
            {
                strWhere += " and (Status=2 or Status=3)";
            }
            else
            {
                strWhere += " and (Status=4 or Status=5)";
            }

            using (var db = new DataCore.EFDBContext())
            {
                string strSqlTotal = "select count(1) FROM [dbo].[Consultation]" + strWhere;
                string strSql = "select * from (SELECT ROW_Number() OVER(ORDER BY LastReplyTime Desc) as row,* FROM [dbo].[Consultation] " + strWhere + ") as T where row BETWEEN " + begin_index.ToString() + " and " + end_index.ToString();
                total = db.Database.SqlQuery<int>(strSqlTotal).FirstOrDefault();
                var db_list = db.Consultations.SqlQuery(strSql).ToList();
                foreach (var item in db_list)
                {
                    var entity_dis = db.ConsultationDiseases.Where(p => p.ID == item.ConsultationDiseaseID).AsNoTracking().FirstOrDefault();
                    Entity.ViewModel.ConsultationDoctor model = new Entity.ViewModel.ConsultationDoctor();
                    model.id = item.ID;
                    model.last_replay_user = item.LastReplayType;
                    model.close_desc = item.CloseDesc;
                    model.age = item.GetUserAge;
                    model.gender = item.GetUserGender;
                    model.avatar = item.GetUserAvatar;
                    model.last_reply_content = item.LastReplyContent;
                    model.last_reply_time = item.LastReplyTime;
                    model.type = type;
                    if (entity_dis == null) model.disease = "未知";
                    else model.disease = entity_dis.Title;
                    model.user_name = item.GetUserName;
                    model.last_reply_time_str = Tools.WebHelper.DateStringFromNow(item.LastReplyTime);
                    result.Add(model);
                }

            }
            return result;
        }

        /// <summary>
        /// 获取用户端我的咨询列表
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="user_id">用户ID</param>
        /// <param name="type">咨询类别，1：进行中，2：已关闭</param>
        /// <returns></returns>
        public static List<Entity.ViewModel.ConsultationUser> GetUserMsgList(int page_size, int page_index, int user_id, int type, out int total)
        {
            total = 0;
            if (page_size <= 1) page_size = 8;
            if (page_index <= 0) page_index = 1;

            int begin_index = page_size * (page_index - 1) + 1;
            int end_index = page_size * page_index;

            string strWhere = " where MPUserID = " + user_id.ToString();
            if (type == 1)
            {
                strWhere += " and (Status=2 or Status=3)";
            }
            else if(type == 2)
            {
                strWhere += " and (Status=4 or Status=5)";
            }else if(type == 3)
            {
                strWhere += " and Status=1";
            }

            using (var db = new DataCore.EFDBContext())
            {
                string strSqlTotal = "select count(1) FROM [dbo].[Consultation]" + strWhere;
                string strSql = "select * from(select *,ROW_Number() OVER(order by last_reply_time desc) as row from(select C.ID as id, " + type + " as type, U.RealName as user_name, U.Avatar as avatar, D.TouXian as touxian, C.LastReplyTime as last_reply_time, C.LastReplayType as last_replay_user, C.LastReplyContent as last_reply_content, C.CloseDesc as close_desc,C.PayNumber as order_num from(SELECT * FROM[dbo].[Consultation] " + strWhere + ") as C LEFT JOIN MPUser as U on C.MPDoctorID = U.ID LEFT JOIN MPUserDoctors as D on C.MPDoctorID = D.ID) as S) as T where row BETWEEN " + begin_index.ToString() + " and " + end_index.ToString();
                total = db.Database.SqlQuery<int>(strSqlTotal).FirstOrDefault();
                var db_list = db.Database.SqlQuery<Entity.ViewModel.ConsultationUser>(strSql).ToList();
                foreach (var item in db_list)
                {
                    item.last_reply_time_str = Tools.WebHelper.DateStringFromNow(item.last_reply_time);
                }
                return db_list;
            }
        }


        /// <summary>
        /// 获取医生端用户咨询结算列表
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="doc_id">医生ID</param>
        /// <param name="type">咨询类别，1：可结算，2：不可结算，3:已结算</param>
        /// <returns></returns>
        public static List<Entity.ViewModel.ConsultationDoctor> GetDoctorsMsgStteList(int page_size, int page_index, int doc_id, int type, out int total)
        {
            List<Entity.ViewModel.ConsultationDoctor> result = new List<Entity.ViewModel.ConsultationDoctor>();
            total = 0;
            if (page_size <= 1) page_size = 8;
            if (page_index <= 0) page_index = 1;

            int begin_index = page_size * (page_index - 1) + 1;
            int end_index = page_size * page_index;

            string strWhere = " where MPDoctorID = " + doc_id.ToString();
            if (type == 1)
            {
                strWhere += " and Settlement = 2";
            }
            else if (type == 2)
            {
                strWhere += " and Settlement =1";
            }
            else if (type == 3)
            {
                strWhere += " and Settlement =3";
            }

            using (var db = new DataCore.EFDBContext())
            {
                string strSqlTotal = "select count(1) FROM [dbo].[Consultation]" + strWhere;
                string strSql = "select * from (SELECT ROW_Number() OVER(ORDER BY LastReplyTime Desc) as row,* FROM [dbo].[Consultation] " + strWhere + ") as T where row BETWEEN " + begin_index.ToString() + " and " + end_index.ToString();
                total = db.Database.SqlQuery<int>(strSqlTotal).FirstOrDefault();
                var db_list = db.Consultations.SqlQuery(strSql).ToList();
                foreach (var item in db_list)
                {
                    var entity_dis = db.ConsultationDiseases.Where(p => p.ID == item.ConsultationDiseaseID).AsNoTracking().FirstOrDefault();
                    Entity.ViewModel.ConsultationDoctor model = new Entity.ViewModel.ConsultationDoctor();
                    model.id = item.ID;
                    model.last_replay_user = item.LastReplayType;
                    model.age = item.GetUserAge;
                    model.gender = item.GetUserGender;
                    model.avatar = item.GetUserAvatar;
                    model.last_reply_content = item.LastReplyContent;
                    model.last_reply_time = item.LastReplyTime;
                    model.type = type;
                    if (entity_dis == null) model.disease = "未知";
                    else model.disease = entity_dis.Title;
                    model.user_name = item.GetUserName;
                    model.last_reply_time_str = Tools.WebHelper.DateStringFromNow(item.LastReplyTime);

                    //结算页面需要用到的
                    model.price = item.PayMoney;
                    model.sett_desc = item.SettDesc;
                    result.Add(model);
                }

            }
            return result;
        }

        /// <summary>
        /// 获取咨询详情列表
        /// </summary>
        /// <param name="main_id">主贴ID</param>
        /// <returns></returns>
        public static Entity.ViewModel.ConsultationDetail GetConsultationInfo(int main_id)
        {
            Entity.ViewModel.ConsultationDetail result = new Entity.ViewModel.ConsultationDetail();
            using (var db = new DataCore.EFDBContext())
            {
                var entity_main = db.Consultations.Where(p => p.ID == main_id).Include(p => p.ConsultationFiles).AsNoTracking().FirstOrDefault();
                if (entity_main == null) return null;
                result.main_id = main_id;
                result.status = entity_main.Status;
                result.user_avatar = entity_main.GetUserAvatar;
                result.doctor_avatar = entity_main.GetDoctorAvatar;

                List<Entity.ViewModel.ConsultationDetailMsg> msg_list = new List<Entity.ViewModel.ConsultationDetailMsg>();

                var entity_msg_main = new Entity.ViewModel.ConsultationDetailMsg();
                entity_msg_main.content = entity_main.Content;
                entity_msg_main.id = -1;
                entity_msg_main.time = entity_main.AddTime;
                entity_msg_main.time_str = Tools.WebHelper.DateStringFromNow(entity_main.AddTime);
                entity_msg_main.user_type = Entity.ReplayUserType.User;//第一条信息，是用户发的
                //添加主贴附件
                foreach (var item in entity_main.ConsultationFiles)
                {
                    switch (item.Type)
                    {
                        case Entity.ConsultationFileType.Image:
                            entity_msg_main.file_image_list.Add(item.FilePath);
                            break;
                        case Entity.ConsultationFileType.Voice:
                            entity_msg_main.file_voice_list.Add(item.FilePath);
                            break;
                        default:
                            break;
                    }
                }
                msg_list.Add(entity_msg_main);
                //-主贴信息添加完毕
                //添加具体回复的信息
                var db_msg_list = db.ConsultationLists.Where(p => p.ConsultationID == main_id).Include(p => p.ConsultationListFiles).OrderBy(p => p.AddTime).AsNoTracking().ToList();
                foreach (var item in db_msg_list)
                {
                    var entity_msg = new Entity.ViewModel.ConsultationDetailMsg();
                    entity_msg.content = item.Content;
                    entity_msg.id = item.ID;
                    entity_msg.time = item.AddTime;
                    entity_msg.time_str = Tools.WebHelper.DateStringFromNow(item.AddTime);
                    entity_msg.user_type = item.UserType;
                    foreach (var file in item.ConsultationListFiles)
                    {
                        switch (file.Type)
                        {
                            case Entity.ConsultationFileType.Image:
                                entity_msg.file_image_list.Add(file.FilePath);
                                break;
                            case Entity.ConsultationFileType.Voice:
                                entity_msg.file_voice_list.Add(file.FilePath);
                                break;
                            default:
                                break;
                        }
                    }
                    msg_list.Add(entity_msg);
                }
                //回复信息添加完毕
                result.msg_list = msg_list;

                return result;
            }
        }


        #region 计划任务使用

        /// <summary>
        /// 机器重启后获取要添加到超时完成咨询的咨询列表
        /// </summary>
        /// <returns></returns>
        public static List<Entity.TaskModel.AdvisoryTimeOut> TaskGetAdvisoryTimeOutIds()
        {
            string strSql = "SELECT ID as id,PayTime as pay_time FROM [dbo].[Consultation] where Status = 3";
            using (var db= new DataCore.EFDBContext())
            {
                return db.Database.SqlQuery<Entity.TaskModel.AdvisoryTimeOut>(strSql).ToList();
            }
        }

        /// <summary>
        /// 机器重启后获取要添加到超时退款的咨询列表
        /// </summary>
        /// <returns></returns>
        public static List<Entity.TaskModel.AdvisoryTimeOut> TaskGetAdvisoryNoNoReplyTimeOutIds()
        {
            string strSql = "SELECT ID as id,PayTime as pay_time FROM [dbo].[Consultation] where Status = 2";
            using (var db = new DataCore.EFDBContext())
            {
                return db.Database.SqlQuery<Entity.TaskModel.AdvisoryTimeOut>(strSql).ToList();
            }
        }

        #endregion

    }
}
