using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 工作任务
    /// </summary>
    public class ViewModelWorkMeeting:ViewModelCustomFormBase
    {
        public ViewModelWorkMeeting()
        {
            this.begin_time = DateTime.Now;
            this.end_time = DateTime.Now.AddMinutes(20);
            this.users_entity = new List<ViewModelDocumentCategory>();
            this.file_list = new List<ViewModelListFile>();
        }

        public int id { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [Required(ErrorMessage = "不能为空"), MaxLength(500, ErrorMessage = "不能超过500个字符")]
        public string title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "不能为空"), MaxLength(500, ErrorMessage = "不能超过500个字符")]
        public string content { get; set; }

        /// <summary>
        /// 会议地点
        /// </summary>
        [Required(ErrorMessage = "不能为空"), MaxLength(100, ErrorMessage = "不能超过100个字符")]
        public string location { get; set; }
                
        public DateTime begin_time { get; set; }

        public DateTime end_time { get; set; }

        /// <summary>
        /// 参与用户
        /// </summary>
        public string user_ids { get; set; }

        /// <summary>
        /// 参与用户信息
        /// </summary>
        public List<ViewModelDocumentCategory> users_entity { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public string files { get; set; }

        /// <summary>
        /// 附件信息
        /// </summary>
        public List<ViewModelListFile> file_list { get; set; }

        /// <summary>
        /// 处理前端拼接的数据，并返回数据库所需数据
        /// </summary>
        public List<Entity.WorkMeetingFile> BuildFileList()
        {
            List<Entity.WorkMeetingFile> db_list = new List<Entity.WorkMeetingFile>();
            if (this.files == null)
                return db_list;
            if (this.files.Length == 0)
                return db_list;
            if (this.files.EndsWith("|"))
                this.files = this.files.Substring(0, this.files.Length - 1);
            this.file_list.Clear();

            foreach (var item in files.Split('|'))
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                ViewModelListFile model = new ViewModelListFile();
                Entity.WorkMeetingFile entity = new Entity.WorkMeetingFile();
                string[] f_len = item.Split(',');
                if (f_len.Length == 3)
                {
                    model.file_path = f_len[0];
                    model.file_name = f_len[1];
                    model.file_size = f_len[2];

                    entity.FilePath = f_len[0];
                    entity.FileName = f_len[1];
                    entity.FileSize = f_len[2];
                    db_list.Add(entity);
                }
                this.file_list.Add(model);
            }

            return db_list;
        }

        /// <summary>
        /// 构造前端展示所需数据
        /// </summary>
        /// <param name="entity"></param>
        public void BuildViewModelListFile(List<Entity.WorkMeetingFile> entity)
        {
            if (entity == null)
                return;
            System.Text.StringBuilder files = new System.Text.StringBuilder();
            foreach (var item in entity)
            {
                if (this.file_list == null)
                    this.file_list = new List<ViewModelListFile>();

                file_list.Add(new ViewModelListFile(item.FilePath, item.FileName, item.FileSize));
                files.Append(item.FilePath + "," + item.FileName + "," + item.FileSize + "|");
            }
            this.files = files.ToString();
        }

    }
}