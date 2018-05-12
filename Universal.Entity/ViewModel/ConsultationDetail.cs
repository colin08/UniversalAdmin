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

        public List<ConsultationDetailMsg> msg_list { get; set; }

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
