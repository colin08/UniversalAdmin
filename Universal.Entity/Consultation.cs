using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{

    /// <summary>
    /// 咨询帖子状态
    /// </summary>
    public enum ConsultationStatus : byte
    {
        [Description("待支付")]
        待支付 = 1,
        [Description("已支付")]
        已支付,
        [Description("进行中")]
        进行中,
        [Description("已关闭")]
        已关闭,
        [Description("已完成")]
        已完成
    }

    /// <summary>
    /// 结算状态
    /// </summary>
    public enum ConsultaionSett : byte
    {
        [Description("不可结算")]
        不可结算=1,
        [Description("待结算")]
        待结算,
        [Description("已结算")]
        已结算
    }

    /// <summary>
    /// 最后回复的用户类别
    /// </summary>
    public enum ReplayUserType : byte
    {
        User = 1,
        Doctor = 2
    }


    /// <summary>
    /// 用户咨询
    /// </summary>
    public class Consultation
    {
        public Consultation()
        {
            this.AddTime = DateTime.Now;
            this.LastReplyTime = DateTime.Now;
            //默认最后回复的是用户
            this.LastReplayType = ReplayUserType.User;
            this.Status = ConsultationStatus.待支付;
            this.IsRefund = false;
            this.Content = "";
            this.CloseDesc = "";
            this.Settlement = ConsultaionSett.不可结算;
            this.SettDesc = "未支付";
            this.PayType = OrderPayType.微信支付;
        }

        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int? MPUserID { get; set; }
        

        /// <summary>
        /// 用户信息
        /// </summary>
        [ForeignKey("MPUserID")]
        public virtual MPUser MPUserInfo { get; set; }

        /// <summary>
        /// 获取咨询者用户名
        /// </summary>
        [NotMapped]
        public string GetUserName
        {
            get
            {
                if (MPUserInfo == null) return "";
                return MPUserInfo.RealName;
            }
        }

        /// <summary>
        /// 获取咨询者头像
        /// </summary>
        [NotMapped]
        public string GetUserAvatar
        {
            get
            {
                if (MPUserInfo == null) return "/Assets/mui/img/default-avatar.jpg";
                return MPUserInfo.GetAvatar;
            }
        }

        /// <summary>
        /// 获取咨询者年龄
        /// </summary>
        [NotMapped]
        public int GetUserAge
        {
            get
            {
                if (MPUserInfo == null) return 0;
                return MPUserInfo.GetAge;
            }
        }
        /// <summary>
        /// 获取咨询者性别
        /// </summary>
        [NotMapped]
        public string GetUserGender
        {
            get
            {
                if (MPUserInfo == null) return "未知";
                return MPUserInfo.GetGenderStr;
            }
        }


        /// <summary>
        /// 医生ID
        /// </summary>
        public int? MPDoctorID { get; set; }

        /// <summary>
        /// 医生信息
        /// </summary>
        [ForeignKey("MPDoctorID")]
        public virtual MPUser MPDoctorInfo { get; set; }

        /// <summary>
        /// 获取医生姓名
        /// </summary>
        [NotMapped]
        public string GetDoctorName
        {
            get
            {
                if (MPDoctorInfo == null) return "";
                return MPDoctorInfo.RealName;
            }
        }

        /// <summary>
        /// 获取医生头像
        /// </summary>
        [NotMapped]
        public string GetDoctorAvatar
        {
            get
            {
                if (MPDoctorInfo == null) return "/Assets/mui/img/default-avatar.jpg";
                return MPDoctorInfo.GetAvatar;
            }
        }

        /// <summary>
        /// 疾病类型
        /// </summary>
        public int? ConsultationDiseaseID { get; set; }

        /// <summary>
        /// 疾病类型信息
        /// </summary>
        public virtual ConsultationDisease ConsultationDisease { get; set; }
        
        /// <summary>
        /// 获取疾病类型标题
        /// </summary>
        [NotMapped]
        public string GetDiseaseTypeTitle
        {
            get
            {
                if (ConsultationDisease == null) return "";
                return ConsultationDisease.Title;
            }
        }
        
        /// <summary>
        /// 所在地
        /// </summary>
        [MaxLength(20)]
        public string Area { get; set; }

        /// <summary>
        /// 咨询状态
        /// </summary>
        public ConsultationStatus Status { get; set; }

        /// <summary>
        /// 获取咨询状态文本
        /// </summary>
        [NotMapped]
        public string GetStatusStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<ConsultationStatus>(Status);
            }
        }

        /// <summary>
        /// 结算状态
        /// </summary>
        public ConsultaionSett Settlement { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public OrderPayType PayType { get; set; }

        /// <summary>
        /// 获取结算状态文本
        /// </summary>
        [NotMapped]
        public string GetSettlementStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<ConsultaionSett>(Settlement);
            }
        }


        /// <summary>
        /// 支付订单号
        /// </summary>
        [MaxLength(50)]
        public string PayNumber { get; set; }

        /// <summary>
        /// 支付微信订单号
        /// </summary>
        [MaxLength(100)]
        public string PayWXNumber { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [Column(TypeName = "money")]
        public decimal PayMoney { get; set; }

        /// <summary>
        /// 提问内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否退款
        /// </summary>
        public bool IsRefund { get; set; }

        /// <summary>
        /// 帖子关闭说明
        /// </summary>
        [MaxLength(255)]
        public string CloseDesc { get; set; }

        /// <summary>
        /// 结算状态说明
        /// </summary>
        [MaxLength(255)]
        public string SettDesc { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 最后回复者的身份
        /// </summary>
        public ReplayUserType LastReplayType { get; set; }

        /// <summary>
        /// 最后回复时间
        /// </summary>
        public DateTime LastReplyTime { get; set; }

        /// <summary>
        /// 最后回复内容
        /// </summary>
        public string LastReplyContent { get; set; }

        /// <summary>
        /// 咨询附件
        /// </summary>
        public virtual ICollection<ConsultationFile> ConsultationFiles { get; set; }

        /// <summary>
        /// 咨询回复列表
        /// </summary>
        public virtual ICollection<ConsultationList> ConsultationLists { get; set; }

    }
}
