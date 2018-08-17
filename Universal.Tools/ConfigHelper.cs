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
        SiteConfig = 1,
        [Description("~/App_Data/CompanyProfile.config")]
        CompanyProfile
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
        /// 获取站点配置文件
        /// </summary>
        /// <returns></returns>
        public static WebSiteModel GetCompanyProfileModel()
        {
            return LoadConfig<WebSiteModel>(ConfigFileEnum.CompanyProfile);
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

        [Display(Name = "备案信息"), Required(ErrorMessage = "不能为空"), MaxLength(300, ErrorMessage = "不能超过300个字符")]
        public string Copyright { get; set; }

        [Display(Name ="附件地址")]
        public string FileUrl { get; set; }

        [Display(Name ="公司名称"),Required(ErrorMessage ="不能为空"),MaxLength(30,ErrorMessage = "不能超过30个字符")]
        public string CompanyName { get; set; }

        [Display(Name = "公司电话"), Required(ErrorMessage = "不能为空"), MaxLength(30, ErrorMessage = "不能超过30个字符")]
        public string CompanyTel { get; set; }

        [Display(Name = "公司邮箱"), Required(ErrorMessage = "不能为空"), MaxLength(30, ErrorMessage = "不能超过30个字符")]
        public string CompanyEmail { get; set; }

        [Display(Name = "公司地址"), Required(ErrorMessage = "不能为空"), MaxLength(30, ErrorMessage = "不能超过100个字符")]
        public string CompanyAddress { get; set; }

        [Display(Name = "站点应用程序池名称"), Required(ErrorMessage = "不能为空"), MaxLength(30, ErrorMessage = "不能超过30个字符")]
        public string AppPoolName { get; set; }
        
        [Display(Name = "页面耗时统计")]
        public bool WebExecutionTime { get; set; }

        [Display(Name = "操作日志是否入库")]
        public bool LogMethodInDB { get; set; }

        [Display(Name = "异常日志是否入库")]
        public bool LogExceptionInDB { get; set; }

        #endregion

        //#region 接口配置

        //[Display(Name = "接口验证是否开启")]
        //public bool WebAPIAuthentication { get; set; }

        //[Display(Name = "是否启用请求信息记录")]
        //public bool WebAPITracker { get; set; }

        //[Display(Name = "Token的KEY名"),Required(ErrorMessage ="不能为空"),MaxLength(50,ErrorMessage ="不能超过50个字符")]
        //public string WebAPITokenKey { get; set; }

        //[Display(Name = "Token混淆字符串"), Required(ErrorMessage = "不能为空"), MaxLength(50, ErrorMessage = "不能超过50个字符")]
        //public string WebAPIMixer { get; set; }

        //[Display(Name = "接口超时时间(分钟)"), Required(ErrorMessage = "不能为空")]
        //public int WebAPITmeOut { get; set; }

        //#endregion

        [Display(Name = "职位咨询电话"),Required(ErrorMessage ="不能为空"),MaxLength(50,ErrorMessage ="不能超过50个字符")]
        public string JobTel { get; set; }

        [Display(Name = "职位投递邮箱"), Required(ErrorMessage = "不能为空"), MaxLength(50, ErrorMessage = "不能超过50个字符")]
        public string JobEmail { get; set; }

        [Display(Name ="轮播图间隔时间"),Required(ErrorMessage ="必填")]
        public int BannerTime { get; set; }

        [Display(Name ="首页经典案例描述"),Required(ErrorMessage ="必填")]
        public string HomeCaseDesc { get; set; }
        
        [Display(Name ="首页大事记时间范围"),Required(ErrorMessage ="必填")]
        public string TimeLineRedCenterTop { get; set; }

        [Display(Name = "首页大事记内容"), Required(ErrorMessage = "必填")]
        public string TimeLineRedCenterDesc { get; set; }

        [Display(Name = "首页合作企业介绍"), Required(ErrorMessage = "必填")]
        public string TeamWorkDesc { get; set; }

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

    }


    /// <summary>
    /// 公司简介实体
    /// </summary>
    public class CompanyProfileModel
    {
        [Display(Name = "标题")]
        public string JJTitle { get; set; }

        [Display(Name = "二级标题")]
        public string JJTitleEr { get; set; }


        [Display(Name = "介绍内容")]
        public string JJDesc { get; set; }


        [Display(Name = "我们的客户")]
        public string JJKeHu { get; set; }

        [Display(Name = "客户介绍")]
        public string JJKeHuDesc { get; set; }

        [Display(Name = "客户数量")]
        public int JJKeHuNum { get; set; }


        [Display(Name = "500强企业数量")]
        public int JJKeHuQiang { get; set; }


        [Display(Name = "上市企业数量")]
        public int JJKeHuShi { get; set; }


        [Display(Name = "办公环境")]
        public string JJBanGong { get; set; }


        [Display(Name = "办公环境介绍")]
        public string JJBGDesc { get; set; }


        [Display(Name = "办公面积")]
        public string JJBGMJ { get; set; }


        [Display(Name = "办公图片")]
        public string JJBGImg { get; set; }


        [Display(Name = "1左侧标题")]
        public string JJOneLeftTitle { get; set; }


        [Display(Name = "1左侧内容")]
        public string JJOneLeftDesc { get; set; }


        [Display(Name = "1左侧图片")]
        public string JJOneLeftImg { get; set; }


        [Display(Name = "1右侧标题")]
        public string JJOneRightTitle { get; set; }


        [Display(Name = "1右侧内容")]
        public string JJOneRightDesc { get; set; }


        [Display(Name = "1右侧图片")]
        public string JJOneRightImg { get; set; }

        [Display(Name = "2左侧标题")]
        public string JJTwoLeftTitle { get; set; }


        [Display(Name = "2左侧内容")]
        public string JJTwoLeftDesc { get; set; }


        [Display(Name = "2左侧图片")]
        public string JJTwoLeftImg { get; set; }


        [Display(Name = "2右侧标题")]
        public string JJTwoRightTitle { get; set; }


        [Display(Name = "2右侧内容")]
        public string JJTwoRightDesc { get; set; }


        [Display(Name = "2右侧图片")]
        public string JJTwoRightImg { get; set; }
    }

}
