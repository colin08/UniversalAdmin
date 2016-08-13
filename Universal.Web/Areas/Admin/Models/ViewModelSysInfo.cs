using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Universal.Web.Areas.Admin.Models
{
    public class ViewModelSysInfo
    {
        /// <summary>
        /// 系统CPU占用比例
        /// </summary>
        public int CpuLoad { get; set; }

        /// <summary>
        /// CPU个数
        /// </summary>
        public int ProcessorTotal { get; set; }

        /// <summary>
        /// 可用内存(单位：G)
        /// </summary>
        public string MemoryAvailable { get; set; }
        
        /// <summary>
        /// 物理内存(单位：G)
        /// </summary>
        public string PhysicalMemory { get; set; }

        /// <summary>
        /// 内存使用比例
        /// </summary>
        public int MemoryScale { get; set; }

        /// <summary>
        /// 磁盘盘符
        /// </summary>
        public string DiskName { get; set; }

        /// <summary>
        /// 磁盘可用空间(单位：G)
        /// </summary>
        public string DiskAvailable { get; set; }

        /// <summary>
        /// 磁盘总空间(单位：G)
        /// </summary>
        public string PhysicalDisk { get; set; }

        /// <summary>
        /// 磁盘使用比例
        /// </summary>
        public float DiskScale { get; set; }

        /// <summary>
        /// 当前站点内存使用率
        /// </summary>
        public float SiteScale { get; set; }

        /// <summary>
        /// 当前站点使用内存
        /// </summary>
        public string SiteMemory { get; set; }
        
        /// <summary>
        /// 内存占用TOP10
        /// </summary>
        public List<ViewModelSysTopList> MemooryTopList { get; set; }

    }

    public class ViewModelSysTopList
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public string ProcessName { get; set; }

        public string Time { get; set; }

        /// <summary>
        /// 使用的内存(单位GB，不足就MB)
        /// </summary>
        public string WorkingSet64 { get; set; }

        /// <summary>
        /// 内存指示颜色
        /// </summary>
        public string MemeoryColor { get; set; }

        public string FileName { get; set; }
    }

}