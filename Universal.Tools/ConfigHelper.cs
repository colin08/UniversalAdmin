using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Security;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Universal.Tools
{
    /// <summary>
    /// 配置文件枚举
    /// </summary>
    public enum ConfigFileEnum
    {
        [Description("~/App_Data/WebSite.config")]
        SiteConfig = 1
    }

    /// <summary>
    /// 站点配置文件
    /// </summary>
    public class ConfigHelper
    {
        private static object lockHelper = new object();

        /// <summary>
        /// 获取站点配置文件
        /// </summary>
        /// <returns></returns>
        public static WebSiteModel GetSiteModel()
        {
            return LoadConfig<WebSiteModel>(ConfigFileEnum.SiteConfig);
        }

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
        #region 站点配置

        [Display(Name = "站点URL"), Required(ErrorMessage = "不能为空"), RegularExpression(@"^(http|https)\://.+$", ErrorMessage = "URL格式错误")]
        public string SiteUrl { get; set; }
        
        [Display(Name ="公司名称"),Required(ErrorMessage ="不能为空"),MaxLength(30,ErrorMessage = "不能超过30个字符")]
        public string CompanyName { get; set; }

        [Display(Name = "版权声明"), Required(ErrorMessage = "不能为空"), MaxLength(100, ErrorMessage = "不能超过100个字符")]
        public string CopyrightNotice { get; set; }


        [Display(Name ="升为VIP所需充值金额"),Required(ErrorMessage ="不能为空")]
        public decimal VIPAmount { get; set; }


        [Display(Name = "站点应用程序池名称"), Required(ErrorMessage = "不能为空"), MaxLength(30, ErrorMessage = "不能超过30个字符")]
        public string AppPoolName { get; set; }
        
        [Display(Name = "页面耗时统计")]
        public bool WebExecutionTime { get; set; }

        [Display(Name = "操作日志是否入库")]
        public bool LogMethodInDB { get; set; }

        [Display(Name = "异常日志是否入库")]
        public bool LogExceptionInDB { get; set; }

        #endregion
        
        #region 邮件配置

        [Display(Name = "SMTP邮件服务器"),Required(ErrorMessage ="不能为空"),MaxLength(100,ErrorMessage ="不能超过100个字符")]
        public string EmailHost { get; set; }
        
        [Display(Name ="SMTP端口"),Required(ErrorMessage ="不能为空"),Range(10, 65535,ErrorMessage = "端口范围10-65535")]
        public int EmailPort { get; set; }
        
        [Display(Name = "是否启用SSL")]
        public bool EmailEnableSsl { get; set; }

        [Display(Name = "发送账户"),Required(ErrorMessage ="不能为空"),MaxLength(50,ErrorMessage ="不能超过50个字符"),RegularExpression(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?",ErrorMessage ="邮箱格式错误")]
        public string EmailFrom { get; set; }
        
        [Display(Name = "账户密码"),Required(ErrorMessage ="不能为空"),MaxLength(255,ErrorMessage ="不能超过255个字符")]
        public string EmailPwd { get; set; }

        #endregion

        #region 微信参数

        [Display(Name = "微信AppID"), Required(ErrorMessage = "不能为空"), MaxLength(255, ErrorMessage = "不能超过255个字符")]
        public string WeChatAppID { get; set; }

        [Display(Name = "微信AppSecret"), Required(ErrorMessage = "不能为空"), MaxLength(255, ErrorMessage = "不能超过255个字符")]
        public string WeChatAppSecret { get; set; }

        [Display(Name = "微信支付商户ID"), Required(ErrorMessage = "不能为空"), MaxLength(20, ErrorMessage = "不能超过20个字符")]
        public string WeChatPayMchid { get; set; }
        
        [Display(Name = "微信支付商户APPID"), Required(ErrorMessage = "不能为空"), MaxLength(20, ErrorMessage = "不能超过20个字符")]
        public string WeChatPayMchAppID { get; set; }
        
        [Display(Name = "微信支付PayKey"), Required(ErrorMessage = "不能为空"), MaxLength(255, ErrorMessage = "不能超过255个字符")]
        public string WeChatPayPayKey { get; set; }
        
        #endregion

    }

}
