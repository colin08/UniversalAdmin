using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Entity.ViewModel
{
    /// <summary>
    /// 咨询详情页数据
    /// </summary>
    public class ConsultationDetail
    {
        /// <summary>
        /// 主咨询ID
        /// </summary>
        public int main_id { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string user_avatar { get; set; }

        /// <summary>
        /// 医生头像
        /// </summary>
        public string doctor_avatar { get; set; }
        
        /// <summary>
        /// 咨询状态
        /// </summary>
        public ConsultationStatus status { get; set; }

        public ConsultationInfo CInfo { get; set; }


        public List<ConsultationDetailMsg> msg_list { get; set; }

    }

    
    public class ConsultationInfo
    {
        public ConsultationInfo()
        {
            this.user_name = "未知";
            this.gender = "未知";
            this.age = "未知";
            this.area = "未知";
            this.dis_str = "未知";
            this.price = "未知";
            this.create_time = "未知";
            this.pay_time = "未知";
        }

        public string user_name { get; set; }

        public string gender { get; set;}
        
        public string age { get; set; }

        public string area { get; set; }
        
        /// <summary>
        /// 病症类型
        /// </summary>
        public string dis_str { get; set; }

        public string price { get; set; }

        public string create_time { get; set; }

        public string pay_time { get; set; }

    }

    public class ConsultationDetailMsg
    {
        public ConsultationDetailMsg()
        {
            this.file_image_list = new List<string>();
            this.file_voice_list = new List<string>();
        }

        public int id { get; set; }

        /// <summary>
        /// 用户类别
        /// </summary>
        public ReplayUserType user_type { get; set; }
        
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime time { get; set; }

        /// <summary>
        /// 时间-多少分钟前
        /// </summary>
        public string time_str { get; set; }
        
        public List<string> file_image_list { get; set; }
        
        public List<string> file_voice_list { get; set; }

        /// <summary>
        /// 图片数量
        /// </summary>
        public int total_img
        {
            get
            {
                return file_image_list.Count;
            }
        }

        /// <summary>
        /// 音频文件数量
        /// </summary>
        public int total_voice
        {
            get
            {
                return file_voice_list.Count;
            }
        }
    }

}
