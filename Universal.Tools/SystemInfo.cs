using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Management;
using System.Runtime.InteropServices;

namespace Universal.Tools
{
    /// <summary>
    /// 系统信息类 - 获取CPU、内存、磁盘、进程信息
    /// 注意，生产环境下需要将对应网站的应用程序池的【进程模型】>【标识】由【ApplicationPoolIdentity】改为：LocalSystem
    /// </summary>
    public class SystemInfo
    {
        private int m_ProcessorCount = 0;   //CPU个数
        private PerformanceCounter pcCpuLoad;   //CPU计数器
        private long m_PhysicalMemory = 0;   //物理内存

        private const int GW_HWNDFIRST = 0;
        private const int GW_HWNDNEXT = 2;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 268435456;
        private const int WS_BORDER = 8388608;

        #region AIP声明
        [DllImport("IpHlpApi.dll")]
        extern static public uint GetIfTable(byte[] pIfTable, ref uint pdwSize, bool bOrder);

        [DllImport("User32")]
        private extern static int GetWindow(int hWnd, int wCmd);

        [DllImport("User32")]
        private extern static int GetWindowLongA(int hWnd, int wIndx);

        [DllImport("user32.dll")]
        private static extern bool GetWindowText(int hWnd, StringBuilder title, int maxBufSize);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static int GetWindowTextLength(IntPtr hWnd);
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数，初始化计数器等
        /// </summary>
        public SystemInfo()
        {
            //初始化CPU计数器
            pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            pcCpuLoad.MachineName = ".";
            pcCpuLoad.NextValue();

            //CPU个数
            m_ProcessorCount = Environment.ProcessorCount;

            //获得物理内存
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo["TotalPhysicalMemory"] != null)
                {
                    m_PhysicalMemory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                }
            }
        }
        #endregion

        #region CPU个数
        /// <summary>
        /// 获取CPU个数
        /// </summary>
        public int ProcessorCount
        {
            get
            {
                return m_ProcessorCount;
            }
        }
        #endregion

        #region CPU占用率
        /// <summary>
        /// 获取CPU占用率
        /// </summary>
        public float CpuLoad
        {
            get
            {
                return pcCpuLoad.NextValue();
            }
        }
        #endregion

        #region 可用内存
        /// <summary>
        /// 获取可用内存
        /// </summary>
        public long MemoryAvailable
        {
            get
            {
                long availablebytes = 0;
                //ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_PerfRawData_PerfOS_Memory");
                //foreach (ManagementObject mo in mos.Get())
                //{
                //    availablebytes = long.Parse(mo["Availablebytes"].ToString());
                //}
                ManagementClass mos = new ManagementClass("Win32_OperatingSystem");
                foreach (ManagementObject mo in mos.GetInstances())
                {
                    if (mo["FreePhysicalMemory"] != null)
                    {
                        availablebytes = 1024 * long.Parse(mo["FreePhysicalMemory"].ToString());
                    }
                }
                return availablebytes;
            }
        }
        #endregion

        #region 物理内存
        /// <summary>
        /// 获取物理内存
        /// </summary>
        public long PhysicalMemory
        {
            get
            {
                return m_PhysicalMemory;
            }
        }
        #endregion

        #region 获得分区信息
        /// <summary>
        /// 获取分区信息
        /// </summary>
        public List<SystemInfo_DiskInfo> GetLogicalDrives()
        {
            List<SystemInfo_DiskInfo> drives = new List<SystemInfo_DiskInfo>();
            ManagementClass diskClass = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection disks = diskClass.GetInstances();
            foreach (ManagementObject disk in disks)
            {
                // DriveType.Fixed 为固定磁盘(硬盘)
                if (int.Parse(disk["DriveType"].ToString()) == (int)DriveType.Fixed)
                {
                    drives.Add(new SystemInfo_DiskInfo(disk["Name"].ToString(), long.Parse(disk["Size"].ToString()), long.Parse(disk["FreeSpace"].ToString())));
                }
            }
            return drives;
        }
        /// <summary>
        /// 获取特定分区信息
        /// </summary>
        /// <param name="DriverID">盘符</param>
        public List<SystemInfo_DiskInfo> GetLogicalDrives(char DriverID)
        {
            List<SystemInfo_DiskInfo> drives = new List<SystemInfo_DiskInfo>();
            WqlObjectQuery wmiquery = new WqlObjectQuery("SELECT * FROM Win32_LogicalDisk WHERE DeviceID = '" + DriverID + ":'");
            ManagementObjectSearcher wmifind = new ManagementObjectSearcher(wmiquery);
            foreach (ManagementObject disk in wmifind.Get())
            {
                if (int.Parse(disk["DriveType"].ToString()) == (int)DriveType.Fixed)
                {
                    drives.Add(new SystemInfo_DiskInfo(disk["Name"].ToString(), long.Parse(disk["Size"].ToString()), long.Parse(disk["FreeSpace"].ToString())));
                }
            }
            return drives;
        }
        #endregion

        #region 获得进程列表
        /// <summary>
        /// 获得进程列表
        /// </summary>
        public List<SystemInfo_ProcessInfo> GetProcessInfo()
        {
            List<SystemInfo_ProcessInfo> pInfo = new List<SystemInfo_ProcessInfo>();
            Process[] processes = Process.GetProcesses();
            foreach (Process instance in processes)
            {
                try
                {
                    pInfo.Add(new SystemInfo_ProcessInfo(instance.Id,
                        instance.ProcessName,
                        instance.TotalProcessorTime.TotalMilliseconds,
                        instance.WorkingSet64,
                        instance.MainModule.FileName,
                        instance.StartTime));
                }
                catch { }
            }
            return pInfo;
        }
        /// <summary>
        /// 获得特定进程信息
        /// </summary>
        /// <param name="ProcessName">进程名称</param>
        public List<SystemInfo_ProcessInfo> GetProcessInfo(string ProcessName)
        {
            List<SystemInfo_ProcessInfo> pInfo = new List<SystemInfo_ProcessInfo>();
            Process[] processes = Process.GetProcessesByName(ProcessName);
            foreach (Process instance in processes)
            {
                try
                {
                        pInfo.Add(new SystemInfo_ProcessInfo(instance.Id,
                        instance.ProcessName,
                        instance.TotalProcessorTime.TotalMilliseconds,
                        instance.WorkingSet64,
                        instance.MainModule.FileName,
                        instance.StartTime));
                }
                catch { }
            }
            return pInfo;
        }
        #endregion

        #region 结束指定进程
        /// <summary>
        /// 结束指定进程
        /// </summary>
        /// <param name="pid">进程的 Process ID</param>
        public static void EndProcess(int pid)
        {
            try
            {
                Process process = Process.GetProcessById(pid);
                process.Kill();
            }
            catch { }
        }
        #endregion


        #region 查找所有应用程序标题
        /// <summary>
        /// 查找所有应用程序标题
        /// </summary>
        /// <returns>应用程序标题范型</returns>
        public static List<string> FindAllApps(int Handle)
        {
            List<string> Apps = new List<string>();

            int hwCurr;
            hwCurr = GetWindow(Handle, GW_HWNDFIRST);

            while (hwCurr > 0)
            {
                int IsTask = (WS_VISIBLE | WS_BORDER);
                int lngStyle = GetWindowLongA(hwCurr, GWL_STYLE);
                bool TaskWindow = ((lngStyle & IsTask) == IsTask);
                if (TaskWindow)
                {
                    int length = GetWindowTextLength(new IntPtr(hwCurr));
                    StringBuilder sb = new StringBuilder(2 * length + 1);
                    GetWindowText(hwCurr, sb, sb.Capacity);
                    string strTitle = sb.ToString();
                    if (!string.IsNullOrEmpty(strTitle))
                    {
                        Apps.Add(strTitle);
                    }
                }
                hwCurr = GetWindow(hwCurr, GW_HWNDNEXT);
            }

            return Apps;
        }
        #endregion     
    }

    /// <summary>
    /// 系统磁盘信息实体类
    /// </summary>
    public class SystemInfo_DiskInfo
    {
        public string Name { get; set; }

        public long Size { get; set; }

        public long FreeSpace { get; set; }

        public SystemInfo_DiskInfo() { }

        public SystemInfo_DiskInfo(string name,long size,long freeSpace)
        {
            this.Name = name;
            this.Size = size;
            this.FreeSpace = freeSpace;
        }

    }

    /// <summary>
    /// 系统进程信息实体类
    /// </summary>
    public class SystemInfo_ProcessInfo
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        
        public string ProcessName { get; set; }

        public double TotalMilliseconds { get; set; }

        public long WorkingSet64 { get; set; }

        public string FileName { get; set; }

        public SystemInfo_ProcessInfo() { }

        public SystemInfo_ProcessInfo(int id,string processName,double totalMilliseconds,long workingSet64,string fileName,DateTime StartTime)
        {
            this.Id = id;
            this.ProcessName = processName;
            this.TotalMilliseconds = totalMilliseconds;
            this.WorkingSet64 = workingSet64;
            this.FileName = fileName;
            this.StartTime = StartTime;
        }

    }

}
