using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Security;

namespace Universal.Tools
{
    /// <summary>
    /// 配置文件枚举
    /// </summary>
    public enum ConfigFileEnum
    {
        [EnumShowNameAttribute("/App_Data/WebSite.config")]
        SiteConfig = 1
    }

    /// <summary>
    /// 站点配置文件
    /// </summary>
    public class ConfigHelper
    {
        private static object lockHelper = new object();

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configPath">配置文件相对路径</param>
        /// <param name="IsCache">是否使用缓存，默认true</param>
        /// <returns></returns>
        public static T LoadConfig<T>(ConfigFileEnum config, bool isCache = true)
        {
            string configPath = IOHelper.GetMapPath(EnumHelper.GetEnumShowName(typeof(ConfigFileEnum), (int)config));
            string cacheKey = "Cache_" + Enum.GetName(typeof(ConfigFileEnum), config);
            if (isCache)
            {
                T model = (T)CacheHelper.Get(cacheKey);
                if (model == null)
                {
                    model = (T)IOHelper.DeserializeFromXML(typeof(T), configPath);
                    if (model != null)
                        CacheHelper.Insert(cacheKey, model, configPath);
                }
                return model;
            }
            else
            {
                return (T)IOHelper.DeserializeFromXML(typeof(T), configPath);
            }

        }

        /// <summary>
        /// 写入配置文件
        /// </summary>
        /// <param name="configPath">配置文件相对路径</param>
        /// <param name="obj">新的内容实体</param>
        /// <returns></returns>
        public static bool SaveConfig(ConfigFileEnum config, object obj)
        {
            string configPath = IOHelper.GetMapPath(EnumHelper.GetEnumShowName(typeof(ConfigFileEnum), (int)config));
            var permissionSet = new PermissionSet(PermissionState.None);
            var writePermission = new FileIOPermission(FileIOPermissionAccess.Write, configPath);
            permissionSet.AddPermission(writePermission);

            if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
            {
                lock (lockHelper)
                {
                    IOHelper.SerializeToXml(obj, configPath);
                }
                return true;
            }
            else
            {
                return false;//没有写入权限
            }
        }
    }

    /// <summary>
    /// 站点配置文件实体类
    /// </summary>
    public class WebSiteModel
    {
        /// <summary>
        /// 站点URL
        /// </summary>
        public string SiteUrl { get; set; }

        /// <summary>
        /// 是否启用页面耗时统计
        /// </summary>
        public bool TimingEnabled { get; set; }
        
        /// <summary>
        /// 操作日志是否入库
        /// </summary>
        public bool LogMethodInDB { get; set; }

        /// <summary>
        /// 异常日志是否入库
        /// </summary>
        public bool LogExceptionInDB { get; set; }

        /// <summary>
        /// 启用接口日志
        /// </summary>
        public bool EnableAPILog { get; set; }

        /// <summary>
        /// 重置的密码
        /// </summary>
        public string ResetPwd { get; set; }
        
    }

}
