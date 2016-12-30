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
    public class ViewModelWorkJob:ViewModelCustomFormBase
    {
        public ViewModelWorkJob()
        {
            this.year = DateTime.Now.Year.ToString();
            this.month = DateTime.Now.Month.ToString();
            this.day = DateTime.Now.Day.ToString();
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
        /// 状态
        /// </summary>
        public Entity.WorkStatus status { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public string year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public string month { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        public string day { get; set; }

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
        public List<Entity.WorkJobFile> BuildFileList()
        {
            List<Entity.WorkJobFile> db_list = new List<Entity.WorkJobFile>();
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
                Entity.WorkJobFile entity = new Entity.WorkJobFile();
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
        public void BuildViewModelListFile(List<Entity.WorkJobFile> entity)
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