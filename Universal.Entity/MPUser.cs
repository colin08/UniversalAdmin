using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 前端用户身份类别
    /// </summary>
    public enum MPUserIdentity : byte
    {
        [Description("普通用户")]
        Normal = 1,
        [Description("VIP用户")]
        VIP = 2,
        [Description("医生")]
        Doctors = 3
    }

    /// <summary>
    /// 用户身份证类别 
    /// 身份证18位
    /// 香港身份证格式：A123456(0)
    /// 澳门身份证格式：5215299(8)  /^[1|5|7][0-9]{6}\([0-9Aa]\)/
    /// </summary>
    public enum MPUserIDCardType : byte
    {
        [Description("大陆身份证")]
        IDCard = 1,
        [Description("香港身份证")]
        HKIDCard = 2,
        [Description("澳门身份证")]
        MOIDCard = 3
    }

    /// <summary>
    /// 微信用户性别
    /// </summary>
    public enum MPUserGenderType : byte
    {
        [Description("未知")]
        unknown = 0,
        [Description("男")]
        male = 1,
        [Description("女")]
        female = 2
    }

    /// <summary>
    /// 前端用户
    /// </summary>
    public class MPUser
    {
        public MPUser()
        {
            this.Identity = MPUserIdentity.Normal;
            this.LastLoginTime = DateTime.Now;
            this.Gender = MPUserGenderType.unknown;
            this.AddTime = DateTime.Now;
            this.Brithday = null;
            this.IDCardType = MPUserIDCardType.IDCard;
            this.AccountBalance = 0;
            this.IsFullInfo = false;
            this.Weight = 99;
            this.Status = true;
        }

        public int ID { get; set; }

        /// <summary>
        /// 微信开放ID
        /// </summary>
        [MaxLength(255), Required(ErrorMessage = "开放ID不能为空"), Index(IsUnique = true)]
        public string OpenID { get; set; }

        /// <summary>
        /// 用户身份类别
        /// </summary>
        public MPUserIdentity Identity { get; set; }

        /// <summary>
        /// 获取身份类别名称
        /// </summary>
        public string GetIdentityStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<MPUserIdentity>(Identity);
            }
        }


        [MaxLength(255)]
        public string Avatar { get; set; }

        /// <summary>
        /// 获取用户头像
        /// </summary>
        [NotMapped]
        public string GetAvatar
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Avatar)) return "/Assets/mui/img/default-avatar.jpg";
                else return this.Avatar;
            }
        }

        /// <summary>
        /// 用户昵称(备用)
        /// </summary>
        [MaxLength(100)]
        public string NickName { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [MaxLength(100)]
        public string RealName { get; set; }

        public string GetShowName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(RealName)) return NickName;
                else return RealName;
            }
        }


        /// <summary>
        /// 身份证类别
        /// </summary>
        public MPUserIDCardType IDCardType { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [MaxLength(30)]
        public string IDCardNumber { get; set; }

        /// <summary>
        /// 电话，大陆+86，香港+852，澳门+853
        /// </summary>
        [MaxLength(50)]
        public string Telphone { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public MPUserGenderType Gender { get; set; }

        /// <summary>
        /// 获取性别
        /// </summary>
        public string GetGenderStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<MPUserGenderType>(Gender);
            }
        }

        /// <summary>
        /// 生日
        /// </summary>
        [Column(TypeName = "Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Brithday { get; set; }

        public string GetBrithday
        {
            get
            {
                if (Brithday == null) return "";
                return Tools.TypeHelper.ObjectToDateTime(Brithday).ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 根据生日获取年龄
        /// </summary>
        public int GetAge
        {
            get
            {
                if (Brithday == null) return 0;
                DateTime now = DateTime.Now;
                var bri = Tools.TypeHelper.ObjectToDateTime(Brithday);
                int age = now.Year - bri.Year;
                if (now.Month < bri.Month || (now.Month == bri.Month && now.Day < bri.Day))
                {
                    age--;
                }
                return age < 0 ? 0 : age;
            }
        }

        /// <summary>
        /// 账户余额
        /// </summary>
        [Column(TypeName = "money")]
        public decimal AccountBalance { get; set; }

        /// <summary>
        /// 是否完善了资料
        /// </summary>
        public bool IsFullInfo { get; set; }

        /// <summary>
        /// 排序数字--医生排序
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [Display(Name = "添加时间"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode =true)]
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }
        
        /// <summary>
        /// 医生信息
        /// </summary>
        public virtual MPUserDoctors DoctorsInfo { get; set; }


        /// <summary>
        /// 获取医生所属诊所名称
        /// </summary>
        [NotMapped]
        public string GetClinicTitle
        {
            get
            {
                if (DoctorsInfo == null) return "";
                if (DoctorsInfo.Clinic == null) return "";
                return DoctorsInfo.Clinic.Title;
            }
        }

    }
}
