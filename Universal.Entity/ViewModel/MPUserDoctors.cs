using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 医生实体-前台视图
    /// </summary>
    public class MPUserDoctors
    {
        public int ID { get; set; }

        /// <summary>
        /// 微信开放ID
        /// </summary>
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

        public string Avatar { get; set; }

        /// <summary>
        /// 获取用户头像
        /// </summary>
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
        public string NickName { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 身份证类别
        /// </summary>
        public MPUserIDCardType IDCardType { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCardNumber { get; set; }

        /// <summary>
        /// 电话，大陆+86，香港+852，澳门+853
        /// </summary>
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
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }


        /// <summary>
        /// 所属诊所ID
        /// </summary>
        public int? ClinicID { get; set; }

        /// <summary>
        /// 诊所名称
        /// </summary>
        public string ClinicTitle { get; set; }

        /// <summary>
        /// 头衔
        /// </summary>
        public string TouXian { get; set; }
        
        /// <summary>
        /// 接收咨询
        /// </summary>
        public bool CanAdvisory { get; set; }

        /// <summary>
        /// 咨询价格
        /// </summary>
        public decimal AdvisoryPrice { get; set; }

    }
}
