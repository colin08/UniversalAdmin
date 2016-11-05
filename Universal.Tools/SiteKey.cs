using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal.Tools
{
    /// <summary>
    /// 站点KEY
    /// </summary>
    public class SiteKey
    {
        /// <summary>
        /// 3DES加密KEY
        /// </summary>
        public static readonly string DES3KEY = "Orderfood";

        /// <summary>
        /// 后台分页默认每页大小
        /// </summary>
        public static readonly int AdminDefaultPageSize = 12;

        /// <summary>
        /// 缓存时间(分钟)
        /// </summary>
        public static readonly int CACHE_TIME = 60;

    }

    /// <summary>
    /// 后台操作数据类型
    /// </summary>
    public enum Admin_Edit_Type
    {
        update,
        add,
        look
    }

    /// <summary>
    /// 后台上传类型
    /// </summary>
    public enum Admin_Upload_Type
    {
        /// <summary>
        /// 一张照片
        /// </summary>
        OnePicture,

        /// <summary>
        /// 多张照片
        /// </summary>
        MorePicture,

        /// <summary>
        /// 安卓安装包
        /// </summary>
        APK,

        /// <summary>
        /// IOS安装包
        /// </summary>
        IPA,

        /// <summary>
        /// 附件压缩包
        /// </summary>
        ZIP,

        /// <summary>
        /// 富文本编辑器
        /// </summary>
        TxtArea
    }


    public class CookieKey
    {
        /// <summary>
        /// 是否记住我
        /// </summary>
        public static readonly string Is_Remeber = "isrm";

        /// <summary>
        /// 用户Id
        /// </summary>
        public static readonly string Login_UserID = "oid";

        /// <summary>
        /// 用户密码
        /// </summary>
        public static readonly string Login_UserPassword = "opwd";


        /// <summary>
        /// 前端是否记住我
        /// </summary>
        public static readonly string Web_Is_Remeber = "wisrm";

        /// <summary>
        /// 前端用户Id
        /// </summary>
        public static readonly string Web_Login_UserID = "wid";

        /// <summary>
        /// 前端用户密码
        /// </summary>
        public static readonly string Web_Login_UserPassword = "wp";

    }


    public class SessionKey
    {
        /// <summary>
        /// 后台管理用户的实体
        /// </summary>
        public static readonly string Admin_User_Info = "ADMIN_USER_INFO";

        /// <summary>
        /// 登陆错误次数
        /// </summary>
        public static readonly string Login_Fail_Total = "SESSION_LOGIN_FAIL";

        /// <summary>
        /// 前端管理用户的实体
        /// </summary>
        public static readonly string Web_User_Info = "WEB_USER_INFO";

    }
}
