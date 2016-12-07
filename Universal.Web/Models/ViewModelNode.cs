using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 节点编辑对象
    /// </summary>
    public class ViewModelNode: ViewModelCustomFormBase
    {
        public ViewModelNode()
        {
            this.user_ids = "";
            this.users_entity = new List<ViewModelDocumentCategory>();
            this.file_list = new List<ViewModelListFile>();
        }

        public int id { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        [Required(ErrorMessage = "不能为空"), MaxLength(255, ErrorMessage = "不能超过255个字符")]
        public string title { get; set; }

        /// <summary>
        /// 办事地址
        /// </summary>
        [Required(ErrorMessage = "不能为空"), MaxLength(500, ErrorMessage = "不能超过500个字符")]
        public string location { get; set; }

        /// <summary>
        /// 办事流程说明
        /// </summary>
        [Required(ErrorMessage = "不能为空"), MaxLength(2000, ErrorMessage = "不能超过2000个字符")]
        public string content { get; set; }
        
        /// <summary>
        /// 参与用户
        /// </summary>
        public string user_ids { get; set; }

        /// <summary>
        /// 参与用户信息
        /// </summary>
        public List<ViewModelDocumentCategory> users_entity { get; set; }

        /// <summary>
        /// 附件信息
        /// </summary>
        public List<ViewModelListFile> file_list { get; set; }

    }

    /// <summary>
    /// 附件列表信息
    /// </summary>
    public class ViewModelListFile
    {
        public ViewModelListFile() { }

        public ViewModelListFile(string file_path,string file_name,string file_size)
        {
            this.file_path = file_path;
            this.file_name = file_name;
            this.file_size = file_size;
        }

        public string file_path { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary>
        public string file_name { get; set; }

        /// <summary>
        /// 附件大小
        /// </summary>
        public string file_size { get; set; }

    }

}