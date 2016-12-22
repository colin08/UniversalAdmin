using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    public class ViewModelDocument : ViewModelCustomFormBase
    {
        public ViewModelDocument()
        {
            this.see_entity = new List<ViewModelDocumentCategory>();
            this.file_list = new List<ViewModelListFile>();
        }

        public int id { get; set; }

        [Display(Name = "标题"), StringLength(255), Required(ErrorMessage = "标题不能为空")]
        public string title { get; set; }


        [MaxLength(500)]
        public string filepath { get; set; }

        [MaxLength(30)]
        public string filesize { get; set; }

        [Required(ErrorMessage = "请选择所属分类")]
        public int category_id { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 秘籍权限
        /// </summary>
        public Entity.DocPostSee post_see { get; set; }

        /// <summary>
        /// 可以看的用户或部门id，逗号分割
        /// </summary>
        public string see_ids { get; set; }

        /// <summary>
        /// 可以看的用户或部门信息，用于还原数据
        /// </summary>
        public List<ViewModelDocumentCategory> see_entity { get; set; }

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
        public List<Entity.DocFile> BuildFileList()
        {
            List<Entity.DocFile> db_list = new List<Entity.DocFile>();
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
                Entity.DocFile entity = new Entity.DocFile();
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
        public void BuildViewModelListFile(List<Entity.DocFile> entity)
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

    public class ViewModelDocumentCategory
    {
        public ViewModelDocumentCategory()
        {

        }

        public ViewModelDocumentCategory(int id, string title)
        {
            this.id = id;
            this.title = title;
        }

        public int id { get; set; }

        public string title { get; set; }
    }

}