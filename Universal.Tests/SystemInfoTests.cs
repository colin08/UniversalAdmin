using Microsoft.VisualStudio.TestTools.UnitTesting;
using Universal.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Tests
{
    [TestClass()]
    public class SystemInfoTests
    {

        [TestMethod()]
        public void SystemInfoTest()
        {
            SystemInfo sys = new SystemInfo();

            float cpuLoad= sys.CpuLoad;

            long memory = sys.MemoryAvailable;
            
            long physical =sys.PhysicalMemory;

            int cpuCount = sys.ProcessorCount;

            List<SystemInfo_DiskInfo> disk_list = sys.GetLogicalDrives();

            List<SystemInfo_ProcessInfo> processInfo = sys.GetProcessInfo();
                       

            Assert.Fail();
        }
    }
}