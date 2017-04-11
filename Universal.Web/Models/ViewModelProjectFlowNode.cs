using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Models
{
    /// <summary>
    /// 项目流程详情
    /// </summary>
    public class ViewModelProjectFlowNode
    {
        public ViewModelProjectFlowNode()
        {
            this.file_list = new List<ViewModelListFile>();
            this.users_entity = new List<ViewModelDocumentCategory>();
            this.node_file_list = new List<ViewModelListFile>();
        }

        public Entity.ProjectFlowNode flow_info { get; set; }

        /// <summary>
        /// 节点信息
        /// </summary>
        public Entity.Node node_info { get; set; }

        /// <summary>
        /// 节点联系人员
        /// </summary>
        public List<ViewModelDocumentCategory> users_entity { get; set; }

        /// <summary>
        /// 附件信息
        /// </summary>
        public List<ViewModelListFile> file_list { get; set; }

        /// <summary>
        /// 构造前端展示所需数据
        /// </summary>
        /// <param name="entity"></param>
        public void BuildViewModelListFile(List<Entity.NodeFile> entity)
        {
            if (entity == null)
                return;
            System.Text.StringBuilder files = new System.Text.StringBuilder();
            foreach (var item in entity)
            {
                if (this.file_list == null)
                    this.file_list = new List<ViewModelListFile>();

                file_list.Add(new ViewModelListFile(item.FilePath, item.FileName, item.FileSize));
            }
        }

        public string node_files { get; set; }

        /// <summary>
        /// 节点附件信息
        /// </summary>
        public List<ViewModelListFile> node_file_list { get; set; }

        /// <summary>
        /// 构造前端展示所需数据
        /// </summary>
        /// <param name="entity"></param>
        public void BuildViewModelNodeListFile(List<Entity.ProjectFlowNodeFile> entity)
        {
            if (entity == null)
                return;
            System.Text.StringBuilder files = new System.Text.StringBuilder();
            foreach (var item in entity)
            {
                if (this.node_file_list == null)
                    this.node_file_list = new List<ViewModelListFile>();

                node_file_list.Add(new ViewModelListFile(item.FilePath, item.FileName, item.FileSize));
                files.Append(item.FilePath + "," + item.FileName + "," + item.FileSize + "|");

            }
            this.node_files = files.ToString();
        }

    }
}