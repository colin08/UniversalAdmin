namespace Universal.Entity
{
    /// <summary>
    /// 咨询结算项具体咨询
    /// </summary>
    public class ConsultationSettlementItem
    {
        public ConsultationSettlementItem()
        {

        }


        public int ID { get; set; }

        /// <summary>
        /// 所属结算
        /// </summary>
        public int ConsultationSettlementID { get; set; }

        public virtual ConsultationSettlement ConsultationSettlement { get; set; }

        /// <summary>
        /// 包含的咨询
        /// </summary>
        public int ConsultationID { get; set; }

        /// <summary>
        /// 咨询详情
        /// </summary>
        public virtual Consultation Consultation { get; set; }
    }
}
