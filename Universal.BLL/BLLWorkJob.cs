using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Universal.BLL
{
    /// <summary>
    /// 任务指派
    /// </summary>
    public class BLLWorkJob
    {
        public static Entity.WorkJob GetModel(int id)
        {
            using (var db = new DataCore.EFDBContext())
            {
                return db.WorkJobs.AsNoTracking().Include(p => p.WorkJobUsers.Select(s => s.CusUser)).Include(p => p.FileList).Include(p => p.CusUser).Where(p => p.ID == id).FirstOrDefault();
            }
        }


        /// <summary>
        /// 获取任务列表，合并的
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="user_id"></param>
        /// <param name="search_word"></param>
        public static List<Entity.WorkJob> GetPageList(int page_size,int page_index,int user_id,string search_word,out int rowCount)
        {
            int begin_index = (page_index - 1) * page_size + 1;
            int end_index = page_index * page_size;

            string strWhere = "";
            if (!string.IsNullOrWhiteSpace(search_word))
                strWhere = " where where charindex('" + search_word + "',Title) > 0";

            string SQLTotal = "select count(1) from (select *,1 as 'type' from WorkJob where CusUserID = " + user_id.ToString() + " UNION select *,2 as 'type' from WorkJob where ID IN(select WorkJobID from WorkJobUser where CusUserID = " + user_id.ToString() + ")) as S ";
            string SQL = "select * from (select *,ROW_NUMBER() OVER(ORDER BY AddTime ASC) as row from (select* from (select *, 1 as 'type' from WorkJob where CusUserID = " + user_id.ToString() + " UNION select *, 2 as 'type' from WorkJob where ID IN(select WorkJobID from WorkJobUser where CusUserID = " + user_id.ToString() + ")) as S " + strWhere + ") as T) as AA where row BETWEEN " + begin_index.ToString() + " and " + end_index + "";
            var db = new DataCore.EFDBContext();
            rowCount = db.Database.SqlQuery<int>(SQLTotal).ToList()[0];
            List<Entity.WorkJob> db_list = db.Database.SqlQuery<Entity.WorkJob>(SQL).ToList();
            foreach (var item in db_list)
            {
                item.StatusText = BLL.BLLWorkJob.GetJobStatus(item.ID);
                item.WorkJobUsers = db.WorkJobUsers.AsNoTracking().Include(p => p.ConfrimFiles).Where(p => p.WorkJobID == item.ID).ToList();
                item.type = (item.CusUserID == user_id) ? 1 : 2;
            }
            return db_list;
        }


        /// <summary>
        /// 用户点击确认完成
        /// </summary>
        /// <param name="id">任务id</param>
        /// <param name="user_id">用户id</param>
        /// <param name="files">完成后提交的附件</param>
        /// <param name="text">完成的说明</param>
        public static void Confirm(int id, int user_id, string text, List<Entity.WorkJobUserFile> files)
        {
            using (var db = new DataCore.EFDBContext())
            {
                var entity_mee = db.WorkJobs.Where(p => p.ID == id).AsNoTracking().FirstOrDefault();
                if (entity_mee == null)
                    return;
                var entity_user = db.CusUsers.Where(p => p.ID == user_id).AsNoTracking().FirstOrDefault();
                if (entity_user == null)
                    return;
                var entity = db.WorkJobUsers.Where(p => p.WorkJobID == id && p.CusUserID == user_id).FirstOrDefault();
                if (entity != null)
                {
                    entity.IsConfirm = true;
                    entity.ConfirmText = text;

                    if(files != null)
                    {
                        foreach (var item in files)
                        {
                            Entity.WorkJobUserFile model = new Entity.WorkJobUserFile();
                            model.FileName = item.FileName;
                            model.FilePath = item.FilePath;
                            model.FileSize = item.FileSize;
                            model.WorkJobUserID = entity.ID;
                            db.WorkJobUserFiles.Add(model);
                        }
                    }

                    db.SaveChanges();
                    //向任务指派者发送通知
                    BLLMsg.PushMsg(entity_mee.CusUserID, Entity.CusUserMessageType.confrimdonejob, string.Format(BLLMsgTemplate.ConfrimDoneJob, entity_user.NickName, entity_mee.Title), id,entity_user.NickName);
                }
            }
        }

        /// <summary>
        /// 获取任务进行状态
        /// </summary>
        /// <returns></returns>
        public static string GetJobStatus(int id)
        {
            BLL.BaseBLL<Entity.WorkJobUser> bll = new BaseBLL<Entity.WorkJobUser>();
            if (bll.GetCount(p => p.WorkJobID == id && p.IsConfirm == false) > 0)
                return "进行中";
            else
                return "已完成";
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static int Add(Entity.WorkJob entity, string ids)
        {
            var db = new DataCore.EFDBContext();
            if (!string.IsNullOrWhiteSpace(ids))
            {
                List<Entity.WorkJobUser> users = new List<Entity.WorkJobUser>();
                foreach (var item in ids.Split(','))
                {
                    int id = Tools.TypeHelper.ObjectToInt(item);
                    if (id == entity.CusUserID) continue;
                    var entity_user = db.CusUsers.Find(id);
                    if (entity_user != null)
                    {
                        var model = new Entity.WorkJobUser();
                        model.CusUserID = id;
                        model.IsConfirm = false;
                        users.Add(model);
                    }
                }
                entity.WorkJobUsers = users;
            }
            var entity_user_2 = db.CusUsers.AsNoTracking().Where(p=>p.ID == entity.CusUserID).FirstOrDefault();
            db.WorkJobs.Add(entity);
            db.SaveChanges();
            db.Dispose();
            BLLMsg.PushSomeUser(ids, Entity.CusUserMessageType.waitjobdone, string.Format(BLLMsgTemplate.WaitMeetingDone, entity.Title), entity.ID,entity_user_2.NickName);
            return entity.ID;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static bool Modify(Entity.WorkJob entity, string ids)
        {
            var db = new DataCore.EFDBContext();
            db.WorkJobUsers.Where(p => p.WorkJobID == entity.ID).ToList().ForEach(p => db.WorkJobUsers.Remove(p));
            db.WorkJobFiles.Where(p => p.WorkJobID == entity.ID).ToList().ForEach(p => db.WorkJobFiles.Remove(p));
            if (!string.IsNullOrWhiteSpace(ids))
            {
                foreach (var item in ids.Split(','))
                {
                    int id = Tools.TypeHelper.ObjectToInt(item);
                    if (id == entity.CusUserID) continue;
                    var entity_user = db.CusUsers.Find(id);
                    if (entity_user != null)
                    {
                        var model = new Entity.WorkJobUser();
                        model.CusUserID = id;
                        model.IsConfirm = false;
                        model.WorkJobID = entity.ID;
                        db.WorkJobUsers.Add(model);
                    }
                }
            }
            List<Entity.WorkJobFile> file_list = entity.FileList.ToList();
            foreach (var item in file_list)
            {
                item.WorkJobID = entity.ID;
                db.WorkJobFiles.Add(item);
            }
            entity.FileList.Clear();
            var ss = db.Entry<Entity.WorkJob>(entity);
            ss.State = System.Data.Entity.EntityState.Modified;
            int row = db.SaveChanges();
            db.Dispose();
            return row > 0;
        }

    }
}
