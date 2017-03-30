using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;


namespace Universal.Tools
{
    /// <summary>
    /// 应用程序池操作枚举
    /// </summary>
    public enum IISHelperAppPoolMethod
    {
        Start,
        Recycle,
        Stop
    }

    /// <summary>
    /// IIS操作
    /// </summary>
    public static class IISHelper
    {
        /// <summary>
        /// 操作应用程序池
        /// </summary>
        /// <param name="AppPoolName">应用程序池名称</param>
        /// <returns></returns>
        public static bool AppPool(IISHelperAppPoolMethod method,string AppPoolName)
        {
            try
            {
                DirectoryEntry appPool = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
                DirectoryEntry findPool = appPool.Children.Find(AppPoolName, "IIsApplicationPool");
                findPool.Invoke(method.ToString(), null);
                appPool.CommitChanges();
                appPool.Close();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("应用程序池"+ method.ToString() + "失败：" + ex.Message);
                return false;
            }

        }


    }
}
