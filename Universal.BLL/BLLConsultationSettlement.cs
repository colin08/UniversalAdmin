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
    /// 咨询结算
    /// </summary>
    public class BLLConsultationSettlement
    {

        /// <summary>
        /// 医生提交结算
        /// </summary>
        /// <param name="doc_id"></param>
        /// <param name="c_ids"></param>
        /// <returns></returns>
        public static bool Add(int doc_id, string c_ids, out string msg)
        {
            msg = "ok";
            var c_id_arr = Array.ConvertAll<string, int>(c_ids.Split(','), int.Parse);
            if (c_id_arr.Length == 0) { msg = "选中咨询为空"; return false; }
            using (var db = new DataCore.EFDBContext())
            {
                var entity_doc = db.MPUsers.Where(p => p.ID == doc_id).AsNoTracking().FirstOrDefault();
                if (entity_doc == null) { msg = "医生不存在"; return false; }
                if (entity_doc.Identity != Entity.MPUserIdentity.Doctors) { msg = "医生不存在2"; return false; }
                var c_db_list = db.Consultations.Where(p => c_id_arr.Contains(p.ID)).ToList();
                decimal total_amount = 0;
                Entity.ConsultationSettlement entity_con = new Entity.ConsultationSettlement();
                entity_con.ConsultationSettlementItem = new List<Entity.ConsultationSettlementItem>();
                foreach (var item in c_db_list)
                {
                    if (item.Settlement != Entity.ConsultaionSett.待结算) continue;
                    total_amount += item.PayMoney;

                    Entity.ConsultationSettlementItem sitem = new Entity.ConsultationSettlementItem();
                    sitem.ConsultationID = item.ID;
                    entity_con.ConsultationSettlementItem.Add(sitem);

                    //置为已结算
                    item.Settlement = Entity.ConsultaionSett.已结算;

                }
                entity_con.Amount = total_amount;
                entity_con.MPUserID = doc_id;
                entity_con.OrderNum = DateTime.Now.ToString("yyyyMMddHHmmss") + Tools.WebHelper.GenerateRandomIntNumber(10);
                entity_con.RelAmount = total_amount;
                db.ConsultationSettlements.Add(entity_con);
                db.SaveChanges();
            }
            return true;
        }


        /// <summary>
        /// 获取医生端用户咨询结算列表
        /// </summary>
        /// <param name="page_size"></param>
        /// <param name="page_index"></param>
        /// <param name="doc_id">医生ID</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Entity.ViewModel.Settlement> GetPageList(int page_size, int page_index, int doc_id, int type, out int total)
        {
            List<Entity.ViewModel.Settlement> result = new List<Entity.ViewModel.Settlement>();
            total = 0;
            if (page_size <= 1) page_size = 8;
            if (page_index <= 0) page_index = 1;
            var sta = (Entity.ConsultationSettlementStatus)type;
            BaseBLL<Entity.ConsultationSettlement> bll = new BaseBLL<Entity.ConsultationSettlement>();
            var db_list = bll.GetPagedList(page_index, page_size, ref total, p => p.MPUserID == doc_id && p.Status == sta, "AddTime DESC", "ConsultationSettlementItem");
            foreach (var item in db_list)
            {
                Entity.ViewModel.Settlement model = new Entity.ViewModel.Settlement();
                model.amount = item.Amount.ToString("F2");
                model.rel_amount = item.RelAmount.ToString("F2");
                model.pay_s_str = item.GetPayStatusStr;
                if (item.Status == Entity.ConsultationSettlementStatus.审核通过)
                {
                    if(string.IsNullOrWhiteSpace(item.PayStatusDESC)) model.pay_desc = "暂无状态";
                    else model.pay_desc = item.PayStatusDESC;
                }
                model.s_desc = item.StatusDESC;
                model.s_str = item.GetStatusStr;
                model.time = item.AddTime.ToString("MM-dd HH:mm");
                model.total = item.ConsultationSettlementItem.Count;
                result.Add(model);
            }

            return result;
        }
        
    }
}
