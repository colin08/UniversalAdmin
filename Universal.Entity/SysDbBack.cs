using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Entity
{
    /// <summary>
    /// 数据库备份模式
    /// </summary>
    public enum SysDbBackType:byte
    {
        [Description("完整备份")]
        full =1,
        [Description("差异备份")]
        diff
    }

    /// <summary>
    /// 数据库备份
    /// </summary>
    public class SysDbBack :BaseAdminEntity
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 备份的数据库名字
        /// </summary>
        [MaxLength(30),Required(ErrorMessage ="数据库名字不能为空"),Display(Name ="数据库名")]
        public string DbName { get; set; }

        /// <summary>
        /// 备份名称
        /// </summary>
        [MaxLength(50,ErrorMessage ="不能超过50个字符"), Required(ErrorMessage = "备份的名称不能为空"), Display(Name = "备份名称")]
        public string BackName { get; set; }

        /// <summary>
        /// 备份类别
        /// </summary>
        [Required(ErrorMessage ="类别必选"),Display(Name ="备份类别")]
        public SysDbBackType BackType { get; set; }

        [NotMapped]
        public string TypeStr
        {
            get
            {
                return Tools.EnumHelper.GetDescription<SysDbBackType>(this.BackType);
            }
        }

        /// <summary>
        /// 备份文件保存的路径
        /// </summary>
        [MaxLength(255),Display(Name ="文件路径")]
        public string FilePath { get; set; }

        /// <summary>
        /// 备份备注
        /// </summary>
        [MaxLength(500, ErrorMessage = "不能超过500个字符"), Display(Name = "备注")]
        public string Remark { get; set; }
    }

    /// <summary>
    /// 系统数据库
    /// </summary>
    public class SysDbInfo
    {
        public string name { get; set; }

        public string filename { get; set; }

        public DateTime crdate { get; set; }
    }

}
